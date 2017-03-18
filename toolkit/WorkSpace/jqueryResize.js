/**
*div的resize方法
*用法$("").reSize();
*/

//错误
(function ($, h, c) {
    //$=jQuery
    //h=this=window
    //c=undefined

    var a = $([]),//创建jquery数组
        e = $.reSize = $.extend($.reSize, {}),//$.reSize={},e=$.reSize
        i,
        k = "setTimeout",
        j = "reSize",
        d = j + "-special-event",
        b = "delay",
        f = "throttleWindow",
        reSize = $.fn.reSize = function (a, c) {
            return arguments.length > 0 ? this.on(j, null, a, c) : this.trigger(j)
        }
        //resize = $.fn.resize = function (a, c) {
        //    return arguments.length > 0 ? this.on($.b, null, a, c) : this.trigger($.b)
        //}
    e[b] = 250;
    e[f] = true;
    //$.event.special自定义事件
    $.event.special[j] = {
        /** 
         * 初始化事件处理器 - this指向元素 
         * @param 附加的数据 
         * @param 事件类型命名空间 
         * @param 回调函数 
         */
        setup: function () {
            if (!e[f] && this[k]) {
                return false;
            }
            var l = $(this);
            a = a.add(l);
            //jquery 的data模块
            // 设置,读取自定义数据
            //data: function( elem, name, data ) {}
            $.data(this, d, {
                w: l.width(),
                h: l.height()
            });
            if (a.length === 1) {
                g();
            }
        },
        /** 
         * 卸载事件处理器 - this指向元素 
         * @param 事件类型命名空间 
         */
        teardown: function () {
            if (!e[f] && this[k]) {
                return false;
            }
            var l = $(this);
            a = a.not(l);
            l.removeData(d);
            if (!a.length) {
                clearTimeout(i);
            }
        },
        add: function (l) {
            if (!e[f] && this[k]) {
                return false;
            }
            var n;
            function m(s, o, p) {
                var q = $(this),
                    r = $.data(this, d);
                r.w = o !== c ? o : q.width();
                r.h = p !== c ? p : q.height();
                n.apply(this, arguments);
            }
            if ($.isFunction(l)) {
                n = l;
                return m;
            } else {
                n = l.handler;
                l.handler = m;
            }
        }
    };
    function g() {
        /*
        setTimeout()方法不断执行 e[b]=250毫秒
        */
        i = h[k](function () {
            a.each(function () {
                var n = $(this),
                    m = n.width(),
                    l = n.height(),
                    o = $.data(this, d);
                if (m !== o.w || l !== o.h) {
                    //当元素的长宽变动的时候，用trigger来触发事件
                    n.trigger(j, [o.w = m, o.h = l]);
                }
            });
            g();
        },
            e[b]);
    }
})(jQuery, this);