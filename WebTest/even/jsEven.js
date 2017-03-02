var EventUtil = {
    /*添加事件监听*/
    addHandler: function (element, type, handler) {
        if (element.addEventListener) {
            element.addEventListener(type, handler, false);
        } else if (element.attachEvent) {
            element.attachEvent("on" + type, handler);
        } else {
            element["on" + type] = handler;
        }
    },
    /*移除事件监听*/
    removeHandler: function (element, type, handler) {
        if (element.removeEventListener) {
            element.removeEventListener(type, handler, false);
        } else if (element.detachEvent) {
            element.detachEvent("on" + type, handler);
        } else {
            element["on" + type] = null;
        }
    },
    /*
        获取even对象
        even对象中包含了事件触发的详细信息
    */
    getEvent: function (event) {
        return event ? event : window.event;
    },
    /*
        获取事件目标
        如果事件绑定在父元素中，则会返回最底层触发事件的元素
        否则直接返回绑定事件的元素
    */
    getTarget: function (event) {
        return event.target || event.srcElement;
    },
    /*
        阻止事件的默认行为
        如link点击事件的默认行为是跳转到其href的链接
    */
    preventDefault: function (event) {
        if (event.preventDefault) {
            event.preventDefault();
        } else {
            event.returnValue = false;
        }
    },
    /*
        取消事件冒泡
        stopPropagation能够取消事件的进一步捕获和冒泡
        cancelBubbles只能够取消事件的冒泡
    */
    stopPropagation: function (event) {
        if (event.stopPropagation) {
            event.stopPropagation();
        } else {
            event.cancelBubbles = true;
        }
    },
    /*
        返回当前事件涉及到的其他DOM元素，如果有的话
        一般在过渡型事件中使用，比如mouseout和mouseover
    */
    getRelatedTarget: function (event) {
        if (event.relatedTarget) {
            return event.relatedTarget;
        } else if (event.toElement) {
            return event.toElement;
        } else if (event.fromElement) {
            return event.fromElement;
        } else { return null; }

    }
}
var bodyEven = document.getElementsByTagName("body");
EventUtil.addHandler(bodyEven[0], "click", function (event) {
    event = EventUtil.getEvent(event);
    var target = EventUtil.getTarget(event);
    if (target.dataset.click != undefined) {
        alert(target.dataset.click)
    }
});