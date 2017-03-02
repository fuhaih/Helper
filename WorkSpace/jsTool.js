/*
*json格式的datetime转为js格式的datetime
*/
function timeJson2Date(time) {
    if (time != null) {
        var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
        return date;
    }
    return null;
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
	//number为分组里的内容
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

var getRandomColor = function () {
    return '#' + (function (color) {
        //这个写法比较有意思,Math.floor(Math.random()*16);返回的是一个小于或等于16的数.然后作为0123456789abcdef的下标,这样每次就会得到一个这个字符串当中的一个字符
        return (color += '0123456789abcdef'[Math.floor(Math.random() * 16)])
            //然后判断这个新字符串的长度是否到6,因为16进制的颜色是由6个字符组成的,如果到6了,就返回这6个字符拼成的字符串,如果没有就执行arguments.callee(color)也就是函数本身.
            && (color.length == 6) ? color : arguments.callee(color); //将''字符串传给color
    })('');
}

/*
* 把字符串转换为适用于.net的unicode编码
*/
String.prototype.ToUnicodeBytes = function () {
    var ch, re = [];
    for (var i = 0; i < this.length; i++) {
        ch = this.charCodeAt(i);  // get char   
        if (ch > 255)// set up "stack"  
        {
            re.push(ch & 0xFF);  // push byte to stack  
            ch = ch >> 8;          // shift value down by 1 byte  
            re.push(ch & 0xFF);
        }
        else {
            re.push(ch & 0xFF);
            re.push(0);
        }
    }
    // return an array of bytes  
    return re;
}

/*
* 把unicode编码转换为字符串
*/
UnicodeToString = function (array) {
    if (array.length & 1 == 1) return;
    var charCode = [];
    for (var i = 0; i < array.length; i = i + 2) {
        var code = array[i + 1];
        code = code << 8;
        code = code | array[i];
        var str = String.fromCharCode(code)
        charCode.push(str);
    }
    var res = charCode.join('');
    return res;
}

/** 
*@param {string} url 完整的URL地址 
*@returns {object} 自定义的对象 
*@description 用法示例：var myURL = parseURL('http://abc.com:8080/dir/index.html?id=255&m=hello#top'); 

myURL.file='index.html' 

myURL.hash= 'top' 

myURL.host= 'abc.com:8081' 

myURL.hostname= 'abc.com' 

myURL.query= '?id=255&m=hello' 

myURL.params= Object = { id: 255, m: hello } 

myURL.path= '/dir/index.html' 

myURL.segments= Array = ['dir', 'index.html'] 

myURL.port= '8081' 

myURL.protocol= 'http' 

myURL.source= 'http://abc.com:8080/dir/index.html?id=255&m=hello#top' 

*/
function parseURL(url) {
    var a = document.createElement('a');
    a.href = url;
    return {
        source: url,
        protocol: a.protocol.replace(':', ''),
        hostname: a.hostname,
        host: a.host,
        port: a.port,
        query: a.search,
        params: (function () {
            var ret = {},
                seg = a.search.replace(/^\?/, '').split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[s[0]] = s[1];
            }
            return ret;
        })(),
        file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
        hash: a.hash.replace('#', ''),
        path: a.pathname.replace(/^([^\/])/, '/$1'),
        relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
        segments: a.pathname.replace(/^\//, '').split('/')
    };
}