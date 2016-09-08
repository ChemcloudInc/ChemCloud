/*
* jQuery form plugin
* @requires jQuery v1.1 or later
*
* Examples at: http://malsup.com/jquery/form/
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Revision: $Id$
* Version: 1.0.3
*/
(function ($) {
    /**
    * ajaxSubmit()  为提交使用AJAX HTML表单的机制。
    * ajaxSubmit会接受它可以是一个成功的回调函数的一个参数
    *或选择对象。如果提供一个函数，将在成功被调用
    *在完成了提交和将传递来自服务器的响应。
    *如果提供了选择对象，以下属性的支持：
    *
    *  target:  标识页面中的元件（多个），以与服务器响应更新。
    *这个值可以被指定为一个jQuery选择字符串，一个jQuery对象，
    *或DOM元素。
    *默认值：空
    *
    *  url:     URL到表单数据将被提交。
    *默认值：表单的“动作”属性的值
    *
    *  type:    在该表单数据应提交的方法，“GET”或“POST”。
    *默认值：表单的“方法”属性的值（或“GET”如果没有找到）
    *
    *  beforeSubmit: 提交表单之前要调用的回调方法。
    *默认值：空
    *
    *  success:  表单已成功提交后，要调用的回调方法
    *和响应已被从服务器返回
    *默认值：空
    *
    *  dataType: 响应的预期数据类型。一：空，“XML”，“脚本”或“JSON”
    *默认值：空
    *
    *  semantic: 布尔标志指示是否数据必须在语义顺序（慢）提交。
    *默认值：false
    *
    *  resetForm: 布尔值，指示表单是否需要重置提交成功
    *
    *  clearForm: 布尔标志指示是否是否提交成功的形式应该被清除
    *
    *

    *在'beforeSubmit“回调可以用于运行预提交逻辑或为被提供为一个钩
    *验证表单数据。如果“beforeSubmit”回调返回false，那么表单会
    *没有提交。在'beforeSubmit'调用回调函数有三个参数：表单数据
    *数组形式，jQuery对象和对象的选择传递给ajaxSubmit会。
    *本表数据数组采用以下形式：
    *
    *     [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
    *
    * If a 'success' 提供回调方法的反应已经返回后调用*从服务器。它（取决于数据类型）被传递给responseText的VS responseXML的值。
    *请参阅jQuery.ajax进一步的细节。
    *
    *
    *数据类型选项提供指定服务器响应应如何处理的手段。
    *这直接映射到和jQuery.httpData方法。下面的值被支持：
    *
    *      'xml':   如果dataType=='XML'服务器的响应被视为XML和“成功回调方法，如果指定，将通过responseXML的值
    *      'json':   if dataType == 'json' 服务器响应进行评估，并传递给*的'成功'回调，如果指定
    *      'script': if dataType == 'script' 服务器响应在全球范围内被评估
    *
    *
    *请注意，这是没有意义的同时使用“target”和“dataType”选项。如果提供目标既*将被忽略。
    *
    * The semantic argument can be used to force form serialization in semantic order.
    * This is normally true anyway, unless the form contains input elements of type='image'.
    * If your form must be submitted with name/value pairs in semantic order and your form
    * contains an input of type='image" then pass true for this arg, otherwise pass false
    * (or nothing) to avoid the overhead for this logic.
    *
    *
    * When used on its own, ajaxSubmit() is typically bound to a form's submit event like this:
    *
    * $("#form-id").submit(function() {
    *     $(this).ajaxSubmit(options);
    *     return false; // cancel conventional submit
    * });
    *
    * When using ajaxForm(), however, this is done for you.
    *
    * @example
    * $('#myForm').ajaxSubmit(function(data) {
    *     alert('Form submit succeeded! Server returned: ' + data);
    * });
    * @desc Submit form and alert server response
    *
    *
    * @example
    * var options = {
    *     target: '#myTargetDiv'
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc Submit form and update page element with server response
    *
    *
    * @example
    * var options = {
    *     success: function(responseText) {
    *         alert(responseText);
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc Submit form and alert the server response
    *
    *
    * @example
    * var options = {
    *     beforeSubmit: function(formArray, jqForm) {
    *         if (formArray.length == 0) {
    *             alert('Please enter data.');
    *             return false;
    *         }
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc Pre-submit validation which aborts the submit operation if form data is empty
    *
    *
    * @example
    * var options = {
    *     url: myJsonUrl.php,
    *     dataType: 'json',
    *     success: function(data) {
    *        // 'data' is an object representing the the evaluated json data
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc json data returned and evaluated
    *
    *
    * @example
    * var options = {
    *     url: myXmlUrl.php,
    *     dataType: 'xml',
    *     success: function(responseXML) {
    *        // responseXML is XML document object
    *        var data = $('myElement', responseXML).text();
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc XML data returned from server
    *
    *
    * @example
    * var options = {
    *     resetForm: true
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc submit form and reset it if successful
    *
    * @example
    * $('#myForm).submit(function() {
    *    $(this).ajaxSubmit();
    *    return false;
    * });
    * @desc Bind form's submit event to use ajaxSubmit
    *
    *
    * @name ajaxSubmit
    * @type jQuery
    * @param options  object literal containing options which control the form submission process
    * @cat Plugins/Form
    * @return jQuery
    */
    $.fn.ajaxSubmit = function (options) {
        if (typeof options == 'function')
            options = { success: options };

        options = $.extend({
            url: this.attr('action') || window.location,
            type: this.attr('method') || 'GET'
        }, options || {});

        var a = this.formToArray(options.semantic);

        // give pre-submit callback an opportunity to abort the submit
        if (options.beforeSubmit && options.beforeSubmit(a, this, options) === false) return this;

        // fire vetoable 'validate' event
        var veto = {};
        $.event.trigger('form.submit.validate', [a, this, options, veto]);
        if (veto.veto)
            return this;

        var q = $.param(a); //.replace(/%20/g,'+');

        if (options.type.toUpperCase() == 'GET') {
            options.url += (options.url.indexOf('?') >= 0 ? '&' : '?') + q;
            options.data = null;  // data is null for 'get'
        }
        else
            options.data = q; // data is the query string for 'post'

        var $form = this, callbacks = [];
        if (options.resetForm) callbacks.push(function () { $form.resetForm(); });
        if (options.clearForm) callbacks.push(function () { $form.clearForm(); });

        // perform a load on the target only if dataType is not provided
        if (!options.dataType && options.target) {
            var oldSuccess = options.success; // || function(){};
            callbacks.push(function (data) {
                $(options.target).attr("innerHTML", data).evalScripts().each(oldSuccess, arguments);
            });
        }
        else if (options.success)
            callbacks.push(options.success);

        options.success = function (data, status) {
            for (var i = 0, max = callbacks.length; i < max; i++)
                callbacks[i](data, status, $form);
        };

        // are there files to upload?
        var files = $('input:file', this).fieldValue();
        var found = false;
        for (var j = 0; j < files.length; j++)
            if (files[j])
                found = true;

        if (options.iframe || found) // options.iframe allows user to force iframe mode
            fileUpload();
        else
            $.ajax(options);

        // fire 'notify' event
        $.event.trigger('form.submit.notify', [this, options]);
        return this;


        // private function for handling file uploads (hat tip to YAHOO!)
        function fileUpload() {
            var form = $form[0];
            var opts = $.extend({}, $.ajaxSettings, options);

            var id = 'jqFormIO' + $.fn.ajaxSubmit.counter++;
            var $io = $('<iframe id="' + id + '" name="' + id + '" />');
            var io = $io[0];
            var op8 = $.browser.opera && window.opera.version() < 9;
            if ($.browser.msie || op8) io.src = 'javascript:false;document.write("");';
            $io.css({ position: 'absolute', top: '-1000px', left: '-1000px' });

            var xhr = { // mock object
                responseText: null,
                responseXML: null,
                status: 0,
                statusText: 'n/a',
                getAllResponseHeaders: function () { },
                getResponseHeader: function () { },
                setRequestHeader: function () { }
            };

            var g = opts.global;
            // trigger ajax global events so that activity/block indicators work like normal
            if (g && !$.active++) $.event.trigger("ajaxStart");
            if (g) $.event.trigger("ajaxSend", [xhr, opts]);

            var cbInvoked = 0;
            var timedOut = 0;

            // take a breath so that pending repaints get some cpu time before the upload starts
            setTimeout(function () {
                $io.appendTo('body');
                // jQuery's event binding doesn't work for iframe events in IE
                io.attachEvent ? io.attachEvent('onload', cb) : io.addEventListener('load', cb, false);

                // make sure form attrs are set
                var encAttr = form.encoding ? 'encoding' : 'enctype';
                var t = $form.attr('target');
                $form.attr({
                    target: id,
                    method: 'POST',
                    encAttr: 'multipart/form-data',
                    action: opts.url
                });

                // support timout
                if (opts.timeout)
                    setTimeout(function () { timedOut = true; cb(); }, opts.timeout);

                form.submit();
                $form.attr('target', t); // reset target
            }, 10);

            function cb() {
                if (cbInvoked++) return;

                io.detachEvent ? io.detachEvent('onload', cb) : io.removeEventListener('load', cb, false);

                var ok = true;
                try {
                    if (timedOut) throw 'timeout';
                    // extract the server response from the iframe
                    var data, doc;
                    doc = io.contentWindow ? io.contentWindow.document : io.contentDocument ? io.contentDocument : io.document;
                    xhr.responseText = doc.body ? doc.body.innerHTML : null;
                    xhr.responseXML = doc.XMLDocument ? doc.XMLDocument : doc;

                    if (opts.dataType == 'json' || opts.dataType == 'script') {
                        var ta = doc.getElementsByTagName('textarea')[0];
                        data = ta ? ta.value : xhr.responseText;
                        if (opts.dataType == 'json')
                            eval("data = " + data);
                        else
                            $.globalEval(data);
                    }
                    else if (opts.dataType == 'xml') {
                        data = xhr.responseXML;
                        if (!data && xhr.responseText != null)
                            data = toXml(xhr.responseText);
                    }
                    else {
                        data = xhr.responseText;
                    }
                }
                catch (e) {
                    ok = false;
                    $.handleError(opts, xhr, 'error', e);
                }

                // ordering of these callbacks/triggers is odd, but that's how $.ajax does it
                if (ok) {
                    opts.success(data, 'success');
                    if (g) $.event.trigger("ajaxSuccess", [xhr, opts]);
                }
                if (g) $.event.trigger("ajaxComplete", [xhr, opts]);
                if (g && ! --$.active) $.event.trigger("ajaxStop");
                if (opts.complete) opts.complete(xhr, ok ? 'success' : 'error');

                // clean up
                setTimeout(function () {
                    $io.remove();
                    xhr.responseXML = null;
                }, 100);
            };

            function toXml(s, doc) {
                if (window.ActiveXObject) {
                    doc = new ActiveXObject('Microsoft.XMLDOM');
                    doc.async = 'false';
                    doc.loadXML(s);
                }
                else
                    doc = (new DOMParser()).parseFromString(s, 'text/xml');
                return (doc && doc.documentElement && doc.documentElement.tagName != 'parsererror') ? doc : null;
            };
        };
    };
    $.fn.ajaxSubmit.counter = 0; // used to create unique iframe ids

    /**
    * ajaxForm() provides a mechanism for fully automating form submission.
    *
    * The advantages of using this method instead of ajaxSubmit() are:
    *
    * 1: This method will include coordinates for <input type="image" /> elements (if the element
    *    is used to submit the form).
    * 2. This method will include the submit element's name/value data (for the element that was
    *    used to submit the form).
    * 3. This method binds the submit() method to the form for you.
    *
    * Note that for accurate x/y coordinates of image submit elements in all browsers
    * you need to also use the "dimensions" plugin (this method will auto-detect its presence).
    *
    * The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
    * passes the options argument along after properly binding events for submit elements and
    * the form itself.  See ajaxSubmit for a full description of the options argument.
    *
    *
    * @example
    * var options = {
    *     target: '#myTargetDiv'
    * };
    * $('#myForm').ajaxSForm(options);
    * @desc Bind form's submit event so that 'myTargetDiv' is updated with the server response
    *       when the form is submitted.
    *
    *
    * @example
    * var options = {
    *     success: function(responseText) {
    *         alert(responseText);
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc Bind form's submit event so that server response is alerted after the form is submitted.
    *
    *
    * @example
    * var options = {
    *     beforeSubmit: function(formArray, jqForm) {
    *         if (formArray.length == 0) {
    *             alert('Please enter data.');
    *             return false;
    *         }
    *     }
    * };
    * $('#myForm').ajaxSubmit(options);
    * @desc Bind form's submit event so that pre-submit callback is invoked before the form
    *       is submitted.
    *
    *
    * @name   ajaxForm
    * @param  options  object literal containing options which control the form submission process
    * @return jQuery
    * @cat    Plugins/Form
    * @type   jQuery
    */
    $.fn.ajaxForm = function (options) {
        return this.ajaxFormUnbind().submit(submitHandler).each(function () {
            // store options in hash
            this.formPluginId = $.fn.ajaxForm.counter++;
            $.fn.ajaxForm.optionHash[this.formPluginId] = options;
            $(":submit,input:image", this).click(clickHandler);
        });
    };

    $.fn.ajaxForm.counter = 1;
    $.fn.ajaxForm.optionHash = {};

    function clickHandler(e) {
        var $form = this.form;
        $form.clk = this;
        if (this.type == 'image') {
            if (e.offsetX != undefined) {
                $form.clk_x = e.offsetX;
                $form.clk_y = e.offsetY;
            } else if (typeof $.fn.offset == 'function') { // try to use dimensions plugin
                var offset = $(this).offset();
                $form.clk_x = e.pageX - offset.left;
                $form.clk_y = e.pageY - offset.top;
            } else {
                $form.clk_x = e.pageX - this.offsetLeft;
                $form.clk_y = e.pageY - this.offsetTop;
            }
        }
        // clear form vars
        setTimeout(function () { $form.clk = $form.clk_x = $form.clk_y = null; }, 10);
    };

    function submitHandler() {
        // retrieve options from hash
        var id = this.formPluginId;
        var options = $.fn.ajaxForm.optionHash[id];
        $(this).ajaxSubmit(options);
        return false;
    };

    /**
    * ajaxFormUnbind unbinds the event handlers that were bound by ajaxForm
    *
    * @name   ajaxFormUnbind
    * @return jQuery
    * @cat    Plugins/Form
    * @type   jQuery
    */
    $.fn.ajaxFormUnbind = function () {
        this.unbind('submit', submitHandler);
        return this.each(function () {
            $(":submit,input:image", this).unbind('click', clickHandler);
        });

    };

    /**
    * formToArray() gathers form element data into an array of objects that can
    * be passed to any of the following ajax functions: $.get, $.post, or load.
    * Each object in the array has both a 'name' and 'value' property.  An example of
    * an array for a simple login form might be:
    *
    * [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
    *
    * It is this array that is passed to pre-submit callback functions provided to the
    * ajaxSubmit() and ajaxForm() methods.
    *
    * The semantic argument can be used to force form serialization in semantic order.
    * This is normally true anyway, unless the form contains input elements of type='image'.
    * If your form must be submitted with name/value pairs in semantic order and your form
    * contains an input of type='image" then pass true for this arg, otherwise pass false
    * (or nothing) to avoid the overhead for this logic.
    *
    * @example var data = $("#myForm").formToArray();
    * $.post( "myscript.cgi", data );
    * @desc Collect all the data from a form and submit it to the server.
    *
    * @name formToArray
    * @param semantic true if serialization must maintain strict semantic ordering of elements (slower)
    * @type Array<Object>
    * @cat Plugins/Form
    */
    $.fn.formToArray = function (semantic) {
        var a = [];
        if (this.length == 0) return a;

        var form = this[0];
        var els = semantic ? form.getElementsByTagName('*') : form.elements;
        if (!els) return a;
        for (var i = 0, max = els.length; i < max; i++) {
            var el = els[i];
            var n = el.name;
            if (!n) continue;

            if (semantic && form.clk && el.type == "image") {
                // handle image inputs on the fly when semantic == true
                if (!el.disabled && form.clk == el)
                    a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
                continue;
            }

            var v = $.fieldValue(el, true);
            if (v && v.constructor == Array) {
                for (var j = 0, jmax = v.length; j < jmax; j++)
                    a.push({ name: n, value: v[j] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: n, value: v });
        }

        if (!semantic && form.clk) {
            // input type=='image' are not found in elements array! handle them here
            var inputs = form.getElementsByTagName("input");
            for (var i = 0, max = inputs.length; i < max; i++) {
                var input = inputs[i];
                var n = input.name;
                if (n && !input.disabled && input.type == "image" && form.clk == input)
                    a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
            }
        }
        return a;
    };


    /**
    * Serializes form data into a 'submittable' string. This method will return a string
    * in the format: name1=value1&amp;name2=value2
    *
    * The semantic argument can be used to force form serialization in semantic order.
    * If your form must be submitted with name/value pairs in semantic order then pass
    * true for this arg, otherwise pass false (or nothing) to avoid the overhead for
    * this logic (which can be significant for very large forms).
    *
    * @example var data = $("#myForm").formSerialize();
    * $.ajax('POST', "myscript.cgi", data);
    * @desc Collect all the data from a form into a single string
    *
    * @name formSerialize
    * @param semantic true if serialization must maintain strict semantic ordering of elements (slower)
    * @type String
    * @cat Plugins/Form
    */
    $.fn.formSerialize = function (semantic) {
        //hand off to jQuery.param for proper encoding
        return $.param(this.formToArray(semantic));
    };


    /**
    * Serializes all field elements in the jQuery object into a query string.
    * This method will return a string in the format: name1=value1&amp;name2=value2
    *
    * The successful argument controls whether or not serialization is limited to
    * 'successful' controls (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
    * The default value of the successful argument is true.
    *
    * @example var data = $("input").formSerialize();
    * @desc Collect the data from all successful input elements into a query string
    *
    * @example var data = $(":radio").formSerialize();
    * @desc Collect the data from all successful radio input elements into a query string
    *
    * @example var data = $("#myForm :checkbox").formSerialize();
    * @desc Collect the data from all successful checkbox input elements in myForm into a query string
    *
    * @example var data = $("#myForm :checkbox").formSerialize(false);
    * @desc Collect the data from all checkbox elements in myForm (even the unchecked ones) into a query string
    *
    * @example var data = $(":input").formSerialize();
    * @desc Collect the data from all successful input, select, textarea and button elements into a query string
    *
    * @name fieldSerialize
    * @param successful true if only successful controls should be serialized (default is true)
    * @type String
    * @cat Plugins/Form
    */
    $.fn.fieldSerialize = function (successful) {
        var a = [];
        this.each(function () {
            var n = this.name;
            if (!n) return;
            var v = $.fieldValue(this, successful);
            if (v && v.constructor == Array) {
                for (var i = 0, max = v.length; i < max; i++)
                    a.push({ name: n, value: v[i] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: this.name, value: v });
        });
        //hand off to jQuery.param for proper encoding
        return $.param(a);
    };


    /**
    * Returns the value(s) of the element in the matched set.  For example, consider the following form:
    *
    *  <form><fieldset>
    *      <input name="A" type="text" />
    *      <input name="A" type="text" />
    *      <input name="B" type="checkbox" value="B1" />
    *      <input name="B" type="checkbox" value="B2"/>
    *      <input name="C" type="radio" value="C1" />
    *      <input name="C" type="radio" value="C2" />
    *  </fieldset></form>
    *
    *  var v = $(':text').fieldValue();
    *  // if no values are entered into the text inputs
    *  v == ['','']
    *  // if values entered into the text inputs are 'foo' and 'bar'
    *  v == ['foo','bar']
    *
    *  var v = $(':checkbox').fieldValue();
    *  // if neither checkbox is checked
    *  v === undefined
    *  // if both checkboxes are checked
    *  v == ['B1', 'B2']
    *
    *  var v = $(':radio').fieldValue();
    *  // if neither radio is checked
    *  v === undefined
    *  // if first radio is checked
    *  v == ['C1']
    *
    * The successful argument controls whether or not the field element must be 'successful'
    * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
    * The default value of the successful argument is true.  If this value is false the value(s)
    * for each element is returned.
    *
    * Note: This method *always* returns an array.  If no valid value can be determined the
    *       array will be empty, otherwise it will contain one or more values.
    *
    * @example var data = $("#myPasswordElement").fieldValue();
    * alert(data[0]);
    * @desc Alerts the current value of the myPasswordElement element
    *
    * @example var data = $("#myForm :input").fieldValue();
    * @desc Get the value(s) of the form elements in myForm
    *
    * @example var data = $("#myForm :checkbox").fieldValue();
    * @desc Get the value(s) for the successful checkbox element(s) in the jQuery object.
    *
    * @example var data = $("#mySingleSelect").fieldValue();
    * @desc Get the value(s) of the select control
    *
    * @example var data = $(':text').fieldValue();
    * @desc Get the value(s) of the text input or textarea elements
    *
    * @example var data = $("#myMultiSelect").fieldValue();
    * @desc Get the values for the select-multiple control
    *
    * @name fieldValue
    * @param Boolean successful true if only the values for successful controls should be returned (default is true)
    * @type Array<String>
    * @cat Plugins/Form
    */
    $.fn.fieldValue = function (successful) {
        for (var val = [], i = 0, max = this.length; i < max; i++) {
            var el = this[i];
            var v = $.fieldValue(el, successful);
            if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length))
                continue;
            v.constructor == Array ? $.merge(val, v) : val.push(v);
        }
        return val;
    };

    /**
    * Returns the value of the field element.
    *
    * The successful argument controls whether or not the field element must be 'successful'
    * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
    * The default value of the successful argument is true.  If the given element is not
    * successful and the successful arg is not false then the returned value will be null.
    *
    * Note: If the successful flag is true (default) but the element is not successful, the return will be null
    * Note: The value returned for a successful select-multiple element will always be an array.
    * Note: If the element has no value the return value will be undefined.
    *
    * @example var data = jQuery.fieldValue($("#myPasswordElement")[0]);
    * @desc Gets the current value of the myPasswordElement element
    *
    * @name fieldValue
    * @param Element el The DOM element for which the value will be returned
    * @param Boolean successful true if value returned must be for a successful controls (default is true)
    * @type String or Array<String> or null or undefined
    * @cat Plugins/Form
    */
    $.fieldValue = function (el, successful) {
        var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
        if (typeof successful == 'undefined') successful = true;

        if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
        (t == 'checkbox' || t == 'radio') && !el.checked ||
        (t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
        tag == 'select' && el.selectedIndex == -1))
            return null;

        if (tag == 'select') {
            var index = el.selectedIndex;
            if (index < 0) return null;
            var a = [], ops = el.options;
            var one = (t == 'select-one');
            var max = (one ? index + 1 : ops.length);
            for (var i = (one ? index : 0); i < max; i++) {
                var op = ops[i];
                if (op.selected) {
                    // extra pain for IE...
                    var v = $.browser.msie && !(op.attributes['value'].specified) ? op.text : op.value;
                    if (one) return v;
                    a.push(v);
                }
            }
            return a;
        }
        return el.value;
    };


    /**
    * Clears the form data.  Takes the following actions on the form's input fields:
    *  - input text fields will have their 'value' property set to the empty string
    *  - select elements will have their 'selectedIndex' property set to -1
    *  - checkbox and radio inputs will have their 'checked' property set to false
    *  - inputs of type submit, button, reset, and hidden will *not* be effected
    *  - button elements will *not* be effected
    *
    * @example $('form').clearForm();
    * @desc Clears all forms on the page.
    *
    * @name clearForm
    * @type jQuery
    * @cat Plugins/Form
    */
    $.fn.clearForm = function () {
        return this.each(function () {
            $('input,select,textarea', this).clearFields();
        });
    };

    /**
    * Clears the selected form elements.  Takes the following actions on the matched elements:
    *  - input text fields will have their 'value' property set to the empty string
    *  - select elements will have their 'selectedIndex' property set to -1
    *  - checkbox and radio inputs will have their 'checked' property set to false
    *  - inputs of type submit, button, reset, and hidden will *not* be effected
    *  - button elements will *not* be effected
    *
    * @example $('.myInputs').clearFields();
    * @desc Clears all inputs with class myInputs
    *
    * @name clearFields
    * @type jQuery
    * @cat Plugins/Form
    */
    $.fn.clearFields = $.fn.clearInputs = function () {
        return this.each(function () {
            var t = this.type, tag = this.tagName.toLowerCase();
            if (t == 'text' || t == 'password' || tag == 'textarea')
                this.value = '';
            else if (t == 'checkbox' || t == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = -1;
        });
    };


    /**
    * Resets the form data.  Causes all form elements to be reset to their original value.
    *
    * @example $('form').resetForm();
    * @desc Resets all forms on the page.
    *
    * @name resetForm
    * @type jQuery
    * @cat Plugins/Form
    */
    $.fn.resetForm = function () {
        return this.each(function () {
            // guard against an input with the name of 'reset'
            // note that IE reports the reset function as an 'object'
            if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType))
                this.reset();
        });
    };

})(jQuery);


//-----  解决错误  TypeError: $.handleError is not a function 
jQuery.extend({
    handleError: function (s, xhr, status, e) {
        if (s.error) {
            s.error.call(s.context || s, xhr, status, e);
        }
        if (s.global) {
            (s.context ? jQuery(s.context) : jQuery.event).trigger("ajaxError", [xhr, s, e]);
        }
    },
    httpData: function (xhr, type, s) {
        var ct = xhr.getResponseHeader("content-type"),
            xml = type == "xml" || !type && ct && ct.indexOf("xml") >= 0,
            data = xml ? xhr.responseXML : xhr.responseText;
        if (xml && data.documentElement.tagName == "parsererror")
            throw "parsererror";
        if (s && s.dataFilter)
            data = s.dataFilter(data, type);
        if (typeof data === "string") {
            if (type == "script")
                jQuery.globalEval(data);
            if (type == "json")
                data = window["eval"]("(" + data + ")");
        }
        return data;
    }
});



