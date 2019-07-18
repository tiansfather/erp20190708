
var config = {
    layuiBase: '/assets/layuiadmin/',
    layuiIndex: 'lib/index',
    layuiExtends: {
        authtree: 'lib/extend/authtree',
        ztree: 'lib/extend/ztree/ztree',
        droptree: 'lib/extend/droptree',
		formSelects: '../lib/extend/formselects/formSelects-v4',
		tableSelect: '../lib/extend/tableselect/tableSelect',
        treeGrid: '../lib/extend/treegrid/treegrid',
        verifyInit: '../lib/extend/verifyInit/verifyInit',
        multiSelect: '../lib/extend/multiSelect/multiSelect',
        suggest: '../lib/extend/suggest/suggest',
        soulTable:'../lib/extend/soulTable/soulTable'
    },
    layuiModules: ['index', 'table', 'layer', 'form', 'element', 'laydate', 'tree', 'upload', 'colorpicker', 'formSelects', 'tableSelect', 'verifyInit', 'multiSelect', 'suggest','soulTable'],
    //页面layui加载完后调用
    ready: function () {
        console.log("onready not implemented");
    },
    //页面layui加载完遍历调用
    readyFuncs:[],
    //table加载完后调用
    onTableDone: function (res, curr, count) {
        //$(window).resize()
        config.onTableDoneExport(res, curr, count);
        console.log("onTableDone not implemented");
    },
    onTableDoneExport: function (res, curr, count) {

    },
    tableRowdone: function (obj) {

    },
    tableCheckBoxdone: function (obj) {
    },
    //检索后调用
    reloadTable: function () {
        console.log("reloadTable not implemented");
    },
    refresh: function () {
        console.log("refresh not implemented");
    },
    showSearchForm: function (moduleKey) {
        //展示检索窗体
        var url = "/ModuleData/Search?moduleKey=" + moduleKey;
        layer.open({
            type: 2,
            title: L('检索'),
            closeBtn: 1,
            shade: 0.1,
            shadeClose: true,
            area: ['600px', '100%'],
            offset: 'l', //左侧弹出
            anim: 3,
            content: [url], //iframe的url，no代表不显示滚动条
            end: function () { //此处用于演示

            }
        });
    },
    showRelativeModuleForm: function (option) {
        var moduleKey = option.moduleKey,
            columnKey = option.columnKey,
            maxReferenceNumber = option.maxReferenceNumber || 1;
        //展示关联引用窗体
        var url = "/ModuleData/RelativeSelect?moduleKey=" + moduleKey + "&columnKey=" + columnKey + "&maxReferenceNumber" + maxReferenceNumber;
        window.referenceLayerIndex = layer.open({
            type: 2,
            title: L('关联查询'),
            closeBtn: 1,
            shade: 0.1,
            shadeClose: true,
            area: ['80%', '100%'],
            offset: 'r', //右侧弹出
            anim: 0,
            content: [url], //iframe的url，no代表不显示滚动条
            end: function () { //此处用于演示

            }
        });
    }
};
//给页面layui加载完遍历调用的数组进行监听
(function () {
    const arrayProto = Array.prototype
    const arrayMethods = Object.create(arrayProto)
    Object.defineProperty(arrayMethods, 'push', {
        value: function mutator() {
            const original = arrayProto['push']
            let args = Array.from(arguments)
            //缓存原生方法，之后调用
            if (func.typeof(args[0]) == 'function') {
                //塞进去
                original.apply(this, args)
            } else {
                console.warn('往这个数组里传非函数类型的，将无效！');
                return
            }
            //当已经加载过layui了，你再push进来我就直接帮你运行
            if (config.readyFuncs.hadRun) {
                args[0]();
                console.warn('layui的use已完成，这个数组已经遍历运行过了，直接执行吧！');
            }
        }

    })
    config.readyFuncs.__proto__ = arrayMethods;
}) ()
$(function () {
    //监听全局，按住alt再按z就会输入φ
    document.addEventListener('keyup', logKey);

    function logKey(e) {
        var tagName = e.target.tagName.toLowerCase();
        if (tagName != 'input' && tagName != 'textarea') { return; }
        var key = e.key?e.key.toLowerCase():'';
        if (key == 'z' && e.altKey) {
            $(e.target).val($(e.target).val() + 'φ')
        }
    }
    //全局事件
    //tip事件
    $("body").on("mouseenter", "*[tips]", function () {
        if (layer) {
            var e = $(this);
            if (!e.attr("tips")) { return; }
            var i = e.attr("tips"),
                t = e.attr("lay-offset"),
                n = e.attr("lay-direction"),
                b = e.attr("lay-background"),
                a = e.attr("lay-area"),
                s = layer.tips(i, this, {
                    tips: [n || 1, b || "#000"],
                    time: -1,
                    anim: -1,
                    maxWidth: a || '',
                    success: function (e, a) {
                        t && e.css("margin-left", t + "px");
                    }
                });
            e.data("index", s)
        }
    }).on("mouseleave", "*[tips]", function () {
        layer&&layer.close($(this).data("index"))
    });
    $("body").on("mouseenter", "*[formtips]", function () {
        var e = $(this);
        if (!e.attr("formtips")) { return; }
        var i = e.attr("formtips"),
            b = e.attr("lay-background"),
            t = e.attr("lay-offset"),
            n = e.attr("lay-direction"),
            s = layer.tips(i, this, {
                tips: [n || 1, b||"#FFB800"],
                time: -1,
                success: function (e, a) {
                    t && e.css("margin-left", t + "px");
                    e.css("width", "auto");
                    e.css("max-width", "600px");
                }
            });
        e.data("index", s);
    }).on("mouseleave", "*[formtips]", function () {
        layer.close($(this).data("index"));
		});
    //time tips
    $("body").on("mouseenter", "*[timetips]", function () {
        var that = this
        var e = $(this);
        var s;
       var tid = setTimeout(function () {
            showImg();

        }, 500);
       e.on("mouseleave", function () {
          
           layer && layer.close(s);
           //console.log(e.data("index"));
           //debugger
            clearTimeout(tid);
        })
        function showImg() {
            if (layer) {
                if (!e.attr("timetips")) { return; }
                var i = e.attr("timetips"),
                    t = e.attr("lay-offset"),
                    n = e.attr("lay-direction"),
                    b = e.attr("lay-background"),
                    a = e.attr("lay-area");
                s = layer.tips(i, that, {
                        tips: [n || 1, b || "#000"],
                        time: -1,
                        anim: -1,
                        maxWidth: a || '',
                        success: function (e, a) {
                            t && e.css("margin-left", t + "px");
                        }
                    });
                //e.data("index", s)
            }

        }
    }) ;
	//通用layer弹出
	$("body").on("click", ".laydialog", function () {
        var obj = $(this);
        var url = obj.attr('url');
        var type;
        var content;
        if (url.indexOf('http') == 0 || url.indexOf('/') == 0) {
            type = 2;
            content = obj.attr("url");
        } else {
            type = 1;
            content = $(url);
            console.log(content)
        }
		layer.open({
			type: type,
            shade: 0.8,
            shadeClose: true,
			title: obj.attr("title")||obj.text()
			, area: [obj.attr("width")||'80%',obj.attr("height")||'80%']
			, content:content 
		});
	});
    //布局初始化,如div自适应
    func.initUI();

    //图片缩略放大事件 2018/5/24 13:55 lijianbo
    $("body").on('click', "img.thumb", function () {
        var img = $(this);
        var fileid = img.attr("FileID");
        var arr = /(.*?)(?:[\?&]w=\d+)|(.*)/.exec(img.attr('src'));
        var src = arr[1] || arr[2];
        layuiExt.fLayerImg('', src);
        //top.layer.open({
        //    title: '图片显示'
        //    , skin: 'picturesshow'
        //    , area: ['80%', '80%']
        //    , content: '<img style=\'width:100%,height:100%\' src=\'/File/Thumb?fileid=' + fileid + '\' >'
        //});
    });

    //清空table的检索缓存
    $("table[module]").each(function () {
        var moduleKey = $(this).attr("module");
        layui.sessionData(moduleKey, null);
    })
})


var func = {
    typeof:function(data){
        if(arguments.length === 0) return new Error('type方法未传参');
        var typeStr = Object.prototype.toString.call(data);
        return typeStr.match(/\[object (.*?)\]/)[1].toLowerCase();      
    },
    array: {
        //扩展array的去重方法
        distinct:function () {
            return this.reduce(function (new_array, old_array_value) {
                if (new_array.indexOf(old_array_value) == -1) new_array.push(old_array_value);
                return new_array; //最终返回的是 prev value 也就是recorder
            }, []);
        }
    },
    splitDrag: function (_ul, _table, options = {}) {
        //左侧tree，右侧table的布局，
        $.extend(options, {
            left: 10,
            right: 500,
        })
        var tParent = _table.parent();
        tParent = !options.wrap ? tParent : typeof options.wrap == 'string' ? $(options.wrap) : options.wrap;
        tParent.append('<div id="r-treedrag"><div id="r-treedrag-ul">\n </div>\n <div id="r-treedrag-width"></div>\n <div id="r-treedrag-table">\n <div class="r-treedrag-table_wrap">\n </div>\n  </div>\n </div>')
        $('#r-treedrag-ul').append(_ul);
        $(".r-treedrag-table_wrap").append(_table);
        //tParent.remove();
        //宽度读取改变应在加载前完成
        var costTWidth = layui.data('manyChangeWidth')[options.key];
        if (costTWidth) {//如果之前写入过了，就读取这个值
            $('#r-treedrag-ul').width(costTWidth);
        } else {//如果没写入过，则设置初始值
            layui.data('manyChangeWidth', {
                key: options.key
                , value: '250'
            });
        }

        (function () {
            var oDiv = document.getElementById('r-treedrag-width');
            var disX = 0;

            oDiv.onmousedown = function (ev) {
                var oEvent = ev || event;
                disX = oEvent.clientX - oDiv.offsetLeft;
                var rowWrapPadding = $('.r-treedrag').outerWidth(true) - $('.r-treedrag').width() / 2;//border+padding+margin合宽
                console.log(rowWrapPadding)
                document.onmousemove = function (ev) {
                    var oEvent = ev || event;
                    var l = oEvent.clientX - disX - rowWrapPadding;//当前鼠标位置-padding宽度
                    //console.log(l)
                    if (l < options.left) {
                        l = options.left;
                    }
                    else if (l > options.right) {
                        l = options.right;
                    }
                    $('#r-treedrag-ul').width(l);

                    layui.data('manyChangeWidth', {
                        key: options.key
                        , value: l
                    });
                };

                document.onmouseup = function () {
                    document.onmousemove = null;
                    document.onmouseup = null;
                };

                return false;
            };

        })()
    },
    getFeeTypeByNum:function(num) {
        var obj = { 0: '承包', 1: '按时间', 2: '按平方', 3: '按长度', 4: '按重量', 5: '按数量' };
        return obj[num] || '';
    },
    makeQueryArray: function () {
        console.log('处理array类型的高级查询')
    },
    makeQuery:function (SearchData) {
        var addsql = '';
        for (var i = 0; i < SearchData.length; i++) {
            SearchData[i].key=SearchData[i].key.replace('，', ',');
            SearchData[i].data=SearchData[i].data.replace('，', ',');
            var keys = SearchData[i].key.split(',');
            var model = keys[0].substring(0, keys[0].lastIndexOf('.'));
            function handleMulti(arr, type) {
                var list = arr.split(',');
                var str = { Like: ['.Contains("', '")'], other: [' = "', '"'] }
                for (var j = 0; j < list.length; j++) {
                    if (j == 0) {
                        addsql = addsql + ' (' + key + str[type][0] + list[j] + str[type][1];
                    }
                    else {
                        addsql = addsql + ' or ' + key + str[type][0] + list[j] + str[type][1];
                    }
                }
                addsql = addsql + ')';
            }

            addsql = addsql + ' and (';
            for (var n = 0; n < keys.length; n++) {
                //构建查询关键字
                var key = "";
                if (n == 0) {
                    key = keys[n];
                }
                else {
                    if (model != "") {
                        key = model + '.' + keys[n];
                    }
                    else {
                        key = keys[n];
                    }

                }
                if (SearchData[i].canAnd) {
                    if (SearchData[i].type == "Date") {
                        SearchData[i].data.split('|');

                        var t1 = SearchData[i].data.split('|')[0];
                        var t2 = SearchData[i].data.split('|')[1];

                        addsql = addsql + ' ' + key + '>="' + t1 + '" and ' + key + '<="' + t2 + '" ';
                    } else {
                        console.log('canAnd为true，且不为时间类型')
                    }
                }
                else {
                    if (SearchData[i].type == "Array" || SearchData[i].type == "Check") {
                        console.log(func.makeQueryArray(SearchData[i]), addsql + func.makeQueryArray(SearchData[i]))
                        addsql = addsql + func.makeQueryArray(SearchData[i])
                    }else if (SearchData[i].type == "Like") {
                        if (n != 0) {
                            addsql += ' or ';
                        }
                        handleMulti(SearchData[i].data, 'Like')
                        //if (n == 0) {
                        //    addsql = addsql + ' ' + key + '.Contains("' + SearchData[i].data + '")';
                        //}
                        //else {
                        //    addsql = addsql + ' or ' + key + '.Contains("' + SearchData[i].data + '")';
                        //}
                    }
                    else {
                        if (n == 0) {
                            var list = SearchData[i].data.split(',');
                            for (var j = 0; j < list.length; j++) {
                                if (j == 0) {
                                    addsql = addsql + ' (' + key + ' = "' + list[j] + '"';
                                }
                                else {
                                    addsql = addsql + ' or ' + key + ' = "' + list[j] + '"';
                                }
                            }
                            addsql = addsql + ')';
                        }
                        else {
                            var list = SearchData[i].data.split(',');
                            for (var j = 0; j < list.length; j++) {
                                if (j == 0) {
                                    addsql = addsql + ' or (' + key + ' = "' + list[j] + '"';
                                }
                                else {
                                    addsql = addsql + ' or ' + key + ' = "' + list[j] + '"';
                                }
                            }
                            addsql = addsql + ')';
                        }
                    }
                }
            }
            addsql = addsql + ')';
        }
        return addsql;
    },
    getHandleDate: function (op, date) {
        //op={sp:'分隔符',type:String['上月','本月'...]},func.getHandleDate({type:'本月'},new Date('2019-05-01'))
        var defaultOp = { sp: ' | ' };
        $.extend(defaultOp, op);
        op = defaultOp;
        op.type = op.type.toLowerCase();
        var D = date || new Date(),
            y = D.getFullYear(),
            m = D.getMonth(),
            d = D.getDate(),
            WeekDay = D.getDay();
        //m = m < 10 ? "0" + m : m;
        //d = d < 10 ? "0" + d : d;
        var startD, endD;
        function getQuarterStartMonth(nowMonth) {
            var quarterStartMonth = 0;
            if (2 < nowMonth && nowMonth < 6) {
                quarterStartMonth = 3;
            }
            if (5 < nowMonth && nowMonth < 9) {
                quarterStartMonth = 6;
            }
            if (nowMonth > 8) {
                quarterStartMonth = 9;
            }
            return quarterStartMonth; 
        }
        function getMonthLastDay(date1 = D) {
            var y = date1.getFullYear(),
                m = date1.getMonth() + 1,
                day = new Date(y, m, 0);
            return day   //获取月份的最后一天
        }
        function formatDate(date2) {
            var myyear = date2.getFullYear();
            var mymonth = date2.getMonth() + 1;
            var myweekday = date2.getDate();

            if (mymonth < 10) {
                mymonth = "0" + mymonth;
            }
            if (myweekday < 10) {
                myweekday = "0" + myweekday;
            }
            return (myyear + "-" + mymonth + "-" + myweekday);
        }
        switch (op.type) {
            case '本月':
                startD = formatDate(new Date(y, m, 1));
                endD = formatDate(getMonthLastDay(D));
                break;
            case '上月':
                D.setMonth(D.getMonth() - 1);
                y = D.getFullYear();
                m = D.getMonth();
                startD = formatDate(new Date(y, m, 1));
                endD = formatDate(getMonthLastDay(D));
                break;
            case '本周':
                startD = formatDate(new Date(y, m, d - WeekDay));
                endD = formatDate(new Date(y, m, d - WeekDay + 6));
                //endD = y + '-' + m + '-' + d;
                //D =new Date( D.getTime() - 7 * 24 * 3600 * 1000);
                //y = D.getFullYear();
                //m = D.getMonth() + 1;
                //d = D.getDate();
                //startD = y + '-' + m + '-' + d;
                break;
            case '上周':
                startD = formatDate(new Date(y, m, d - WeekDay - 7));
                endD = formatDate(new Date(y, m, d - WeekDay + 6 - 7));
                break;
            case '本季':
                m = getQuarterStartMonth(m);
                startD = formatDate(new Date(y,m, 1));
                D.setMonth(m + 2);
                endD = formatDate(getMonthLastDay(D));  
                break;
            case '上季':
                D.setFullYear(y, m - 3, 1);
                m = getQuarterStartMonth(D.getMonth());
                startD = formatDate(new Date(D.getFullYear(), m, 1));
                D.setMonth(m + 2);
                endD = formatDate(getMonthLastDay(D));  
                break;
            case '本年':
                startD = formatDate(new Date(y, 0, 1));
                D =new Date( D.setMonth(11));
                endD = formatDate(getMonthLastDay(D));
                break;
            case '上年':
                startD = formatDate(new Date(y - 1, 0, 1));
                D = new Date(D.setFullYear(y - 1, 11));
                endD = formatDate(getMonthLastDay(D));
                break;
            case '今天':
                startD = endD = formatDate(D);
                break;
            case '昨天':
                startD = endD = formatDate(new Date(D.getTime() - 24 * 60 * 60 * 1000));
                break;
            case '近14天':
                endD = formatDate(D);
                startD = formatDate(new Date(D.getTime() - 24 * 60 * 60 * 1000 * 14));
                break;
            case '近7天':
                endD = formatDate(D);
                startD = formatDate(new Date(D.getTime() - 24 * 60 * 60 * 1000 * 7));
                break;
            case '近3天':
                endD = formatDate(D);
                startD = formatDate(new Date(D.getTime() - 24 * 60 * 60 * 1000 * 3));
                break;
            default:
                startD = endD = formatDate(D);
        }
        return startD + op.sp + endD;
    },
    formatDate : function (now, op) {
        //type取值范围
        //var types = { 'S':8, 'M':5, 'H':2, 'Day':true,'Mounth'}
        var defaultOp = { type: 'S', split: '-' },
            op = $.extend(defaultOp, op),
            split = op.split,
            y = now.getFullYear(),
            m = now.getMonth() + 1,
            d = now.getDate();
        var ymd = y + split + (m < 10 ? "0" + m : m) + split + (d < 10 ? "0" + d : d)

        var rData;
        switch (op.type) {
            case 'S':
                rData = ymd + " " + now.toTimeString().substr(0, 8);
                break;
            case 'M':
                rData = ymd + " " + now.toTimeString().substr(0, 5);
                break;
            case 'H':
                rData = ymd + " " + now.toTimeString().substr(0, 2);
                break;
            case 'Day':
                rData = ymd;
                break;
            case 'Mounth':
                rData = (m < 10 ? "0" + m : m) + split + (d < 10 ? "0" + d : d);
                break;
            default:
                rData = ymd + " " + now.toTimeString().substr(0, 8);
        }
        return rData
    },
    getProgressColor: function (ProgressType) {
        var obj = { 0: 'layui-bg-blue', 1: 'my-bg-green', 2: 'layui-bg-blue', 3: 'layui-bg-orange' };
        return obj[ProgressType];
    },
    getReportType: function (data,toNum='tonum') {
        var obj = { "到料": 1, "上机": 2, "加工": 3, "暂停": 4, "下机": 5, "重新开始": 6, }
        toNum = toNum.toLowerCase();
        if (toNum=='tonum') {
            return obj[data]
        } else {
            for (var i in obj) {
                if (obj[i] == data) {
                    return i
                }
            }
        }
    },

    getProcessTaskStatus:function (key, key2title = false) {//关键字，是不是给中文要status,默认是
        var obj = { 1: '待上机', 2: '已到料', 3: '加工中', 4: '已完成', 5: '暂停中', '-1': '已取消' }
        if (key2title) {
            return obj[key]
        }
        else {
            for (var i in obj) {
                if (obj[i] == key) {
                    return i
                }
            }
        }
    },
    getProcessTaskStatusColor: function (status) {
        var typeBack;
        switch (status) {
            case 0:
                typeBack = { color: "#dbdada", name:'未开单'};
                break;
            case 1:
                typeBack = { color: "#fa763b", name: '未上机' };
                break;
            case 2:
                typeBack = { color: "#00fdff", name: '已到料' };
                break;
            case 3:
                typeBack = { color: "#2699f6", name: '已上机' };
                break;
            case 4:
                typeBack = { color: "#75d239", name: '已完成' };
                break;
            case 5:
                typeBack = { color: "#fcfc00", name: '暂停中' };
                break;
            case 6:
                typeBack = { color: "green", name: '已对账' };
                break;
            default:
                typeBack = { color: "#bdbdbd", name: '其他' };
        }
        
        //0//未开单fa763b 1//未上机，橙色 2//加工点已到料 青色, 3//已上机，蓝色 4//已完成,绿色 5//暂停中,黄色 //其他状态为深灰色
        return typeBack;
    },
    judgeFileType: function (files) {
        var defaultObj = { type: 'default', iconfont:'iconfont icon-m-fileFormat' , color: '#000' }; //默认对象
        if (files instanceof Array) {
            var returnArr = [];
            files.forEach(function (f) {
                returnArr.push(fSwitch(f));
            })
            return returnArr
        } else if (typeof files == 'string') {
            return fSwitch(files);
        }
        function fSwitch(file) {//传进来一个字符串
            //预处理字符串
            if (/./g.test(file)) {//传进来的是带.的，代表是完整链接，要处理一下
                file= file.split('.');
                file = file[file.length - 1];
            }
            var obj;
            var allFiles = {
                excal: [['excal', 'docx', 'doc', 'xls', 'xlsx'], 'icon-excel', '#289be5'],
                zip: [['zip','gzip'], 'icon-yasuobao', '#289be5'],
                pdf: [['pdf'], 'icon-pdf1','#289be5'],
                txt: [['txt'], 'icon-txt','#289be5'],
                video: [['mp4', 'avi', 'WAV', 'rm', 'rmvb', 'mpeg1', 'mpeg2', 'mpeg3', 'mpeg4', 'mov', 'mtv', 'dat', 'wmv', 'avi', '3gp', 'amv', 'dmv', 'flv'], 'icon-filevideo', '#289be5'],
                cad: [['dwg', 'dwt', 'dxf', 'dws'], 'icon-CAD','#289be5'],
                jpg: ["bmp,jpg,png,tif,gif,pcx,tga,exif,fpx,svg,psd,cdr,pcd,dxf,ufo,eps,ai,raw,WMF,webp,jpeg,svg,smp".split(','), '', '#289be5'],//所有类型，图标，颜色
                //['PSD', 'PDD', 'GIF', 'JPEG', 'JPG', 'SVG', 'png', 'BMP', 'CDR']
            };
            for (n in allFiles) {
                allFiles[n][0].forEach(function (m) {
                    if (m.toLowerCase() == file.toLowerCase()) {
                        obj={ type: n, iconfont: 'iconfont '+allFiles[n][1], color: allFiles[n][2]}
                    }
                })
                //if (allFiles[n].indexOf('b') != -1) {
                //    obj = n;
                //};
            }
            if (obj) {
                return obj;
            } else {
                return defaultObj;
            }
        }
    },
	//上传文件绑定
	renderUpload: function (selector, options) {
		var opt = $.extend({
            trigger: 'dblclick',
            uploadType: 'single',
            multiple:false
        }, options);
        //console.log(opt)
		$("body").on(opt.trigger, selector, function () {
			try {
				window.upload = {
					element: this,
					callback: opt.callback || ($(this).attr("callback") ? eval($(this).attr("callback")) : $.noop)
				};
				layer.open({
					type: 2,
					title: "文件上传",
					shadeClose: false,
					shade: 0.8,
                    area: ['500px','400px'],
                    content: '/File/CommonUpload?multiple='+opt.multiple
				});
			} catch(e){
				//console.log(e);
			}
			
		});
		
	},
    //获取模块的js名
    getModuleServiceName: function (name) {
        return name ? (name[0].toLowerCase() + name.substring(1)) : "moduleData";
    },
    //模块按钮事件
    callModuleButtonEvent: function (element) {
        var ev = getEvent();
        if ($.isPlainObject(element)) {
            //不是jq对象,参考partCommon.cshtml的goquote方法
            element.attr=(key) => {
                return element[key]
            }
        }
		var ele = element || $(ev.srcElement || ev.target),
            moduleKey = ele.attr("modulekey"),
            layevent = ele.attr("lay-event"),
            dataid = ele.attr("dataid"),
            confirmmsg = ele.attr("confirmmsg"),
            title=ele.attr("title"),
            buttonname = ele.attr("buttonname"),
            buttonactiontype = ele.attr("buttonactiontype"),
            buttonactionparam = ele.attr("params"),
            buttonactionurl = ele.attr("buttonactionurl"),
            callback = ele.attr("callback"),
            opentop = ele.attr("opentop"),
            fornonerow = ele.attr("fornonerow");

        //提交的数据
        var data = [];
        data = layui.table.checkStatus(moduleKey).data.map(function (o) { return o.id || o.Id; });
        if (dataid) { data = [dataid]; }
		else if (!fornonerow) {
			
            if (data.length === 0) {
                abp.message.info("请先选择记录");
                return false;
            }
		}
		var hash = buttonactionurl.indexOf('#') > 0 ? buttonactionurl.split('#')[1] : '';
		var url = buttonactionurl.split('#')[0] + (buttonactionurl.indexOf("?") > 0 ? "&" : "?") + "modulekey=" + moduleKey + "&data=" + data.join(',') + '#' + hash; //url

        var funcProxyWrapper;//方法包装
        //异步提交方式
        if (buttonactiontype === "Ajax") {
            var funcProxy = eval(buttonactionurl);
            if (!funcProxy) {
                layer.alert("未找到代理方法" + buttonactionurl);
                return false;
            }

			funcProxyWrapper = function () {
				func.runAsync(funcProxy(data).done(function (data) {
                    var result=true;
                    callback && (result = eval(callback)(data));
                    //如果自定义回调返回false,则不执行默认处理
                    if (!result) { return;}
                    parent.layer.msg("提交成功");
					moduleKey && func.reload(moduleKey);
				}))
                //abp.ui.setBusy(
                //    $('body'),
                //    funcProxy(data, {
                //        success: function (data) {
                //            //abp.message.success("提交成功");
                //            parent.layer.msg("提交成功");
                //            callback && eval(callback)(data);
                //            moduleKey && func.reload(moduleKey);
                //        }
                //    })

                //);
            }

        }
        //else if (buttonactiontype === "Func") {
        //    var funcProxy = eval(buttonactionurl);
        //    if (!funcProxy) {
        //        layer.alert("未找到代理方法" + buttonactionurl);
        //        return false;
        //    }
        //    funcProxyWrapper = function () {
        //        funcProxy(data);
        //    }
        //}
        //展示窗体
        else if (buttonactiontype === "Form") {
            var defaultOption = {
                type: 2,
				title: title||buttonname,
				scrollbar: false,
                shadeClose: false,
                shade: 0.8,
                area: ['80%', '80%'],
                content: url,
				btn: ['提交', '关闭'],
				btnAlign: 'l',
                yes: function (index, layero) {
                    //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();

                    var iframeWin = layero.find('iframe')[0].contentWindow;
                    //var iframeWin = window[layero.find('iframe')[0]['name']]; 
                    if (iframeWin.submit) { iframeWin.submit(); return false; }
                },
                btn2: function (index, layero) {
                    var iframeWin = layero.find('iframe')[0].contentWindow;
                    //var iframeWin = window[layero.find('iframe')[0]['name']];
                    if (iframeWin.submit2) { iframeWin.submit2(); return false; }
                },
                btn3: function (index, layero) {
                    var iframeWin = layero.find('iframe')[0].contentWindow;
                    //var iframeWin = window[layero.find('iframe')[0]['name']];
                    if (iframeWin.submit3) { iframeWin.submit3(); return false; }
                }
            };
            var param = buttonactionparam ? $.parseJSON(buttonactionparam) : {};

            funcProxyWrapper = function () {
                (opentop ? top.layer : layer).open($.extend(defaultOption, param));
            }
        }
        //打开Tab
        else if (buttonactiontype === "Tab") {
            funcProxyWrapper = function () {
                top.layui.index.openTabsPage(url, buttonname);
            }
        }

        //提交
        if (confirmmsg) {
            abp.message.confirm(confirmmsg, funcProxyWrapper);
        } else {
            funcProxyWrapper();
        }
    },
    //表格重载
    reload: function (tableid, option) {
        layui.table.reload(tableid, option);
    },
    //异步执行
    runAsync: function (fun) {
        top.abp.ui.setBusy(
            null,
            fun
        );
    },
    //表单初始化
    initForm: function () {
        if (layui.data('session').loginInfo) {
            $.extend(abp.session, JSON.parse(layui.data('session').loginInfo));
        }
        $("div.layui-inline").each(function () {
            var parentNode = $(this).parent();
            var prevNode = $(this).prev();
            if (!parentNode.is(".layui-form-item")) {
                if (prevNode.is(".layui-form-item")) {
                    $(this).appendTo(prevNode);
                } else {
                    $(this).wrap('<div class="layui-form-item"></div>');
                }
            }
        })
        $("table[lay-filter]").each(function () {
            var fillterId = $(this).attr('lay-filter');
            layui.table.on('row(' + fillterId + ')', function (obj) {
                if ($(obj.tr).hasClass('my-table-active')) {
                    $(obj.tr).removeClass('my-table-active');
                } else {
                    $(obj.tr).addClass('my-table-active');
                    $(obj.tr).siblings().removeClass('my-table-active');
                }
                config.tableRowdone(obj);
            });
            layui.table.on('checkbox(' + fillterId + ')', function (obj) {
                if (obj.type == 'all') {
                    if (obj.checked) {
                        $('[lay-filter="' + fillterId + '"]').siblings('.layui-table-view').find('tbody tr').addClass('my-table-active_checked')
                    } else {
                        $('[lay-filter="' + fillterId + '"]').siblings('.layui-table-view').find('tbody tr').removeClass('my-table-active_checked')
                    }
                } else {
                    if (obj.checked) {
                        $(obj.tr).addClass('my-table-active_checked');
                    } else {
                        $(obj.tr).removeClass('my-table-active_checked');
                        $(obj.tr).removeClass('my-table-active');
                    }
                }
                config.tableCheckBoxdone(obj);
            });
        })
    },
    initUI: function ($body) {
        //元素自适应高度
		func.initLayout($body);
		//通用上传绑定;
		func.renderUpload(".uploadinsert");
    },
    //初始化元素自适应
    initLayout: function ($body) {
        var $target = $body || $("body");
        //<div layouth='130'></div>
        $target.find("[layouth]").each(function () {
            var layouth = $(this).attr("layouth");
            var h = top.$(".layui-body").height();
            //var h = $(document).height();
            h = h - parseInt(layouth);
            $(this).css("overflow-y", "auto");
            $(this).css("height", h + "px");
        })
    },
    //构建查询条件
    buildSearchCondition: function (moduleKey) {
        var conditions = layui.sessionData(moduleKey).conditions;
        if (!conditions || conditions == "") {
            return "";
        } else {
            //var conditionStr = "";
            //for (var i = 0; i < conditions.length; i++) {
            //    var condition = conditions[i];
            //    conditionStr += condition.leftBracket +' '+ condition.column.columnKey + ' ' + condition.operator + ' ' + condition.value + ' ' + condition.rightBracket + ' ' + condition.joiner+' ';
            //}
            return JSON.stringify(conditions);
        }
    },
    //查找返回,子页面调用
    bringBack: function (moduleKey, isReturn) {
        var checkStatus = layui.table.checkStatus(moduleKey);
        if (checkStatus.data.length == 0) {
            layer.msg(L('请至少选择一项'), { icon: 5, anim: 6 });
            return false;
        }
        var key = $.getUrlParam("key");
        parent.func.getBringBack(checkStatus.data, key, isReturn);
    },
    //获取返回数据,父页面调用
    getBringBack: function (data, key, isReturn) {
        //调用页面中定义的回调函数
        if (func.bringBackFuncs[key](data)) {
            //成功回调
            if (isReturn) {
                layer.closeAll('iframe');
            }
        }

        console.log(data);
    },
    bringBackFuncs: [],
    referenceDatas: []
};
/*多语种*/
abp.localization.defaultSourceName = "Master";
//修正前台权限判定，当权限定义不存在时返回true
abp.auth.isGranted = function (n) { return abp.auth.grantedPermissions[n] != undefined || !abp.auth.allPermissions[n] }
function L(name) {
    return abp.localization.localize(name);
}

function getEvent() { //同时兼容ie和ff的写法 
    if (document.all) return window.event;
    var func = getEvent.caller;
    while (func !== null && func.arguments) {
        var arg0 = func.arguments[0];
        if (arg0) {
            if ((arg0.constructor === Event || arg0.constructor === MouseEvent)
                || (typeof (arg0) === "object" && arg0.preventDefault && arg0.stopPropagation)) {
                return arg0;
            }
        }
        func = func.caller;
    }
    return null;
}



/*jquery扩展*/
//获取url的参数值
$.extend({
    //生成随机颜色
    randomRGB: function (type = 'rgb') {
        function getRandom() { return parseInt(Math.random() * (255 + 1)) }
        function getRgb() {
            var n = getRandom().toString(16);
            n.length == 1 && (n + '0');
            return n;
        }
        if (type == 'rgb') {
            return '#' + getRgb() + getRgb() + getRgb();
        }
        return `rgb(${getRandom()},${getRandom()},${getRandom()})`
        
    },
    //计算重量
    calWeight: function (spec, density, decimal) {
        decimal = decimal || 100;
        var re = /^([\d\.]+)\s*[\*Xx]\s*([\d\.]+)\s*[\*Xx]\s*([\d\.]+)$/ig;
        var volumn;
        if (re.test(spec)) {
            var spec_margin = "0*0*0";

            var a = spec.replace(re, "$1");
            var b = spec.replace(re, "$2");
            var c = spec.replace(re, "$3");
            var a_m = spec_margin.replace(re, "$1");
            var b_m = spec_margin.replace(re, "$2");
            var c_m = spec_margin.replace(re, "$3");
            var a_real = parseFloat(a) + parseFloat(a_m);
            var b_real = parseFloat(b) + parseFloat(b_m);
            var c_real = parseFloat(c) + parseFloat(c_m);
            return Math.round(a_real * b_real * c_real * density / 1000000 * decimal) / decimal;
        } else {

            var re2 = /^[Φφ]([\d\.]+)\s*[\*Xx]\s*([\d\.]+)$/ig;
            var d = spec.replace(re2, "$1");
            var l = spec.replace(re2, "$2");
            return Math.round(d * d / 4 * 3.14 * l * density / 1000000 * decimal) / decimal;
        }
    },
    num2CN: function (n) {
        if(!/^(0|[1-9]\d*)(\.\d+)?$/.test(n)) return "数据非法";
        var unit = "京亿万仟佰拾兆万仟佰拾亿仟佰拾万仟佰拾元角分", str = "";
        n += "00";
        var p = n.indexOf('.');
        if (p >= 0)
            n = n.substring(0, p) + n.substr(p + 1, 2);
        unit = unit.substr(unit.length - n.length);
        for (var i = 0; i < n.length; i++) str += '零壹贰叁肆伍陆柒捌玖'.charAt(n.charAt(i)) + unit.charAt(i);
        return str.replace(/零(仟|佰|拾|角)/g, "零").replace(/(零)+/g, "零").replace(/零(兆|万|亿|元)/g, "$1").replace(/(兆|亿)万/g, "$1").replace(/(京|兆)亿/g, "$1").replace(/(京)兆/g, "$1").replace(/(京|兆|亿|仟|佰|拾)(万?)(.)仟/g, "$1$2零$3仟").replace(/^元零?|零分/g, "").replace(/(元|角)$/g, "$1整");
    },
    //把name/value的数组转为obj对象
    arrayToObj:function (array) {
        var result = {};
        for (var i = 0; i < array.length; i++) {
            var field = array[i];
            if (field.name in result) {
                result[field.name] += ',' + field.value;
            } else {
                result[field.name] = field.value;
            }
        }
        return result;
    },
    getUrlParam : function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r !== null) return decodeURIComponent(r[2]); return null;
    },
    newid: function () {
        return new Date().getTime() + '' + Math.round(Math.random() * 1000);
    }
})
function getCheckboxValue(name) {
    var data = [];
    $(":checked[name='" + name + "']").each(function () {
        data.push($(this).val());
    })
    return data;
}

var layuiExt = {
    CreatTableCondi:function (value,andOr, leftBracket, rightBracket, comparer) {
        andOr = this.andOr;
        leftBracket = this.leftBracket;
        rightBracket = this.rightBracket;
        comparer = this.comparer;
        value = this.value;
    },
    reRender: function (tableId, parentOptions) {
        //在表格加载完成后执行的函数
        if (parentOptions) {
            var donefunction = 'done' in parentOptions ? parentOptions.done : function () { };

        }
        var option = window[tableId][0];
        var arr = window[tableId][1];
        window.filterArr = [];
        window.gudingArr = [];
        arr[0].forEach(function (n, index) {
            if (n.filterField) {
                filterArr.push(index);
            }
            if (n.gudinglie != false) {
                gudingArr.push(index);
            }
            //delete n.gudinglie
            //delete n.filterField;
            //return n
        });
        var Intermediate = option.done;
        option.done = function (res, curr, count) {
            Intermediate(res,curr,count);
            if (typeof donefunction == 'function') {
                donefunction();
            }
            //cols的筛选列显示
            if (option.defaultToolbar) {
                layuiExt.fCol(tableId)
            }
            $(window).resize()
        }
        option = $.extend({}, option, { cols: arr });
        console.log(option)
        var table = layui.table;
        table.render(option);
        
        layuiExt.fThEvent();
        if (option.defaultToolbar&&option.defaultToolbar.length>0) {
            //total合计行，在有垂直滚动条时，有bug
            setTimeout(ftotalDebug, 1000);
            function ftotalDebug() {
                //console.log($('.layui-table-total tbody>tr').html())
                $('.layui-table-total tbody>tr').append('.<th class="layui-table-patch"><div class="layui-table-cell" style="width: 17px;"></div></th>');
            }
            
        }
    },
    //给不同的th都加图标
    fThEvent: function () {
        var laytpl = layui.laytpl;
        var data;
        var aShaiXuanDone = filterObject.aShaiXuanDone;
        function fAppendHtml(evString,arr,html) {
            $(evString).each(function (index) {
                var $this = $(this);
                if (arr.indexOf(index)!=-1) {
                    $this.append(html)
                }
            })
        }
        function fLaytpl(getTpl, data, evString, arr) {
            laytpl(getTpl).render(data, function (html) {
                fAppendHtml(evString, arr, html);
            });
        }
        var getTplGuding =document.getElementById('colsThGuding').innerHTML;
        var getTplShaixuan = document.getElementById('colsThShaixuan').innerHTML;
        data = {};
        fLaytpl(getTplGuding, data,'.layui-table-header:first th', gudingArr);
        data.lockDone = 'my-colsth-done';
        fLaytpl(getTplGuding, data, '.layui-table-fixed-l th', gudingArr);//必须是-fixed-l
        data = {};
        fLaytpl(getTplShaixuan, data, '.layui-table-header:first th', filterArr);
        fLaytpl(getTplShaixuan, data, '.layui-table-fixed-l th', filterArr);

        //遍历已筛选数组，在每次render后恢复原样
        if (typeof (aShaiXuanDone) != "undefined") {
            aShaiXuanDone.forEach(function (el, index) {
                $('.layui-table-header:first th:eq(' + el + ') div:last-child').addClass('my-colsth-done');
                $('.layui-table-fixed th:eq(' + el + ') div:last-child').addClass('my-colsth-done');
            })
        }
        //移除因添加排序给th添加的点击事件
        $('th').unbind('click');
        //th移上去显示固定列
        $('.layui-table-header:first th').hover(function () {
            $(this).find('.my-colsth-div').css('display', 'block')
        }, function () {
            $(this).find('.my-colsth-div').css('display', 'none')
        });
        $('.layui-table-fixed th ').hover(function () {             //怎样能找到有done就不加hover事件
            $(this).find('.my-colsth-shaixuan').css('display', 'block')
        }, function () {
            $(this).find('.my-colsth-shaixuan').css('display', 'none')
            });
    },
    guDingLie: function (ev) {
        var table = layui.table;
        var tableId = $(ev).closest('.layui-table-view').siblings("table").attr('id');
        var arr = window[tableId][1];
        var evIndex = $(ev).parent().parent().index();//th的index
        var proColsLength = arr[0].length;
        //存取宽度
        function fThGetWidth() {
            var aThWidth = [];
            $('.layui-table-header:first thead tr th').each(function () {
                aThWidth.push($(this).width());
            });
            for (let nGuDingLieC = 0; nGuDingLieC < proColsLength; nGuDingLieC++) {
                arr[0][nGuDingLieC].width = aThWidth[nGuDingLieC];
            }
        };
        if (!$(ev).parent().hasClass('my-colsth-done')) {
            for (let nGuDingLieA = 0; nGuDingLieA <= evIndex; nGuDingLieA++) {
                arr[0][nGuDingLieA].fixed = 'left';
            }
            fThGetWidth();
            table.reload(tableId, {
                cols: arr
            });
            layuiExt.fThEvent();
        }
        else {
            for (let nGuDingLieB = evIndex; nGuDingLieB < proColsLength; nGuDingLieB++) {
                if (arr[0][nGuDingLieB].hasOwnProperty('fixed')) {
                    delete arr[0][nGuDingLieB].fixed;
                }
            };
            fThGetWidth();
            layuiExt.reRender(tableId);
        }
    },
    shaiXuanLie: function (ev) {
        var laytpl = layui.laytpl;
        var formSelects = layui.formSelects;
        var evIndex = $(ev).parent().index();//第index个th
        var tableId = $(ev).closest('.layui-table-view').siblings('table').attr('id');//得到table的lay-filter
        var filterFieldName = window[tableId][1][0][evIndex].filterField;//'ProcessType.ProcessTypeName' 
        var filterSelectOffsetId = '#filterSelectOffset' + evIndex;
        var filterSelectId = 'filterSelect' + evIndex;
        var evOffset = $(ev).offset();//获取点击的偏移位
        var tableFilterIndex = -1;
        
        function fSelectHide() {
            $(filterSelectOffsetId).hide(500, function () { $('#domSelect').remove(); });
        }
        requestDealArr();
        function requestDealArr() {
            //请求
            var aUrlSplit = window[tableId][0].url.split('/');
            var tableIdLower = aUrlSplit[4].slice(0, 1).toLowerCase().concat(aUrlSplit[4].slice(1));//小写首字母
            //abp.services.app.processTask.getFilterColumnPageResult({page:1,limit:1000,filterField:'ProcessType.ProcessTypeName'})
            abp.services[aUrlSplit[3]][tableIdLower].getFilterColumnPageResult({ where: filterObject.whereFilter, tableFilter: JSON.stringify(filterObject.tableFilter), page: 1, limit: 1000, filterField: filterFieldName }).done(function (filterData) {
                filterData = filterData.filter(function (str) { return (str != 'null' && str!='') })
                var filterDataArr = [];
                var aDoneConditions ;
                filterObject.tableFilter.forEach(function (o,index) {
                    if (o.columnName == filterFieldName) {
                        aDoneConditions=o.conditions ;
                        tableFilterIndex = index;
                    }
                })
                filterData.forEach(function (str) {
                    var key = str;
                    if (aDoneConditions instanceof Array) {
                        var bHadFilter=aDoneConditions.some(function (elem) {
                            return elem.value==key
                        })
                        if (bHadFilter) {
                            filterDataArr.push({ "name": key, "value": key, selected: "selected" })
                        } else{
                            filterDataArr.push({ "name": key, "value": key })
                        }
                    } else {//之前没筛选过
                        filterDataArr.push({ "name": key, "value": key })
                    }
                })
                // formselect
                
                var renderData = { id: evIndex };//laytpl的data//标识码为'filterSelectOffset'+evIndex
                var getSelectTpl = shaixuanSelect.innerHTML;
                laytpl(getSelectTpl).render(renderData, function (html) {
                    $('body').append(html)
                    //$('#LAY-app-message').after(html);
                });
                formSelects.data(filterSelectId, 'local', { arr: filterDataArr });
                formSelects.btns(filterSelectId,
                    [{icon: 'iconfont icon-016',
                        name: '确定',
                        click: function (id) {
                            console.log(id)
                            //添加进已筛选的数组,去重
                            $(ev).parent().addClass('my-colsth-done');
                            //1.获取选中的value;如果value为空，则取消图标样式，并且原始不为空，则重载table
                            var value = formSelects.value(filterSelectId);
                            //console.log(value)
                            var arrIndex = filterObject.aShaiXuanDone.indexOf(evIndex);
                            if (value.length == 0 && filterObject.tableFilter.length == 0) {
                                $(ev).parent().removeClass('my-colsth-done');
                                filterObject.aShaiXuanDone.splice(arrIndex)
                            } else {
                                if (value.length == 0) {
                                    $(ev).parent().removeClass('my-colsth-done');
                                    filterObject.aShaiXuanDone.splice(arrIndex);
                                } else {
                                    $(ev).parent().addClass('my-colsth-done');
                                    filterObject.aShaiXuanDone.push(evIndex);
                                }
                                //2.更改url
                                //重载
                                function CreatTableCondi(value, andOr, leftBracket, rightBracket, comparer) {
                                    andOr = this.andOr;
                                    leftBracket = this.leftBracket;
                                    rightBracket = this.rightBracket;
                                    comparer = this.comparer;
                                    value = this.value;
                                }
                                var atmpConditions = [];
                                value.forEach(function (o) {
                                    atmpConditions.push({value: o.name})
                                })
                                //var tmpdid = false;
                                //filterObject.tableFilter.forEach(function (o) {
                                //    if (o.columnName == filterFieldName) {
                                //        o.conditions = atmpConditions;
                                //        tmpdid = true;
                                //    }
                                //})
                                if (tableFilterIndex == -1) {
                                    filterObject.tableFilter.push({ columnName: filterFieldName, conditions: atmpConditions })
                                } else {
                                    filterObject.tableFilter[tableFilterIndex].conditions = atmpConditions;
                                }
                                console.log(filterObject.tableFilter);
                                //filterObject.tableFilter = tmpTableFilter;
                                var where = window[tableId][0].where;
                                where.tableFilter = JSON.stringify(filterObject.tableFilter);
                                //////var where = { where: filterObject.oriWhere, tableFilter: JSON.stringify(filterObject.tableFilter)};
                                layui.table.reload(tableId, {
                                    where: where
                                })
                            }
                            layuiExt.fThEvent()
                            //隐藏
                            fSelectHide();
                        }
                    },
                        'select', 
                        'remove',
                         'reverse',
                    {
                        icon: 'iconfont icon-qingkong1',   //自定义图标, 可以使用layui内置图标
                        name: '取消',
                        click: function (id) {
                            //隐藏
                            fSelectHide();
                        }}
                    ], { show: 'name', space: '15px' });
                function func() {
                    console.log($('.xm-select-dl').scrollTop())
                    $('.xm-select-tips:first-child').css('top', $('.xm-select-dl').scrollTop())
                }

                $('.xm-select-dl').on('scroll', func)

                //设置偏移位,如果是最后几个，则往左边偏移
                var nTableBodyWidth = $('.layui-table-body').width()-290;
                if (evOffset.left > nTableBodyWidth) {
                    $(filterSelectOffsetId).offset({ top: evOffset.top + 18, left: evOffset.left -284 });
                } else{
                    $(filterSelectOffsetId).offset({ top: evOffset.top + 18, left: evOffset.left + 18 });
                }
                //打开下拉框
                $(filterSelectOffsetId +' .xm-form-select').addClass('xm-form-selected')
                //自动触发
                //点击当前页其他位置隐藏
                function changeTaggle() {
                    var target = $(event.srcElement);
                    console.log(target.closest('#domSelect'))
                    $(filterSelectOffsetId).hide(500, function () {
                        $('#domSelect').remove();
                        $('body').off('click', changeTaggle)
                    });
                }
                $('body').on('click', changeTaggle);
                var tableBody = document.getElementsByClassName('layui-table-body layui-table-main')[0];
                function scrolltaggle() {
                    $(filterSelectOffsetId).hide(500, function () {
                        $('#domSelect').remove();
                        tableBody.removeEventListener('scroll', scrolltaggle)
                    });
                }
                tableBody.addEventListener('scroll', scrolltaggle , false);

            });
        }
    },
    fLayerImg: function (ev, src, options = {}) {

        var areaHeight = window.innerHeight * 0.86;
        var areaWidth = areaHeight * 1.618;
        if (areaWidth > window.innerWidth * 0.86) {
            areaWidth = window.innerWidth * 0.86;
        }
        var nOriWidth = options.oriwidth ||Math.floor(areaHeight*0.8);
        var srcori, src, nIndex;
        if (options.oriSrc) {
            src = options.oriSrc;
            if (!ev) { nIndex = $.newid()}
        } else {
            srcori = src.split('?')[0];
            src = src.split('?')[0] + '?gap=false&w=' + nOriWidth;
        }
        if (ev || nIndex ) {
            var thIndex = $(ev).closest('tr').index();
            var liIndex = $(ev).closest('li').index();
            var nIndex = thIndex + 'li' + liIndex;
            console.log(nIndex)
        } else {
            var aSrc = srcori.split('/');
            nIndex=aSrc[aSrc.length - 1].split('.')[0];
        }
        function fLayerOpen() {
            layui.layer.open({
                type: 1,
                content: $('#oDiv' + nIndex),
                title: false,
                closeBtn: 0,
                area: [areaWidth+'px', areaHeight+'px'],
                offset: ['50px','50px'],
                skin: 'layui-layer-nobg', //没有背景色
                shadeClose: true,
                success: function (layero, index) {
                    oriOffset = $(`#oDiv${nIndex}`).parent().parent().offset();
                }
            })
            
        };
        //console.log(`<img src='${src}'id='oImg${nIndex}'/>`)
        if ($(`#oImg${nIndex}`).length == 0) {
            $(`<div id='oDiv${nIndex}'>
                <img src='${src}'id='oImg${nIndex}'/>
                <i class="iconfont icon-fangda my-flayerimg-icon" style="left:100px;" title="放大"></i>
                <i class="iconfont icon-suoxiao my-flayerimg-icon" style="left:140px;" title="缩小"></i>
                <i class="iconfont icon-zhongzhi my-flayerimg-icon" style="left:180px;" title="重置图片"></i>
                <i class="iconfont icon-xiangzuoxuanzhuandu my-flayerimg-icon" style="left:220px;" title="向左旋转"></i>
                <i class="iconfont icon-xiangyouxuanzhuandu my-flayerimg-icon" style="left:260px;" title="向右旋转"></i>
                <button class="layui-btn layui-btn-sm flayerimg-save" style="left:330px;position:fixed;top:16px;">保存</button>
               </div>`).appendTo($("body"));
            $('#oDiv' + nIndex).css('display', 'none');
            var oImg = document.getElementById(`oImg${nIndex}`);
            //oImg.onload = function () {
            //    fLayerOpen();
            //};
            function resizeImg($img, rate) {
                if (!rate) {
                    $img.css({ width: 'auto', height: 'auto' });
                    return;
                }
                if ($img.width() * rate < 1200 && $img.width() * rate > 100) {
                    $img.css("width", $img.width() * rate + "px");
                    $img.css("height", $img.height() * rate + "px");
                }
            }
            $(`#oImg${nIndex}`).on("mousewheel", function (e) {
                var delta = e.originalEvent.deltaY;
                var rate = delta > 0 ? 1.2 : 0.8;
                resizeImg($(`#oImg${nIndex}`), rate);
                return false;
            });
            $('.icon-zhongzhi.my-flayerimg-icon').on('click', function () {
                resizeImg($(`#oImg${nIndex}`), false);
            });
            $('.icon-fangda.my-flayerimg-icon').on('click', function () {
                resizeImg($(`#oImg${nIndex}`), 1.2);
            });
            $('.icon-suoxiao.my-flayerimg-icon').on('click', function () {
                resizeImg($(`#oImg${nIndex}`), 0.8);
            })

            var imgC = new CanvasImg(src);
            $('.icon-xiangzuoxuanzhuandu.my-flayerimg-icon').on('click', function () {//向左旋转
                $(`#oImg${nIndex}`)[0].src = imgC.goRotate()
                console.log(imgC.step)
            });
            $('.icon-xiangyouxuanzhuandu.my-flayerimg-icon').on('click', function () {//向右旋转
                $(`#oImg${nIndex}`)[0].src = imgC.goRotate('right')
                console.log(imgC.step)
            })
            $('.flayerimg-save').on('click', function () {
                $.post("/File/UploadByBase64", { data: $(`#oImg${nIndex}`)[0].src, oriVirtualPath: srcori }, function (res) {
                    layer.msg('保存成功')
                    options.callback && options.callback();
                    console.log(res);
                }, 'json');
            })
            fLayerOpen();
        } else {
            fLayerOpen();
        }



        //var oImg = document.createElement("img");
        //oImg.id = "oImg" + nIndex
        //oImg.src = src;
        //oImg.style.display="none";
        //document.body.appendChild(oImg);
    }
    , fCol: function (str) {
        if (window.localStorage.hasOwnProperty('layTable-cols-' + str)) {
            var colsArr = JSON.parse(window.localStorage.getItem('layTable-cols-' + str));
            //console.log(colsArr)
            colsArr.forEach(function (b, index) {
                $('*[data-key="1-0-' + index + '"]')[b ? 'removeClass' : 'addClass']('layui-hide');
                
            })
        }
        (function () {
            $('div[lay-event="LAYTABLE_COLS"]').click(function () {
                setTimeout(fColsHide,0)
                function fColsHide() {
                    colsArr.forEach(function (b, index) {
                        if (!b) {
                            $('div[lay-event="LAYTABLE_COLS"] input[data-key="0-' + index + '"] ').siblings('.layui-form-checked').removeClass('layui-form-checked');
                            $('div[lay-event="LAYTABLE_COLS"] input[data-key="0-' + index + '"]').click();
                        }
                    })
                }
                function docuClick(e) {
                    if (!$(e.target).closest("[lay-event='LAYTABLE_COLS']").length) {
                        setItem();
                        $(document).off("click", docuClick);
                    } 
                }
                $(document).on("click", docuClick);
            })
        })();
        function setItem() {
            var colsArr = [];
            $('.layui-table-header').find('th').each(function () {
                colsArr.push(!$(this).hasClass('layui-hide'));
            })
            colsArr = JSON.stringify(colsArr);
            window.localStorage.setItem('layTable-cols-' + str, colsArr);
        }
        window.addEventListener('unload', () => {
            setItem();
        });
    }
}

function Full() {//全屏
    var fullState = $('#LAY_app_tabs').offset().left;
    console.log(fullState)
    if (fullState > 0) {
        $('#LAY_app_tabs').addClass('quanping-tabs');
        $('#LAY_app_body').addClass('quanping-body');
    } else if (fullState == 0) {
        $('#LAY_app_tabs').removeClass('quanping-tabs');
        $('#LAY_app_body').removeClass('quanping-body');
    }
}
//给全局jquery对象加一个risize方法，$('.a').resize(function(){callback});
/*
(function ($, h, c) {
    var a = $([]), e = $.resize = $.extend($.resize, {}), i, k = "setTimeout", j = "resize", d = j
        + "-special-event", b = "delay", f = "throttleWindow";
    e[b] = 350;
    e[f] = true;
    $.event.special[j] = {
        setup: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.add(l);
            $.data(this, d, {
                w: l.width(),
                h: l.height()
            });
            if (a.length === 1) {
                g()
            }
        },
        teardown: function () {
            if (!e[f] && this[k]) {
                return false
            }
            var l = $(this);
            a = a.not(l);
            l.removeData(d);
            if (!a.length) {
                clearTimeout(i)
            }
        },
        add: function (l) {
            if (!e[f] && this[k]) {
                return false
            }
            var n;
            function m(s, o, p) {
                var q = $(this), r = $.data(this, d);
                r.w = o !== c ? o : q.width();
                r.h = p !== c ? p : q.height();
                n.apply(this, arguments)
            }
            if ($.isFunction(l)) {
                n = l;
                return m
            } else {
                n = l.handler;
                l.handler = m
            }
        }
    };
    function g() {
        i = h[k](function () {
            a.each(function () {
                var n = $(this), m = n.width(), l = n.height(), o = $
                    .data(this, d);
                if (m !== o.w || l !== o.h) {
                    n.trigger(j, [o.w = m, o.h = l])
                }
            });
            g()
        }, e[b])
    }
})(jQuery, this); 
*/
//格式化时间
Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份         
        "d+": this.getDate(), //日         
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时         
        "H+": this.getHours(), //小时         
        "m+": this.getMinutes(), //分         
        "s+": this.getSeconds(), //秒         
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度         
        "S": this.getMilliseconds() //毫秒         
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    if (+this===0||isNaN(+this)) {
        //当new Date(null)或者new Date(undefined)时会进到这里
        return ''
    }
    return fmt;
}
Date.prototype.addDay = function (number = 1,interval='d',pattern) {
    switch (interval.toLowerCase()) {
        case "y": this.setFullYear(this.getFullYear() + number);break;
        case "m": this.setMonth(this.getMonth() + number); break;
        case "d": this.setDate(this.getDate() + number); break;
        case "w": this.setDate(this.getDate() + 7 * number); break;
        case "h": this.setHours(this.getHours() + number); break;
        case "n": this.setMinutes(this.getMinutes() + number); break;
        case "s": this.setSeconds(this.getSeconds() + number); break;
        case "l": this.setMilliseconds(this.getMilliseconds() + number); break;
    }
    return pattern ? this.pattern(pattern) : this;
}
//动态改变disabled状态
Vue.directive('disabled', function (el, binding) {
    if (binding.value) {
        $(el).addClass('layui-disabled')
        $(el).attr("disabled", true);
    } else {
        $(el).removeClass('layui-disabled')
        $(el).attr("disabled", false);
    }
})

Vue.filter('objEmptyStr', function (value,str) {
    if (!value) return '';
    return value[str]
})
