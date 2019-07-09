/**
 * Created by YujieYang.
 * @name:  表格筛选扩展
 * @author: 杨玉杰
 * @version: 1.0
 */
layui.define(['tableFilter', 'tableChild'], function (exports) {

    var tableFilter = layui.tableFilter,
        tableChild = layui.tableChild,
        $ = layui.$,
        HIDE = 'layui-hide';

    // 封装方法
    var mod = {
        render: function (myTable) {
            tableFilter.render(myTable);
            tableChild.render(myTable);

            if (typeof myTable.drag == 'undefined' || myTable.drag) {
                this.drag(myTable);
            }
        }
        , export: function (myTable, curExcel) {
            tableFilter.export(myTable.config, curExcel);
        }
        /**
         * 左右拖拽调整列顺序、向上拖隐藏列
         * @param myTable
         */
        , drag: function (myTable) {
            var _this = this,
                $table = $(myTable.elem),
                $tableHead = $table.next().children('.layui-table-box').children('.layui-table-header').children('table'),
                $tableBody = $table.next().children('.layui-table-box').children('.layui-table-body').children('table'),
                $totalTable = $table.next().children('.layui-table-total').children('table'),
                columns = [].concat.apply([], myTable.cols),
                tableId = $table.attr('id'),
                isDraging = false, isStart = false,
                isModel = myTable.elem.attr('model') !== undefined, hadMoved = false;

            var fieldMap = {};
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].field) {
                    fieldMap[columns[i]['field']] = columns[i]
                } else if (columns[i].type==='numbers') {
                    fieldMap[i] = columns[i]
                }
            }

            if (!$tableHead.attr('drag')) {
                $tableHead.attr('drag', true);
                $tableHead.find('th').each(function () {
                    var $this = $(this);
                    if (!fieldMap[$this.data('field')] || fieldMap[$this.data('field')].fixed) {
                        return true;
                    }
                    // 绑定鼠标按下事件
                    $(this).find('span:first')
                        .css('cursor', 'move')
                        .on('mousedown', function (e) {
                            if (e.button != 0) {
                                return;
                            }
                            e.preventDefault();
                            var $cloneHead = $this.clone().css('visibility', 'hidden'),
                                originLeft = $this.position().left,
                                originTop = $this.offset().top,
                                disX = e.clientX - originLeft,
                                color = $this.parents('tr').css("background-color"),
                                width = $this.width(), moveDistince = 0,
                                $that = $(this);
                            isStart = true;
                            //区分click、drag事件


                            // 阻止文本选中
                            $(document).bind("selectstart", function () {
                                return false;
                            });

                            // 移动事件
                            $('body').on('mousemove', function (e) {
                                if (isStart && $cloneHead) {
                                    if (!isDraging) {
                                        $this.after($cloneHead);
                                        $this.css({
                                            'position': 'absolute',
                                            'z-index': 1,
                                            'border-left': '1px solid #e6e6e6',
                                            'background-color': color,
                                            'width': width + 1
                                        });

                                        $tableBody.find('td[data-field=' + $this.data('field') + ']').each(function (e) {
                                            $(this).after($(this).clone().css('visibility', 'hidden').attr('data-clone', ''));
                                            $(this).css({
                                                'position': 'absolute',
                                                'z-index': 1,
                                                'border-left': '1px solid #e6e6e6',
                                                'background-color': $(this).css('background-color'),
                                                'width': width
                                            });
                                        })
                                        if ($totalTable.length>0) {
                                            $totalTable.find('td[data-field=' + $this.data('field') + ']').each(function (e) {
                                                $(this).after($(this).clone().css('visibility', 'hidden').attr('data-clone', ''));
                                                $(this).css({
                                                    'position': 'absolute',
                                                    'z-index': 1,
                                                    'border-left': '1px solid #e6e6e6',
                                                    'background-color': $(this).css('background-color'),
                                                    'width': width
                                                });
                                            })
                                        }
                                    }
                                    isDraging = true;
                                    var left = e.clientX - disX,
                                        leftMove = $cloneHead.position().left - left > $cloneHead.prev().prev().width() / 2.0,
                                        rightMove = left - $cloneHead.position().left > $cloneHead.next().width() / 2.0;
                                    moveDistince = Math.abs($cloneHead.position().left - left); //记录移动距离
                                    if ($cloneHead.position().left - left > 0
                                        ? myTable.cols[$cloneHead.prev().prev().data('key').split('-')[1]][$cloneHead.prev().prev().data('key').split('-')[2]].fixed ||['checkbox','radio'].indexOf(myTable.cols[$cloneHead.prev().prev().data('key').split('-')[1]][$cloneHead.prev().prev().data('key').split('-')[2]].type)!==-1
                                        : myTable.cols[$cloneHead.prev().prev().data('key').split('-')[1]][$cloneHead.next().data('key').split('-')[2]].fixed || ['checkbox','radio'].indexOf(myTable.cols[$cloneHead.prev().prev().data('key').split('-')[1]][$cloneHead.next().data('key').split('-')[2]].type)!==-1) {
                                        $this.css('left',$cloneHead.position().left);
                                        $tableBody.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {
                                            $(this).prev().css('left', $cloneHead.position().left);
                                        })
                                        if ($totalTable.length>0) {
                                            $totalTable.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {
                                                $(this).prev().css('left', $cloneHead.position().left);
                                            })
                                        }
                                        return;
                                    }
                                    $this.css('left', left);
                                    if (leftMove) {
                                        if ($cloneHead.prev().prev().length != 0) {
                                            $cloneHead.after($cloneHead.prev().prev());

                                            // 更新隐藏列顺序
                                            $('#soul-columns' + tableId + '>li[data-value=' + $this.data('field') + ']').after($('#soul-columns' + tableId + '>li[data-value=' + $this.data('field') + ']').prev())

                                            // 更新配置信息
                                            var x, y;//找到当前是第几个
                                            for (var i = 0; i < myTable.cols.length; i++) {
                                                for (var j = 0; j < myTable.cols[i].length; j++) {
                                                    if (myTable.cols[i][j].key == $this.data('key').substring(2)) {
                                                        x = i;
                                                        y = j;
                                                        break;
                                                    }
                                                }
                                                if (typeof x != 'undefined' && typeof y != 'undefined') {
                                                    break;
                                                }
                                            }
                                            var tempCols = myTable.cols[x][y - 1];
                                            myTable.cols[x][y - 1] = myTable.cols[x][y];
                                            myTable.cols[x][y] = tempCols;

                                            //当移动过了
                                            hadMoved = true;
                                        }
                                    } else if (!$cloneHead.next().hasClass('layui-table-patch') && rightMove) {
                                        if ($cloneHead.next().length != 0) {
                                            $cloneHead.prev().before($cloneHead.next());

                                            // 更新隐藏列顺序
                                            $('#soul-columns' + tableId + '>li[data-value=' + $this.data('field') + ']').before($('#soul-columns' + tableId + '>li[data-value=' + $this.data('field') + ']').next())

                                            // 更新配置信息
                                            var x, y;
                                            for (var i = 0; i < myTable.cols.length; i++) {
                                                for (var j = 0; j < myTable.cols[i].length; j++) {
                                                    if (myTable.cols[i][j].key == $this.data('key').substring(2)) {
                                                        x = i;
                                                        y = j;
                                                        break;
                                                    }
                                                }
                                                if (typeof x != 'undefined' && typeof y != 'undefined') {
                                                    break;
                                                }
                                            }
                                            var tempCols = myTable.cols[x][y + 1];
                                            myTable.cols[x][y + 1] = myTable.cols[x][y];
                                            myTable.cols[x][y] = tempCols;
                                            
                                            //当移动过了
                                            hadMoved = true;
                                        }
                                    }

                                    $tableBody.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {
                                        $(this).prev().css('left', left);

                                        if (leftMove) {
                                            if ($(this).prev().prev().length != 0) {
                                                $(this).after($(this).prev().prev());
                                            }
                                        } else if (rightMove) {
                                            if ($(this).next().length != 0) {
                                                $(this).prev().before($(this).next());
                                            }
                                        }
                                    })
                                    if ($totalTable.length>0) {
                                        $totalTable.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {
                                            $(this).prev().css('left', left);

                                            if (leftMove) {
                                                if ($(this).prev().prev().length != 0) {
                                                    $(this).after($(this).prev().prev());
                                                }
                                            } else if (rightMove) {
                                                if ($(this).next().length != 0) {
                                                    $(this).prev().before($(this).next());
                                                }
                                            }
                                        })
                                    }

                                    /* 拖动隐藏列 */
                                    if (e.clientY - originTop < -15) {
                                        if ($('#column-remove').length == 0) {
                                            $('body').append('<i id="column-remove" class="layui-red layui-icon layui-icon-delete"></i>')
                                        }
                                        $('#column-remove').css({
                                            top: e.clientY - $('#column-remove').height() / 2,
                                            left: e.clientX - $('#column-remove').width() / 2,
                                            'font-size': (originTop - e.clientY) + 'px'
                                        })
                                        $('#column-remove').show();
                                    } else {
                                        $('#column-remove').hide();
                                    }
                                }
                            }).on('mouseup', function () {
                                if (isModel && hadMoved) {
                                    hadMoved = false;
                                    //如果是模块的话，在移动位置后将向后台发送请求
                                    var sortData = [];
                                        myTable.cols[0].forEach((elm, index) => {
                                        if (elm.field) {
                                            sortData .push(elm.field)
                                        }
                                    });
                                    var tableId = $table.attr('id');
                                    abp.services.app.moduleInfo.setColumnSort(tableId, sortData).done(res => console.log('排序成功'))
                                    console.log(myTable, sortData);
                                }
                        


                                $(document).unbind("selectstart");
                                $('body').off('mousemove').off('mouseup')
                                if (isStart && $cloneHead) {
                                    isStart = false;
                                    if (isDraging) {
                                        $that.on('click', function (e) {
                                            e.stopPropagation();
                                        });
                                        isDraging = false;
                                        $this.css({
                                            'position': 'relative',
                                            'z-index': 'inherit',
                                            'left': 'inherit',
                                            'border-left': 'inherit'
                                            //由于table都只有右侧边框，拉出来是要添加左侧,现在变回去
                                        });
                                        $this.next().remove();
                                        $tableBody.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {//[data-clone]是隐藏的那个，把它的上一个同级元素设置成原先的样式，再把自身删掉
                                            $(this).prev().css({
                                                'position': 'relative',
                                                'z-index': 'inherit',
                                                'left': 'inherit',
                                                'border-left': 'inherit'
                                            });
                                            $(this).remove();
                                        });
                                        if ($totalTable.length>0) {
                                            $totalTable.find('td[data-field=' + $this.data('field') + '][data-clone]').each(function (e) {
                                                $(this).prev().css({
                                                    'position': 'relative',
                                                    'z-index': 'inherit',
                                                    'left': 'inherit',
                                                    'border-left': 'inherit'
                                                });
                                                $(this).remove();
                                            });
                                        }
                                        $cloneHead = null;
                                    } else {
                                        $that.unbind('click');
                                    }
                                    if ($('#column-remove').is(':visible')) {
                                        if (isModel) {
                                            //在模块情况下，当鼠标放开，任然是隐藏，就将调用后台接口将模块中设置也改为隐藏
                                            (function () {
                                                var tableId = $table.attr('id');
                                                var columnKey = $this.data('field');
                                                //var columnKey = $this.data('field').replace(/^([a-z])/g, (L) => L.toUpperCase());
                                                abp.services.app.moduleInfo.setColumnVisible(tableId, columnKey, false).done(res => console.log('隐藏成功'))
                                            })()
                                        }

                                        $this.data('field');
                                        $table.next().children('.layui-table-box').children('.layui-table-header').find('thead>tr>th[data-field=' + $this.data('field') + ']').addClass(HIDE);
                                        $table.next().children('.layui-table-box').children('.layui-table-body').find('tbody>tr>td[data-field=' + $this.data('field') + ']').addClass(HIDE);
                                        $table.next().children('.layui-table-total').find('tbody>tr>td[data-field=' + $this.data('field') + ']').addClass(HIDE);
                                        if ($this.data('fixed')) {
                                            $table.next().children('.layui-table-box').children('.layui-table-fixed-' + $this.data('fixed').substr(0, 1)).find('[data-field=' + $this.data('field') + ']').addClass(HIDE);
                                        }
                                        // 同步配置
                                        for (var i = 0; i < columns.length; i++) {
                                            if (columns[i].field && columns[i].field == $this.data('field')) {
                                                columns[i]['hide'] = true;
                                            }
                                        }
                                        // 更新下拉隐藏
                                        $('#soul-columns' + tableId).find('li[data-value="' + $this.data('field') + '"]>input').prop('checked', false);
                                    }
                                    $('#column-remove').hide();
                                }
                            })
                        });
                })
            }
        },

    }

    // 输出
    exports('soulTable', mod);
});