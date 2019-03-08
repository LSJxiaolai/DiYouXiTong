// JavaScript Document
(function () {
    if (window.__sogou_secure_input) {
        window.__sogou_secure_input.check();
        return;
    }

    //私有变量
    var inputs = [];
    var timers = []; // timer太多了，用一个数组，新的把旧的顶掉
    var timerId = null;
    var divs = []; // 有些傻逼网站点登陆竟然不是切换登录div的显示与否，而是直接把登录div干掉，下次再生成一个。服了！
    var lastCaretInput = {
        input: null,
        selection: { start: 0, end: 0 }
    };

    var PADDING_RIGHT = 5;
    var IMAGE_WIDTH = 18;
    var IMAGE_HEIGHT = 14;
    var STYLE_TEXT = "margin: 0px; padding: 0px; width: " + IMAGE_WIDTH + "px; height:" + IMAGE_HEIGHT + "px; ";

    var container = null;
    //Amax: min-width: 0;在这里是为了fix ie7中的一个hasLayout相关的bug，参见http://www.satzansatz.de/cssd/onhavinglayout.html
    //text-align: left是对应position:absolute的left竟然受text-align的影响。。。
    var CONTAINER_STYLE_TEXT = "position: relative; min-width: 0; margin: 0px; padding: 0px; z-index: 2147482647; text-align: left;";

    var log = (function () {
            return function (msg) { window.external.secureinputdebug(msg); };
    })();

    var addEvent = (function () {
        if (window.addEventListener)
            return function (elem, type, func) { elem.addEventListener(type, func, false); };
        else
            return function (elem, type, func) { elem.attachEvent("on" + type, func); };
    })();

    function clearSecureInputs() {
        for (var i = 0, len = inputs.length; i < len; ++i) {
            inputs[i].__sogou_secure_input = null;
        }
        inputs = [];
        divs = [];
        container = null;
    }
    function injectSogouSecureInput() {
        try {
            if (typeof (window.__sogou_secure_input) === "undefined")
                window.__sogou_secure_input = {};

            window.__sogou_secure_input.check = function () {
                if (timers.length != 0)
                    window.clearTimeout(timerId);
                timers = [10, 10, 100, 100, 1000, 1000];
                timerId = setTimeout(checkSogouSecureInput, 10);
            };
            window.__sogou_secure_input.getInput = function (id) {
                return inputs[id];
            };
            window.__sogou_secure_input.setDriverState = function (state) {
                setDriverState(state);
            };
        } catch (e) {
            log("in injectSogouSecureInput: " + e.message);
        }
    }

    function isVisible(elem) {
        try {
            var rect = elem.getBoundingClientRect();
            if (rect.left === 0 || rect.left === rect.right)
                return false;
            var currentElem = document.elementFromPoint(rect.right - 5 - IMAGE_WIDTH - 1, (rect.top + rect.bottom) / 2);
            if (currentElem === elem)
                return true;
            return false;
        } catch (e) { return false; }
    }

    var isIE7 = (function () {
        var agent = navigator.userAgent;
        return agent.indexOf("MSIE 7.0") !== -1 && agent.indexOf("Trident") === -1;
    } ());

    function getZoomFactor() {
        try {
            if (isIE7) {
                var body = document.body, r = body.getBoundingClientRect();
                return (r.right - r.left) / body.offsetWidth;
            }
            if (screen.deviceXDPI)
                return screen.deviceXDPI / screen.logicalXDPI;
            return 1;
        } catch (e) {
            return 1;
        }
    }

    var getPos = function (el) {
        var left = 0,
			top = 0,
			right = el.offsetWidth, //默认元素可见，对于本应用而言可以这么写
			bottom = el.offsetHeight; //默认元素可见，对于本应用而言可以这么写

        while (el.offsetParent) {//是使用el.offsetParent还是使用el.parentNode
            left += el.offsetLeft;
            top += el.offsetTop;
            el = el.offsetParent;
        }

        right += left;
        bottom += top;

        return { left: left, top: top, right: right, bottom: bottom };
    };

    function getInputSelection(el) {
        var start = 0, end = 0, normalizedValue, range,
        textInputRange, len, endRange;

        range = document.selection.createRange();

        if (range && range.parentElement() == el) {
            len = el.value.length;
            normalizedValue = el.value.replace(/\r\n/g, "\n");

            // Create a working TextRange that lives only in the input
            textInputRange = el.createTextRange();
            textInputRange.moveToBookmark(range.getBookmark());

            // Check if the start and end of the selection are at the very end
            // of the input, since moveStart/moveEnd doesn't return what we want
            // in those cases
            endRange = el.createTextRange();
            endRange.collapse(false);

            if (textInputRange.compareEndPoints("StartToEnd", endRange) > -1) {
                start = end = len;
            } else {
                start = -textInputRange.moveStart("character", -len);
                start += normalizedValue.slice(0, start).split("\n").length - 1;

                if (textInputRange.compareEndPoints("EndToEnd", endRange) > -1) {
                    end = len;
                } else {
                    end = -textInputRange.moveEnd("character", -len);
                    end += normalizedValue.slice(0, end).split("\n").length - 1;
                }
            }
        }

        return {
            start: start,
            end: end
        };
    }

    function setCaretToInput(input) {
        input.focus();
        if (lastCaretInput.selection.start !== -1) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', lastCaretInput.selection.end);
            range.moveStart('character', lastCaretInput.selection.start);
            range.select();
        }

    }

    function insertAfter(newElem, prevSibling) {
        var parent = prevSibling.parentNode;
        var nextSibling = prevSibling.nextSibling;
        if (nextSibling) {
            parent.insertBefore(newElem, nextSibling);
        }
        else {
            parent.appendChild(newElem);
        }
    }

    function getStyleNumber(elem, key) {
        var iStyle = (+elem.currentStyle[key].slice(0, -2));
        if (isNaN(iStyle))
            return 0;
        return iStyle;
    }

    function createClickHandler(input) {
        var index = 0;
        if (input.__sogou_secure_input) {
            for (var i = 0, len = inputs.length; i < len; ++i) {
                if (inputs[i] === input)
                    index = i;
            }
        }
        else {
            index = inputs.length;
            inputs[index] = input;
        }

        log("createClickHandler");

        input = null; // to prevent Memory Leak in IE
        return function (event) {
            try {
                log("ClickHandler");
                var input = inputs[index];
                if (!input)
                    return;
                setCaretToInput(input);
                var rect = input.getBoundingClientRect();
                var scrLeft = window.screenLeft;
                var scrTop = window.screenTop;
                var factor = isIE7 ? 1 : getZoomFactor();
                if (lastCaretInput.input === input)      
                {
                    window.external.popupsecureinput(window, index, (scrLeft + rect.left) * factor, (scrTop + rect.top) * factor, (scrLeft + rect.right) * factor, (scrTop + rect.bottom) * factor, lastCaretInput.selection.start, lastCaretInput.selection.end, driverState);
                }
                else
                {
                    log("lastCaretInput.input != input");
                    window.external.popupsecureinput(window, index, (scrLeft + rect.left) * factor, (scrTop + rect.top) * factor, (scrLeft + rect.right) * factor, (scrTop + rect.bottom) * factor, -1, -1, driverState);
                }
            } catch (e) { log("in click handler: " + e.message); }
            if (event.stopPropagation) {
                event.stopPropagation();
            }
            else if (window.event) {
                window.event.cancelBubble = true;
            }
            if (event.preventDefault) {
                event.preventDefault();
            }
            return false;
        }
    }

    function recordLastCaret(elem) {
        lastCaretInput.input = elem;
        lastCaretInput.selection = getInputSelection(elem);
    }

    function setSelectionHandler() {
        var elem = window.event.srcElement;
        recordLastCaret(elem);
    }

    //为了用户教育增加的状态。。。
    var BACKGORUND_STYLE_NORMAL = "url('https://BCC0E825-2420-4190-AF25-ABD45D41EA3A/se/secureinput/icon.png') left top no-repeat";
    var BACKGORUND_STYLE_DRIVER_ERROR = "url('https://BCC0E825-2420-4190-AF25-ABD45D41EA3A/se/secureinput/icon_driver_error.png') left top no-repeat";
    var BACKGORUND_STYLE_REMOTE_ENV = "url('https://BCC0E825-2420-4190-AF25-ABD45D41EA3A/se/secureinput/icon_remote_env.png') left top no-repeat";
    var State_NoDriver = 0;
    var State_Normal = 1;
    var State_DriverError = 2;
    var State_RemoteEnv = 3;
    var State_DriverError_RepairFailed = 4;
    var driverState = 0;
    function setDriverStateForInputDiv(div) {
        switch (driverState) {
            case State_NoDriver:
            case State_DriverError_RepairFailed:
                div.setAttribute("title", "点击可打开软键盘输入密码，输入更安全。");
                div.style.background = BACKGORUND_STYLE_NORMAL;
                break;
            case State_Normal:
                div.setAttribute("title", "搜狗键盘安全服务正在保护您安全输入密码。");
                div.style.background = BACKGORUND_STYLE_NORMAL;
                break;
            case State_DriverError:
                div.setAttribute("title", "搜狗键盘安全服务被破坏，点击按钮修复。");
                div.style.background = BACKGORUND_STYLE_DRIVER_ERROR;
                break;
            case State_RemoteEnv:
                div.setAttribute("title", "您似乎处于远程桌面下，请注意输入安全");
                div.style.background = BACKGORUND_STYLE_REMOTE_ENV;
                break;
        }
    }

    function setDriverState(state) {
        driverState = state;
        for (var i = 0, len = divs.length; i < len; ++i) {
            setDriverStateForInputDiv(divs[i]);
        }
    }

    function createSecureInputDiv(input) {
        if (!input)
            return;
        var div = document.createElement("div");
        setDriverStateForInputDiv(div);
        container.appendChild(div);
        div.__input__ = input;
        divs.push(div);
        addEvent(div, "click", createClickHandler(input));
        addEvent(input, "click", setSelectionHandler);
        addEvent(input, "select", setSelectionHandler);
        addEvent(input, "keyup", setSelectionHandler);
        addEvent(input, "keyup", checkSogouSecureInput);
        addEvent(window, "scroll", checkSogouSecureInput);
        addEvent(window, "click", function () { setTimeout(checkSogouSecureInput, 0); });
        input.__sogou_secure_input = div;

        //fix一个weibo only的问题
        if (window.location.host.indexOf("weibo.com") !== -1)
            input.fireEvent("onblur");

    }

    function checkSogouSecureInput() {
        //alert("checksogouinput");
        try {
            if (document.body) {
                if (container === null) {
                    // 如果没有任何可以注入的inputs，则直接返回
                    var count = 0;
                    var inputs = document.getElementsByTagName("input");
                    for (var i = 0, len = inputs.length; i < len; ++i) {
                        if (inputs[i].type === "password" && !inputs[i].readOnly && isVisible(inputs[i])) {
                            ++count;
                        }
                    }
                    if (count === 0) {
                        if (timers.length !== 0) {
                            timerId = setTimeout(checkSogouSecureInput, timers.shift());
                        }
                        return;
                    }
                    container = document.createElement("div");
                    container.setAttribute("id", "sogou_secure_inputs_container");
                    document.body.appendChild(container);
                    container.style.cssText = CONTAINER_STYLE_TEXT;
                }

                // IEbug, 此时计算比较准确，当把所有div都删除后，位置可能会变化
                var bContainerOnDocument = true;
                try {
                    var rectContainer = container.getBoundingClientRect();
                    if (rectContainer.left === 0 && rectContainer.top === 0)
                        bContainerOnDocument = false;
                } catch (e) {
                    bContainerOnDocument = false;
                }
                if (!bContainerOnDocument) {
                    clearSecureInputs();
                    if (timers.length !== 0) {
                        timerId = setTimeout(checkSogouSecureInput, timers.shift());
                    }
                    return;
                }
                for (var i = 0, len = divs.length; i < len; ++i) {
                    if (!isVisible(divs[i].__input__)) {
                        divs[i].style.display = "none";
                    }
                }

                inputs = document.getElementsByTagName("input");
                for (var i = 0, len = inputs.length; i < len; ++i) {
                    if (inputs[i].type === "password" && !inputs[i].readOnly) {
                        var input = inputs[i];
                        if (!input.__sogou_secure_input)
                            createSecureInputDiv(input);
                        var div = input.__sogou_secure_input;
                        if (isVisible(input)) {
                            var rect = input.getBoundingClientRect();
                            var left = rect.left - rectContainer.left;
                            var top = rect.top - rectContainer.top;
                            var width = rect.width || (rect.right - rect.left);
                            if (isIE7) {
                                var factor = getZoomFactor();
                                left /= factor;
                                top /= factor;
                                width /= factor;
                            }

                            var offsetTop = Math.floor((rect.bottom - rect.top - IMAGE_HEIGHT) / 2);
                            offsetTop = offsetTop > 0 ? offsetTop : 0;
                            top = top + offsetTop;
                            left = left + width - (IMAGE_WIDTH + PADDING_RIGHT);
                            if (!div.__state__ || !div.__state__.visible || div.__state__.left !== left || div.__state__.top !== top) {
                                div.style.cssText = "position:absolute; left: " + left + "px; top: " + top + "px; display: block;" + STYLE_TEXT;
                                setDriverStateForInputDiv(div);
                                div.__state__ = { visible: true, left: left, top: top };
                            }

                        }
                        else {
                            div.style.cssText = "position:absolute; display: none;" + STYLE_TEXT;
                            div.__state__ = { visible: false };
                        }
                    }
                }
            }
        }
        catch (e) { log("checkSogouSecureInput: " + e.message); }

        if (timers.length !== 0) {
            timerId = setTimeout(checkSogouSecureInput, timers.shift());
        }
    }

    injectSogouSecureInput();
    window.__sogou_secure_input.check();
    addEvent(window, "resize", window.__sogou_secure_input.check);
    addEvent(window, "unload", function () {
        // to prevent memory leak
        inputs = null;
        container = null;
        divs = null;
    })
})();
var jsNode = document.getElementById("sbid-secureinput");if (jsNode){jsNode.parentNode.removeChild(jsNode);}