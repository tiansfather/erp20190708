
    (function (Handsontable) {
        /**
     * Retrieves asynchronously the display value corresponding to a key from the source items.
     *
     * @param {Object[]|Function} source List of source items or function returning source items asynchronously.
     * @param {String} keyProperty Name of the key property on a source item.
     * @param {String} valueProperty Name of the value property on a source item.
     * @param {String} keyValue Key of the source item we want to retrieve the display value.
     * @param {Function} callback Callback called when the value lookup is done.
     */
        //function getDisplayValue(source, keyProperty, valueProperty, keyValue, callback) {
        //    getSourceItem.call(this, source, keyProperty, keyValue, (item) => {
        //        callback(item ? item[valueProperty] : null);
        //    });
        //}

        function getDisplayValue(instance, row, col, keyProperty, valueProperty, keyValue, callback) {
            if (!keyValue) {
                callback(null);
                return;
            }
            var data = instance.getCellMeta(row, col)["selection"];
            if (data) {
                data = JSON.parse(data);
            }
            callback(data ? data[valueProperty] : null);
        }

        function getSourceItems(source, callback) {
            if (typeof source === 'function') {
                source.call(this, null, callback);
            } else if (Array.isArray(source)) {
                callback(source);
            } else {
                callback(null);
            }
        }
        /**
     * Returns the item corresponding to a key from a list of items.
     *
     * @param {Object[]} items List of source items.
     * @param {String} keyProperty Name of the key property on a source item.
     * @param {String} keyValue Key of the source item we want to retrieve the display value.
     *
     * @returns {String|null} The display value or `null` if not found.
     */
        function _getSourceItem(items, keyProperty, keyValue) {
            const sourceItem = items.find((item) => {
                const key = item[keyProperty];

                let castedKeyValue = keyValue;

                // HoT will sometimes cast the value to string,
                // so we have to cast it to original type for comparison
                if (typeof castedKeyValue !== typeof key) {
                    if (typeof key === 'number') {
                        castedKeyValue = Number(keyValue);
                    } else if (typeof key === 'boolean') {
                        castedKeyValue = keyValue === 'true';
                    }
                }

                return key === castedKeyValue;
            });

            return sourceItem;
        }

        /**
         * Retrieves asynchronously the item corresponding to a key from the source items.
         *
         * @param {Object[]|Function} source List of source items or function returning source items asynchronously.
         * @param {String} keyProperty Name of the key property on a source item.
         * @param {String} keyValue Key of the source item we want to retrieve the display value.
         * @param {Function} callback Callback called when the item lookup is done.
         */
        function getSourceItem(source, keyProperty, keyValue, callback) {
            getSourceItems.call(this, source, (items) => {
                if (items) {
                    callback(_getSourceItem(items, keyProperty, keyValue));
                } else {
                    callback(null);
                }
            });
        }

        const { getCaretPosition, getSelectionEndPosition, setCaretPosition } = Handsontable.dom;

        class KeyValueEditor extends Handsontable.editors.AutocompleteEditor {

            prepare(row, col, prop, td, originalValue, cellProperties, ...args) {
                super.prepare(row, col, prop, td, originalValue, cellProperties, ...args);
                this.cellProperties.strict = true; // Force strict mode (key-value context)
            }

            getValue() {
                const selection = this.htEditor.getSelectedLast();
                if (selection) {
                    var data = this.htEditor.getSourceDataAtRow(selection[0]);
                    console.log(data);
                    this.instance.setCellMeta(this.row, this.col, "selection", JSON.stringify(data));
                    return data[this.cellProperties.keyProperty];
                    //return JSON.stringify(this.htEditor.getSourceDataAtRow(selection[0]));
                }
            }

            setValue(value) {
                if (this.state === 'STATE_EDITING') {
                    const colIndex = this.instance.toPhysicalColumn(this.col);
                    const columnSettings = this.instance.getSettings().columns[colIndex];

                    //super.setValue.apply(this, value);

                    getDisplayValue.call(
                        this.cellProperties,
                        this.instance,
                        this.row,
                        this.col,                        
                        //columnSettings.source,
                        columnSettings.keyProperty,
                        columnSettings.valueProperty,
                        value,
                        (displayValue) => {
                            super.setValue.apply(this, [displayValue !== null ? displayValue : value]);
                        },
                    );
                }
            }

            queryChoices(query) {
                this.query = query;
                const { source } = this.cellProperties;

                if (typeof source === 'function') {
                    source.call(this.cellProperties, query, (choices) => {
                        this.rawChoices = choices;
                        this.updateChoicesList(choices);
                    });
                } else if (Array.isArray(source)) {
                    this.rawChoices = source;
                    this.updateChoicesList(source);
                } else {
                    this.updateChoicesList([]);
                }
            }

            open(...args) {
                super.open(args);

                // Autocomplete actually creates a HOT-in-HOT instance, in which we load objects as data.
                // Update this HOT-in-HOT instance settings so that only the display value column is shown.
                const choicesListHot = this.htEditor.getInstance();
                var columns = this.cellProperties.showItems ? this.cellProperties.showItems : [{
                    data: this.cellProperties.valueProperty
                }];
                choicesListHot.updateSettings({
                    columns: columns,
                });
                //choicesListHot.updateSettings({
                //    columns: [
                //        {
                //            data: this.cellProperties.valueProperty,
                //        },
                //    ],
                //});
            }

            updateChoicesList(choicesList) {
                // Almost a copy-paste of the `updateChoicesList` of `AutocompleteEditor`.
                // We just changed some parts so that the relevance algorithm is applied on the display values.
                const pos = getCaretPosition(this.TEXTAREA);
                const endPos = getSelectionEndPosition(this.TEXTAREA);
                const sortByRelevanceSetting = this.cellProperties.sortByRelevance;
                const filterSetting = this.cellProperties.filter;
                const valuePropertySetting = this.cellProperties.valueProperty;
                let orderByRelevance = null;
                let highlightIndex = null;

                let choices = choicesList;

                if (sortByRelevanceSetting) {
                    orderByRelevance = KeyValueEditor.sortByRelevance(
                        this.stripValueIfNeeded(this.TEXTAREA.value),
                        choicesList.map(choice => choice[valuePropertySetting]),
                        this.cellProperties.filteringCaseSensitive,
                    );
                }
                const orderByRelevanceLength = Array.isArray(orderByRelevance) ? orderByRelevance.length : 0;

                if (filterSetting === false) {
                    if (orderByRelevanceLength) {
                        // eslint-disable-next-line prefer-destructuring
                        highlightIndex = orderByRelevance[0];
                    }
                } else {
                    const sorted = [];

                    for (let i = 0, choicesCount = choices.length; i < choicesCount; i++) {
                        if (sortByRelevanceSetting && orderByRelevanceLength <= i) {
                            break;
                        }
                        if (orderByRelevanceLength) {
                            sorted.push(choices[orderByRelevance[i]]);
                        } else {
                            sorted.push(choices[i]);
                        }
                    }

                    highlightIndex = 0;
                    choices = sorted;
                }

                this.strippedChoices = choices;
                this.htEditor.loadData(choices);

                this.updateDropdownHeight();

                this.flipDropdownIfNeeded();

                this.highlightBestMatchingChoice(highlightIndex);

                this.instance.listen(false);

                setCaretPosition(this.TEXTAREA, pos, (pos === endPos ? void 0 : endPos));
            }

        }

        Handsontable.editors.registerEditor('key-value', KeyValueEditor);

        /**
 * Key-value pair renderer.
 *
 * @param {Object} instance Currently processed Handsontable instance.
 * @param {HTMLElement} td Currently rendered cell TD element.
 * @param {Number} row Row index.
 * @param {Number} col Column index.
 * @param {String|Number} prop Column index or property name.
 * @param {String} value Cell contents.
 * @param {Object} cellProperties Currently processed cell properties object, containing the cell's metadata.
 */
        function keyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
            const colIndex = instance.toPhysicalColumn(col);
            const columnSettings = instance.getSettings().columns[colIndex];
            console.log(row, col, value);
            getDisplayValue.call(
                this.cellProperties,
                instance,
                row,
                col,
                //columnSettings.source,
                columnSettings.keyProperty,
                columnSettings.valueProperty,
                value,
                (displayValue) => {
                    Handsontable.renderers.getRenderer('dropdown').apply(
                        this,
                        [instance, td, row, col, prop, displayValue !== null ? displayValue : value, cellProperties],
                    );
                },
            );

        }

        Handsontable.renderers.registerRenderer('key-value', keyValueRenderer);

        /**
 * Key-value pair validator.
 *
 * @param {string} value The value to validate.
 * @param {Function} callback The callback to call with `true` if `value` is valid or `false` otherwise.
 */
        function keyValueValidator(value, callback) {
            callback(true);
            //getSourceItem.call(
            //    this,
            //    this.source,
            //    this.keyProperty,
            //    value,
            //    item => (item ? callback(true) : callback(false)),
            //);
        }

        Handsontable.validators.registerValidator('key-value', keyValueValidator);

        Handsontable.cellTypes.registerCellType('key-value', {
            editor: KeyValueEditor,
            renderer: keyValueRenderer,
            validator: keyValueValidator,
            allowInvalid: false,
        });

    })(Handsontable)