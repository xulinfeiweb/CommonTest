; (function () {
    if (typeof window.CustomEvent === 'function') return false;
    function CustomEvent(event, params) {
        params = params || { bubbles: false, cancelable: false, detail: undefined };
        var evt = document.createEvent('CustomEvent');
        evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);
        return evt;
    }
    CustomEvent.prototype = window.Event.prototype;
    window.CustomEvent = CustomEvent;
})();
; (function () {
    window._xhrs = window._xhrs || { count: 0, result: [], send: 0 };
    function ajaxEventTrigger(event) {
        var ajaxEvent = new CustomEvent(event, { detail: this });
        window.dispatchEvent(ajaxEvent);
        if (event === 'ajaxLoadStart') {
            window._xhrs['count']++;
        }
        if (event === 'ajaxLoadEnd') {
            if (/^<[\\s\\S]*>$/gi.test(this['responseText'])) {
                alert(this['responseText']);
                //console.log(this['responseText']);
            } else {
                window._xhrs['result'].push(this['responseText']);
            }
            returnCSharp();
        }
    }
    var oldXHR = window.XMLHttpRequest;
    function newXHR() {
        var realXHR = new oldXHR();
        realXHR.addEventListener('abort', function () { ajaxEventTrigger.call(this, 'ajaxAbort'); }, false);
        realXHR.addEventListener('error', function () { ajaxEventTrigger.call(this, 'ajaxError'); }, false);
        realXHR.addEventListener('load', function () { ajaxEventTrigger.call(this, 'ajaxLoad'); }, false);
        realXHR.addEventListener('loadstart', function () { ajaxEventTrigger.call(this, 'ajaxLoadStart'); }, false);
        realXHR.addEventListener('progress', function () { ajaxEventTrigger.call(this, 'ajaxProgress'); }, false);
        realXHR.addEventListener('timeout', function () { ajaxEventTrigger.call(this, 'ajaxTimeout'); }, false);
        realXHR.addEventListener('loadend', function () { ajaxEventTrigger.call(this, 'ajaxLoadEnd'); }, false);
        realXHR.addEventListener('readystatechange', function () { ajaxEventTrigger.call(this, 'ajaxReadyStateChange'); }, false);
        return realXHR;
    }
    window.XMLHttpRequest = newXHR;
})();
function returnCSharp() {
    var xel = document.getElementById('live-search');
    if (xel) {
        xel.value = window._xhrs.count + ' : ' + window._xhrs.result.length + ' : ' + window._xhrs['xhr'];
    }
    if (window._xhrs.count === window._xhrs.result.length && !window._xhrs['send']) {
        window._xhrs['send'] = 1;
        setTimeout(function () {
            var results = [];
            window._xhrs['result'].forEach(function (item) {
                //if (item.indexOf('code') > -1 && item.indexOf('msg') > -1 && item.indexOf('data') > -1) {
                //    results.push(item);
                //}
                if (item.indexOf('type') > -1 && item.indexOf('message') > -1 && item.indexOf('resultdata') > -1) {
                    results.push(item);
                }
            });
            window.external.sendToCSharp(results);
            //window.external.sendToCSharp(window._xhrs['result']);
            returnCSharp();
        }, 1000);
    }
}