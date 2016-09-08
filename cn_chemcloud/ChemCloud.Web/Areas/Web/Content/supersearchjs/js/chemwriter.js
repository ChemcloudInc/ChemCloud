(function() {
    function e() {
        return function() {}
    }
    function aa(a) {
        return function(b) {
            this[a] = b
        }
    }
    function h(a) {
        return function() {
            return this[a]
        }
    }
    function p(a) {
        return function() {
            return a
        }
    }
    var q, s = this;
    function ba(a, b) {
        var c = a.split("."),
        d = s;
        c[0] in d || !d.execScript || d.execScript("var " + c[0]);
        for (var f; c.length && (f = c.shift());) c.length || void 0 === b ? d = d[f] ? d[f] : d[f] = {}: d[f] = b
    }
    function ca(a) {
        var b = typeof a;
        if ("object" == b) if (a) {
            if (a instanceof Array) return "array";
            if (a instanceof Object) return b;
            var c = Object.prototype.toString.call(a);
            if ("[object Window]" == c) return "object";
            if ("[object Array]" == c || "number" == typeof a.length && "undefined" != typeof a.splice && "undefined" != typeof a.propertyIsEnumerable && !a.propertyIsEnumerable("splice")) return "array";
            if ("[object Function]" == c || "undefined" != typeof a.call && "undefined" != typeof a.propertyIsEnumerable && !a.propertyIsEnumerable("call")) return "function"
        } else return "null";
        else if ("function" == b && "undefined" == typeof a.call) return "object";
        return b
    }
    function da(a) {
        return void 0 !== a
    }
    function ea(a) {
        return "array" == ca(a)
    }
    function fa(a) {
        var b = ca(a);
        return "array" == b || "object" == b && "number" == typeof a.length
    }
    function t(a) {
        return "string" == typeof a
    }
    function u(a) {
        return "number" == typeof a
    }
    function ga(a) {
        var b = typeof a;
        return "object" == b && null != a || "function" == b
    }
    function v(a) {
        return a[ha] || (a[ha] = ++ia)
    }
    var ha = "closure_uid_" + (1E9 * Math.random() >>> 0),
    ia = 0;
    function ja(a, b, c) {
        return a.call.apply(a.bind, arguments)
    }
    function la(a, b, c) {
        if (!a) throw Error();
        if (2 < arguments.length) {
            var d = Array.prototype.slice.call(arguments, 2);
            return function() {
                var c = Array.prototype.slice.call(arguments);
                Array.prototype.unshift.apply(c, d);
                return a.apply(b, c)
            }
        }
        return function() {
            return a.apply(b, arguments)
        }
    }
    function ma(a, b, c) {
        ma = Function.prototype.bind && -1 != Function.prototype.bind.toString().indexOf("native code") ? ja: la;
        return ma.apply(null, arguments)
    }
    var na = Date.now ||
    function() {
        return + new Date
    };
    function w(a, b) {
        function c() {}
        c.prototype = b.prototype;
        a.da = b.prototype;
        a.prototype = new c;
        a.prototype.constructor = a
    };
    var oa;
    function pa(a) {
        if (!qa.test(a)) return a; - 1 != a.indexOf("&") && (a = a.replace(ra, "&amp;")); - 1 != a.indexOf("<") && (a = a.replace(sa, "&lt;")); - 1 != a.indexOf(">") && (a = a.replace(ta, "&gt;")); - 1 != a.indexOf('"') && (a = a.replace(ua, "&quot;"));
        return a
    }
    var ra = /&/g,
    sa = /</g,
    ta = />/g,
    ua = /\"/g,
    qa = /[&<>\"]/;
    var y = Array.prototype,
    va = y.indexOf ?
    function(a, b, c) {
        return y.indexOf.call(a, b, c)
    }: function(a, b, c) {
        c = null == c ? 0 : 0 > c ? Math.max(0, a.length + c) : c;
        if (t(a)) return t(b) && 1 == b.length ? a.indexOf(b, c) : -1;
        for (; c < a.length; c++) if (c in a && a[c] === b) return c;
        return - 1
    },
    wa = y.forEach ?
    function(a, b, c) {
        y.forEach.call(a, b, c)
    }: function(a, b, c) {
        for (var d = a.length,
        f = t(a) ? a.split("") : a, g = 0; g < d; g++) g in f && b.call(c, f[g], g, a)
    },
    xa = y.filter ?
    function(a, b, c) {
        return y.filter.call(a, b, c)
    }: function(a, b, c) {
        for (var d = a.length,
        f = [], g = 0, k = t(a) ? a.split("") : a, l = 0; l < d; l++) if (l in k) {
            var m = k[l];
            b.call(c, m, l, a) && (f[g++] = m)
        }
        return f
    };
    function ya(a) {
        if (!ea(a)) for (var b = a.length - 1; 0 <= b; b--) delete a[b];
        a.length = 0
    }
    function za(a, b) {
        var c = va(a, b),
        d; (d = 0 <= c) && y.splice.call(a, c, 1);
        return d
    }
    function Aa(a) {
        var b = a.length;
        if (0 < b) {
            for (var c = Array(b), d = 0; d < b; d++) c[d] = a[d];
            return c
        }
        return []
    }
    function Ba(a, b, c, d) {
        y.splice.apply(a, Ca(arguments, 1))
    }
    function Ca(a, b, c) {
        return 2 >= arguments.length ? y.slice.call(a, b) : y.slice.call(a, b, c)
    }
    function Da(a, b) {
        return a > b ? 1 : a < b ? -1 : 0
    };
    var Ea, Fa, Ga, Ha, z, Ia;
    function Ja() {
        return s.navigator ? s.navigator.userAgent: null
    }
    Ha = Ga = Fa = Ea = !1;
    var Ka;
    if (Ka = Ja()) {
        var La = s.navigator;
        Ea = 0 == Ka.lastIndexOf("Opera", 0);
        Fa = !Ea && ( - 1 != Ka.indexOf("MSIE") || -1 != Ka.indexOf("Trident"));
        Ga = !Ea && -1 != Ka.indexOf("WebKit");
        Ha = !Ea && !Ga && !Fa && "Gecko" == La.product
    }
    var Ma = Ea,
    A = Fa,
    B = Ha,
    C = Ga,
    Na, Oa = s.navigator;
    Na = Oa && Oa.platform || "";
    z = -1 != Na.indexOf("Mac");
    Ia = -1 != Na.indexOf("Win");
    var Pa = -1 != Na.indexOf("Linux");
    function Qa() {
        var a = s.document;
        return a ? a.documentMode: void 0
    }
    var Ra;
    a: {
        var Sa = "",
        Ta;
        if (Ma && s.opera) var Ua = s.opera.version,
        Sa = "function" == typeof Ua ? Ua() : Ua;
        else if (B ? Ta = /rv\:([^\);]+)(\)|;)/: A ? Ta = /\b(?:MSIE|rv)[: ]([^\);]+)(\)|;)/: C && (Ta = /WebKit\/(\S+)/), Ta) var Va = Ta.exec(Ja()),
        Sa = Va ? Va[1] : "";
        if (A) {
            var Wa = Qa();
            if (Wa > parseFloat(Sa)) {
                Ra = String(Wa);
                break a
            }
        }
        Ra = Sa
    }
    var Xa = {};
    function D(a) {
        var b;
        if (! (b = Xa[a])) {
            b = 0;
            for (var c = String(Ra).replace(/^[\s\xa0]+|[\s\xa0]+$/g, "").split("."), d = String(a).replace(/^[\s\xa0]+|[\s\xa0]+$/g, "").split("."), f = Math.max(c.length, d.length), g = 0; 0 == b && g < f; g++) {
                var k = c[g] || "",
                l = d[g] || "",
                m = RegExp("(\\d*)(\\D*)", "g"),
                r = RegExp("(\\d*)(\\D*)", "g");
                do {
                    var x = m.exec(k) || ["", "", ""], n = r.exec(l) || ["", "", ""];
                    if (0 == x[0].length && 0 == n[0].length) break;
                    b = ((0 == x[1].length ? 0 : parseInt(x[1], 10)) < (0 == n[1].length ? 0 : parseInt(n[1], 10)) ? -1 : (0 == x[1].length ? 0 : parseInt(x[1], 10)) > (0 == n[1].length ? 0 : parseInt(n[1], 10)) ? 1 : 0) || ((0 == x[2].length) < (0 == n[2].length) ? -1 : (0 == x[2].length) > (0 == n[2].length) ? 1 : 0) || (x[2] < n[2] ? -1 : x[2] > n[2] ? 1 : 0)
                } while ( 0 == b )
            }
            b = Xa[a] = 0 <= b
        }
        return b
    }
    var Ya = s.document,
    Za = Ya && A ? Qa() || ("CSS1Compat" == Ya.compatMode ? parseInt(Ra, 10) : 5) : void 0;
    var $a = !A || A && 9 <= Za; ! B && !A || A && A && 9 <= Za || B && D("1.9.1");
    A && D("9");
    function ab(a) {
        a = a.className;
        return t(a) && a.match(/\S+/g) || []
    }
    function bb(a, b) {
        for (var c = ab(a), d = Ca(arguments, 1), f = c.length + d.length, g = c, k = 0; k < d.length; k++) 0 <= va(g, d[k]) || g.push(d[k]);
        a.className = c.join(" ");
        return c.length == f
    }
    function cb(a, b) {
        var c = ab(a),
        d = Ca(arguments, 1),
        c = db(c, d);
        a.className = c.join(" ")
    }
    function db(a, b) {
        return xa(a,
        function(a) {
            return ! (0 <= va(b, a))
        })
    };
    function E(a, b) {
        this.x = da(a) ? a: 0;
        this.y = da(b) ? b: 0
    }
    q = E.prototype;
    q.va = function() {
        return new E(this.x, this.y)
    };
    q.toString = function() {
        return "(" + this.x + ", " + this.y + ")"
    };
    q.ceil = function() {
        this.x = Math.ceil(this.x);
        this.y = Math.ceil(this.y);
        return this
    };
    q.floor = function() {
        this.x = Math.floor(this.x);
        this.y = Math.floor(this.y);
        return this
    };
    q.round = function() {
        this.x = Math.round(this.x);
        this.y = Math.round(this.y);
        return this
    };
    q.translate = function(a, b) {
        a instanceof E ? (this.x += a.x, this.y += a.y) : (this.x += a, u(b) && (this.y += b));
        return this
    };
    q.scale = function(a, b) {
        var c = u(b) ? b: a;
        this.x *= a;
        this.y *= c;
        return this
    };
    function eb(a, b) {
        this.width = a;
        this.height = b
    }
    q = eb.prototype;
    q.va = function() {
        return new eb(this.width, this.height)
    };
    q.toString = function() {
        return "(" + this.width + " x " + this.height + ")"
    };
    q.Ma = function() {
        return ! (this.width * this.height)
    };
    q.ceil = function() {
        this.width = Math.ceil(this.width);
        this.height = Math.ceil(this.height);
        return this
    };
    q.floor = function() {
        this.width = Math.floor(this.width);
        this.height = Math.floor(this.height);
        return this
    };
    q.round = function() {
        this.width = Math.round(this.width);
        this.height = Math.round(this.height);
        return this
    };
    q.scale = function(a, b) {
        var c = u(b) ? b: a;
        this.width *= a;
        this.height *= c;
        return this
    };
    function fb(a, b) {
        for (var c in a) b.call(void 0, a[c], c, a)
    }
    var gb = "constructor hasOwnProperty isPrototypeOf propertyIsEnumerable toLocaleString toString valueOf".split(" ");
    function hb(a, b) {
        for (var c, d, f = 1; f < arguments.length; f++) {
            d = arguments[f];
            for (c in d) a[c] = d[c];
            for (var g = 0; g < gb.length; g++) c = gb[g],
            Object.prototype.hasOwnProperty.call(d, c) && (a[c] = d[c])
        }
    }
    function ib(a) {
        var b = arguments.length;
        if (1 == b && ea(arguments[0])) return ib.apply(null, arguments[0]);
        for (var c = {},
        d = 0; d < b; d++) c[arguments[d]] = !0;
        return c
    };
    function jb(a, b) {
        fb(b,
        function(b, d) {
            "style" == d ? a.style.cssText = b: "class" == d ? a.className = b: "for" == d ? a.htmlFor = b: d in kb ? a.setAttribute(kb[d], b) : 0 == d.lastIndexOf("aria-", 0) || 0 == d.lastIndexOf("data-", 0) ? a.setAttribute(d, b) : a[d] = b
        })
    }
    var kb = {
        cellpadding: "cellPadding",
        cellspacing: "cellSpacing",
        colspan: "colSpan",
        frameborder: "frameBorder",
        height: "height",
        maxlength: "maxLength",
        role: "role",
        rowspan: "rowSpan",
        type: "type",
        usemap: "useMap",
        valign: "vAlign",
        width: "width"
    };
    function lb(a) {
        a = a.document;
        a = "CSS1Compat" == a.compatMode ? a.documentElement: a.body;
        return new eb(a.clientWidth, a.clientHeight)
    }
    function F(a, b, c) {
        return mb(document, arguments)
    }
    function mb(a, b) {
        var c = b[0],
        d = b[1];
        if (!$a && d && (d.name || d.type)) {
            c = ["<", c];
            d.name && c.push(' name="', pa(d.name), '"');
            if (d.type) {
                c.push(' type="', pa(d.type), '"');
                var f = {};
                hb(f, d);
                delete f.type;
                d = f
            }
            c.push(">");
            c = c.join("")
        }
        c = a.createElement(c);
        d && (t(d) ? c.className = d: ea(d) ? bb.apply(null, [c].concat(d)) : jb(c, d));
        2 < b.length && nb(a, c, b, 2);
        return c
    }
    function nb(a, b, c, d) {
        function f(c) {
            c && b.appendChild(t(c) ? a.createTextNode(c) : c)
        }
        for (; d < c.length; d++) {
            var g = c[d]; ! fa(g) || ga(g) && 0 < g.nodeType ? f(g) : wa(ob(g) ? Aa(g) : g, f)
        }
    }
    function pb(a) {
        return a && a.parentNode ? a.parentNode.removeChild(a) : null
    }
    function ob(a) {
        if (a && "number" == typeof a.length) {
            if (ga(a)) return "function" == typeof a.item || "string" == typeof a.item;
            if ("function" == ca(a)) return "function" == typeof a.item
        }
        return ! 1
    }
    function qb(a) {
        this.ba = a || s.document || document
    }
    q = qb.prototype;
    q.Be = aa("ba");
    q.ia = h("ba");
    q.aa = function(a) {
        return t(a) ? this.ba.getElementById(a) : a
    };
    q.xa = function(a, b, c) {
        return mb(this.ba, arguments)
    };
    q.createElement = function(a) {
        return this.ba.createElement(a)
    };
    q.createTextNode = function(a) {
        return this.ba.createTextNode(String(a))
    };
    q.appendChild = function(a, b) {
        a.appendChild(b)
    };
    q.append = function(a, b) {
        nb(9 == a.nodeType ? a: a.ownerDocument || a.document, a, arguments, 1)
    };
    q.removeNode = pb;
    q.contains = function(a, b) {
        if (a.contains && 1 == b.nodeType) return a == b || a.contains(b);
        if ("undefined" != typeof a.compareDocumentPosition) return a == b || Boolean(a.compareDocumentPosition(b) & 16);
        for (; b && a != b;) b = b.parentNode;
        return b == a
    };
    var rb = !A || A && 9 <= Za,
    sb = A && !D("9"); ! C || D("528");
    B && D("1.9b") || A && D("8") || Ma && D("9.5") || C && D("528");
    B && !D("8") || A && D("9");
    function tb() {
        0 != ub && (this.Aj = Error().stack, vb[v(this)] = this)
    }
    var ub = 0,
    vb = {};
    tb.prototype.Fg = !1;
    tb.prototype.Le = function() {
        if (!this.Fg && (this.Fg = !0, this.fb(), 0 != ub)) {
            var a = v(this);
            delete vb[a]
        }
    };
    tb.prototype.fb = function() {
        if (this.mh) for (; this.mh.length;) this.mh.shift()()
    };
    function G(a, b) {
        this.type = a;
        this.currentTarget = this.target = b
    }
    q = G.prototype;
    q.fb = e();
    q.Le = e();
    q.Nc = !1;
    q.defaultPrevented = !1;
    q.Ah = !0;
    q.stopPropagation = function() {
        this.Nc = !0
    };
    q.preventDefault = function() {
        this.defaultPrevented = !0;
        this.Ah = !1
    };
    function wb(a) {
        wb[" "](a);
        return a
    }
    wb[" "] = e();
    function xb(a, b) {
        a && this.le(a, b)
    }
    w(xb, G);
    q = xb.prototype;
    q.target = null;
    q.relatedTarget = null;
    q.offsetX = 0;
    q.offsetY = 0;
    q.clientX = 0;
    q.clientY = 0;
    q.screenX = 0;
    q.screenY = 0;
    q.button = 0;
    q.keyCode = 0;
    q.charCode = 0;
    q.ctrlKey = !1;
    q.altKey = !1;
    q.shiftKey = !1;
    q.metaKey = !1;
    q.jj = !1;
    q.Cc = null;
    q.le = function(a, b) {
        var c = this.type = a.type;
        G.call(this, c);
        this.target = a.target || a.srcElement;
        this.currentTarget = b;
        var d = a.relatedTarget;
        if (d) {
            if (B) {
                var f;
                a: {
                    try {
                        wb(d.nodeName);
                        f = !0;
                        break a
                    } catch(g) {}
                    f = !1
                }
                f || (d = null)
            }
        } else "mouseover" == c ? d = a.fromElement: "mouseout" == c && (d = a.toElement);
        this.relatedTarget = d;
        this.offsetX = C || void 0 !== a.offsetX ? a.offsetX: a.layerX;
        this.offsetY = C || void 0 !== a.offsetY ? a.offsetY: a.layerY;
        this.clientX = void 0 !== a.clientX ? a.clientX: a.pageX;
        this.clientY = void 0 !== a.clientY ? a.clientY: a.pageY;
        this.screenX = a.screenX || 0;
        this.screenY = a.screenY || 0;
        this.button = a.button;
        this.keyCode = a.keyCode || 0;
        this.charCode = a.charCode || ("keypress" == c ? a.keyCode: 0);
        this.ctrlKey = a.ctrlKey;
        this.altKey = a.altKey;
        this.shiftKey = a.shiftKey;
        this.metaKey = a.metaKey;
        this.jj = z ? a.metaKey: a.ctrlKey;
        this.state = a.state;
        this.Cc = a;
        a.defaultPrevented && this.preventDefault();
        delete this.Nc
    };
    q.stopPropagation = function() {
        xb.da.stopPropagation.call(this);
        this.Cc.stopPropagation ? this.Cc.stopPropagation() : this.Cc.cancelBubble = !0
    };
    q.preventDefault = function() {
        xb.da.preventDefault.call(this);
        var a = this.Cc;
        if (a.preventDefault) a.preventDefault();
        else if (a.returnValue = !1, sb) try {
            if (a.ctrlKey || 112 <= a.keyCode && 123 >= a.keyCode) a.keyCode = -1
        } catch(b) {}
    };
    q.fb = e();
    var yb = "closure_listenable_" + (1E6 * Math.random() | 0);
    function zb(a) {
        try {
            return ! (!a || !a[yb])
        } catch(b) {
            return ! 1
        }
    }
    var Ab = 0;
    function Bb(a, b, c, d, f) {
        this.hd = a;
        this.qf = null;
        this.src = b;
        this.type = c;
        this.capture = !!d;
        this.Se = f;
        this.key = ++Ab;
        this.Md = this.Je = !1
    }
    function Cb(a) {
        a.Md = !0;
        a.hd = null;
        a.qf = null;
        a.src = null;
        a.Se = null
    };
    function Db(a) {
        this.src = a;
        this.ob = {};
        this.Fe = 0
    }
    Db.prototype.add = function(a, b, c, d, f) {
        var g = this.ob[a];
        g || (g = this.ob[a] = [], this.Fe++);
        var k = Eb(g, b, d, f); - 1 < k ? (a = g[k], c || (a.Je = !1)) : (a = new Bb(b, this.src, a, !!d, f), a.Je = c, g.push(a));
        return a
    };
    Db.prototype.remove = function(a, b, c, d) {
        if (! (a in this.ob)) return ! 1;
        var f = this.ob[a];
        b = Eb(f, b, c, d);
        return - 1 < b ? (Cb(f[b]), y.splice.call(f, b, 1), 0 == f.length && (delete this.ob[a], this.Fe--), !0) : !1
    };
    function Fb(a, b) {
        var c = b.type;
        c in a.ob && za(a.ob[c], b) && (Cb(b), 0 == a.ob[c].length && (delete a.ob[c], a.Fe--))
    }
    function Gb(a, b) {
        var c = 0,
        d;
        for (d in a.ob) if (!b || d == b) {
            for (var f = a.ob[d], g = 0; g < f.length; g++)++c,
            Cb(f[g]);
            delete a.ob[d];
            a.Fe--
        }
    }
    Db.prototype.Qf = function(a, b, c, d) {
        a = this.ob[a];
        var f = -1;
        a && (f = Eb(a, b, c, d));
        return - 1 < f ? a[f] : null
    };
    function Eb(a, b, c, d) {
        for (var f = 0; f < a.length; ++f) {
            var g = a[f];
            if (!g.Md && g.hd == b && g.capture == !!c && g.Se == d) return f
        }
        return - 1
    };
    var Hb = "closure_lm_" + (1E6 * Math.random() | 0),
    Ib = {},
    Jb = 0;
    function I(a, b, c, d, f) {
        if (ea(b)) {
            for (var g = 0; g < b.length; g++) I(a, b[g], c, d, f);
            return null
        }
        c = Kb(c);
        return zb(a) ? a.oc.add(String(b), c, !1, d, f) : Lb(a, b, c, !1, d, f)
    }
    function Lb(a, b, c, d, f, g) {
        if (!b) throw Error("Invalid event type");
        var k = !!f,
        l = Mb(a);
        l || (a[Hb] = l = new Db(a));
        c = l.add(b, c, d, f, g);
        if (c.qf) return c;
        d = Nb();
        c.qf = d;
        d.src = a;
        d.hd = c;
        a.addEventListener ? a.addEventListener(b, d, k) : a.attachEvent(b in Ib ? Ib[b] : Ib[b] = "on" + b, d);
        Jb++;
        return c
    }
    function Nb() {
        var a = Ob,
        b = rb ?
        function(c) {
            return a.call(b.src, b.hd, c)
        }: function(c) {
            c = a.call(b.src, b.hd, c);
            if (!c) return c
        };
        return b
    }
    function Pb(a, b, c, d, f) {
        if (ea(b)) for (var g = 0; g < b.length; g++) Pb(a, b[g], c, d, f);
        else c = Kb(c),
        zb(a) ? a.oc.add(String(b), c, !0, d, f) : Lb(a, b, c, !0, d, f)
    }
    function J(a, b, c, d, f) {
        if (ea(b)) for (var g = 0; g < b.length; g++) J(a, b[g], c, d, f);
        else c = Kb(c),
        zb(a) ? a.oc.remove(String(b), c, d, f) : a && (a = Mb(a)) && (b = a.Qf(b, c, !!d, f)) && Qb(b)
    }
    function Qb(a) {
        if (!u(a) && a && !a.Md) {
            var b = a.src;
            if (zb(b)) Fb(b.oc, a);
            else {
                var c = a.type,
                d = a.qf;
                b.removeEventListener ? b.removeEventListener(c, d, a.capture) : b.detachEvent && b.detachEvent(c in Ib ? Ib[c] : Ib[c] = "on" + c, d);
                Jb--; (c = Mb(b)) ? (Fb(c, a), 0 == c.Fe && (c.src = null, b[Hb] = null)) : Cb(a)
            }
        }
    }
    function Rb(a, b, c, d) {
        var f = 1;
        if (a = Mb(a)) if (b = a.ob[b]) for (b = Aa(b), a = 0; a < b.length; a++) {
            var g = b[a];
            g && (g.capture == c && !g.Md) && (f &= !1 !== Sb(g, d))
        }
        return Boolean(f)
    }
    function Sb(a, b) {
        var c = a.hd,
        d = a.Se || a.src;
        a.Je && Qb(a);
        return c.call(d, b)
    }
    function Ob(a, b) {
        if (a.Md) return ! 0;
        if (!rb) {
            var c;
            if (! (c = b)) a: {
                c = ["window", "event"];
                for (var d = s,
                f; f = c.shift();) if (null != d[f]) d = d[f];
                else {
                    c = null;
                    break a
                }
                c = d
            }
            f = c;
            c = new xb(f, this);
            d = !0;
            if (! (0 > f.keyCode || void 0 != f.returnValue)) {
                a: {
                    var g = !1;
                    if (0 == f.keyCode) try {
                        f.keyCode = -1;
                        break a
                    } catch(k) {
                        g = !0
                    }
                    if (g || void 0 == f.returnValue) f.returnValue = !0
                }
                f = [];
                for (g = c.currentTarget; g; g = g.parentNode) f.push(g);
                for (var g = a.type,
                l = f.length - 1; ! c.Nc && 0 <= l; l--) c.currentTarget = f[l],
                d &= Rb(f[l], g, !0, c);
                for (l = 0; ! c.Nc && l < f.length; l++) c.currentTarget = f[l],
                d &= Rb(f[l], g, !1, c)
            }
            return d
        }
        return Sb(a, new xb(b, this))
    }
    function Mb(a) {
        a = a[Hb];
        return a instanceof Db ? a: null
    }
    var Tb = "__closure_events_fn_" + (1E9 * Math.random() >>> 0);
    function Kb(a) {
        return "function" == ca(a) ? a: a[Tb] || (a[Tb] = function(b) {
            return a.handleEvent(b)
        })
    };
    var Ub = Array(4).join(String.fromCharCode(119));
    function K(a, b, c, d) {
        a = a.substring(b, c).trim();
        if (a === Vb) return 0;
        a -= 0;
        if (isNaN(a)) throw Wb;
        return a ? a: d || 0
    }
    function L(a, b, c, d) {
        Xb(a, b, c, -1, d)
    }
    function Xb(a, b, c, d, f) {
        a.toFixed && (a = a.toFixed(f || 0));
        c -= a.length;
        if (0 > c) throw Yb;
        f = "";
        for (var g = 0; g < c; g++) f += " ";
        1 === d ? (b.append(a), b.append(f)) : -1 === d && (b.append(f), b.append(a))
    }
    var Zb = /\\n/g,
    Vb = "",
    Wb = "string-number-format",
    Yb = "string-too-long";
    function $b(a) {
        var b = a.getAttribute("id"),
        b = b ? F("div", {
            "class": "chemwriter",
            id: b
        }) : F("div", {
            "class": "chemwriter"
        }),
        c = a.getAttribute("data-chemwriter-width");
        a = (a = a.getAttribute("data-chemwriter-height")) ? a + "px": "100%";
        c = c ? c + "px": "100%";
        if (c instanceof eb) a = c.height,
        c = c.width;
        else if (void 0 == a) throw Error("missing height argument");
        b.style.width = ac(c);
        b.style.height = ac(a);
        return b
    }
    function bc(a, b) {
        var c = document.createElementNS("http://www.w3.org/2000/svg", a);
        if (b) for (var d in b) c.setAttribute(d, b[d]);
        return c
    };
    function M() {
        tb.call(this);
        this.oc = new Db(this);
        this.Mh = this
    }
    w(M, tb);
    M.prototype[yb] = !0;
    q = M.prototype;
    q.pf = null;
    q.Ta = aa("pf");
    q.addEventListener = function(a, b, c, d) {
        I(this, a, b, c, d)
    };
    q.removeEventListener = function(a, b, c, d) {
        J(this, a, b, c, d)
    };
    q.dispatchEvent = function(a) {
        var b, c = this.pf;
        if (c) for (b = []; c; c = c.pf) b.push(c);
        var c = this.Mh,
        d = a.type || a;
        if (t(a)) a = new G(a, c);
        else if (a instanceof G) a.target = a.target || c;
        else {
            var f = a;
            a = new G(d, c);
            hb(a, f)
        }
        var f = !0,
        g;
        if (b) for (var k = b.length - 1; ! a.Nc && 0 <= k; k--) g = a.currentTarget = b[k],
        f = cc(g, d, !0, a) && f;
        a.Nc || (g = a.currentTarget = c, f = cc(g, d, !0, a) && f, a.Nc || (f = cc(g, d, !1, a) && f));
        if (b) for (k = 0; ! a.Nc && k < b.length; k++) g = a.currentTarget = b[k],
        f = cc(g, d, !1, a) && f;
        return f
    };
    q.fb = function() {
        M.da.fb.call(this);
        this.oc && Gb(this.oc, void 0);
        this.pf = null
    };
    function cc(a, b, c, d) {
        b = a.oc.ob[String(b)];
        if (!b) return ! 0;
        b = Aa(b);
        for (var f = !0,
        g = 0; g < b.length; ++g) {
            var k = b[g];
            if (k && !k.Md && k.capture == c) {
                var l = k.hd,
                m = k.Se || k.src;
                k.Je && Fb(a.oc, k);
                f = !1 !== l.call(m, d) && f
            }
        }
        return f && !1 != d.Ah
    }
    q.Qf = function(a, b, c, d) {
        return this.oc.Qf(String(a), b, c, d)
    };
    function dc(a, b, c, d) {
        this.top = a;
        this.right = b;
        this.bottom = c;
        this.left = d
    }
    q = dc.prototype;
    q.va = function() {
        return new dc(this.top, this.right, this.bottom, this.left)
    };
    q.toString = function() {
        return "(" + this.top + "t, " + this.right + "r, " + this.bottom + "b, " + this.left + "l)"
    };
    q.contains = function(a) {
        return this && a ? a instanceof dc ? a.left >= this.left && a.right <= this.right && a.top >= this.top && a.bottom <= this.bottom: a.x >= this.left && a.x <= this.right && a.y >= this.top && a.y <= this.bottom: !1
    };
    q.ceil = function() {
        this.top = Math.ceil(this.top);
        this.right = Math.ceil(this.right);
        this.bottom = Math.ceil(this.bottom);
        this.left = Math.ceil(this.left);
        return this
    };
    q.floor = function() {
        this.top = Math.floor(this.top);
        this.right = Math.floor(this.right);
        this.bottom = Math.floor(this.bottom);
        this.left = Math.floor(this.left);
        return this
    };
    q.round = function() {
        this.top = Math.round(this.top);
        this.right = Math.round(this.right);
        this.bottom = Math.round(this.bottom);
        this.left = Math.round(this.left);
        return this
    };
    q.translate = function(a, b) {
        a instanceof E ? (this.left += a.x, this.right += a.x, this.top += a.y, this.bottom += a.y) : (this.left += a, this.right += a, u(b) && (this.top += b, this.bottom += b));
        return this
    };
    q.scale = function(a, b) {
        var c = u(b) ? b: a;
        this.left *= a;
        this.right *= a;
        this.top *= c;
        this.bottom *= c;
        return this
    };
    function ac(a) {
        "number" == typeof a && (a = Math.round(a) + "px");
        return a
    };
    function ec() {}
    ec.Lg = function() {
        return ec.Wg ? ec.Wg: ec.Wg = new ec
    };
    ec.prototype.qi = 0;
    ec.Lg();
    function N(a) {
        M.call(this);
        a || (a = oa || (oa = new qb));
        this.Hg = a;
        this.nj = fc
    }
    w(N, M);
    N.prototype.ii = ec.Lg();
    var fc = null;
    q = N.prototype;
    q.Dd = null;
    q.Xb = !1;
    q.ea = null;
    q.nj = null;
    q.ni = null;
    q.Xa = null;
    q.xb = null;
    q.Rb = null;
    q.uj = !1;
    function gc(a) {
        return a.Dd || (a.Dd = ":" + (a.ii.qi++).toString(36))
    }
    q.aa = h("ea");
    q.Pc = function(a) {
        if (this == a) throw Error("Unable to set parent component");
        if (a && this.Xa && this.Dd && this.Xa.Rb && this.Dd && (this.Dd in this.Xa.Rb && this.Xa.Rb[this.Dd]) && this.Xa != a) throw Error("Unable to set parent component");
        this.Xa = a;
        N.da.Ta.call(this, a)
    };
    q.getParent = h("Xa");
    q.Ta = function(a) {
        if (this.Xa && this.Xa != a) throw Error("Method not supported");
        N.da.Ta.call(this, a)
    };
    q.xa = function() {
        this.ea = this.Hg.createElement("div")
    };
    function hc(a, b, c) {
        if (a.Xb) throw Error("Component already rendered");
        a.ea || a.xa();
        b ? b.insertBefore(a.ea, c || null) : a.Hg.ia().body.appendChild(a.ea);
        a.Xa && !a.Xa.Xb || a.ma()
    }
    q.ma = function() {
        this.Xb = !0;
        ic(this,
        function(a) { ! a.Xb && a.aa() && a.ma()
        })
    };
    q.de = function() {
        ic(this,
        function(a) {
            a.Xb && a.de()
        });
        this.Re && Gb(this.Re);
        this.Xb = !1
    };
    q.fb = function() {
        this.Xb && this.de();
        this.Re && (this.Re.Le(), delete this.Re);
        ic(this,
        function(a) {
            a.Le()
        }); ! this.uj && this.ea && pb(this.ea);
        this.Xa = this.ni = this.ea = this.Rb = this.xb = null;
        N.da.fb.call(this)
    };
    function O(a, b) {
        jc(a, b, a.xb ? a.xb.length: 0)
    }
    function jc(a, b, c) {
        if (b.Xb) throw Error("Component already rendered");
        if (0 > c || c > (a.xb ? a.xb.length: 0)) throw Error("Child component index out of bounds");
        a.Rb && a.xb || (a.Rb = {},
        a.xb = []);
        if (b.getParent() == a) {
            var d = gc(b);
            a.Rb[d] = b;
            za(a.xb, b)
        } else {
            var d = a.Rb,
            f = gc(b);
            if (f in d) throw Error('The object already contains the key "' + f + '"');
            d[f] = b
        }
        b.Pc(a);
        Ba(a.xb, c, 0, b);
        b.Xb && a.Xb && b.getParent() == a ? (a = a.Kf(), a.insertBefore(b.aa(), a.childNodes[c] || null)) : (a.ea || a.xa(), c = a.xb ? a.xb[c + 1] || null: null, hc(b, a.Kf(), c ? c.ea: null))
    }
    q.Kf = h("ea");
    function ic(a, b) {
        a.xb && wa(a.xb, b, void 0)
    }
    q.removeChild = function(a, b) {
        if (a) {
            var c = t(a) ? a: gc(a);
            a = this.Rb && c ? (c in this.Rb ? this.Rb[c] : void 0) || null: null;
            if (c && a) {
                var d = this.Rb;
                c in d && delete d[c];
                za(this.xb, a);
                b && (a.de(), a.ea && pb(a.ea));
                a.Pc(null)
            }
        }
        if (!a) throw Error("Child is not in parent component");
        return a
    };
    function kc(a, b, c, d) {
        this.ya = a;
        this.za = b;
        this.$a = c;
        this.ab = d
    }
    kc.prototype.va = function() {
        return new kc(this.ya, this.za, this.$a, this.ab)
    };
    function lc(a, b) {
        this.x = a;
        this.y = b
    }
    w(lc, E);
    lc.prototype.va = function() {
        return new lc(this.x, this.y)
    };
    lc.prototype.scale = E.prototype.scale;
    lc.prototype.add = function(a) {
        this.x += a.x;
        this.y += a.y;
        return this
    };
    lc.prototype.rotate = function(a) {
        var b = Math.cos(a);
        a = Math.sin(a);
        var c = this.y * b + this.x * a;
        this.x = this.x * b - this.y * a;
        this.y = c;
        return this
    };
    function mc(a, b, c) {
        a = a.va();
        a.x -= b.x;
        a.y -= b.y;
        return a.rotate(c).add(b)
    };
    function nc(a) {
        var b = a.ja;
        if (0 === b.length) return 1;
        a = [];
        for (var c = 0; c < b.length; c++) {
            var d = b[c];
            a.push(oc(d.ka.ca, d.pa.ca))
        }
        y.sort.call(a, Da);
        return 0 === a.length % 2 ? (b = a.length / 2, (a[b - 1] + a[b]) / 2) : a[(a.length - 1) / 2]
    }
    function pc(a, b, c, d) {
        return Math.sqrt((c - a) * (c - a) + (d - b) * (d - b))
    }
    function oc(a, b) {
        var c = a.x - b.x,
        d = a.y - b.y;
        return Math.sqrt(c * c + d * d)
    }
    function qc(a, b) {
        var c = b.y + b.height,
        d = b.y,
        f = b.x,
        g = b.x + b.width,
        k; (k = rc(a, new kc(f, d, f, c))) || (k = rc(a, new kc(f, d, g, d))) || (k = rc(a, new kc(f, c, g, c))) || (k = rc(a, new kc(g, d, g, c)));
        return k
    }
    function rc(a, b) {
        var c = a.ya,
        d = a.za,
        f = a.$a,
        g = a.ab,
        k = b.ya,
        l = b.za,
        m = b.$a,
        r = b.ab,
        x = (r - l) * (f - c) - (m - k) * (g - d);
        if (0 !== x && (m = ((m - k) * (d - l) - (r - l) * (c - k)) / x, k = ((f - c) * (d - l) - (g - d) * (c - k)) / x, 0 <= m && 1 >= m && 0 <= k && 1 >= k)) return new E(c + m * (f - c), d + m * (g - d))
    }
    function sc(a, b) {
        var c = Math.sin(b),
        d = Math.cos(b),
        f = a.$a - a.ya,
        g = a.ab - a.za;
        a.$a = a.ya + (f * d - g * c);
        a.ab = a.za + (f * c + g * d)
    }
    function tc(a, b) {
        var c = uc(a),
        d = uc(b),
        f = d - c;
        return f > Math.PI ? d - 2 * Math.PI - c: f < -Math.PI ? d - c + 2 * Math.PI: f
    }
    function uc(a) {
        a = Math.atan2(a.ab - a.za, a.$a - a.ya);
        return 0 > a ? 2 * Math.PI + a: a
    }
    function vc(a, b) {
        var c = a.ya - a.$a,
        d = a.za - a.ab,
        f = b / Math.sqrt(c * c + d * d);
        a.ya += f * d;
        a.za -= f * c;
        a.$a += f * d;
        a.ab -= f * c
    }
    function wc(a, b) {
        return xc(Math.atan2(b.y - a.y, b.x - a.x))
    }
    function xc(a) {
        0 > a && (a += 2 * Math.PI);
        a >= 2 * Math.PI && (a -= 2 * Math.PI);
        return a
    }
    function yc(a, b, c) {
        a = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
        return 0 === a ? 0 : 0 < a ? 1 : -1
    }
    function zc(a) {
        return Ac(a.ka, a.pa)
    }
    function Ac(a, b) {
        return wc(a.ca, b.ca)
    }
    function Bc(a) {
        switch (a.ua.length) {
        case 1:
            return xc(Ac(a, a.ua[0]) + Math.PI);
        case 2:
            return Cc(a);
        case 3:
            return Dc(a)
        }
        return Math.PI / 6
    }
    function Ec(a) {
        switch (a.ua.length) {
        case 0:
            return Math.PI / 6;
        case 1:
            return Fc(a);
        case 2:
            return Cc(a);
        case 3:
            return Dc(a)
        }
        return 0
    }
    function Fc(a) {
        var b = a.ua[0];
        if (2 == b.ua.length) {
            var b = a.ua[0],
            c = b.ua,
            c = c[0] == a ? c[1] : c[0];
            if (c === a) throw Error("cousin is node");
            var d = Ac(b, a);
            a = xc(d + Math.PI / 3);
            d = xc(d - Math.PI / 3);
            b = Ac(c, b);
            return Math.abs(b - a) > Math.abs(b - d) ? d: a
        }
        b = Ac(b, a);
        if (0 <= b && b <= Math.PI / 2) return b - Math.PI / 3;
        if (b > Math.PI / 2 && b <= Math.PI) return b + Math.PI / 3;
        if (b > Math.PI && b <= 3 * Math.PI / 2) return b - Math.PI / 3;
        if (b > 3 * Math.PI / 2 && b <= 2 * Math.PI) return b + Math.PI / 3;
        throw Error("Unable to assign angle");
    }
    function Cc(a) {
        var b = a.ua,
        c = Ac(a, b[0]);
        a = Ac(a, b[1]);
        c > a && (b = c, c = a, a = b);
        b = a - c;
        b > Math.PI && (b = -(2 * Math.PI + c - a));
        return xc(c + 0.5 * b + Math.PI)
    }
    function Dc(a) {
        for (var b = a.ua,
        c = 0; c < b.length; c++) {
            var d = b[c];
            if (1 < d.ua.length) return Ac(d, a)
        }
        return Ac(a, b[0])
    };
    function Gc(a, b, c, d) {
        this.sg = a;
        this.sd = b;
        this.Kd = void 0 === c ? 1 : c;
        this.be = d || []
    }
    Gc.prototype.Ec = h("sg");
    function Hc(a, b) {
        this.sd = a;
        this.Kd = 0 === b ? 0 : b || 1
    }
    var Ic = new Hc("#000000", 0);
    function P(a, b, c, d, f, g) {
        if (6 == arguments.length) this.setTransform(a, b, c, d, f, g);
        else {
            if (0 != arguments.length) throw Error("Insufficient matrix parameters");
            this.ib = this.Bb = 1;
            this.Ab = this.zb = this.Jb = this.Kb = 0
        }
    }
    q = P.prototype;
    q.va = function() {
        return new P(this.ib, this.Ab, this.zb, this.Bb, this.Jb, this.Kb)
    };
    q.setTransform = function(a, b, c, d, f, g) {
        if (! (u(a) && u(b) && u(c) && u(d) && u(f) && u(g))) throw Error("Invalid transform parameters");
        this.ib = a;
        this.Ab = b;
        this.zb = c;
        this.Bb = d;
        this.Jb = f;
        this.Kb = g;
        return this
    };
    q.scale = function(a, b) {
        this.ib *= a;
        this.Ab *= a;
        this.zb *= b;
        this.Bb *= b;
        return this
    };
    q.translate = function(a, b) {
        this.Jb += a * this.ib + b * this.zb;
        this.Kb += a * this.Ab + b * this.Bb;
        return this
    };
    q.rotate = function(a, b, c) {
        var d = new P,
        f = Math.cos(a);
        a = Math.sin(a);
        b = d.setTransform(f, a, -a, f, b - b * f + c * a, c - b * a - c * f);
        return Jc(this, b)
    };
    q.toString = function() {
        return "matrix(" + [this.ib, this.Ab, this.zb, this.Bb, this.Jb, this.Kb].join() + ")"
    };
    function Jc(a, b) {
        var c = a.ib,
        d = a.zb;
        a.ib = b.ib * c + b.Ab * d;
        a.zb = b.zb * c + b.Bb * d;
        a.Jb += b.Jb * c + b.Kb * d;
        c = a.Ab;
        d = a.Bb;
        a.Ab = b.ib * c + b.Ab * d;
        a.Bb = b.zb * c + b.Bb * d;
        a.Kb += b.Jb * c + b.Kb * d;
        return a
    }
    q.transform = function(a, b, c, d, f) {
        var g = b;
        for (b += 2 * f; g < b;) {
            f = a[g++];
            var k = a[g++];
            c[d++] = f * this.ib + k * this.zb + this.Jb;
            c[d++] = f * this.Ab + k * this.Bb + this.Kb
        }
    };
    var Kc = {
        A: {
            bounds: [0.3399, 0, 8.4727, 9.2461],
            commands: "MLLZMLLLLLLLZ",
            bearing: [0.3399, 0.375],
            points: [4.6114, 6.6622, 3.4278, 3.0469, 5.7247, 3.0469, 3.3868, 9.2461, 5.7657, 9.2461, 8.8125, 0, 6.6504, 0, 6, 2.0508, 3.1524, 2.0508, 2.502, 0, 0.3399, 0]
        },
        B: {
            bounds: [1.1016, 0, 7.1543, 9.2461],
            commands: "MLLQQQZMQQQQQQQQLLZMQQQQQQQQQQQLLZ",
            bearing: [1.1016, 0.8731],
            points: [3.2637, 7.752, 3.2637, 5.4375, 4.0782, 5.4375, 6.0118, 5.4375, 6.0118, 6.6504, 6.0118, 7.1836, 5.7481, 7.4122, 5.3614, 7.752, 4.0899, 7.752, 3.961, 1.4883, 4.9336, 1.4883, 5.1504, 1.5469, 5.3672, 1.6055, 5.5313, 1.6641, 5.6954, 1.7227, 5.7891, 1.8165, 5.8829, 1.9102, 5.9649, 2.0391, 6.1231, 2.2735, 6.1231, 2.7247, 6.1231, 3.1758, 5.9883, 3.4542, 5.8536, 3.7325, 5.5665, 3.879, 5.0801, 4.1192, 4.0665, 4.1192, 3.2637, 4.1192, 3.2637, 1.4883, 3.5508, 9.2461, 5.1446, 9.2461, 5.9239, 9.0323, 6.7032, 8.8184, 7.0782, 8.5547, 7.9219, 7.9688, 7.9219, 6.7969, 7.9219, 6.0118, 7.3536, 5.4874, 6.7852, 4.9629, 5.918, 4.875, 7.6231, 4.6172, 8.0743, 3.6622, 8.2559, 3.2754, 8.2559, 2.7042, 8.2559, 2.1329, 8.0684, 1.6583, 7.8809, 1.1836, 7.5469, 0.8731, 7.2129, 0.5625, 6.7032, 0.3633, 5.7833, 0, 4.254, 0, 1.1016, 0, 1.1016, 9.2461]
        },
        C: {
            bounds: [0.4454, -0.17, 6.8379, 9.5567],
            commands: "MQQLQQQQQQQQLLLLQQQZ",
            bearing: [0.4454, 0.1641],
            points: [2.5958, 4.5821, 2.5958, 1.4004, 4.7051, 1.4004, 5.7657, 1.4004, 6.6973, 2.0391, 7.2833, 0.5157, 6.7793, 0.0938, 5.5196, -0.0938, 5.0333, -0.17, 4.5059, -0.17, 2.6075, -0.17, 1.5264, 1.0811, 0.4454, 2.3321, 0.4454, 4.6377, 0.4454, 6.9434, 1.5059, 8.1651, 2.5665, 9.3868, 4.5059, 9.3868, 5.795, 9.3868, 6.8438, 8.9532, 7.0723, 8.8536, 7.1778, 8.7833, 7.2833, 8.7129, 6.6973, 7.1895, 6.5918, 7.2657, 6.4571, 7.3536, 5.7188, 7.8282, 4.6612, 7.8282, 3.6036, 7.8282, 3.0997, 7.0547, 2.5958, 6.2813, 2.5958, 4.5821]
        },
        D: {
            bounds: [1.1075, 0, 7.6114, 9.2461],
            commands: "MQQQQQQLLLQQZMQQLLLQZ",
            bearing: [1.1075, 0.7149],
            points: [6.5274, 3.8086, 6.5508, 4.1485, 6.5508, 4.5997, 6.5508, 5.0508, 6.5274, 5.3877, 6.504, 5.7247, 6.4307, 6.0733, 6.3575, 6.4219, 6.2344, 6.668, 5.9297, 7.2422, 5.3467, 7.5059, 4.7637, 7.7696, 3.7559, 7.7696, 3.2696, 7.7696, 3.2696, 1.4883, 3.7559, 1.4883, 5.2735, 1.4883, 5.8536, 2.0772, 6.4336, 2.6661, 6.5274, 3.8086, 8.7188, 4.5821, 8.7188, 2.3614, 7.5616, 1.1807, 6.4043, 0, 3.9844, 0, 1.1075, 0, 1.1075, 9.2461, 3.9844, 9.2461, 8.7188, 9.2461, 8.7188, 4.5821]
        },
        E: {
            bounds: [1.1075, 0, 6.3282, 9.2461],
            commands: "MLLLLLLLLLLLZ",
            bearing: [1.1075, 0.5801],
            points: [7.4356, 0, 1.1075, 0, 1.1075, 9.2461, 7.377, 9.2461, 7.377, 7.67, 3.2637, 7.67, 3.2637, 5.4434, 6.8497, 5.4434, 6.8497, 4.1192, 3.2637, 4.1192, 3.2637, 1.5586, 7.4356, 1.5586]
        },
        F: {
            bounds: [1.1075, 0, 6.3282, 9.2461],
            commands: "MLLLLLLLLLZ",
            bearing: [1.1075, 0.0704],
            points: [3.2637, 4.1133, 3.2637, 0, 1.1075, 0, 1.1075, 9.2461, 7.4356, 9.2461, 7.4356, 7.67, 3.2637, 7.67, 3.2637, 5.4375, 6.9317, 5.4375, 6.9317, 4.1133]
        },
        G: {
            bounds: [0.6973, -0.2168, 7.7696, 9.586],
            commands: "MLLLLLLLQQQQQQQQQQLLQQQQQQQQQZ",
            bearing: [0.6973, 0.9786],
            points: [6.5743, 1.7813, 6.5743, 3.8379, 4.4766, 3.8379, 4.4766, 5.1504, 8.4668, 5.1504, 8.4668, 0, 7.0782, 0, 6.7969, 0.7383, 6.5625, 0.293, 5.9766, 0.0381, 5.3907, -0.2168, 4.6407, -0.2168, 2.8067, -0.2168, 1.752, 0.9903, 0.6973, 2.1973, 0.6973, 4.5118, 0.6973, 6.7852, 1.8692, 8.0772, 3.0411, 9.3692, 5.209, 9.3692, 5.7657, 9.3692, 6.2549, 9.2959, 6.7442, 9.2227, 7.0489, 9.1172, 7.3536, 9.0118, 7.5733, 8.9063, 7.793, 8.8008, 7.8868, 8.7305, 7.9864, 8.6661, 7.3594, 7.2247, 7.2305, 7.2891, 7.0108, 7.3887, 6.7911, 7.4883, 6.2315, 7.6553, 5.6719, 7.8223, 5.2559, 7.8223, 4.1133, 7.8223, 3.4864, 6.9786, 2.8594, 6.1348, 2.8594, 4.5, 2.8594, 3.5977, 3.0645, 2.9151, 3.2696, 2.2325, 3.7178, 1.8194, 4.1661, 1.4063, 4.8165, 1.4063, 5.8711, 1.4063, 6.5743, 1.7813]
        },
        H: {
            bounds: [0.8965, 0, 7.5938, 9.2461],
            commands: "MLLLLLLLLLLLZ",
            bearing: [0.8965, 0.8965],
            points: [6.3282, 0, 6.3282, 4.125, 3.0586, 4.125, 3.0586, 0, 0.8965, 0, 0.8965, 9.2461, 3.0586, 9.2461, 3.0586, 5.4493, 6.3282, 5.4493, 6.3282, 9.2461, 8.4903, 9.2461, 8.4903, 0]
        },
        I: {
            bounds: [0.4512, 0, 4.1602, 9.2461],
            commands: "MLLLLLLLLLLLZ",
            bearing: [0.4512, 0.4512],
            points: [0.4512, 0.9961, 1.4473, 0.9961, 1.4473, 8.2442, 0.4512, 8.2442, 0.4512, 9.2461, 4.6114, 9.2461, 4.6114, 8.2442, 3.6094, 8.2442, 3.6094, 0.9961, 4.6114, 0.9961, 4.6114, 0, 0.4512, 0]
        },
        J: {
            bounds: [0.1817, -0.504, 4.0665, 9.75],
            commands: "MQQQLLLQQZ",
            bearing: [0.1817, 0.9961],
            points: [0.1817, 0.9024, 0.8965, 0.9844, 1.2862, 1.1866, 1.6758, 1.3887, 1.8575, 1.8077, 2.0391, 2.2266, 2.0391, 2.9649, 2.0391, 9.2461, 4.2012, 9.2461, 4.2012, 3.1583, 4.2481, 1.254, 3.4131, 0.3956, 2.5782, -0.4629, 0.7325, -0.504]
        },
        K: {
            bounds: [1.1192, 0, 7.793, 9.2461],
            commands: "MLLLLLLLLLLLLZ",
            bearing: [1.1192, -0.0586],
            points: [3.2813, 3.961, 3.2813, 0, 1.1192, 0, 1.1192, 9.2461, 3.2813, 9.2461, 3.2813, 5.2793, 3.7618, 5.2793, 6.3868, 9.2461, 8.5899, 9.2461, 5.4668, 4.67, 8.9122, 0, 6.6563, 0, 3.7149, 3.961]
        },
        L: {
            bounds: [0.8145, 0, 6.1583, 9.2461],
            commands: "MLLLLLZ",
            bearing: [0.8145, 0],
            points: [6.9727, 0, 0.8145, 0, 0.8145, 9.2461, 2.9708, 9.2461, 2.9708, 1.4825, 6.9727, 1.4825]
        },
        M: {
            bounds: [1.002, 0, 9.7793, 9.2461],
            commands: "MLLLLLLLLLLLLZ",
            bearing: [1.002, 0.961],
            points: [4.8926, 0, 2.795, 6.3575, 2.795, 0, 1.002, 0, 1.002, 9.2461, 3.5508, 9.2461, 5.8477, 2.3907, 8.2266, 9.2461, 10.7813, 9.2461, 10.7813, 0, 8.9883, 0, 8.9883, 6.3575, 6.9083, 0]
        },
        N: {
            bounds: [0.8731, 0, 7.8047, 9.2461],
            commands: "MLLLLLLLLLZ",
            bearing: [0.8731, 0.9493],
            points: [8.6778, 0, 6.4454, 0, 2.8008, 6.293, 2.8008, 0, 0.8731, 0, 0.8731, 9.2461, 3.1817, 9.2461, 6.7618, 3.0411, 6.7618, 9.2461, 8.6778, 9.2461]
        },
        O: {
            bounds: [0.6036, -0.1641, 8.3204, 9.5684],
            commands: "MQQQQQQQQQQQQZMQQQQQQQQQQQQQQQQQQQQZ",
            bearing: [0.6036, 0.6094],
            points: [4.7637, 9.4043, 6.8555, 9.4043, 7.8897, 8.1475, 8.9239, 6.8907, 8.9239, 4.6231, 8.9239, 3.5098, 8.6749, 2.6338, 8.4258, 1.7579, 7.9219, 1.1309, 7.418, 0.504, 6.6211, 0.17, 5.8243, -0.1641, 4.7637, -0.1641, 3.7032, -0.1641, 2.9063, 0.17, 2.1094, 0.504, 1.6055, 1.1309, 1.1016, 1.7579, 0.8526, 2.6338, 0.6036, 3.5098, 0.6036, 4.6231, 0.6036, 6.8907, 1.6377, 8.1475, 2.6719, 9.4043, 4.7637, 9.4043, 4.7637, 7.8458, 4.3008, 7.8458, 3.9493, 7.6788, 3.5977, 7.5118, 3.375, 7.2305, 3.1524, 6.9493, 3.0147, 6.5303, 2.877, 6.1114, 2.8213, 5.6514, 2.7657, 5.1915, 2.7657, 4.6231, 2.7657, 4.0547, 2.8213, 3.5948, 2.877, 3.1348, 3.0147, 2.7159, 3.1524, 2.2969, 3.375, 2.0127, 3.5977, 1.7286, 3.9493, 1.5616, 4.3008, 1.3946, 4.7637, 1.3946, 5.2266, 1.3946, 5.5782, 1.5616, 5.9297, 1.7286, 6.1524, 2.0127, 6.375, 2.2969, 6.5127, 2.7159, 6.6504, 3.1348, 6.7061, 3.5948, 6.7618, 4.0547, 6.7618, 4.6231, 6.7618, 5.1915, 6.7061, 5.6514, 6.6504, 6.1114, 6.5127, 6.5303, 6.375, 6.9493, 6.1524, 7.2305, 5.9297, 7.5118, 5.5782, 7.6788, 5.2266, 7.8458, 4.7637, 7.8458]
        },
        P: {
            bounds: [0.8145, 0, 7.0372, 9.2461],
            commands: "MQQQQQQQQQLLZMQQQQQLLLLLQQQQQZ",
            bearing: [0.8145, 0.6797],
            points: [3.3282, 4.5235, 3.7559, 4.5235, 4.0987, 4.5704, 4.4415, 4.6172, 4.7461, 4.7315, 5.0508, 4.8458, 5.2559, 5.0245, 5.461, 5.2032, 5.5782, 5.4844, 5.6954, 5.7657, 5.6954, 6.1407, 5.6954, 6.6504, 5.5489, 7.0049, 5.4024, 7.3594, 5.1094, 7.5616, 4.8165, 7.7637, 4.4297, 7.8487, 4.043, 7.9336, 3.504, 7.9336, 2.9766, 7.9336, 2.9766, 4.5235, 7.8516, 6.1641, 7.8516, 5.4727, 7.6377, 4.9571, 7.4239, 4.4415, 7.0459, 4.1163, 6.668, 3.7911, 6.0879, 3.5918, 5.5079, 3.3926, 4.8516, 3.3135, 4.1954, 3.2344, 3.3399, 3.2344, 2.9766, 3.2344, 2.9766, 0, 0.8145, 0, 0.8145, 9.2461, 3.504, 9.2461, 4.3184, 9.2461, 4.9512, 9.1612, 5.584, 9.0762, 6.1436, 8.8653, 6.7032, 8.6543, 7.0694, 8.3116, 7.4356, 7.9688, 7.6436, 7.4268, 7.8516, 6.8848, 7.8516, 6.1641]
        },
        Q: {
            bounds: [0.6036, -2.461, 8.3204, 11.8653],
            commands: "MQQQQQQQQQQQQQQQQQQQQZMQQQQQLLLQQQQQQZ",
            bearing: [0.6036, 0.6094],
            points: [4.7637, 7.8458, 4.3008, 7.8458, 3.9522, 7.6788, 3.6036, 7.5118, 3.3809, 7.2305, 3.1583, 6.9493, 3.0176, 6.5303, 2.877, 6.1114, 2.8213, 5.6514, 2.7657, 5.1915, 2.7657, 4.6231, 2.7657, 4.0547, 2.8213, 3.5948, 2.877, 3.1348, 3.0176, 2.7159, 3.1583, 2.2969, 3.378, 2.0127, 3.5977, 1.7286, 3.9493, 1.5616, 4.3008, 1.3946, 4.7637, 1.3946, 5.2266, 1.3946, 5.5782, 1.5616, 5.9297, 1.7286, 6.1495, 2.0127, 6.3692, 2.2969, 6.5098, 2.7159, 6.6504, 3.1348, 6.7061, 3.5948, 6.7618, 4.0547, 6.7618, 4.6231, 6.7618, 5.1915, 6.7061, 5.6514, 6.6504, 6.1114, 6.5098, 6.5303, 6.3692, 6.9493, 6.1465, 7.2305, 5.9239, 7.5118, 5.5752, 7.6788, 5.2266, 7.8458, 4.7637, 7.8458, 4.7637, 9.4043, 6.8555, 9.4043, 7.8897, 8.1475, 8.9239, 6.8907, 8.9239, 4.6231, 8.9239, 3.3399, 8.5987, 2.379, 8.2735, 1.418, 7.585, 0.7852, 6.8965, 0.1524, 5.8653, -0.0586, 7.5528, -1.3711, 6.5333, -2.461, 3.8614, -0.0938, 3.0352, 0.0352, 2.4112, 0.4219, 1.7872, 0.8086, 1.3917, 1.4209, 0.9961, 2.0333, 0.7999, 2.8331, 0.6036, 3.6329, 0.6036, 4.6231, 0.6036, 6.8907, 1.6377, 8.1475, 2.6719, 9.4043, 4.7637, 9.4043]
        },
        R: {
            bounds: [0.8145, 0, 7.1778, 9.2461],
            commands: "MQQQQLLLQQQQLLLLLQQQQQQQQQQZMQQQQQQQQQQLLZ",
            bearing: [0.8145, 0.6387],
            points: [5.1856, 4.6055, 5.8243, 4.5411, 6.3018, 4.4004, 6.7793, 4.2598, 7.1778, 3.9961, 7.5762, 3.7325, 7.7842, 3.2901, 7.9922, 2.8477, 7.9922, 2.2383, 7.9922, 0, 5.8536, 0, 5.8536, 2.3848, 5.8536, 2.8536, 5.7042, 3.17, 5.5547, 3.4864, 5.253, 3.6563, 4.9512, 3.8262, 4.5762, 3.8936, 4.2012, 3.961, 3.668, 3.961, 2.9766, 3.961, 2.9766, 0, 0.8145, 0, 0.8145, 9.2461, 3.2696, 9.2461, 4.3653, 9.2461, 5.1651, 9.1465, 5.9649, 9.0469, 6.4952, 8.8624, 7.0254, 8.6778, 7.3418, 8.379, 7.6583, 8.0801, 7.7901, 7.7198, 7.9219, 7.3594, 7.9219, 6.8731, 7.9219, 6.3926, 7.7901, 6.0206, 7.6583, 5.6485, 7.4209, 5.3965, 7.1836, 5.1446, 6.835, 4.9747, 6.4864, 4.8047, 6.085, 4.7168, 5.6836, 4.629, 5.1856, 4.6055, 3.2286, 5.2676, 3.7618, 5.2676, 4.1397, 5.2999, 4.5176, 5.3321, 4.8399, 5.42, 5.1622, 5.5079, 5.3555, 5.6602, 5.5489, 5.8125, 5.6573, 6.0528, 5.7657, 6.293, 5.7657, 6.6211, 5.7657, 6.9375, 5.6836, 7.1602, 5.6016, 7.3829, 5.4375, 7.5352, 5.2735, 7.6875, 4.9893, 7.7754, 4.7051, 7.8633, 4.3448, 7.8985, 3.9844, 7.9336, 3.4747, 7.9336, 2.9766, 7.9336, 2.9766, 5.2676]
        },
        S: {
            bounds: [0.545, -0.2403, 6.8672, 9.668],
            commands: "MQQQQQLLLQQQQQQQQQQQQQLQQQQQQQQQQQLLQQQQQQQQQQQQQQLQQZ",
            bearing: [0.545, 0.5977],
            points: [7.4122, 2.7305, 7.4122, -0.2403, 3.9903, -0.2403, 3.4805, -0.2403, 2.959, -0.1465, 2.4375, -0.0528, 2.0948, 0.0586, 1.752, 0.17, 1.3917, 0.337, 1.0313, 0.504, 0.9258, 0.5625, 0.6973, 0.7032, 0.6856, 0.709, 1.1602, 2.3321, 1.2247, 2.2911, 1.3389, 2.2266, 1.4532, 2.1622, 1.7872, 1.9981, 2.1211, 1.834, 2.4317, 1.711, 2.7422, 1.5879, 3.1348, 1.4854, 3.5274, 1.3829, 3.8204, 1.3887, 4.0958, 1.3946, 4.2569, 1.4004, 4.418, 1.4063, 4.629, 1.4356, 4.8399, 1.4649, 4.96, 1.5235, 5.0801, 1.5821, 5.1944, 1.6788, 5.3086, 1.7754, 5.3584, 1.9278, 5.4083, 2.0801, 5.4083, 2.2852, 5.4083, 2.6602, 5.2295, 2.918, 5.0508, 3.1758, 4.5879, 3.3633, 2.1036, 4.3829, 1.336, 4.6993, 0.9405, 5.3321, 0.545, 5.9649, 0.545, 6.7911, 0.545, 7.3184, 0.7413, 7.7608, 0.9375, 8.2032, 1.2686, 8.5049, 1.5997, 8.8067, 2.042, 9.0206, 2.4844, 9.2344, 2.9678, 9.3311, 3.4512, 9.4278, 3.9551, 9.4278, 4.4004, 9.4278, 4.8897, 9.3428, 5.379, 9.2579, 5.7598, 9.1407, 6.1407, 9.0235, 6.46, 8.9063, 6.7793, 8.7891, 6.9551, 8.7071, 7.125, 8.625, 6.5743, 6.9961, 6.5274, 7.0313, 6.4454, 7.084, 6.3633, 7.1368, 6.1026, 7.2745, 5.8418, 7.4122, 5.5752, 7.5176, 5.3086, 7.6231, 4.9249, 7.711, 4.5411, 7.7989, 4.1778, 7.7989, 3.9434, 7.7989, 3.7647, 7.7901, 3.586, 7.7813, 3.3809, 7.752, 3.1758, 7.7227, 3.0293, 7.6641, 2.8829, 7.6055, 2.7569, 7.5147, 2.6309, 7.4239, 2.5694, 7.2833, 2.5079, 7.1426, 2.5079, 6.961, 2.5079, 6.8204, 2.543, 6.712, 2.5782, 6.6036, 2.7481, 6.4542, 2.918, 6.3047, 3.2227, 6.1817, 5.3086, 5.3555, 6.1758, 5.0098, 6.794, 4.3067, 7.4122, 3.6036, 7.4122, 2.7305]
        },
        T: {
            bounds: [ - 0.0704, 0, 6.9141, 9.2461],
            commands: "MLLLLLLLZ",
            bearing: [ - 0.0704, -0.0645],
            points: [6.8438, 7.7579, 4.4883, 7.7579, 4.4883, 0, 2.3262, 0, 2.3262, 7.7579, -0.0704, 7.7579, -0.0704, 9.2461, 6.8438, 9.2461]
        },
        U: {
            bounds: [0.8204, -0.2168, 7.8223, 9.4629],
            commands: "MQQQQQQQQQQQQQLLLQQQQLLZ",
            bearing: [0.8204, 0.8204],
            points: [2.9825, 4.2129, 3, 3.1407, 3.0645, 2.8331, 3.129, 2.5254, 3.1934, 2.3174, 3.2579, 2.1094, 3.3487, 1.9747, 3.4395, 1.8399, 3.5684, 1.711, 3.6973, 1.5821, 3.8672, 1.5059, 4.2247, 1.3418, 4.629, 1.3418, 5.0333, 1.3418, 5.2735, 1.3917, 5.5137, 1.4415, 5.6895, 1.5528, 5.8653, 1.6641, 5.9971, 1.7989, 6.129, 1.9336, 6.2139, 2.1446, 6.2989, 2.3555, 6.3545, 2.5577, 6.4102, 2.7598, 6.4336, 3.0528, 6.4805, 3.5391, 6.4805, 4.2129, 6.4805, 9.2461, 8.6426, 9.2461, 8.6426, 4.2364, 8.6426, 2.0508, 7.7081, 0.917, 6.7735, -0.2168, 4.7344, -0.2168, 2.6661, -0.2168, 1.7227, 0.9551, 0.8204, 2.0801, 0.8204, 4.2364, 0.8204, 9.2461, 2.9825, 9.2461]
        },
        V: {
            bounds: [ - 0.0469, 0, 7.8985, 9.2461],
            commands: "MLLLLLLZ",
            bearing: [ - 0.0469, -0.0528],
            points: [1.9688, 9.2461, 3.9258, 2.9473, 5.8125, 9.2461, 7.8516, 9.2461, 4.9688, 0, 2.8243, 0, -0.0469, 9.2461]
        },
        W: {
            bounds: [0.129, 0, 11.4493, 9.2461],
            commands: "MLLLLLLLLLLLLZ",
            bearing: [0.129, 0.1172],
            points: [4.7051, 9.17, 7.0079, 9.17, 8.2208, 2.625, 9.4922, 9.2461, 11.5782, 9.2461, 9.5977, 0, 7.2247, 0, 5.8536, 7.0606, 4.4883, 0, 2.1094, 0, 0.129, 9.2461, 2.2208, 9.2461, 3.4922, 2.625]
        },
        X: {
            bounds: [ - 0.0293, 0, 8.0743, 9.2461],
            commands: "MLLLLLLLLLLLZ",
            bearing: [ - 0.0293, -0.0293],
            points: [4.0079, 6.252, 5.8008, 9.2461, 8.045, 9.2461, 5.2149, 4.67, 8.045, 0, 5.754, 0, 4.0079, 2.8946, 2.2618, 0, -0.0293, 0, 2.8008, 4.67, -0.0293, 9.2461, 2.2149, 9.2461]
        },
        Y: {
            bounds: [ - 0.3633, 0, 7.9805, 9.2461],
            commands: "MLLLLLLLLZ",
            bearing: [ - 0.3633, -0.375],
            points: [3.6094, 5.3028, 5.4258, 9.2461, 7.6172, 9.2461, 4.629, 3.0352, 4.629, 0, 2.5899, 0, 2.5899, 3.0352, -0.3633, 9.2461, 1.793, 9.2461]
        },
        Z: {
            bounds: [0.3106, 0, 6.504, 9.2461],
            commands: "MLLLLLLLLLZ",
            bearing: [0.3106, 0.3985],
            points: [6.8028, 9.2461, 6.8028, 7.9336, 2.7305, 1.7344, 6.8145, 1.7344, 6.8145, 0, 0.3106, 0, 0.3106, 1.4708, 4.4239, 7.7579, 0.3106, 7.7579, 0.3106, 9.2461]
        },
        a: {
            bounds: [0.5508, -0.1524, 6.0528, 7.1602],
            commands: "MQQQQLQQQZMQLQQQLLLQQQQQQQQQQZ",
            bearing: [0.5508, 0.7149],
            points: [2.9239, 2.9942, 2.6016, 2.7833, 2.6016, 2.2852, 2.6016, 1.7872, 2.8301, 1.5293, 3.0586, 1.2715, 3.4688, 1.2715, 4.1133, 1.2715, 4.5645, 1.8165, 4.5645, 3.1583, 4.5762, 3.17, 4.3389, 3.1875, 4.1016, 3.2051, 4.002, 3.2051, 3.2461, 3.2051, 2.9239, 2.9942, 3.6563, 5.5899, 2.4786, 5.5899, 1.4708, 5.1153, 0.9844, 6.3399, 2.0625, 7.0079, 3.7266, 7.0079, 5.6368, 7.0079, 6.2461, 5.7891, 6.6036, 5.086, 6.6036, 3.9493, 6.6036, 0, 5.0157, 0, 4.7344, 0.879, 4.4356, 0.3633, 4.0137, 0.1055, 3.5918, -0.1524, 2.9356, -0.1524, 1.8047, -0.1524, 1.1778, 0.4366, 0.5508, 1.0254, 0.5508, 2.1182, 0.5508, 3.211, 1.3711, 3.7413, 2.1915, 4.2715, 3.668, 4.2715, 4.1836, 4.2715, 4.5645, 4.2012, 4.5645, 4.8809, 4.5059, 5.0333, 4.3829, 5.3672, 4.1866, 5.4786, 3.9903, 5.5899, 3.6563, 5.5899]
        },
        b: {
            bounds: [1.1075, -0.1524, 6.4747, 10.2598],
            commands: "MQLLLLLQQQQQZMQQQQLQZ",
            bearing: [1.1075, 0.4571],
            points: [4.8223, -0.1524, 3.3516, -0.1524, 2.9883, 0.9727, 2.543, 0, 1.1075, 0, 1.1075, 9.8321, 3.1641, 10.1075, 3.1641, 6.211, 3.8672, 7.0079, 5.0157, 7.0079, 5.8301, 7.0079, 6.4102, 6.5567, 7.5821, 5.6543, 7.5821, 3.4336, 7.5821, 0.6915, 5.8946, 0.0352, 5.4258, -0.1524, 4.8223, -0.1524, 4.3477, 1.1719, 5.5313, 1.1719, 5.5313, 3.4336, 5.5313, 4.7579, 5.2852, 5.2383, 5.0391, 5.6954, 4.5293, 5.6954, 3.8555, 5.6954, 3.1641, 5.0157, 3.1641, 1.7344, 3.6797, 1.1719, 4.3477, 1.1719]
        },
        c: {
            bounds: [0.586, -0.1465, 5.6543, 7.1543],
            commands: "MQQQQQQQLQQQQQQQLQQQQQZ",
            bearing: [0.586, 0.3633],
            points: [0.8438, 1.8633, 0.586, 2.5372, 0.586, 3.3487, 0.586, 4.1602, 0.7676, 4.7374, 0.9493, 5.3145, 1.2657, 5.7247, 1.5821, 6.1348, 2.004, 6.4278, 2.8418, 7.0079, 3.8907, 7.0079, 4.67, 7.0079, 5.1827, 6.8584, 5.6954, 6.709, 6.2403, 6.4219, 5.8594, 5.1036, 5.7012, 5.2559, 5.209, 5.4405, 4.7168, 5.625, 4.2422, 5.625, 3.3692, 5.625, 3.0088, 5.1182, 2.6485, 4.6114, 2.6485, 3.4336, 2.6485, 1.2364, 4.4004, 1.2364, 5.0508, 1.2364, 5.7774, 1.7051, 5.8536, 1.752, 5.8536, 1.7579, 6.2344, 0.4395, 5.877, 0.252, 5.6133, 0.1465, 4.8926, -0.1465, 4.043, -0.1465, 3.1934, -0.1465, 2.5958, 0.0879, 1.9981, 0.3223, 1.5499, 0.7559, 1.1016, 1.1895, 0.8438, 1.8633]
        },
        d: {
            bounds: [0.627, -0.1524, 6.4864, 10.2657],
            commands: "MQQQQQLLLLLQQQQQQZMQQLQQQZ",
            bearing: [0.627, 0.9375],
            points: [0.7149, 2.2735, 0.627, 2.7833, 0.627, 3.6563, 0.627, 4.5293, 0.9317, 5.3174, 1.2364, 6.1055, 1.834, 6.5567, 2.4317, 7.0079, 3.2461, 7.0079, 4.418, 7.0079, 5.0567, 6.129, 5.0567, 9.8321, 7.1133, 10.1133, 7.1133, 0, 5.6661, 0, 5.2208, 0.9727, 5.0918, 0.5333, 4.6114, 0.2227, 4.0372, -0.1524, 3.3692, -0.1524, 2.795, -0.1524, 2.3204, 0.0323, 1.8458, 0.2168, 1.5323, 0.5362, 1.2188, 0.8555, 1.0108, 1.3096, 0.8028, 1.7637, 0.7149, 2.2735, 2.6778, 3.4336, 2.6778, 1.2305, 3.8614, 1.2305, 4.6934, 1.2305, 5.0567, 1.7344, 5.0567, 5.0157, 4.3594, 5.6368, 3.6797, 5.6368, 2.7774, 5.6368, 2.6954, 4.1075, 2.6778, 3.8497, 2.6778, 3.4336]
        },
        e: {
            bounds: [0.545, -0.1524, 6.211, 7.1602],
            commands: "MQQQZMLQQQQQQQQLLQQQZ",
            bearing: [0.545, 0.4688],
            points: [5.0801, 3.961, 5.0977, 5.625, 3.9024, 5.625, 3.504, 5.625, 3.2168, 5.3907, 2.6719, 4.9395, 2.6719, 3.961, 6.0176, 1.7813, 6.4043, 0.4102, 5.3145, -0.1465, 4.125, -0.1524, 1.8399, -0.1524, 0.961, 1.5118, 0.545, 2.3145, 0.5684, 3.3165, 0.5918, 4.3184, 0.8497, 4.9747, 1.1075, 5.6309, 1.5586, 6.0762, 2.4961, 7.0079, 3.9375, 7.0079, 4.8106, 7.0079, 5.4493, 6.6211, 6.7559, 5.8243, 6.7442, 3.9024, 6.7442, 2.9415, 2.6778, 2.9415, 2.7012, 2.168, 3.129, 1.6993, 3.5567, 1.2364, 4.2833, 1.2422, 5.2852, 1.2481, 6.0176, 1.7813]
        },
        f: {
            bounds: [0.3633, 0, 4.8458, 9.9317],
            commands: "MQQLLLLLLLLLQQQLQZ",
            bearing: [0.3633, -0.1231],
            points: [4.336, 8.4141, 3.9258, 8.4141, 3.7295, 8.0655, 3.5333, 7.7168, 3.5333, 6.8028, 5.1739, 6.8028, 5.1739, 5.9532, 3.5274, 5.9532, 3.5274, 0, 1.4766, 0, 1.4766, 5.9532, 0.3633, 5.9532, 0.3633, 6.8028, 1.4766, 6.8028, 1.4766, 8.3555, 2.0625, 9.1436, 2.6485, 9.9317, 3.8672, 9.9317, 4.5352, 9.9317, 5.209, 9.7618, 5.209, 8.2676, 4.8165, 8.4141, 4.336, 8.4141]
        },
        g: {
            bounds: [0.4571, -3.2754, 7.084, 10.2833],
            commands: "MQQQQQQQQQQQLQQQZMLQQQQQQQZMQQQQQLLLQQQQQQQQQQQLQQQQQQLLZ",
            bearing: [0.4571, -0.1172],
            points: [2.1827, -0.6241, 2.086, -0.8028, 2.086, -0.9756, 2.086, -1.1485, 2.1065, -1.2569, 2.127, -1.3653, 2.2061, -1.4766, 2.2852, -1.5879, 2.4375, -1.6465, 2.7774, -1.7813, 3.3868, -1.7813, 3.9961, -1.7813, 4.3448, -1.7315, 4.6934, -1.6817, 4.8956, -1.6026, 5.0977, -1.5235, 5.2149, -1.3887, 5.3965, -1.1719, 5.3995, -0.8438, 5.4024, -0.5157, 5.2911, -0.3575, 5.0625, -0.0293, 3.8438, 0.0879, 3.0235, 0.17, 2.9766, 0.17, 2.7627, 0, 2.5489, -0.17, 2.4141, -0.3077, 2.2793, -0.4454, 2.1827, -0.6241, 3.2227, 3.545, 3.9375, 3.6036, 4.254, 3.627, 4.5235, 3.9844, 4.7872, 4.3477, 4.7754, 4.711, 4.7754, 5.6719, 3.7208, 5.6719, 3.1758, 5.6719, 2.9004, 5.4522, 2.625, 5.2325, 2.625, 4.6495, 2.625, 4.0665, 3.0118, 3.6915, 3.1055, 3.6036, 3.2227, 3.545, 6.0938, 5.6719, 6.4688, 5.1094, 6.4688, 4.7872, 6.4688, 4.4649, 6.4102, 4.1954, 6.3516, 3.9258, 6.1846, 3.6211, 6.0176, 3.3165, 5.7598, 3.0821, 5.168, 2.5723, 4.0723, 2.4668, 2.2969, 2.2969, 2.4024, 1.5938, 3.9727, 1.4766, 5.6133, 1.3536, 6.3985, 0.8057, 7.1836, 0.2579, 7.1836, -0.8497, 7.1836, -3.2754, 3.4336, -3.2754, 0.8731, -3.2754, 0.5274, -1.9278, 0.4571, -1.6524, 0.4571, -1.1954, 0.4571, -0.7383, 0.7735, -0.2813, 1.0899, 0.1758, 1.67, 0.4688, 0.7559, 0.5567, 0.6387, 1.5762, 0.6036, 2.0684, 0.668, 2.2442, 0.7325, 2.42, 0.8379, 2.5665, 1.0665, 2.8946, 1.6758, 3.0938, 2.1504, 3.2227, 1.3477, 3.2579, 0.9961, 3.8379, 0.7969, 4.1602, 0.7969, 4.6348, 0.7969, 5.754, 1.5381, 6.3809, 2.2793, 7.0079, 3.5274, 7.0079, 4.7754, 7.0079, 5.3204, 6.6563, 6.0997, 6.9961, 6.9493, 6.9961, 7.5411, 6.9961, 7.5411, 5.6719]
        },
        h: {
            bounds: [1.1075, 0, 6.3165, 10.125],
            commands: "MLQQQQQLLLQQQQQLLLZ",
            bearing: [1.1075, 0.7208],
            points: [3.1583, 10.125, 3.1583, 6.1934, 3.1934, 6.2813, 3.378, 6.4483, 3.5625, 6.6153, 3.7559, 6.7266, 4.2422, 7.0079, 4.9922, 7.0079, 6.1465, 7.0079, 6.7852, 6.3106, 7.4239, 5.6133, 7.4239, 4.2364, 7.4239, 0, 5.3731, 0, 5.3731, 4.377, 5.3731, 5.004, 5.0889, 5.3204, 4.8047, 5.6368, 4.4151, 5.6368, 4.0254, 5.6368, 3.7999, 5.5694, 3.5743, 5.502, 3.4571, 5.42, 3.3399, 5.3379, 3.1583, 5.1797, 3.1583, 0, 1.1075, 0, 1.1075, 9.7735]
        },
        i: {
            bounds: [1.1543, -0.0176, 2.2793, 10.5704],
            commands: "MQQQQQQQQZMLLLZ",
            bearing: [1.1543, 0.8379],
            points: [1.1543, 9.4161, 1.1543, 9.9024, 1.4795, 10.2276, 1.8047, 10.5528, 2.2852, 10.5528, 2.7657, 10.5528, 3.0997, 10.2276, 3.4336, 9.9024, 3.4336, 9.4161, 3.4336, 8.9297, 3.0997, 8.6045, 2.7657, 8.2793, 2.2852, 8.2793, 1.8047, 8.2793, 1.4795, 8.6045, 1.1543, 8.9297, 1.1543, 9.4161, 3.3106, -0.0176, 1.2598, -0.0176, 1.2598, 6.7793, 3.3106, 6.7793]
        },
        j: {
            bounds: [ - 0.0528, -2.0391, 3.3516, 12.5743],
            commands: "MQQQQLLLQQQQZMQQQQQQQQZ",
            bearing: [ - 0.0528, 0.9786],
            points: [ - 0.0528, -0.6622, 0.252, -0.4922, 0.4512, -0.337, 0.6504, -0.1817, 0.8262, 0.0469, 1.002, 0.2754, 1.087, 0.586, 1.1719, 0.8965, 1.1719, 1.3067, 1.1719, 6.7969, 3.2286, 6.7969, 3.2286, 1.1895, 3.2286, 0.5684, 3.0176, 0.0118, 2.8067, -0.545, 2.4405, -0.9522, 2.0743, -1.3594, 1.6026, -1.6377, 1.1309, -1.9161, 0.5977, -2.0391, 1.084, 9.4014, 1.084, 9.8907, 1.3975, 10.2129, 1.711, 10.5352, 2.1915, 10.5352, 2.6719, 10.5352, 2.9854, 10.2129, 3.2989, 9.8907, 3.2989, 9.4014, 3.2989, 8.9122, 2.9854, 8.587, 2.6719, 8.2618, 2.1915, 8.2618, 1.711, 8.2618, 1.3975, 8.587, 1.084, 8.9122, 1.084, 9.4014]
        },
        k: {
            bounds: [1.1075, 0, 6.9258, 10.1192],
            commands: "MLLLLLLLLLLLZ",
            bearing: [1.1075, -0.0118],
            points: [3.1641, 2.3965, 3.1641, 0, 1.1075, 0, 1.1075, 9.8321, 3.1641, 10.1192, 3.1641, 4.3125, 5.7422, 6.8497, 7.9747, 6.8497, 4.9747, 3.8028, 8.0333, 0, 5.7188, 0, 3.7032, 2.9063]
        },
        l: {
            bounds: [1.0196, 0, 2.0567, 10.1133],
            commands: "MLLLZ",
            bearing: [1.0196, 0.9551],
            points: [3.0762, 10.1133, 3.0762, 0, 1.0196, 0, 1.0196, 9.8321]
        },
        m: {
            bounds: [1.1075, 0, 9.7911, 7.0079],
            commands: "MQLLLLLQQQQQQQQLLLQQQLLLQQQZ",
            bearing: [1.1075, 0.7208],
            points: [3.961, 5.5665, 3.4043, 5.3086, 3.1641, 4.8692, 3.1641, 0, 1.1075, 0, 1.1075, 6.7969, 2.5606, 6.7969, 3.0176, 5.754, 3.2579, 6.3926, 3.6915, 6.6856, 4.1719, 7.0079, 4.8633, 7.0079, 5.5196, 7.0079, 5.9737, 6.7823, 6.4278, 6.5567, 6.7032, 5.9883, 7.0606, 6.504, 7.5381, 6.7559, 8.0157, 7.0079, 8.7833, 7.0079, 9.8321, 7.0079, 10.3653, 6.334, 10.8985, 5.6602, 10.8985, 4.4239, 10.8985, 0, 8.8418, 0, 8.8418, 4.5821, 8.8418, 5.625, 8.1446, 5.625, 7.8692, 5.625, 7.6231, 5.4932, 7.377, 5.3614, 7.0547, 5.0508, 7.0547, 0, 4.9981, 0, 4.9981, 4.6407, 4.9981, 5.4434, 4.6055, 5.584, 4.4942, 5.625, 4.3243, 5.625, 4.0899, 5.625, 3.961, 5.5665]
        },
        n: {
            bounds: [1.0723, 0, 6.1524, 7.0079],
            commands: "MQQQLLLQQQQLLLLZ",
            bearing: [1.0723, 0.7618],
            points: [3.0235, 5.9239, 3.5508, 7.0079, 4.8135, 7.0079, 6.0762, 7.0079, 6.6504, 6.3194, 7.2247, 5.6309, 7.2247, 4.2247, 7.2247, 0, 5.168, 0, 5.168, 4.377, 5.168, 5.0625, 4.96, 5.3409, 4.752, 5.6192, 4.3096, 5.6338, 3.8672, 5.6485, 3.5713, 5.461, 3.2754, 5.2735, 3.129, 5.0157, 3.129, 0, 1.0723, 0, 1.0723, 6.7969, 2.6309, 6.7969]
        },
        o: {
            bounds: [0.4102, -0.1524, 6.668, 7.1602],
            commands: "MQQQQQQQQZMQQQQQQQQZ",
            bearing: [0.4102, 0.4278],
            points: [3.75, 1.2188, 4.4063, 1.2188, 4.7227, 1.752, 5.0391, 2.2852, 5.0391, 3.4219, 5.0391, 4.5586, 4.7227, 5.0918, 4.4063, 5.625, 3.75, 5.625, 3.0938, 5.625, 2.7774, 5.0918, 2.461, 4.5586, 2.461, 3.4219, 2.461, 2.2852, 2.7774, 1.752, 3.0938, 1.2188, 3.75, 1.2188, 3.7442, 7.0079, 5.3438, 7.0079, 6.211, 6.0616, 7.0782, 5.1153, 7.0782, 3.4102, 7.0782, 1.7168, 6.2168, 0.7823, 5.3555, -0.1524, 3.75, -0.1524, 2.1446, -0.1524, 1.2774, 0.7823, 0.4102, 1.7168, 0.4102, 3.4102, 0.4102, 5.1153, 1.2774, 6.0616, 2.1446, 7.0079, 3.7442, 7.0079]
        },
        p: {
            bounds: [0.9375, -2.379, 6.504, 9.3868],
            commands: "MQLLLLLQQQQQQQZMQQQQQQQLQQQQZ",
            bearing: [0.9375, 0.4571],
            points: [4.5938, -0.1524, 3.6211, -0.1524, 2.9942, 0.7032, 2.9942, -2.379, 0.9375, -2.379, 0.9375, 6.7969, 2.502, 6.7969, 2.8887, 5.9122, 3.0645, 6.4336, 3.5157, 6.7208, 3.9727, 7.0079, 4.7286, 7.0079, 5.4844, 7.0079, 5.9795, 6.7559, 6.4747, 6.504, 6.7969, 6.0176, 7.4415, 5.045, 7.4415, 3.3311, 7.4415, 1.6172, 6.6592, 0.7325, 5.877, -0.1524, 4.5938, -0.1524, 5.3672, 2.9385, 5.379, 3.2168, 5.379, 3.5479, 5.379, 3.879, 5.3672, 4.0958, 5.3555, 4.3125, 5.3233, 4.5733, 5.2911, 4.834, 5.2208, 5.0098, 5.1504, 5.1856, 5.0391, 5.3438, 4.8282, 5.6485, 4.3536, 5.6485, 3.6036, 5.6485, 2.9942, 5.0215, 2.9942, 1.8985, 3.8321, 1.2188, 4.377, 1.2188, 4.834, 1.2188, 5.0508, 1.5645, 5.2676, 1.9102, 5.3116, 2.2852, 5.3555, 2.6602, 5.3672, 2.9385]
        },
        q: {
            bounds: [0.6036, -2.379, 6.4864, 9.3868],
            commands: "MQQQLLLLLQQQQQZMQQQQQQQLQQQQQZ",
            bearing: [0.6036, 0.879],
            points: [2.3731, 6.8321, 2.8536, 7.0079, 3.4278, 7.0079, 4.002, 7.0079, 4.4473, 6.6915, 4.7813, 6.4805, 5.045, 5.8829, 5.3321, 6.7969, 7.0899, 6.7969, 7.0899, -2.379, 5.0333, -2.379, 5.0333, 0.709, 4.3067, -0.1465, 3.4336, -0.1465, 2.8125, -0.1465, 2.2911, 0.0586, 1.7696, 0.2637, 1.4766, 0.627, 0.6036, 1.6934, 0.6036, 3.3106, 0.6036, 6.17, 2.3731, 6.8321, 2.7012, 4.2833, 2.6485, 3.9317, 2.6485, 3.5186, 2.6485, 3.1055, 2.6602, 2.8858, 2.6719, 2.6661, 2.7042, 2.3936, 2.7364, 2.1211, 2.8038, 1.9307, 2.8711, 1.7403, 2.9766, 1.5704, 3.1934, 1.2247, 3.6504, 1.2247, 4.3418, 1.2247, 5.0333, 1.9043, 5.0333, 4.8692, 4.7813, 5.2735, 4.5586, 5.4317, 4.2833, 5.625, 3.8672, 5.625, 3.4922, 5.625, 3.2491, 5.4463, 3.0059, 5.2676, 2.8799, 4.9512, 2.754, 4.6348, 2.7012, 4.2833]
        },
        r: {
            bounds: [1.043, 0, 4.0547, 6.8086],
            commands: "MQQQQLQQLLLLZ",
            bearing: [1.043, 0.0704],
            points: [3.0411, 5.7188, 3.1758, 6.0704, 3.3575, 6.252, 3.5567, 6.4454, 3.7383, 6.5333, 3.9375, 6.6329, 4.2305, 6.7208, 4.5235, 6.8086, 5.0977, 6.8086, 5.0977, 5.0567, 4.7989, 5.1622, 4.4297, 5.1622, 3.7559, 5.1622, 3.0997, 4.6641, 3.0997, 0, 1.043, 0, 1.043, 6.7969, 2.8067, 6.7969]
        },
        s: {
            bounds: [0.7208, -0.1465, 5.3848, 7.1543],
            commands: "MQQQQQQLLQQQQQQQLQQQQQLQQQQQZ",
            bearing: [0.7208, 0.6563],
            points: [2.6426, 2.9063, 1.4766, 3.3575, 1.1309, 3.7325, 0.7208, 4.1778, 0.7208, 5.0625, 0.7208, 5.9473, 1.4444, 6.4776, 2.168, 7.0079, 3.4629, 7.0079, 4.1485, 7.0079, 4.8399, 6.8292, 5.5313, 6.6504, 5.836, 6.5215, 5.9766, 6.4688, 5.5899, 5.168, 5.1622, 5.3321, 4.5938, 5.4639, 4.0254, 5.5958, 3.627, 5.6045, 3.2286, 5.6133, 2.9942, 5.5459, 2.7598, 5.4786, 2.6749, 5.3907, 2.5899, 5.3028, 2.5752, 5.0743, 2.5606, 4.8458, 2.6719, 4.7344, 2.7833, 4.6231, 3.0997, 4.5, 4.8458, 3.8321, 5.2149, 3.6973, 5.4727, 3.5098, 6.1055, 3.0528, 6.1055, 2.0391, 6.1055, 0.6563, 4.8926, 0.1524, 4.1661, -0.1465, 3.3399, -0.1465, 1.7461, -0.1465, 0.7383, 0.4571, 1.125, 1.793, 1.5645, 1.5938, 2.1856, 1.4239, 2.7657, 1.2715, 3.211, 1.2598, 3.7208, 1.2481, 3.9258, 1.3653, 4.1954, 1.5176, 4.1954, 1.9864, 4.1895, 2.2969, 3.5625, 2.543]
        },
        t: {
            bounds: [0, -0.0938, 5.379, 9.92],
            commands: "MQQQQLLLLLLLLLLLQQQQQLQQQQQZ",
            bearing: [0, 0.1407],
            points: [3.3809, -0.0938, 2.8594, -0.0938, 2.4698, 0.0557, 2.0801, 0.2051, 1.7989, 0.5274, 1.5176, 0.8497, 1.3741, 1.3829, 1.2305, 1.9161, 1.2305, 2.6543, 1.2305, 5.9532, 0, 5.9532, 0, 6.7969, 1.2833, 6.7969, 1.3418, 9.334, 3.2754, 9.8262, 3.2754, 6.7969, 5.1856, 6.7969, 5.1856, 5.9532, 3.2754, 5.9532, 3.2754, 2.4493, 3.2754, 1.9219, 3.4366, 1.6436, 3.5977, 1.3653, 3.9844, 1.3594, 4.1836, 1.3594, 4.4122, 1.4327, 4.6407, 1.5059, 4.7696, 1.5821, 4.9043, 1.6524, 4.8985, 1.6641, 5.379, 0.4278, 5.3438, 0.4043, 5.2823, 0.3692, 5.2208, 0.334, 5.0127, 0.2461, 4.8047, 0.1583, 4.585, 0.0879, 4.3653, 0.0176, 4.0342, -0.0381, 3.7032, -0.0938, 3.3809, -0.0938]
        },
        u: {
            bounds: [1.1719, -0.1524, 6.0528, 6.9493],
            commands: "MQQQLLLLLQQQQLLZ",
            bearing: [1.1719, 0.6915],
            points: [3.1993, 2.4786, 3.1934, 1.8985, 3.3985, 1.5586, 3.5918, 1.2422, 4.0782, 1.2188, 4.7051, 1.2657, 5.168, 1.7813, 5.168, 6.7969, 7.2247, 6.7969, 7.2247, 0, 5.7774, 0, 5.3262, 1.0372, 5.0743, 0.3926, 4.7872, 0.1758, 4.3711, -0.1524, 3.6797, -0.1524, 2.4024, -0.1524, 1.7872, 0.545, 1.1719, 1.2422, 1.1719, 2.6309, 1.1719, 6.7969, 3.1993, 6.7969]
        },
        v: {
            bounds: [0.0352, 0, 6.8438, 6.7969],
            commands: "MLLLLLLZ",
            bearing: [0.0352, 0.0352],
            points: [4.6934, 0, 2.209, 0, 0.0352, 6.7969, 2.1915, 6.7969, 3.4512, 1.7637, 4.6993, 6.7969, 6.879, 6.7969]
        },
        w: {
            bounds: [0.1407, 0, 9.2403, 6.7969],
            commands: "MLLLLLLLLLLLLZ",
            bearing: [0.1407, 0.1407],
            points: [5.6133, 0, 4.7344, 3.8204, 3.879, 0, 1.834, 0, 0.1407, 6.7969, 2.045, 6.7969, 2.9356, 2.3614, 3.8614, 6.7969, 5.6602, 6.7969, 6.5801, 2.3731, 7.5118, 6.7969, 9.3809, 6.7969, 7.6817, 0]
        },
        x: {
            bounds: [ - 0.0293, 0, 6.7969, 6.7969],
            commands: "MLLLLLLLLLLLZ",
            bearing: [ - 0.0293, -0.0586],
            points: [3.3809, 2.0274, 2.2852, 0, 0.0176, 0, 2.3379, 3.4219, -0.0293, 6.7969, 2.2852, 6.7969, 3.4043, 4.8809, 4.4942, 6.7969, 6.7676, 6.7969, 4.4532, 3.4219, 6.7325, 0, 4.4942, 0]
        },
        y: {
            bounds: [0.1407, -2.7422, 6.9141, 9.5391],
            commands: "MLLLLLLQQLQQQQZ",
            bearing: [0.1407, 0.1348],
            points: [2.9649, 0.252, 0.1407, 6.7969, 2.2383, 6.7969, 3.8555, 2.1915, 4.9805, 6.7969, 7.0547, 6.7969, 4.7344, -0.1875, 4.3125, -1.4883, 3.5391, -2.1094, 2.7657, -2.7305, 1.4766, -2.7422, 0.9024, -1.3536, 1.4649, -1.3125, 1.8223, -1.21, 2.1797, -1.1075, 2.4024, -0.8907, 2.625, -0.6739, 2.7364, -0.4249, 2.8477, -0.1758, 2.9649, 0.252]
        },
        z: {
            bounds: [0.5977, 0, 5.127, 6.8086],
            commands: "MLLLLLLLLLZ",
            bearing: [0.5977, 0.4454],
            points: [3.4571, 5.2559, 0.668, 5.2618, 0.668, 6.8086, 5.6602, 6.8086, 5.6602, 5.4551, 2.8418, 1.5469, 5.7247, 1.5469, 5.7247, 0, 0.5977, 0, 0.5977, 1.3653]
        },
        0 : {
            bounds: [0.8262, -0.1583, 6.8672, 7.2247],
            commands: "MQQQQQQQQQQQQQQQQQQZMQQQQQQQQQQQZ",
            bearing: [0.8262, 0.8438],
            points: [4.2481, 1.3653, 4.5938, 1.3653, 4.8575, 1.4737, 5.1211, 1.5821, 5.294, 1.7725, 5.4668, 1.9629, 5.5752, 2.2413, 5.6836, 2.5196, 5.7276, 2.8301, 5.7715, 3.1407, 5.7715, 3.5274, 5.7715, 3.9668, 5.6924, 4.3184, 5.6133, 4.67, 5.4405, 4.9512, 5.2676, 5.2325, 4.9659, 5.3877, 4.6641, 5.543, 4.2481, 5.543, 3.9141, 5.543, 3.6534, 5.4288, 3.3926, 5.3145, 3.2256, 5.1241, 3.0586, 4.9336, 2.9502, 4.6641, 2.8418, 4.3946, 2.7979, 4.1016, 2.754, 3.8086, 2.7598, 3.4688, 2.7657, 2.9825, 2.8389, 2.6133, 2.9122, 2.2442, 3.0792, 1.9542, 3.2461, 1.6641, 3.542, 1.5147, 3.8379, 1.3653, 4.2481, 1.3653, 4.2481, 7.0665, 5.3614, 7.0665, 6.1436, 6.6241, 6.9258, 6.1817, 7.3096, 5.376, 7.6934, 4.5704, 7.6934, 3.4571, 7.6934, 2.3497, 7.3038, 1.5411, 6.9141, 0.7325, 6.1348, 0.2872, 5.3555, -0.1583, 4.251, -0.1583, 3.1465, -0.1583, 2.3702, 0.2901, 1.5938, 0.7383, 1.21, 1.544, 0.8262, 2.3497, 0.8262, 3.4571, 0.8262, 5.1563, 1.7051, 6.1114, 2.584, 7.0665, 4.2481, 7.0665]
        },
        1 : {
            bounds: [0.586, 0, 3.504, 6.8614],
            commands: "MLLLLLLZ",
            bearing: [0.586, 1.254],
            points: [2.0508, 0, 2.0508, 5.2266, 1.1368, 4.834, 0.586, 6.2286, 2.1504, 6.8614, 4.0899, 6.8497, 4.0899, 0]
        },
        2 : {
            bounds: [0.8204, 0, 5.3731, 6.9786],
            commands: "MQQQQLQLLLQQQQQQQQQQQLLLLLLQQLQLQQQQZ",
            bearing: [0.8204, 0.6856],
            points: [4.2422, 4.6055, 4.2422, 5.0333, 4.0284, 5.2413, 3.8145, 5.4493, 3.3399, 5.4493, 3.0821, 5.4493, 2.7481, 5.3672, 2.4141, 5.2852, 2.0977, 5.1563, 1.629, 4.9629, 1.4766, 4.8985, 1.3594, 4.8399, 1.3536, 4.834, 0.8204, 6.2344, 0.8321, 6.2403, 0.9786, 6.3165, 1.1631, 6.3956, 1.3477, 6.4747, 1.7461, 6.6299, 2.1446, 6.7852, 2.6045, 6.8819, 3.0645, 6.9786, 3.4688, 6.9786, 3.8731, 6.9786, 4.2481, 6.9141, 4.6231, 6.8497, 4.9834, 6.6944, 5.3438, 6.5391, 5.6075, 6.3077, 5.8711, 6.0762, 6.0323, 5.71, 6.1934, 5.3438, 6.1934, 4.8868, 6.1934, 4.1954, 5.8565, 3.6299, 5.5196, 3.0645, 4.6758, 2.3204, 3.8321, 1.5704, 6.0118, 1.5704, 6.0118, 0, 0.9844, 0, 0.9844, 1.2715, 2.8887, 2.8829, 2.9122, 2.9004, 3.0674, 3.0293, 3.2227, 3.1583, 3.2754, 3.2051, 3.4688, 3.375, 3.6153, 3.504, 3.6739, 3.5743, 3.8438, 3.7618, 3.961, 3.8848, 4.0137, 3.9727, 4.0665, 4.0606, 4.128, 4.1749, 4.1895, 4.2891, 4.2159, 4.3946, 4.2422, 4.5, 4.2422, 4.6055]
        },
        3 : {
            bounds: [0.7032, -1.0313, 5.3321, 8.0743],
            commands: "MQQQQQQQQLLLQQQQQQQQQQQQQQQQQQQQQLLLQQQQQQQQLQQQQZ",
            bearing: [0.7032, 0.8731],
            points: [4.0079, 4.8575, 4.0079, 5.1856, 3.7413, 5.3467, 3.4747, 5.5079, 3.0411, 5.5079, 2.8887, 5.5079, 2.7188, 5.4844, 2.5489, 5.461, 2.3907, 5.4258, 2.2325, 5.3907, 2.0772, 5.3467, 1.9219, 5.3028, 1.7959, 5.2588, 1.67, 5.2149, 1.5733, 5.1768, 1.4766, 5.1387, 1.4239, 5.1211, 1.3711, 5.0977, 0.7032, 6.4395, 0.8086, 6.4981, 0.92, 6.5567, 1.125, 6.6475, 1.3301, 6.7383, 1.585, 6.8262, 1.8399, 6.9141, 2.1915, 6.9786, 2.543, 7.043, 2.8946, 7.043, 3.4278, 7.043, 3.8584, 6.9727, 4.2891, 6.9024, 4.6583, 6.7383, 5.0274, 6.5743, 5.2764, 6.3194, 5.5254, 6.0645, 5.669, 5.6866, 5.8125, 5.3086, 5.8243, 4.8223, 5.836, 4.2715, 5.5137, 3.8057, 5.1915, 3.3399, 4.6465, 3.2051, 5.2442, 3.0586, 5.6397, 2.5401, 6.0352, 2.0215, 6.0352, 1.4004, 6.0352, 0.7793, 5.8067, 0.3047, 5.5782, -0.17, 5.1651, -0.4571, 4.752, -0.7442, 4.2071, -0.8877, 3.6622, -1.0313, 3, -1.0313, 2.625, -1.0313, 2.2618, -0.9903, 1.8985, -0.9493, 1.6465, -0.8907, 1.3946, -0.8321, 1.1983, -0.7735, 1.002, -0.7149, 0.9083, -0.6739, 0.8086, -0.6329, 1.295, 0.8438, 1.3008, 0.8438, 1.4297, 0.7911, 1.6055, 0.7295, 1.7813, 0.668, 2.1973, 0.5743, 2.6133, 0.4805, 2.9415, 0.4805, 3.5333, 0.4805, 3.835, 0.6856, 4.1368, 0.8907, 4.1368, 1.3829, 4.1368, 1.7344, 3.8819, 1.9952, 3.627, 2.2559, 3.2374, 2.376, 2.8477, 2.4961, 2.4024, 2.4844, 2.4024, 3.9258, 2.7247, 3.9258, 3, 3.9756, 3.2754, 4.0254, 3.5069, 4.1309, 3.7383, 4.2364, 3.8731, 4.4239, 4.0079, 4.6114, 4.0079, 4.8575]
        },
        4 : {
            bounds: [0.4922, -1.2715, 6.334, 8.4024],
            commands: "MLLLLLLLLLLZMLLZ",
            bearing: [0.4922, 0.6797],
            points: [4.295, 7.1309, 5.7715, 6.7442, 5.7715, 1.8575, 6.8262, 1.8575, 6.8262, 0.3458, 5.7715, 0.3458, 5.7715, -1.2715, 4.084, -1.2657, 4.084, 0.3458, 0.7325, 0.3458, 0.4922, 1.5235, 2.4434, 1.8458, 4.084, 1.8458, 4.084, 4.295]
        },
        5 : {
            bounds: [0.7559, -1.0899, 5.3907, 7.9395],
            commands: "MLQQQQQQQQQLLLLLLQQQQQQQQQQZ",
            bearing: [0.7559, 0.7325],
            points: [1.002, 0.9434, 1.125, 0.8965, 1.2071, 0.8672, 1.4473, 0.7911, 1.6875, 0.7149, 1.9219, 0.6592, 2.1563, 0.6036, 2.4756, 0.5538, 2.795, 0.504, 3.0645, 0.504, 3.6973, 0.4981, 4.0489, 0.7823, 4.4004, 1.0665, 4.3946, 1.6407, 4.3887, 2.2442, 4.0928, 2.6075, 3.7969, 2.9708, 3.2227, 2.9649, 2.6309, 2.9649, 1.7579, 2.4903, 0.9375, 3.2168, 0.9375, 6.8497, 5.6543, 6.8497, 5.6543, 5.4141, 2.584, 5.4141, 2.584, 4.0958, 2.584, 4.1075, 2.6807, 4.1631, 2.7774, 4.2188, 2.9854, 4.2803, 3.1934, 4.3418, 3.4219, 4.3418, 4.6758, 4.3418, 5.4112, 3.6182, 6.1465, 2.8946, 6.1465, 1.6641, 6.1465, 0.8204, 5.751, 0.1905, 5.3555, -0.4395, 4.6553, -0.7647, 3.9551, -1.0899, 3.0528, -1.0665, 2.4844, -1.0489, 1.8721, -0.8965, 1.2598, -0.7442, 0.7559, -0.5274]
        },
        6 : {
            bounds: [0.8028, -0.586, 6.1641, 8.543],
            commands: "MQQQQQQQQQQQQZMQQQQQQQQQQQQQLQQQQQQZ",
            bearing: [0.8028, 0.4571],
            points: [2.7129, 2.6778, 2.7247, 2.2735, 2.7979, 1.9629, 2.8711, 1.6524, 3.0206, 1.418, 3.17, 1.1836, 3.4219, 1.0606, 3.6739, 0.9375, 4.0196, 0.9375, 4.2774, 0.9375, 4.4737, 1.0225, 4.67, 1.1075, 4.793, 1.251, 4.9161, 1.3946, 4.9922, 1.6055, 5.0684, 1.8165, 5.0977, 2.0391, 5.127, 2.2618, 5.127, 2.5372, 5.127, 3.0762, 4.8575, 3.3575, 4.5879, 3.6387, 4.0665, 3.6387, 3.2989, 3.6387, 2.7129, 3.0821, 6.9668, 2.2618, 6.9668, 1.6348, 6.7647, 1.1104, 6.5625, 0.586, 6.1905, 0.211, 5.8184, -0.1641, 5.2647, -0.375, 4.711, -0.586, 4.0313, -0.586, 3.2813, -0.586, 2.6631, -0.3282, 2.045, -0.0704, 1.6436, 0.3721, 1.2422, 0.8145, 1.0225, 1.4063, 0.8028, 1.9981, 0.8028, 2.6719, 0.8028, 3.5508, 1.0401, 4.3125, 1.2774, 5.0743, 1.6875, 5.6485, 2.0977, 6.2227, 2.6749, 6.6827, 3.252, 7.1426, 3.9141, 7.4502, 4.5762, 7.7579, 5.3379, 7.9571, 5.8067, 6.4102, 4.0782, 6.0586, 3.3106, 5.0391, 3.7325, 5.1211, 4.1075, 5.1211, 4.7461, 5.1211, 5.2911, 4.8868, 5.836, 4.6524, 6.1993, 4.2569, 6.5625, 3.8614, 6.7647, 3.3428, 6.9668, 2.8243, 6.9668, 2.2618]
        },
        7 : {
            bounds: [0.7618, -1.3829, 5.625, 8.2325],
            commands: "MLLLLLLZ",
            bearing: [0.7618, 0.211],
            points: [5.9063, 6.8497, 6.3868, 5.795, 2.9415, -1.3829, 1.2012, -0.6094, 4.0606, 5.3379, 0.7618, 5.3379, 0.7618, 6.8497]
        },
        8 : {
            bounds: [0.8145, -0.7442, 6.0235, 8.4961],
            commands: "MQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQZMQQQQQQQQZMQQQQQQZ",
            bearing: [0.8145, 0.8145],
            points: [3.8292, 7.752, 4.4297, 7.752, 4.9424, 7.6055, 5.4551, 7.459, 5.8448, 7.1807, 6.2344, 6.9024, 6.4571, 6.4659, 6.6797, 6.0293, 6.6797, 5.4844, 6.6797, 4.7637, 6.2461, 4.2422, 5.8125, 3.7208, 5.1387, 3.586, 5.4668, 3.5391, 5.7715, 3.3721, 6.0762, 3.2051, 6.3135, 2.9532, 6.5508, 2.7012, 6.6944, 2.3497, 6.8379, 1.9981, 6.8379, 1.6114, 6.8379, 1.1192, 6.6739, 0.7208, 6.5098, 0.3223, 6.2256, 0.0528, 5.9415, -0.2168, 5.5518, -0.3985, 5.1622, -0.5801, 4.7315, -0.6622, 4.3008, -0.7442, 3.8262, -0.7442, 3.3516, -0.7442, 2.9209, -0.6622, 2.4903, -0.5801, 2.1036, -0.3985, 1.7168, -0.2168, 1.4327, 0.0528, 1.1485, 0.3223, 0.9815, 0.7208, 0.8145, 1.1192, 0.8145, 1.6114, 0.8145, 1.9981, 0.9581, 2.3497, 1.1016, 2.7012, 1.3418, 2.9532, 1.5821, 3.2051, 1.8868, 3.3721, 2.1915, 3.5391, 2.5137, 3.586, 1.8458, 3.7208, 1.4092, 4.2422, 0.9727, 4.7637, 0.9727, 5.4844, 0.9727, 6.0293, 1.1954, 6.4659, 1.418, 6.9024, 1.8077, 7.1807, 2.1973, 7.459, 2.7129, 7.6055, 3.2286, 7.752, 3.8292, 7.752, 3.8292, 0.8028, 4.4004, 0.8028, 4.6758, 1.0137, 4.9512, 1.2247, 4.9512, 1.7403, 4.9512, 2.3086, 4.6641, 2.6485, 4.377, 2.9883, 3.8262, 2.9883, 3.2754, 2.9883, 2.9883, 2.6485, 2.7012, 2.3086, 2.7012, 1.7403, 2.7071, 1.2247, 2.9825, 1.0137, 3.2579, 0.8028, 3.8292, 0.8028, 3.8262, 4.418, 4.3418, 4.418, 4.5938, 4.6524, 4.8458, 4.8868, 4.8458, 5.4024, 4.8458, 6.2637, 3.8262, 6.2637, 2.8067, 6.2637, 2.8067, 5.4024, 2.8125, 4.8868, 3.0616, 4.6524, 3.3106, 4.418, 3.8262, 4.418]
        },
        9 : {
            bounds: [0.6797, -1.5821, 6.1758, 8.543],
            commands: "MQQQQLLQQQQQQQQZMQQQQQQQQQQQQLQQQZ",
            bearing: [0.6797, 0.5567],
            points: [2.5254, 4.0079, 2.5196, 3.5098, 2.795, 3.1993, 3.0704, 2.8887, 3.5215, 2.8887, 3.7266, 2.8887, 4.0694, 2.9151, 4.4122, 2.9415, 4.6524, 2.9649, 4.8868, 2.9883, 4.9747, 3.7969, 4.9688, 4.1719, 4.8956, 4.462, 4.8223, 4.752, 4.67, 4.9747, 4.5176, 5.1973, 4.2657, 5.3174, 4.0137, 5.4375, 3.668, 5.4375, 3.3223, 5.4375, 3.0938, 5.3467, 2.8653, 5.2559, 2.7452, 5.0596, 2.625, 4.8633, 2.5782, 4.6202, 2.5313, 4.377, 2.5254, 4.0079, 3.545, 1.4356, 2.2559, 1.4356, 1.4678, 2.1417, 0.6797, 2.8477, 0.6797, 4.0899, 0.6797, 4.9278, 1.0342, 5.5811, 1.3887, 6.2344, 2.0391, 6.5977, 2.6895, 6.961, 3.5391, 6.961, 5.1153, 6.961, 5.9854, 6.1026, 6.8555, 5.2442, 6.8555, 3.709, 6.8555, 2.8125, 6.5684, 2.0098, 6.2813, 1.2071, 5.8184, 0.6211, 5.3555, 0.0352, 4.752, -0.4307, 4.1485, -0.8965, 3.5333, -1.1719, 2.918, -1.4473, 2.3086, -1.5821, 1.8516, -0.0235, 2.7364, 0.1583, 3.378, 0.5772, 4.0196, 0.9961, 4.377, 1.5411, 3.9844, 1.4356, 3.545, 1.4356]
        },
        "+": {
            bounds: [0.6739, 0.5684, 4.9454, 5.6954],
            commands: "MLLLLLLLLLLLZ",
            bearing: [0.6739, 0.6153],
            points: [5.6192, 2.6368, 3.9844, 2.6368, 3.9844, 0.5684, 2.2969, 0.5684, 2.2969, 2.6368, 0.6739, 2.6368, 0.6739, 4.2129, 2.2969, 4.2129, 2.2969, 6.2637, 3.9844, 6.2637, 3.9844, 4.2129, 5.6192, 4.2129]
        },
        "-": {
            bounds: [0.9141, 3.3633, 4.6524, 1.5352],
            commands: "MLLLZ",
            bearing: [0.9141, 0.9961],
            points: [5.5665, 3.3633, 0.9141, 3.3633, 0.9141, 4.8985, 5.5665, 4.8985]
        }
    };
    function Lc(a, b, c, d) {
        this.Ba = a;
        this.Ca = b;
        this.wc = c;
        this.xe = d;
        this.tb = new P;
        this.ca = new E(0, 0);
        this.Ej = this.Db = 0.6
    }
    q = Lc.prototype;
    q.gb = function() {
        return Mc(this.Ba, this.tb)
    };
    q.Va = h("Ca");
    q.Ec = function() {
        return this.wc + this.Ca.width + this.xe
    };
    q.ee = function() {
        return this.Ca.height - (this.ca.y - this.Ca.y)
    };
    q.Mf = function() {
        return this.ca.y - this.Ca.y
    };
    q.Nf = function() {
        return this.Ca.height
    };
    q.moveTo = function(a) {
        this.translate( - (this.tb.Jb - a.x), -(this.tb.Kb - a.y))
    };
    q.translate = function(a, b) {
        this.tb.translate(a, b);
        this.Ca.x += a;
        this.Ca.y += b;
        this.ca.x += a;
        this.ca.y += b
    };
    function Nc(a) {
        this.wh = Kc;
        this.rb = a / this.wh.F.bounds[3];
        this.tb = (new P).setTransform(this.rb, 0, 0, this.rb, 0, 0);
        this.zg = a
    };
    function Oc() {
        this.rb = 1;
        this.Zf = new Gc(Pc(this), Q["line-color"]);
        this.oj = this.Ye = new Gc(Pc(this), Q["edit-color"], Q["edit-transparency"], [Pc(this), 2 * Pc(this)]);
        this.Ha = new Nc(Q["node-cap-height"] * this.rb)
    }
    var Qc, Rc, Sc, Tc, Uc;
    Oc.prototype.Qd = function(a) {
        this.rb = a;
        a = Pc(this);
        this.Zf.sg = a;
        this.Ye.sg = a;
        this.Ye.be = [a, 2 * a];
        this.Ha = new Nc(Q["node-cap-height"] * this.rb)
    };
    Oc.prototype.Oe = h("rb");
    function Pc(a) {
        return Q["line-width"] * a.rb
    }
    function Vc(a) {
        return Q["line-offset"] * a.rb
    }
    function Wc(a) {
        return Q["stereo-width"] * a.rb
    }
    function Xc(a) {
        return Q["node-radius"] * a.rb
    }
    function Yc(a) {
        for (var b in a) if ("node-colors" === b) {
            var c = a[b] || {},
            d;
            for (d in c) Q[b][d] = a[b][d]
        } else Q[b] = a[b];
        a = Q["node-colors"];
        for (var f in a) Zc[f] = new Hc(a[f]);
        Qc = new Hc(Q["unknown-node-color"]);
        Sc = new Hc(Q["edit-color"], Q["edit-transparency"]);
        Tc = new Hc("#ff0000", 0.75);
        Uc = Sc;
        Qc = new Hc(Q["unknown-node-color"]);
        Rc = new Hc(Q["line-color"])
    }
    var Q = {
        "node-colors": {}
    },
    Zc = {};
    Yc({
        border: 0.15,
        "line-width": 0.1,
        "line-offset": 0.2,
        "line-end-padding": 0.08,
        "stereo-width": 0.25,
        "node-cap-height": 0.42,
        "node-radius": 0.3,
        "methyl-visible": !1,
        "taper-hash": !0,
        "edit-color": "#00ff00",
        "edit-transparency": 0.75,
        "line-color": "#000000",
        "unknown-node-color": "#555555",
        "node-colors": {
            N: "#0000ff",
            O: "#ff0000",
            F: "#ff00ff",
            P: "#ff9900",
            S: "#ff9900",
            Cl: "#00ff00",
            Br: "#CC3333",
            I: "#940094"
        }
    });
    function $c(a, b, c) {
        this.x = a;
        this.y = b;
        this.lj = c
    };
    function ad(a, b, c, d) {
        this.x = a;
        this.y = b;
        this.width = c;
        this.height = d
    }
    ad.prototype.pc = function() {
        return new E(this.x + 0.5 * this.width, this.y + 0.5 * this.height)
    };
    ad.prototype.va = function() {
        return new this.constructor(this.x, this.y, this.width, this.height)
    };
    function R() {
        this.Ya = [];
        this.bb = [];
        this.wb = []
    }
    R.prototype.Sb = null;
    R.prototype.eb = null;
    R.prototype.Rc = !0;
    var bd = [2, 2, 6, 6, 0];
    function cd(a, b) {
        b.eb && (Array.prototype.push.apply(a.Ya, b.Ya), Array.prototype.push.apply(a.bb, b.bb), Array.prototype.push.apply(a.wb, b.wb), a.eb = b.eb.concat(), a.Sb = b.Sb.concat(), a.Rc = a.Rc && b.Rc)
    }
    q = R.prototype;
    q.clear = function() {
        this.Ya.length = 0;
        this.bb.length = 0;
        this.wb.length = 0;
        delete this.Sb;
        delete this.eb;
        delete this.Rc;
        return this
    };
    q.moveTo = function(a, b) {
        0 == this.Ya[this.Ya.length - 1] ? this.wb.length -= 2 : (this.Ya.push(0), this.bb.push(1));
        this.wb.push(a, b);
        this.eb = this.Sb = [a, b];
        return this
    };
    q.lineTo = function(a) {
        var b = this.Ya[this.Ya.length - 1];
        if (null == b) throw Error("Path cannot start with lineTo");
        1 != b && (this.Ya.push(1), this.bb.push(0));
        for (b = 0; b < arguments.length; b += 2) {
            var c = arguments[b],
            d = arguments[b + 1];
            this.wb.push(c, d)
        }
        this.bb[this.bb.length - 1] += b / 2;
        this.eb = [c, d];
        return this
    };
    q.lb = function(a) {
        var b = this.Ya[this.Ya.length - 1];
        if (null == b) throw Error("Path cannot start with curve");
        2 != b && (this.Ya.push(2), this.bb.push(0));
        for (b = 0; b < arguments.length; b += 6) {
            var c = arguments[b + 4],
            d = arguments[b + 5];
            this.wb.push(arguments[b], arguments[b + 1], arguments[b + 2], arguments[b + 3], c, d)
        }
        this.bb[this.bb.length - 1] += b / 6;
        this.eb = [c, d];
        return this
    };
    q.close = function() {
        var a = this.Ya[this.Ya.length - 1];
        if (null == a) throw Error("Path cannot start with close");
        4 != a && (this.Ya.push(4), this.bb.push(1), this.eb = this.Sb);
        return this
    };
    q.Bf = function(a, b, c, d) {
        var f = this.eb[0] - a * Math.cos(c * Math.PI / 180),
        g = this.eb[1] - b * Math.sin(c * Math.PI / 180),
        k = d * Math.PI / 180;
        d = Math.ceil(2 * (Math.abs(k) / Math.PI));
        k /= d;
        c = c * Math.PI / 180;
        for (var l = 0; l < d; l++) {
            var m = Math.cos(c),
            r = Math.sin(c),
            x = 4 / 3 * Math.sin(k / 2) / (1 + Math.cos(k / 2)),
            n = f + (m - x * r) * a,
            H = g + (r + x * m) * b;
            c += k;
            m = Math.cos(c);
            r = Math.sin(c);
            this.lb(n, H, f + (m + x * r) * a, g + (r - x * m) * b, f + m * a, g + r * b)
        }
        return this
    };
    function dd(a, b) {
        for (var c = a.wb,
        d = 0,
        f = 0,
        g = a.Ya.length; f < g; f++) {
            var k = a.Ya[f],
            l = bd[k] * a.bb[f];
            b(k, c.slice(d, d + l));
            d += l
        }
    }
    q.va = function() {
        var a = new this.constructor;
        a.Ya = this.Ya.concat();
        a.bb = this.bb.concat();
        a.wb = this.wb.concat();
        a.Sb = this.Sb && this.Sb.concat();
        a.eb = this.eb && this.eb.concat();
        a.Rc = this.Rc;
        return a
    };
    var ed = {};
    ed[0] = R.prototype.moveTo;
    ed[1] = R.prototype.lineTo;
    ed[4] = R.prototype.close;
    ed[2] = R.prototype.lb;
    ed[3] = R.prototype.Bf;
    function fd(a) {
        if (a.Rc) return a.va();
        var b = new R;
        dd(a,
        function(a, d) {
            ed[a].apply(b, d)
        });
        return b
    }
    function Mc(a, b) {
        var c = fd(a);
        c.transform(b);
        return c
    }
    R.prototype.transform = function(a) {
        if (!this.Rc) throw Error("Non-simple path");
        a.transform(this.wb, 0, this.wb, 0, this.wb.length / 2);
        this.Sb && a.transform(this.Sb, 0, this.Sb, 0, 1);
        this.eb && this.Sb != this.eb && a.transform(this.eb, 0, this.eb, 0, 1);
        return this
    };
    R.prototype.Ma = function() {
        return 0 == this.Ya.length
    };
    R.prototype.Va = function() {
        if (!this.Ma()) {
            var a, b = a = Number.POSITIVE_INFINITY,
            c, d = c = Number.NEGATIVE_INFINITY;
            dd(this,
            function(f, g) {
                for (var k = 0,
                l = g.length; k < l; k += 2) b = Math.min(b, g[k]),
                d = Math.max(d, g[k]),
                a = Math.min(a, g[k + 1]),
                c = Math.max(c, g[k + 1])
            });
            return new ad(b, a, d - b, c - a)
        }
    };
    function gd(a) {
        var b = [];
        dd(a,
        function(a, d) {
            switch (a) {
            case 0:
                b.push(hd);
                Array.prototype.push.apply(b, d);
                break;
            case 1:
                b.push(id);
                Array.prototype.push.apply(b, d);
                break;
            case 2:
                b.push(jd);
                Array.prototype.push.apply(b, d);
                break;
            case 3:
                var f = d[3];
                b.push(kd, d[0], d[1], 0, 180 < Math.abs(f) ? 1 : 0, 0 < f ? 1 : 0, d[4], d[5]);
                break;
            case 4:
                b.push(ld)
            }
        });
        return b.join(" ")
    }
    var hd = "M",
    id = "L",
    jd = "C",
    kd = "A",
    ld = "Z";
    function md(a) {
        this.Ra = [];
        this.bg = 0;
        this.ca = new E(0, 0);
        this.dh = a || 0
    }
    function S(a, b, c, d, f) {
        for (var g = 0; g < b.length; g++) {
            var k;
            var l = c;
            if (k = l.wh[b[g]]) {
                for (var m = l,
                r = k.commands.split(""), x = k.points, n = new R, H = 0, ka = 0, be = void 0, Zf = void 0, ce = void 0, de = void 0, H = 0; H < r.length; H += 1) switch (r[H]) {
                case "M":
                    n.moveTo(x[ka++], x[ka++]);
                    break;
                case "L":
                    n.lineTo(x[ka++], x[ka++]);
                    break;
                case "C":
                    throw {
                        name:
                        "UnsupportedOperation",
                        message: "Path command C not used."
                    };
                case "Q":
                    be = x[ka++];
                    Zf = x[ka++];
                    ce = x[ka++];
                    de = x[ka++];
                    n.lb(be, Zf, ce, de, ce, de);
                    break;
                case "Z":
                    n.close();
                    break;
                default:
                    throw {
                        name:
                        "UnsupportedOperation",
                        message: "Command " + r[H] + " not known."
                    };
                }
                n.transform(m.tb);
                l = l.tb.ib;
                k = new Lc(n, new ad(k.bounds[0] * l, k.bounds[1] * l, k.bounds[2] * l, k.bounds[3] * l), k.bearing[0] * l, k.bearing[1] * l)
            } else k = void 0;
            k && (d ? (n = k, l = new P, l.scale(n.Db, n.Db), n.Ba = Mc(n.Ba, l), n.wc *= n.Db, n.xe *= n.Db, n.Ca.width *= n.Db, n.Ca.height *= n.Db, n.Ca.x = n.ca.x + n.wc, n.Ca.y = n.ca.y - n.Ca.y * n.Db) : f && (n = k, l = c.zg, m = new P, r = l - n.ee() * n.Db, m.translate(0, r), m.scale(n.Db, n.Db), n.Ba.transform(m), n.wc *= n.Db, n.xe *= n.Db, n.Ca.height *= n.Db, n.Ca.y = n.ca.y + l - n.Ca.height, n.Ca.x = n.ca.x + n.wc, n.Ca.width *= n.Db), n = a, k.moveTo(new E(n.ca.x + n.bg, n.ca.y)), n.Ra.push(k), n.bg += k.Ec())
        }
    }
    q = md.prototype;
    q.gb = function() {
        for (var a = new R,
        b = 0; b < this.Ra.length; b++) cd(a, this.Ra[b].gb());
        return a
    };
    q.Nf = function() {
        return this.ee() + this.Mf()
    };
    q.ee = function() {
        for (var a = 0,
        b = 0; b < this.Ra.length; b++) a = Math.max(a, this.Ra[b].ee());
        return a
    };
    q.Mf = function() {
        for (var a = 0,
        b = 0; b < this.Ra.length; b++) a = Math.max(a, this.Ra[b].Mf());
        return a
    };
    q.translate = function(a, b) {
        for (var c = 0; c < this.Ra.length; c++) this.Ra[c].translate(a, b);
        this.ca.x += a;
        this.ca.y += b
    };
    function nd(a, b) {
        if (0 !== a.Ra.length) {
            var c = a.Ra[0],
            d = a.Ra[a.Ra.length - 1].Va(),
            d = d.x + d.width - c.Va().x,
            c = b.x - a.ca.x - c.wc - 0.5 * d,
            d = b.y - a.ca.y - 0.5 * a.ee();
            a.translate(c, d)
        }
    }
    function od(a, b) {
        var c = b.ca;
        a.translate(c.x - a.ca.x - a.Ec(), c.y - a.ca.y)
    }
    function pd(a, b) {
        var c = b.ca;
        a.translate(c.x - a.ca.x + b.Ec(), c.y - a.ca.y)
    }
    q.Ec = function() {
        for (var a = 0,
        b = 0; b < this.Ra.length; b++) a += this.Ra[b].Ec();
        return a
    };
    function qd(a, b, c) {
        var d = c.Va(),
        f = c.ca;
        a.translate(b.x - (f.x + c.wc + 0.5 * d.width), b.y - (f.y + 0.5 * (d.y + d.height - f.y)))
    };
    function rd(a) {
        this.qb = a
    }
    rd.prototype.Rf = function() {
        var a = sd,
        b = this.qb.ua;
        if (0 === b.length && this.qb.aa) {
            var c;
            c = this.qb.aa().fa;
            if (16 === c || 17 === c) return td
        }
        c = {};
        c[ud] = 0;
        c[vd] = 0;
        c[sd] = 0;
        for (var d = c[td] = 0; d < b.length; d++) {
            var f = c,
            g = Ac(this.qb, b[d]),
            k = Math.max(0, Math.sin(g)),
            l = Math.max(0, -Math.sin(g)),
            m = Math.max(0, Math.cos(g)),
            g = Math.max(0, -Math.cos(g)),
            k = k * k,
            l = l * l,
            m = m * m,
            g = g * g;
            f[ud] += k;
            f[vd] += l;
            f[sd] += m;
            f[td] += g
        }
        b = [sd, td, ud, vd];
        for (d = 0; d < b.length; d++) {
            f = b[d];
            k = !0;
            for (l = d + 1; l < b.length; l++) if (1E-4 < c[f] - c[b[l]]) {
                k = !1;
                break
            }
            if (k) {
                a = f;
                break
            }
        }
        return a
    };
    var ud = "node-descriptor-north",
    vd = "node-descriptor-south",
    sd = "node-descriptor-east",
    td = "node-descriptor-west";
    function wd(a, b, c, d, f, g, k) {
        this.Yb = 0.15 * d.zg;
        this.ca = a;
        this.vf = b;
        this.Yh = c;
        this.Ha = d;
        this.Cd = 0 < f ? "H": "";
        this.Ue = 1 < f ? f.toString() : "";
        this.mc = "";
        void 0 !== g && (this.mc = 1 < Math.abs(g) ? Math.abs(g).toString() : "", g && (this.mc += 0 < g ? "+": "-"));
        this.Sa = k ? k.toString() : "";
        this.Ba = new R
    }
    wd.prototype.gb = h("Ba");
    function xd(a) {
        this.Da = a;
        this.wg = this.Gh = this.Yd = this.Ee = 0
    }
    xd.prototype.Rf = function() {
        if (3 < this.Da.ka.ua.length || 3 < this.Da.pa.ua.length) return yd;
        for (var a = this.Da.ka.ua,
        b = 0; b < a.length; b++) {
            var c = a[b];
            c !== this.Da.pa && (0 <= yc(this.Da.ka.ca, this.Da.pa.ca, c.ca) ? (this.Vb = c, this.Ee++) : (this.Zd = c, this.Yd++))
        }
        a = this.Da.pa.ua;
        for (b = 0; b < a.length; b++) c = a[b],
        c !== this.Da.ka && (0 <= yc(this.Da.ka.ca, this.Da.pa.ca, c.ca) ? (this.dc = c, this.Ee++) : (this.$d = c, this.Yd++));
        if (0 === this.Ee && 0 === this.Yd) return yd;
        if (this.Ee > this.Yd) return zd;
        if (this.Ee < this.Yd) return Ad;
        if (this.Vb && !this.Zd && !this.dc && this.$d || this.Zd && !this.Vb && !this.$d && this.dc) return zd;
        if (this.Vb && this.Zd && !this.dc && !this.$d || this.dc && this.$d && !this.Vb && !this.Zd) return yd;
        Bd(this) && this.Gh++;
        this.wg++;
        return this.Gh >= this.wg ? zd: Ad
    };
    function Bd(a) {
        if (a.Vb === a.dc) return [a.Vb];
        if ( - 1 !== a.Vb.ua.indexOf(a.dc)) return [a.Vb, a.dc];
        for (var b = a.Vb.ua,
        c = a.dc.ua,
        d = 0; d < b.length; d++) {
            var f = b[d];
            if (f !== a.Da.ka) for (var g = 0; g < c.length; g++) {
                var k = c[g];
                if (k !== a.Da.pa) {
                    if (f === k) return [a.Vb, f, a.dc];
                    if ( - 1 !== f.ua.indexOf(k)) return [a.Vb, f, k, a.dc]
                }
            }
        }
    }
    var yd = "edge-descriptor-center",
    zd = "edge-descriptor-top",
    Ad = "edge-descriptor-bottom";
    function Cd(a) {
        this.sa = a;
        this.gd = {};
        this.ce = {}
    }
    q = Cd.prototype;
    q.clear = function() {
        this.gd = {};
        this.ce = {}
    };
    q.Vc = function(a) {
        if ("C" === Dd(a.aa()) ? !0 === Q["methyl-visible"] && 1 === a.ua.length || void 0 !== a.he() || 0 !== a.fe() || 0 === a.ua.length: 1) {
            var b;
            b = this.sa.Ha;
            var c = a.ca,
            d = Dd(a.aa()),
            f = a.gc(),
            g = new rd(a),
            k = a.fe(),
            l = a.he() ? a.he().Gd: void 0;
            b = new wd(c, d, g, b, f, k, l);
            b.Ba.clear();
            var m;
            switch (b.Yh.Rf()) {
            case sd:
                m = new md(b.Yb);
                S(m, b.vf, b.Ha);
                S(m, b.Cd, b.Ha);
                S(m, b.Ue, b.Ha, !0);
                S(m, b.mc, b.Ha, !1, !0);
                qd(m, b.ca, m.Ra[0]);
                b.Sa && (c = new md(b.Yb), S(c, b.Sa, b.Ha, !1, !0), od(c, m), cd(b.Ba, c.gb()));
                c = (c = m.Ra[0]) ? c.Va() : void 0;
                b.ae = c;
                break;
            case td:
                m = new md(b.Yb);
                S(m, b.mc, b.Ha, !1, !0);
                S(m, b.Cd, b.Ha);
                S(m, b.Ue, b.Ha, !0);
                S(m, b.Sa, b.Ha, !1, !0);
                S(m, b.vf, b.Ha);
                qd(m, b.ca, m.Ra[m.Ra.length - 1]);
                c = (c = m.Ra[m.Ra.length - 1]) ? c.Va() : void 0;
                b.ae = c;
                break;
            case ud:
                m = new md(b.Yb);
                S(m, b.vf, b.Ha);
                nd(m, b.ca);
                b.mc !== Vb && (c = new md(b.Yb), S(c, b.mc, b.Ha, !1, !0), pd(c, m), cd(b.Ba, c.gb()));
                b.Sa !== Vb && (c = new md(b.Yb), S(c, b.Sa, b.Ha, !1, !0), od(c, m), cd(b.Ba, c.gb()));
                b.Cd !== Vb && (c = new md(b.Yb), S(c, b.Cd, b.Ha), S(c, b.Ue, b.Ha, !0), f = m.ca, d = f.x - c.ca.x, f = f.y - c.ca.y + m.Nf() + c.dh, c.translate(d, f), cd(b.Ba, c.gb()));
                b.ae = m.gb().Va();
                break;
            case vd:
                m = new md(b.Yb),
                S(m, b.vf, b.Ha),
                nd(m, b.ca),
                b.Sa !== Vb && (c = new md(b.Yb), S(c, b.Sa, b.Ha, !1, !0), od(c, m), cd(b.Ba, c.gb())),
                b.mc !== Vb && (c = new md(b.Yb), S(c, b.mc, b.Ha, !0), pd(c, m), cd(b.Ba, c.gb())),
                b.Cd !== Vb && (c = new md(b.Yb), S(c, b.Cd, b.Ha), S(c, b.Ue, b.Ha, !0), f = m, g = f.ca, d = g.x - c.ca.x, f = g.y - c.ca.y - c.Nf() - f.dh, c.translate(d, f), cd(b.Ba, c.gb())),
                b.ae = m.gb().Va()
            }
            b.xj = m.ca.y;
            cd(b.Ba, m.gb());
            this.gd[v(a)] = b;
            return b.gb()
        }
    };
    q.removeNode = function(a) {
        delete this.gd[v(a)]
    };
    q.kb = function(a) {
        delete this.ce[v(a)]
    };
    q.ub = function(a) {
        var b = this.gd[v(a.ka)];
        a: {
            var c = this.gd[v(a.pa)];
            switch (a.cb) {
            case 2:
                b = Ed(this, a, b, c);
                break a;
            case 4:
                if (a.nb() === Fd) {
                    var d = Gd(a),
                    f = d.va(),
                    g = Vc(this.sa);
                    Hd(this, d, b, c);
                    Hd(this, f, b, c);
                    vc(d, -g / 2);
                    vc(f, g / 2);
                    b = d.ya;
                    c = d.za;
                    g = f.za;
                    d.ya = f.ya;
                    d.za = g;
                    f.ya = b;
                    f.za = c;
                    b = [d, f]
                } else {
                    var d = new xd(a),
                    f = Gd(a),
                    g = Vc(this.sa),
                    k;
                    switch (d.Rf()) {
                    case zd:
                        k = f.va();
                        Hd(this, f, b, c);
                        Hd(this, k, b, c);
                        vc(k, g);
                        b || Id(f, k, d.Vb);
                        c || Jd(f, k, d.dc);
                        break;
                    case Ad:
                        k = f.va();
                        Hd(this, f, b, c);
                        Hd(this, k, b, c);
                        vc(k, -g);
                        b || Id(f, k, d.Zd);
                        c || Jd(f, k, d.$d);
                        break;
                    case yd:
                        Hd(this, f, b, c),
                        k = f.va(),
                        vc(f, -g / 2),
                        vc(k, g / 2)
                    }
                    b = [f, k]
                }
                break a;
            case 6:
                f = Gd(a);
                d = Vc(this.sa);
                Hd(this, f, b, c);
                b = f.va();
                c = f.va();
                f = [f, b, c];
                vc(b, -d);
                vc(c, d);
                b = f;
                break a;
            default:
                b = Ed(this, a, b, c)
            }
        }
        return this.ce[v(a)] = b
    };
    function Kd(a, b) {
        return new $c(b.ka.ca.x + 0.5 * (b.pa.ca.x - b.ka.ca.x), b.ka.ca.y + 0.5 * (b.pa.ca.y - b.ka.ca.y), Xc(a.sa))
    }
    q.zf = function(a) {
        return a.Ba
    };
    function Ld(a) {
        var b, c;
        for (c in a.ce) {
            var d = a.ce[c],
            f;
            if (void 0 === d.Va) {
                f = a;
                var g = b;
                for (b = 0; b < d.length; b++) {
                    var k = d[b];
                    g || (g = new dc(k.za, k.ya, k.za, k.ya));
                    var l = Q.border * f.sa.rb;
                    g.left = Math.min(g.left, Math.min(k.ya, k.$a) - l);
                    g.right = Math.max(g.right, Math.max(k.ya, k.$a) + l);
                    g.top = Math.max(g.top, Math.max(k.za, k.ab) + l);
                    g.bottom = Math.min(g.bottom, Math.min(k.za, k.ab) - l)
                }
                f = g
            } else f = a,
            d = d.Va(),
            b || (b = new dc(d.y + d.height, d.x + d.width, d.y, d.x)),
            f = Q.border * f.sa.rb,
            b.left = Math.min(b.left, d.x - f),
            b.right = Math.max(b.right, d.x + d.width + f),
            b.top = Math.max(b.top, d.y + d.height + f),
            b.bottom = Math.min(b.bottom, d.y - f),
            f = b;
            b = f
        }
        for (c in a.gd) f = a,
        d = b,
        b = a.gd[c].gb().Va(),
        d || (d = new dc(b.y + b.height, b.x + b.width, b.y, b.x)),
        f = Q.border * f.sa.rb,
        d.left = Math.min(d.left, b.x - f),
        d.right = Math.max(d.right, b.x + b.width + f),
        d.top = Math.max(d.top, b.y + b.height + f),
        d.bottom = Math.min(d.bottom, b.y - f),
        b = d;
        return b
    }
    function Ed(a, b, c, d) {
        switch (b.nb()) {
        case Md:
            return b = Gd(b),
            Hd(a, b, c, d),
            [b];
        case Nd:
            var f = Gd(b);
            Hd(a, f, c, d);
            d = oc(b.ka.ca, b.pa.ca);
            var g = b.ka.ca,
            k = pc(g.x, g.y, f.ya, f.za);
            c = pc(g.x, g.y, f.$a, f.ab);
            f = Wc(a.sa);
            a = 0.5 * k * f / d;
            f = 0.5 * c * f / d;
            d = new R;
            d.moveTo(k, a);
            d.lineTo(c, f);
            d.lineTo(c, -f);
            d.lineTo(k, -a);
            d.close();
            a = new P;
            b = zc(b);
            a.translate(g.x, g.y);
            a.rotate(b, 0, 0);
            d.transform(a);
            return d;
        case Od:
            k = Gd(b);
            Hd(a, k, c, d);
            c = oc(b.ka.ca, b.pa.ca);
            g = b.ka.ca;
            d = pc(g.x, g.y, k.ya, k.za);
            var f = 2 * Pc(a.sa),
            l = pc(g.x, g.y, k.$a, k.ab),
            k = new R;
            for (a = Wc(a.sa); l >= d;) {
                var m = (Q["taper-hash"] ? 0.5 * l: a) * a / c;
                k.moveTo(l, m);
                k.lineTo(l, -m);
                l -= f
            }
            a = new P;
            b = zc(b);
            a.translate(g.x, g.y);
            a.rotate(b, 0, 0);
            k.transform(a);
            return k;
        case Pd:
            g = Gd(b);
            Hd(a, g, c, d);
            k = pc(g.ya, g.za, g.$a, g.ab);
            c = 0.2 * Wc(a.sa);
            d = 2 * c;
            f = Math.floor(k / (2 * c));
            l = 0;
            a = new R;
            0 === f && (a.moveTo(2 * c, 0), a.Bf(c, d, 0, 180));
            for (var r = 0; r < f; r++) l += 2 * c,
            m = r % 2 ? -1 : 1,
            a.moveTo(l, 0),
            a.Bf(c, 2 * l / k * d, 0, 180 * m);
            k = new P;
            b = zc(b);
            g = new E(g.ya, g.za);
            k.translate(g.x, g.y);
            k.rotate(b, 0, 0);
            a.transform(k);
            return a
        }
    }
    function Hd(a, b, c, d) {
        a = Q["line-end-padding"] * a.sa.rb;
        if (c) {
            c = c.ae.va();
            var f = a || 0;
            c.x -= f;
            c.y -= f;
            c.width += 2 * f;
            c.height += 2 * f;
            if (c = qc(b, c)) b.ya = c.x,
            b.za = c.y
        }
        d && (d = d.ae.va(), a = a || 0, d.x -= a, d.y -= a, d.width += 2 * a, d.height += 2 * a, d = qc(b, d)) && (b.$a = d.x, b.ab = d.y)
    }
    function Id(a, b, c) {
        c && (c = c.ca, c = new kc(a.ya, a.za, c.x, c.y), sc(c, -tc(a, c) / 2), a = rc(b, c)) && (b.ya = a.x, b.za = a.y)
    }
    function Jd(a, b, c) {
        c && (c = c.ca, c = new kc(a.$a, a.ab, c.x, c.y), sc(c, tc(c, new kc(a.$a, a.ab, a.ya, a.za)) / 2), a = rc(b, c)) && (b.$a = a.x, b.ab = a.y)
    }
    function Gd(a) {
        var b = a.ka.ca;
        a = a.pa.ca;
        return new kc(b.x, b.y, a.x, a.y)
    };
    function Qd() {
        N.call(this)
    }
    w(Qd, N);
    Qd.prototype.clear = function() {
        for (var a = this.aa(); a.hasChildNodes();) a.removeChild(a.lastChild)
    };
    Qd.prototype.xa = function() {
        this.ea = bc("g", {
            "class": "cw-group"
        })
    };
    Qd.prototype.Wb = function(a) {
        this.aa().appendChild(a)
    };
    Qd.prototype.setTransform = function(a) {
        this.aa().setAttribute("transform", a.toString())
    };
    function Rd() {};
    function Sd() {
        this.Oa = [];
        this.Ef = [];
        this.Lh = [];
        this.nf = [];
        this.nf[0] = 128;
        for (var a = 1; 64 > a; ++a) this.nf[a] = 0;
        this.reset()
    }
    w(Sd, Rd);
    Sd.prototype.reset = function() {
        this.Oa[0] = 1732584193;
        this.Oa[1] = 4023233417;
        this.Oa[2] = 2562383102;
        this.Oa[3] = 271733878;
        this.Oa[4] = 3285377520;
        this.ng = this.ke = 0
    };
    function Td(a, b, c) {
        c || (c = 0);
        var d = a.Lh;
        if (t(b)) for (var f = 0; 16 > f; f++) d[f] = b.charCodeAt(c) << 24 | b.charCodeAt(c + 1) << 16 | b.charCodeAt(c + 2) << 8 | b.charCodeAt(c + 3),
        c += 4;
        else for (f = 0; 16 > f; f++) d[f] = b[c] << 24 | b[c + 1] << 16 | b[c + 2] << 8 | b[c + 3],
        c += 4;
        for (f = 16; 80 > f; f++) {
            var g = d[f - 3] ^ d[f - 8] ^ d[f - 14] ^ d[f - 16];
            d[f] = (g << 1 | g >>> 31) & 4294967295
        }
        b = a.Oa[0];
        c = a.Oa[1];
        for (var k = a.Oa[2], l = a.Oa[3], m = a.Oa[4], r, f = 0; 80 > f; f++) 40 > f ? 20 > f ? (g = l ^ c & (k ^ l), r = 1518500249) : (g = c ^ k ^ l, r = 1859775393) : 60 > f ? (g = c & k | l & (c | k), r = 2400959708) : (g = c ^ k ^ l, r = 3395469782),
        g = (b << 5 | b >>> 27) + g + m + r + d[f] & 4294967295,
        m = l,
        l = k,
        k = (c << 30 | c >>> 2) & 4294967295,
        c = b,
        b = g;
        a.Oa[0] = a.Oa[0] + b & 4294967295;
        a.Oa[1] = a.Oa[1] + c & 4294967295;
        a.Oa[2] = a.Oa[2] + k & 4294967295;
        a.Oa[3] = a.Oa[3] + l & 4294967295;
        a.Oa[4] = a.Oa[4] + m & 4294967295
    }
    Sd.prototype.update = function(a, b) {
        da(b) || (b = a.length);
        for (var c = b - 64,
        d = 0,
        f = this.Ef,
        g = this.ke; d < b;) {
            if (0 == g) for (; d <= c;) Td(this, a, d),
            d += 64;
            if (t(a)) for (; d < b;) {
                if (f[g] = a.charCodeAt(d), ++g, ++d, 64 == g) {
                    Td(this, f);
                    g = 0;
                    break
                }
            } else for (; d < b;) if (f[g] = a[d], ++g, ++d, 64 == g) {
                Td(this, f);
                g = 0;
                break
            }
        }
        this.ke = g;
        this.ng += b
    };
    function Ud(a) {
        for (var b = [], c = 0, d = 0; d < a.length; d++) {
            for (var f = a.charCodeAt(d); 255 < f;) b[c++] = f & 255,
            f >>= 8;
            b[c++] = f
        }
        a = new Sd;
        a.update(b);
        b = [];
        d = 8 * a.ng;
        56 > a.ke ? a.update(a.nf, 56 - a.ke) : a.update(a.nf, 64 - (a.ke - 56));
        for (c = 63; 56 <= c; c--) a.Ef[c] = d & 255,
        d /= 256;
        Td(a, a.Ef);
        for (c = d = 0; 5 > c; c++) for (f = 24; 0 <= f; f -= 8) b[d] = a.Oa[c] >> f & 255,
        ++d;
        a = [];
        for (c = 0; c < b.length; c++) a[2 * c] = "0123456789abcdef".charAt(b[c] >> 4 & 15),
        a[2 * c + 1] = "0123456789abcdef".charAt(b[c] & 15);
        return a.join("")
    };
    var Vd = RegExp("^(?:([^:/?#.]+):)?(?://(?:([^/?#]*)@)?([^/#?]*?)(?::([0-9]+))?(?=[/#?]|$))?([^?#]+)?(?:\\?([^#]*))?(?:#(.*))?$");
    function Wd(a) {
        if (Xd) {
            Xd = !1;
            var b = s.location;
            if (b) {
                var c = b.href;
                if (c && (c = (c = Wd(c)[3] || null) && decodeURIComponent(c)) && c != b.hostname) throw Xd = !0,
                Error();
            }
        }
        return a.match(Vd)
    }
    var Xd = C;
    function Yd(a) {
        M.call(this);
        this.pg = a;
        this.Zb = {
            status: void 0,
            body: void 0
        }
    }
    w(Yd, M);
    Yd.prototype.send = function() {
        var a = Wd(this.pg)[3],
        b = Wd(window.location.href)[3];
        a && a !== b && "undefined" !== typeof XDomainRequest ? Zd(this) : $d(this)
    };
    function $d(a) {
        var b = new XMLHttpRequest;
        b.onreadystatechange = function() {
            4 === b.readyState && (this.Zb.status = b.status, this.Zb.body = b.responseText, this.dispatchEvent(ae))
        }.bind(a);
        b.open("GET", a.pg, !0);
        b.send()
    }
    function Zd(a) {
        var b = new XDomainRequest;
        b.timeout = 9999;
        b.open("GET", a.pg);
        b.onerror = function() {
            this.Zb.status = 400;
            delete this.Zb.body;
            this.dispatchEvent(ae)
        }.bind(a);
        b.onprogress = function() {
            this.dispatchEvent(ee)
        }.bind(a);
        b.ontimeout = function() {
            this.Zb.status = 400;
            delete this.Zb.body;
            this.dispatchEvent(ae)
        }.bind(a);
        b.onload = function() {
            this.Zb.status = 200;
            this.Zb.body = b.responseText;
            this.dispatchEvent(ae)
        }.bind(a);
        b.send()
    }
    var ae = "request-ready",
    ee = "request-progress";
    function fe() {
        M.call(this);
        this.ue = {};
        this.If = void 0;
        this.Fd = this.rg = !1
    }
    w(fe, M);
    var ge = void 0;
    function he() {
        if (!ge) {
            var a = new fe,
            b = document.querySelector("script[data-chemwriter-license]");
            b ? (b = b.getAttribute("data-chemwriter-license")) ? (b = new Yd(b), Pb(b, ae, a.Si, !1, a), b.send()) : (a.Fd = !0, a.dispatchEvent(ie)) : (a.Fd = !0, a.dispatchEvent(ie));
            ge = a
        }
        return ge
    }
    fe.prototype.Gc = h("rg");
    fe.prototype.Si = function(a) {
        a = a.target;
        if (200 === a.Zb.status) {
            this.If = (a = a.Zb.body) || "";
            this.Fd = !0;
            a = a.split("\n");
            for (var b = 0; b < a.length; b++) {
                var c = a[b];
                "#" !== c[0] && (c = c.split(":"), 2 === c.length && (this.ue[c[0]] = c[1].trim()))
            }
            if (this.If && (b = this.If.split("\n"), 1 !== b.length && (a = b.pop().split(":")[1], void 0 !== a))) {
                a = a.trim();
                for (var c = b.join("\n"), d = "", f = 4, g = 0; 10 > g; g++) f += 7 * g,
                d += "0123456789abcdef".charAt(f % 16);
                b = d + Ud(b[4] || "");
                b = Ud(c + b);
                if (a === b && (a = this.ue["Deployment Expires"]) && !(Date.parse(a) < Date.parse(new Date))) switch (this.ue["License Type"]) {
                case "website":
                    this.rg = window.location.hostname === this.ue["Licensed Host"] || window.location.hostname === [Ub, this.ue["Licensed Host"]].join(".")
                }
            }
        } else this.Fd = !0;
        this.dispatchEvent(ie)
    };
    var ie = "molfile-loaded";
    function je(a) {
        N.call(this);
        this.qa = a;
        this.Xf = he()
    }
    w(je, Qd);
    je.prototype.ma = function() {
        if (!this.Xf.Gc()) {
            je.da.ma.call(this);
            var a = new R;
            a.moveTo(205.296, 64.33);
            a.lineTo(226.064, 99.258);
            a.lineTo(185.543, 98.541);
            a.lb(183.108, 84.311, 191.051, 69.885, 205.296, 64.33);
            a.close();
            a.moveTo(110.267, 247.395);
            a.lb(106.23, 247.395, 102.256, 246.33, 98.783, 244.324);
            a.lineTo(11.597, 193.991);
            a.lb(4.501, 189.887, 0.09, 182.264, 0.104, 174.095);
            a.lineTo(0.104, 73.4);
            a.lb(0.104, 65.217, 4.501, 57.596, 11.585, 53.509);
            a.lineTo(98.793, 3.176);
            a.lb(114.288, 0.102, 118.248, 1.165, 121.75, 3.176);
            a.lineTo(191.854, 43.664);
            a.lb(166.365, 41.689, 147.172, 74.737, 160.117, 98.22);
            a.lineTo(110.253, 69.34);
            a.lineTo(63.137, 96.529);
            a.lineTo(63.137, 150.97);
            a.lineTo(110.276, 178.174);
            a.lb(142.892, 159.473, 163.069, 147.904, 171.362, 143.189);
            a.lb(177.199, 139.85, 183.217, 138.159, 189.227, 138.159);
            a.lb(203.739, 138.159, 216.54, 148.286, 220.352, 162.774);
            a.lb(223.887, 176.187, 218.394, 188.869, 206.022, 195.863);
            a.lb(188.434, 205.827, 121.751, 244.323, 121.751, 244.323);
            a.lb(118.248, 246.334, 114.288, 247.395, 110.267, 247.395);
            a.close();
            var b = this.qa.Va().width,
            c = this.qa.Va().height,
            d = a.Va(),
            f;
            f = this.qa.Va();
            f = f.width / f.height > d.width / d.height ? f.height / d.height: f.width / d.width;
            var b = 0.5 * b / f,
            c = 0.5 * c / f,
            g = d.x + 0.5 * d.width,
            d = d.y + 0.5 * d.height,
            k = new P;
            k.scale(f, f);
            k.translate(b - g, c - d);
            a.transform(k);
            ke(this.qa, a, void 0, new Hc("#000000", 0.2), this);
            Pb(this.Xf, ie, this.qe, !1, this)
        }
    };
    je.prototype.qe = function() {
        this.Xf.Gc() && this.qa.removeChild(this, !0)
    };
    function le() {
        N.call(this);
        this.Gb = new Qd;
        this.Gj = new je(this);
        this.tb = new P;
        O(this, this.Gb)
    }
    w(le, N);
    function ke(a, b, c, d, f) {
        b = bc("path", {
            d: gd(b)
        });
        c && (b.setAttribute("stroke", c.sd), b.setAttribute("stroke-width", c.Ec()), b.setAttribute("stroke-opacity", c.Kd), c.be && b.setAttribute("stroke-dasharray", c.be.join(",").toString()));
        d && (b.setAttribute("fill", d.sd), b.setAttribute("fill-opacity", d.Kd)); (f || a.Gb).Wb(b);
        return b
    }
    function me(a, b, c, d) {
        b = bc("line", {
            x1: b.ya,
            y1: b.za,
            x2: b.$a,
            y2: b.ab,
            "stroke-width": c.Ec(),
            "stroke-opacity": c.Kd,
            stroke: c.sd
        });
        c.be && b.setAttribute("stroke-dasharray", c.be.join(",").toString()); (d || a.Gb).Wb(b);
        return b
    }
    function ne(a, b, c, d) {
        b = bc("circle", {
            cx: b.x,
            cy: b.y,
            r: b.lj
        });
        c && (b.setAttribute("fill", c.sd), b.setAttribute("fill-opacity", c.Kd)); (d || a.Gb).Wb(b);
        return b
    }
    function oe(a, b) {
        a.setAttribute("fill", b.sd);
        a.setAttribute("fill-opacity", b.Kd)
    }
    q = le.prototype;
    q.setTransform = function(a) {
        var b = new P,
        c = this.Va();
        b.translate(0, c.height);
        b.scale(1, -1);
        Jc(b, a);
        this.Gb.setTransform(b);
        this.tb = b
    };
    q.translate = function(a) {
        this.tb.translate(a.x, a.y);
        this.Gb.setTransform(this.tb)
    };
    q.scale = function(a) {
        this.tb.scale(a, a);
        this.Gb.setTransform(this.tb)
    };
    q.Va = function() {
        var a = this.getParent().aa().getBoundingClientRect();
        return new ad(a.left, a.top, a.width || 100, a.height || 100)
    };
    q.xa = function() {
        this.ea = bc("svg", {
            "class": "chemwriter-graphics"
        })
    };
    q.ma = function() {
        le.da.ma.call(this);
        if (this.me()) this.af();
        else var a = setInterval(function() {
            this.me() && (this.af(), clearInterval(a))
        }.bind(this), 500);
        this.setTransform(new P)
    };
    q.me = function() {
        var a = this.aa();
        return a ? (a = a.getBoundingClientRect(), 0 !== a.width && 0 !== a.height) : !1
    };
    q.af = function() {
        var a = he();
        a.Fd ? a.Gc() || jc(this, new je(this), 0) : Pb(a, ie, this.qe, !1, this)
    };
    q.qe = function(a) {
        a.target.Gc() || jc(this, new je(this), 0)
    };
    function pe(a) {
        N.call(this);
        this.qa = new le;
        this.sa = a;
        this.Ja = {
            od: new Qd,
            ze: new Qd,
            Xe: new Qd,
            Bd: new Qd
        };
        this.ag = {};
        this.Hf = {};
        this.lg = {};
        this.Wf = {};
        this.Tf = {};
        this.Jc = {};
        this.Og = {};
        this.Bc = {};
        O(this.qa.Gb, this.Ja.od);
        O(this.qa.Gb, this.Ja.ze);
        O(this.qa.Gb, this.Ja.Xe);
        O(this.qa.Gb, this.Ja.Bd);
        O(this, this.qa)
    }
    w(pe, N);
    q = pe.prototype;
    q.clear = function() {
        this.Ja.od.clear();
        this.Ja.ze.clear();
        this.Ja.Xe.clear();
        this.Ja.Bd.clear();
        this.qa.setTransform(new P)
    };
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-document-view"
        })
    };
    q.ma = function() {
        pe.da.ma.call(this)
    };
    q.xd = function(a, b) {
        var c = a.Vc(b);
        if (c) {
            var d = Zc[Dd(b.aa())] || Qc,
            c = ke(this.qa, c, void 0, d, this.Ja.od);
            this.ag[v(b)] = c
        }
    };
    q.qc = function(a) {
        var b = a.x + this.aa().getBoundingClientRect().left;
        a = a.y + this.aa().getBoundingClientRect().top;
        b = (b = document.elementFromPoint(b, a)) ? v(b) : void 0;
        return this.Tf[b]
    };
    q.zd = function(a) {
        var b = a.x + this.aa().getBoundingClientRect().left;
        a = a.y + this.aa().getBoundingClientRect().top;
        b = (b = document.elementFromPoint(b, a)) ? v(b) : void 0;
        return this.Og[b]
    };
    q.we = function(a, b) {
        this.xf(a, b);
        this.xd(a, b)
    };
    q.xf = function(a, b) {
        var c = this.ag[v(b)];
        c && (this.Ja.od || this.qa.Gb).aa().removeChild(c);
        a.removeNode(b);
        delete this.ag[v(b)]
    };
    q.wd = function(a, b) {
        var c = a.ub(b),
        d = [];
        if (void 0 === c.length) {
            if (c.close) {
                var f = ke(this.qa, c, this.sa.Zf, Rc, this.Ja.od);
                d.push(f)
            }
        } else for (var g = 0; g < c.length; g++) f = me(this.qa, c[g], this.sa.Zf, this.Ja.od),
        d.push(f);
        this.Hf[v(b)] = d
    };
    q.ve = function(a, b) {
        this.wf(a, b);
        this.wd(a, b)
    };
    q.wf = function(a, b) {
        for (var c = this.Hf[v(b)], d = 0; d < c.length; d++) {
            var f = c[d]; (this.Ja.od || this.qa.Gb).aa().removeChild(f)
        }
        a.kb(b);
        delete this.Hf[v(b)]
    };
    function qe(a, b, c) {
        for (var d = [], f = c.Ne(), g = 0; g < f.length; g++) {
            var k = f[g];
            d.push(new $c(k.x, k.y, Xc(b.sa)))
        }
        b = [];
        k = Uc;
        for (f = 0; f < d.length; f++) g = ne(a.qa, d[f], k, a.Ja.ze),
        b.push(g);
        d = [];
        f = c.Ne();
        for (g = 1; g < f.length; g++) {
            var k = f[g - 1],
            l = f[g],
            k = new kc(k.x, k.y, l.x, l.y);
            d.push(k)
        }
        2 < f.length && (k = f[f.length - 1], f = f[0], k = new kc(k.x, k.y, f.x, f.y), d.push(k));
        k = a.sa.oj;
        for (f = 0; f < d.length; f++) g = me(a.qa, d[f], k, a.Ja.ze),
        b.push(g);
        a.lg[v(c)] = b
    }
    function re(a, b, c) {
        b = b.zf(c);
        b = ke(a.qa, b, a.sa.Ye, Ic, a.Ja.Xe);
        a.Wf[v(c)] = b
    }
    function se(a, b) {
        var c = a.Wf[v(b)];
        c && (a.Ja.Xe.aa().removeChild(c), delete a.Wf[v(b)])
    }
    function te(a, b) {
        for (var c = a.lg[v(b)], d = 0; d < c.length; d++) {
            var f = c[d];
            a.Ja.ze.aa().removeChild(f)
        }
        delete a.lg[v(b)]
    }
    function ue(a, b) {
        b || (b = new dc(0, 10, -10, 0));
        var c;
        c = b;
        var d = a.qa.Va();
        c = d.width / d.height > (c.right - c.left) / (c.top - c.bottom) ? d.height / (c.top - c.bottom) : d.width / (c.right - c.left);
        var d = 0.5 * a.qa.Va().width / c,
        f = 0.5 * a.qa.Va().height / c,
        g = b.left + 0.5 * (b.right - b.left),
        k = b.bottom + 0.5 * (b.top - b.bottom),
        l = new P;
        l.scale(c, c);
        l.translate(d - g, f - k);
        a.qa.setTransform(l)
    }
    function ve(a, b) {
        var c;
        c = a.qa.tb;
        var d = c.ib * c.Bb - c.zb * c.Ab;
        c = new P(c.Bb / d, -c.Ab / d, -c.zb / d, c.ib / d, (c.zb * c.Kb - c.Bb * c.Jb) / d, (c.Ab * c.Jb - c.ib * c.Kb) / d);
        d = [b.x, b.y];
        c.transform(d, 0, d, 0, 1);
        return new E(d[0], d[1])
    }
    q.translate = function(a) {
        this.qa.translate(a)
    };
    q.scale = function(a) {
        this.qa.scale(a)
    };
    q.tf = function(a, b) {
        var c = a.va();
        c.x *= -(b - 1);
        c.y *= -(b - 1);
        this.qa.translate(c);
        this.qa.scale(b)
    };
    q.Oe = function() {
        return this.qa.tb.ib
    };
    function we(a, b, c) {
        this.x = da(a) ? a: 0;
        this.y = da(b) ? b: 0;
        this.z = da(c) ? c: 0
    }
    we.prototype.va = function() {
        return new we(this.x, this.y, this.z)
    };
    we.prototype.toString = function() {
        return "(" + this.x + ", " + this.y + ", " + this.z + ")"
    };
    function xe(a) {
        M.call(this);
        this.Xa = a;
        this.ua = [];
        this.ja = [];
        this.hc = this.Nb = !1;
        this.ca = new we(0, 0, 0)
    }
    w(xe, M);
    q = xe.prototype;
    q.Pf = function() {
        return this.Xa.ra.indexOf(this)
    };
    q.getParent = h("Xa");
    q.Pc = function(a) {
        this.Xa = a;
        this.Ta(a || null)
    };
    q.Ob = function(a) {
        this.hc !== a && (this.hc = a, this.fireEvent(ye, this))
    };
    q.wa = function(a) {
        this.Nb !== a && (this.Nb = a, this.fireEvent(ze, this))
    };
    q.ub = function(a) {
        this.ja.push(a);
        this.ua.push(Ae(a, this));
        this.fireEvent(Be, this)
    };
    q.kb = function(a) {
        za(this.ja, a);
        za(this.ua, Ae(a, this));
        this.fireEvent(Be, this)
    };
    q.move = function(a, b, c) {
        if (a !== this.ca.x || b !== this.ca.y || (c || 0) !== this.ca.z) {
            var d = this.Ia();
            this.ca.x = a;
            this.ca.y = b;
            this.ca.z = void 0 === c ? 0 : c;
            this.fireEvent(Ce, this, d)
        }
    };
    q.ac = function(a) {
        a.td && (a = a.td, this.move(a.x, a.y, a.z))
    };
    q.Ia = function() {
        return {
            td: this.ca.va()
        }
    };
    q.fireEvent = function(a, b, c) {
        a = new G(a, b);
        a.oe = c;
        this.dispatchEvent(a)
    };
    var Be = "node-definition-changed",
    ze = "node-selection-changed",
    ye = "node-hover-changed",
    Ce = "node-moved";
    function De(a, b, c, d) {
        M.call(this);
        this.hc = this.Nb = !1;
        this.ka = a;
        this.pa = b;
        this.Xa = c;
        this.Nd = d || Md;
        this.Ta(c)
    }
    w(De, M);
    q = De.prototype;
    q.Pf = function() {
        return this.Xa.ja.indexOf(this)
    };
    q.nb = h("Nd");
    function Ee(a, b) {
        if (a.Nd !== b) {
            var c = a.Ia();
            a.Nd = b;
            a.fireEvent(Fe, a, c)
        }
    }
    q.getParent = h("Xa");
    q.Pc = aa("Xa");
    q.Ob = function(a) {
        a != this.hc && (this.hc = a, this.fireEvent(Ge, this))
    };
    q.wa = function(a) {
        a !== this.Nb && (this.Nb = a, this.fireEvent(He, this))
    };
    function Ae(a, b) {
        if (a.ka === b) return a.pa;
        if (a.pa === b) return a.ka
    }
    q.Ia = function() {
        return {
            source: this.ka,
            target: this.pa,
            zh: this.Nd
        }
    };
    q.ac = function(a) {
        var b = a.source,
        c = a.target;
        if (b !== this.ka || c !== this.pa) {
            var d = this.Ia();
            this.ka = b;
            this.pa = c;
            this.fireEvent(Fe, this, d)
        }
        void 0 !== a.zh && Ee(this, a.zh)
    };
    q.fireEvent = function(a, b, c) {
        a = new G(a, b);
        a.oe = c;
        this.dispatchEvent(a)
    };
    var Md = "none",
    Nd = "wedge",
    Od = "hash",
    Pd = "either",
    Fd = "cis-or-trans",
    He = "edge-selection-changed",
    Ge = "edge-hover-changed",
    Fe = "edge-definition-changed";
    function Ie() {
        M.call(this);
        this.ja = [];
        this.ra = []
    }
    w(Ie, M);
    function Je(a) {
        for (var b = 0; b < a.ra.length; b++) a.ra[b].wa(!1);
        for (b = 0; b < a.ja.length; b++) a.ja[b].wa(!1)
    }
    function Ke(a) {
        for (var b = [], c = 0; c < a.ra.length; c++) {
            var d = a.ra[c];
            d.Nb && b.push(d)
        }
        return b
    }
    q = Ie.prototype;
    q.Vc = function(a) {
        this.ra.push(a);
        this.Qa(Le, a)
    };
    q.removeNode = function(a) {
        za(this.ra, a);
        this.Qa(Me, a)
    };
    q.containsNode = function(a) {
        return - 1 !== this.ra.indexOf(a)
    };
    q.ub = function(a) {
        this.ja.push(a);
        this.Qa(Ne, a)
    };
    q.kb = function(a) {
        za(this.ja, a);
        this.Qa(Oe, a)
    };
    q.Kg = function(a) {
        return this.ja[a]
    };
    q.ed = function(a) {
        return this.ra[a]
    };
    q.Ua = function(a, b) {
        var c = new De(a, b, this);
        a.ub(c);
        b.ub(c);
        this.ja.push(c);
        return c
    };
    q.Ma = function() {
        return 0 === this.ja.length && 0 === this.ra.length
    };
    q.clear = function() {
        this.ra = [];
        this.ja = [];
        this.Qa(Pe, this)
    };
    q.Qa = function(a, b) {
        this.dispatchEvent(new G(a, b))
    };
    var Le = "node-added",
    Me = "node-removed",
    Ne = "edge-added",
    Oe = "edge-removed",
    Pe = "graph-cleared";
    function Qe(a, b) {
        xe.call(this, b);
        this.ea = a;
        this.Za = this.ea.gc(0, this.ea.$);
        this.$ = this.ea.$ - this.ea.gc(0, this.ea.$)
    }
    w(Qe, xe);
    q = Qe.prototype;
    q.aa = h("ea");
    function Re(a, b) {
        if (a.ea !== b) {
            var c = a.Ia();
            a.ea = b;
            for (var d = 0,
            f = 0; f < a.ja.length; f++) d += a.ja[f].cb;
            a.$ = a.ea.$ - d / 2;
            a.Za = 0;
            a.Za = a.ea.gc(Se(a), a.$);
            a.$ -= a.Za;
            delete a.Sa;
            a.fireEvent(Be, a, c)
        }
    }
    q.he = h("Sa");
    function Te(a, b) {
        if (! (a.Sa && a.Sa.Gd === b || !a.Sa && 0 === b)) {
            var c = a.Ia();
            0 === b ? delete a.Sa: a.Sa = a.ea.he(b);
            a.fireEvent(Be, a, c)
        }
    }
    q.gc = h("Za");
    q.fe = function() {
        return this.ea.fe(this.$ + Se(this) / 2)
    };
    function Se(a) {
        for (var b = 2 * a.Za,
        c = 0; c < a.ja.length; c++) b += a.ja[c].cb;
        return b
    }
    q.sc = function(a) {
        var b = this.Ia();
        this.$ += this.Za - a;
        this.Za = 0;
        this.Za = this.ea.gc(Se(this), this.$);
        this.$ -= this.Za;
        this.fireEvent(Be, this, b)
    };
    function Ue(a) {
        for (var b = 0,
        c = 0; c < a.ja.length; c++) b += a.ja[c].cb / 2 - 1;
        return b
    }
    q.ub = function(a) {
        this.$ += this.Za - a.cb / 2;
        this.Za = 0;
        this.Za = this.ea.gc(Se(this) + a.cb, this.$);
        this.$ -= this.Za;
        Qe.da.ub.call(this, a)
    };
    q.kb = function(a) {
        this.Ia();
        this.$ += this.Za + a.cb / 2;
        this.Za = 0;
        this.Za = this.ea.gc(Se(this) - a.cb, this.$);
        this.$ -= this.Za;
        Qe.da.kb.call(this, a)
    };
    q.ac = function(a) {
        Qe.da.ac.call(this, a);
        a.element && Re(this, a.element);
        void 0 !== a.Zg && Te(this, a.Zg);
        if (void 0 !== a.Ih) {
            var b = a.Ih;
            a = a.tj;
            if (this.Za !== b || this.$ != a) {
                var c = this.Ia();
                this.Za = b;
                this.$ = a;
                this.fireEvent(Be, this, c)
            }
        }
    };
    q.Ia = function() {
        var a = Qe.da.Ia.call(this);
        a.element = this.ea;
        a.Zg = this.Sa ? this.Sa.Gd: 0;
        a.Ih = this.Za;
        a.tj = this.$;
        return a
    };
    function Ve(a, b, c, d, f) {
        De.call(this, a, b, d, f);
        if (0 > c) throw We;
        if (c % 2) throw Xe;
        this.cb = c
    }
    w(Ve, De);
    Ve.prototype.sc = function(a) {
        if (0 !== a) {
            var b = this.Ia();
            Ye(this, -a);
            2 < this.cb && (this.Nd = Md);
            this.fireEvent(Fe, this, b)
        }
    };
    function Ze(a) {
        var b = a.Ia();
        6 === a.cb ? Ye(a, -4) : Ye(a, 2);
        a.Nd = Md;
        a.fireEvent(Fe, a, b)
    }
    Ve.prototype.Ia = function() {
        var a = Ve.da.Ia.call(this);
        a.Ig = this.cb;
        return a
    };
    Ve.prototype.ac = function(a) {
        Ve.da.ac.call(this, a);
        if (a.Ig !== this.cb) {
            var b = this.Ia();
            this.cb = a.Ig;
            this.fireEvent(Fe, this, b)
        }
    };
    function Ye(a, b) {
        a.cb += b;
        a.ka.sc(b / 2);
        a.pa.sc(b / 2)
    }
    var We = "bond-electron-count-negative",
    Xe = "bond-electron-count-odd";
    function $e(a, b, c) {
        this.wj = a;
        this.Gd = b;
        this.kj = !0 === c ? !0 : !1
    };
    function af(a, b, c) {
        this.Rd = a;
        this.bf = b;
        this.Sh = c;
        this.tc = [];
        this.jc = !1;
        this.fa = -1;
        this.$ = 0
    }
    function Dd(a) {
        return a.Rd
    }
    q = af.prototype;
    q.getName = h("bf");
    function T(a, b, c) {
        a.tc.push(new $e(a.Sh, b, c))
    }
    q.he = function(a) {
        for (var b = 0; b < this.tc.length; b++) {
            var c = this.tc[b];
            if (c.Gd === a) return c
        }
        throw bf;
    };
    q.fe = function(a) {
        return this.$ - a
    };
    q.gc = function(a, b) {
        return Math.min(8 - a - b, b)
    };
    q.toString = function() {
        for (var a = this.Rd,
        b = [], c = 0; c < this.tc.length; c++) b.push(this.tc[c].Gd);
        0 !== b.length && (a += "[");
        a += b.join("-");
        0 !== b.length && (a += "]");
        return a
    };
    var bf = "element-isotope-not-found";
    function cf() {
        this.mg = {}
    }
    cf.prototype.addElement = function(a, b, c) {
        b = new af(a, b, c);
        return this.mg[a] = b
    };
    cf.prototype.aa = function(a) {
        a = this.mg[a];
        if (!a) throw df;
        return a
    };
    function ef() {
        if (!ff) {
            ff = new cf;
            var a, b = ff;
            a = b.addElement("H", "Hydrogen", 1);
            T(a, 1, !0);
            T(a, 2);
            T(a, 3);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("He", "Helium", 2);
            T(a, 3);
            T(a, 4, !0);
            a.$ = 2;
            a.fa = 18;
            a = b.addElement("Li", "Lithium", 3);
            T(a, 6);
            T(a, 7, !0);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Be", "Beryllium", 4);
            T(a, 9);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("B", "Boron", 5);
            T(a, 10);
            T(a, 11, !0);
            a.$ = 3;
            a.jc = !0;
            a.fa = 13;
            a = b.addElement("C", "Carbon", 6);
            T(a, 12, !0);
            T(a, 13);
            a.$ = 4;
            a.jc = !0;
            a.fa = 14;
            a = b.addElement("N", "Nitrogen", 7);
            T(a, 14, !0);
            T(a, 15);
            a.$ = 5;
            a.jc = !0;
            a.fa = 15;
            a = b.addElement("O", "Oxygen", 8);
            T(a, 16, !0);
            T(a, 17);
            T(a, 18);
            a.$ = 6;
            a.jc = !0;
            a.fa = 16;
            a = b.addElement("F", "Fluorine", 9);
            T(a, 19, !0);
            a.$ = 7;
            a.jc = !0;
            a.fa = 17;
            a = b.addElement("Ne", "Neon", 10);
            T(a, 20, !0);
            T(a, 21);
            T(a, 22);
            a.$ = 8;
            a.fa = 18;
            a = b.addElement("Na", "Sodium", 11);
            T(a, 23, !0);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Mg", "Magnesium", 12);
            T(a, 24, !0);
            T(a, 25);
            T(a, 26);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("Al", "Aluminum", 13);
            T(a, 27, !0);
            a.$ = 3;
            a.fa = 13;
            a = b.addElement("Si", "Silicon", 14);
            T(a, 28, !0);
            T(a, 29);
            T(a, 30);
            a.$ = 4;
            a.fa = 14;
            a = b.addElement("P", "Phosphorous", 15);
            T(a, 31, !0);
            a.$ = 5;
            a.jc = !0;
            a.fa = 15;
            a = b.addElement("S", "Sulfur", 16);
            T(a, 32, !0);
            T(a, 33);
            T(a, 34);
            T(a, 36);
            a.$ = 6;
            a.jc = !0;
            a.fa = 16;
            a = b.addElement("Cl", "Chlorine", 17);
            T(a, 35, !0);
            T(a, 37);
            a.$ = 7;
            a.jc = !0;
            a.fa = 17;
            a = b.addElement("Ar", "Argon", 18);
            T(a, 36);
            T(a, 38);
            T(a, 40, !0);
            a.$ = 8;
            a.fa = 18;
            a = b.addElement("K", "Potassium", 19);
            T(a, 39, !0);
            T(a, 40);
            T(a, 41);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Ca", "Calcium", 20);
            T(a, 40, !0);
            T(a, 42);
            T(a, 43);
            T(a, 44);
            T(a, 46);
            T(a, 48);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("Sc", "Scandium", 21);
            T(a, 45, !0);
            a.$ = 3;
            a.fa = 3;
            a = b.addElement("Ti", "Titanium", 22);
            T(a, 46);
            T(a, 47);
            T(a, 48, !0);
            T(a, 49);
            T(a, 50);
            a.$ = 4;
            a.fa = 4;
            a = b.addElement("V", "Vanadium", 23);
            T(a, 50);
            T(a, 51, !0);
            a.$ = 5;
            a.fa = 5;
            a = b.addElement("Cr", "Chromium", 24);
            T(a, 50);
            T(a, 52, !0);
            T(a, 53);
            T(a, 54);
            a.$ = 6;
            a.fa = 6;
            a = b.addElement("Mn", "Manganese", 25);
            T(a, 55, !0);
            a.$ = 7;
            a.fa = 7;
            a = b.addElement("Fe", "Iron", 26);
            T(a, 54);
            T(a, 56, !0);
            T(a, 57);
            T(a, 58);
            a.$ = 8;
            a.fa = 8;
            a = b.addElement("Co", "Cobalt", 27);
            T(a, 59, !0);
            a.$ = 9;
            a.fa = 9;
            a = b.addElement("Ni", "Nickel", 28);
            T(a, 58, !0);
            T(a, 60);
            T(a, 61);
            T(a, 62);
            T(a, 64);
            a.$ = 10;
            a.fa = 10;
            a = b.addElement("Cu", "Copper", 29);
            T(a, 63, !0);
            T(a, 65);
            a.$ = 11;
            a.fa = 11;
            a = b.addElement("Zn", "Zinc", 30);
            T(a, 64, !0);
            T(a, 66);
            T(a, 67);
            T(a, 68);
            T(a, 70);
            a.$ = 12;
            a.fa = 12;
            a = b.addElement("Ga", "Gallium", 31);
            T(a, 69, !0);
            T(a, 71);
            a.$ = 3;
            a.fa = 13;
            a = b.addElement("Ge", "Germanium", 32);
            T(a, 70);
            T(a, 72);
            T(a, 73);
            T(a, 74, !0);
            T(a, 76);
            a.$ = 4;
            a.fa = 14;
            a = b.addElement("As", "Arsenic", 33);
            T(a, 75, !0);
            a.$ = 5;
            a.fa = 15;
            a = b.addElement("Se", "Selenium", 34);
            T(a, 74);
            T(a, 76);
            T(a, 77);
            T(a, 78);
            T(a, 80, !0);
            T(a, 82);
            a.$ = 6;
            a.fa = 16;
            a = b.addElement("Br", "Bromine", 35);
            T(a, 79, !0);
            T(a, 81);
            a.$ = 7;
            a.jc = !0;
            a.fa = 17;
            a = b.addElement("Kr", "Krypton", 36);
            T(a, 78);
            T(a, 80);
            T(a, 82);
            T(a, 83);
            T(a, 84, !0);
            T(a, 86);
            a.$ = 8;
            a.fa = 18;
            a = b.addElement("Rb", "Rubidium", 37);
            T(a, 85, !0);
            T(a, 87);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Sr", "Strontium", 38);
            T(a, 84);
            T(a, 86);
            T(a, 87);
            T(a, 88, !0);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("Y", "Yttrium", 39);
            T(a, 89, !0);
            a.$ = 3;
            a.fa = 3;
            a = b.addElement("Zr", "Zirconium", 40);
            T(a, 90, !0);
            T(a, 91);
            T(a, 92);
            T(a, 94);
            T(a, 96);
            a.$ = 4;
            a.fa = 4;
            a = b.addElement("Nb", "Niobium", 41);
            T(a, 93, !0);
            a.$ = 5;
            a.fa = 5;
            a = b.addElement("Mo", "Molybdenum", 42);
            T(a, 92);
            T(a, 94);
            T(a, 95);
            T(a, 96);
            T(a, 97);
            T(a, 98, !0);
            T(a, 100);
            a.$ = 6;
            a.fa = 6;
            a = b.addElement("Tc", "Technetium", 43);
            a.$ = 7;
            a.fa = 7;
            a = b.addElement("Ru", "Ruthenium", 44);
            T(a, 96);
            T(a, 98);
            T(a, 99);
            T(a, 100);
            T(a, 101);
            T(a, 102, !0);
            T(a, 104);
            a.$ = 8;
            a.fa = 8;
            a = b.addElement("Rh", "Rhodium", 45);
            T(a, 103, !0);
            a.$ = 9;
            a.fa = 9;
            a = b.addElement("Pd", "Palladium", 46);
            T(a, 102);
            T(a, 104);
            T(a, 105);
            T(a, 106, !0);
            T(a, 108);
            T(a, 110);
            a.$ = 10;
            a.fa = 10;
            a = b.addElement("Ag", "Silver", 47);
            T(a, 107, !0);
            T(a, 109);
            a.$ = 11;
            a.fa = 11;
            a = b.addElement("Cd", "Cadmium", 48);
            T(a, 106);
            T(a, 108);
            T(a, 110);
            T(a, 111);
            T(a, 112);
            T(a, 113);
            T(a, 114, !0);
            T(a, 116);
            a.$ = 12;
            a.fa = 12;
            a = b.addElement("In", "Indium", 49);
            T(a, 113);
            T(a, 115, !0);
            a.$ = 3;
            a.fa = 13;
            a = b.addElement("Sn", "Tin", 50);
            T(a, 112);
            T(a, 114);
            T(a, 115);
            T(a, 116);
            T(a, 117);
            T(a, 118);
            T(a, 119);
            T(a, 120, !0);
            T(a, 122);
            T(a, 124);
            a.$ = 4;
            a.fa = 14;
            a = b.addElement("Sb", "Antimony", 51);
            T(a, 121, !0);
            T(a, 123);
            a.$ = 5;
            a.fa = 15;
            a = b.addElement("Te", "Tellurium", 52);
            T(a, 120);
            T(a, 122);
            T(a, 123);
            T(a, 124);
            T(a, 125);
            T(a, 126);
            T(a, 128);
            T(a, 130, !0);
            a.$ = 6;
            a.fa = 16;
            a = b.addElement("I", "Iodine", 53);
            T(a, 127, !0);
            T(a, 131);
            a.$ = 7;
            a.jc = !0;
            a.fa = 17;
            a = b.addElement("Xe", "Xenon", 54);
            T(a, 124);
            T(a, 126);
            T(a, 128);
            T(a, 129);
            T(a, 130);
            T(a, 131);
            T(a, 132, !0);
            T(a, 134);
            T(a, 136);
            a.$ = 8;
            a.fa = 18;
            a = b.addElement("Cs", "Cesium", 55);
            T(a, 133, !0);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Ba", "Barium", 56);
            T(a, 130);
            T(a, 132);
            T(a, 134);
            T(a, 135);
            T(a, 136);
            T(a, 137);
            T(a, 138, !0);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("La", "Lanthanum", 57);
            T(a, 138);
            T(a, 139, !0);
            a.$ = 3;
            a = b.addElement("Ce", "Cerium", 58);
            T(a, 136);
            T(a, 138);
            T(a, 140, !0);
            T(a, 142);
            a.$ = 4;
            a = b.addElement("Pr", "Praseodymium", 59);
            T(a, 141, !0);
            a.$ = 5;
            a = b.addElement("Nd", "Neodymium", 60);
            T(a, 142, !0);
            T(a, 143);
            T(a, 144);
            T(a, 145);
            T(a, 146);
            T(a, 148);
            T(a, 150);
            a.$ = 6;
            a = b.addElement("Pm", "Promethium", 61);
            a.$ = 7;
            a = b.addElement("Sm", "Samarium", 62);
            T(a, 144);
            T(a, 147);
            T(a, 148);
            T(a, 149);
            T(a, 150);
            T(a, 152, !0);
            T(a, 154);
            a.$ = 8;
            a = b.addElement("Eu", "Europium", 63);
            T(a, 151);
            T(a, 153, !0);
            a.$ = 9;
            a = b.addElement("Gd", "Gadolinium", 64);
            T(a, 152);
            T(a, 154);
            T(a, 155);
            T(a, 156);
            T(a, 157);
            T(a, 158, !0);
            T(a, 160);
            a.$ = 10;
            a = b.addElement("Tb", "Terbium", 65);
            T(a, 159, !0);
            a.$ = 11;
            a = b.addElement("Dy", "Dysprosium", 66);
            T(a, 156);
            T(a, 158);
            T(a, 160);
            T(a, 161);
            T(a, 162);
            T(a, 163);
            T(a, 164, !0);
            a.$ = 12;
            a = b.addElement("Ho", "Holmium", 67);
            T(a, 165, !0);
            a.$ = 13;
            a = b.addElement("Er", "Erbium", 68);
            T(a, 162);
            T(a, 164);
            T(a, 166, !0);
            T(a, 167);
            T(a, 168);
            T(a, 170);
            a.$ = 14;
            a = b.addElement("Tm", "Thulium", 69);
            T(a, 169, !0);
            a.$ = 15;
            a = b.addElement("Yb", "Ytterbium", 70);
            T(a, 168);
            T(a, 170);
            T(a, 171);
            T(a, 172);
            T(a, 173);
            T(a, 174, !0);
            T(a, 176);
            a.$ = 16;
            a = b.addElement("Lu", "Lutetium", 71);
            T(a, 175, !0);
            T(a, 176);
            a.$ = 17;
            a.fa = 3;
            a = b.addElement("Hf", "Hafnium", 72);
            T(a, 174);
            T(a, 176);
            T(a, 177);
            T(a, 178);
            T(a, 179);
            T(a, 180, !0);
            a.$ = 4;
            a.fa = 4;
            a = b.addElement("Ta", "Tantalum", 73);
            T(a, 180);
            T(a, 181, !0);
            a.$ = 5;
            a.fa = 5;
            a = b.addElement("W", "Tungsten", 74);
            T(a, 180);
            T(a, 182);
            T(a, 183);
            T(a, 184, !0);
            T(a, 186);
            a.$ = 6;
            a.fa = 6;
            a = b.addElement("Re", "Rhenium", 75);
            T(a, 185);
            T(a, 187, !0);
            a.$ = 7;
            a.fa = 7;
            a = b.addElement("Os", "Osmium", 76);
            T(a, 184);
            T(a, 186);
            T(a, 187);
            T(a, 188);
            T(a, 189);
            T(a, 190);
            T(a, 192, !0);
            a.$ = 8;
            a.fa = 8;
            a = b.addElement("Ir", "Iridium", 77);
            T(a, 191);
            T(a, 193, !0);
            a.$ = 9;
            a.fa = 9;
            a = b.addElement("Pt", "Platinum", 78);
            T(a, 190);
            T(a, 192);
            T(a, 194);
            T(a, 195, !0);
            T(a, 196);
            T(a, 198);
            a.$ = 10;
            a.fa = 10;
            a = b.addElement("Au", "Gold", 79);
            T(a, 197, !0);
            a.$ = 11;
            a.fa = 11;
            a = b.addElement("Hg", "Mercury", 80);
            T(a, 196);
            T(a, 198);
            T(a, 199);
            T(a, 200);
            T(a, 201);
            T(a, 202, !0);
            T(a, 204);
            a.$ = 12;
            a.fa = 12;
            a = b.addElement("Tl", "Thalium", 81);
            T(a, 203);
            T(a, 205, !0);
            a.$ = 3;
            a.fa = 13;
            a = b.addElement("Pb", "Lead", 82);
            T(a, 204);
            T(a, 206);
            T(a, 207);
            T(a, 208, !0);
            a.$ = 4;
            a.fa = 14;
            a = b.addElement("Bi", "Bismuth", 83);
            T(a, 209, !0);
            a.$ = 5;
            a.fa = 15;
            a = b.addElement("Po", "Polonium", 84);
            a.$ = 6;
            a.fa = 16;
            a = b.addElement("At", "Astatine", 85);
            a.$ = 7;
            a.fa = 17;
            a = b.addElement("Rn", "Radon", 86);
            a.$ = 8;
            a.fa = 18;
            a = b.addElement("Fr", "Francium", 87);
            a.$ = 1;
            a.fa = 1;
            a = b.addElement("Ra", "Radium", 88);
            a.$ = 2;
            a.fa = 2;
            a = b.addElement("Ac", "Actinium", 89);
            a.$ = 3;
            a = b.addElement("Th", "Thorium", 90);
            T(a, 232, !0);
            a.$ = 4;
            a = b.addElement("Pa", "Protactinium", 91);
            T(a, 231, !0);
            a.$ = 5;
            a = b.addElement("U", "Uranium", 92);
            T(a, 234);
            T(a, 235);
            T(a, 238, !0);
            a.$ = 6;
            a = b.addElement("Np", "Neptunium", 93);
            a.$ = 7;
            a = b.addElement("Pu", "Plutonium", 94);
            a.$ = 8;
            a = b.addElement("Am", "Americium", 95);
            a.$ = 9;
            a = b.addElement("Cm", "Curium", 96);
            a.$ = 10;
            a = b.addElement("Bk", "Berkelium", 97);
            a.$ = 11;
            a = b.addElement("Cf", "Californium", 98);
            a.$ = 12;
            a = b.addElement("Es", "Einsteinium", 99);
            a.$ = 13;
            a = b.addElement("Fm", "Fermium", 100);
            a.$ = 14;
            a = b.addElement("Md", "Mendelevium", 101);
            a.$ = 15;
            a = b.addElement("No", "Nobelium", 102);
            a.$ = 16;
            a = b.addElement("Lr", "Lawrencium", 103);
            a.$ = 17;
            a = b.addElement("Rf", "Rutherfordium", 104);
            a.$ = 4;
            a = b.addElement("Db", "Dubnium", 105);
            a.$ = 5;
            a = b.addElement("Sg", "Seaborgium", 106);
            a.$ = 6;
            a = b.addElement("Bh", "Bhorium", 107);
            a.$ = 7;
            a = b.addElement("Hs", "Hassium", 108);
            a.$ = 8;
            a = b.addElement("Mt", "Meitnerium", 109);
            a.$ = 9;
            a = b.addElement("Ds", "Darmstadtium", 110);
            a.$ = 10;
            a = b.addElement("Rg", "Roentgenium", 111);
            a.$ = 11;
            a = b.addElement("Cn", "Copernicium", 112);
            a.$ = 12
        }
        return ff
    }
    var df = "atomic-system-no-such-element",
    ff = void 0;
    function gf() {
        Ie.call(this)
    }
    w(gf, Ie);
    q = gf.prototype;
    q.Wd = function(a, b, c, d) {
        a = ef().aa(a);
        a = new Qe(a);
        a.move(b || 0, c || 0, d);
        this.Vc(a);
        return a
    };
    q.Vc = function(a) {
        a.Pc(this);
        gf.da.Vc.call(this, a)
    };
    q.removeNode = function(a) {
        if (a.getParent() !== this) throw hf;
        var b = a.ja,
        c = [],
        d;
        for (d = 0; d < b.length; d++) c.push(b[d]);
        for (d = 0; d < c.length; d++) this.kb(c[d]);
        a.Pc(void 0);
        gf.da.removeNode.call(this, a)
    };
    q.He = function(a, b, c, d) {
        a = new Ve(a, b, c, this, d);
        this.ub(a);
        return a
    };
    q.ub = function(a) {
        a.Pc(this);
        a.ka.ub(a);
        a.pa.ub(a);
        gf.da.ub.call(this, a)
    };
    q.kb = function(a) {
        a.Pc(void 0);
        a.ka.kb(a);
        a.pa.kb(a);
        gf.da.kb.call(this, a)
    };
    var hf = "molecule-no-such-atom";
    function jf(a, b) {
        null != a && this.append.apply(this, arguments)
    }
    jf.prototype.qd = "";
    jf.prototype.append = function(a, b, c) {
        this.qd += a;
        if (null != b) for (var d = 1; d < arguments.length; d++) this.qd += arguments[d];
        return this
    };
    jf.prototype.clear = function() {
        this.qd = ""
    };
    jf.prototype.toString = h("qd");
    function kf() {
        this.ta = -1
    }
    function lf(a) {
        if (! (a.Ed && a.ta < a.Ed.length - 1)) throw mf;
        a.ta++;
        return a.Ed[a.ta]
    }
    kf.prototype.Mg = function() {
        return this.Ed ? this.Ed[this.ta] : void 0
    };
    kf.prototype.Sf = function() {
        return this.Ed.join("\n")
    };
    kf.prototype.rf = function(a) {
        this.Ed = -1 !== a.indexOf("\r\n") ? a.split("\r\n") : a.split("\n");
        this.ta = 0
    };
    var mf = "line-reader-no-next-line";
    function nf(a, b) {
        this.Yf = b;
        this.message = a + " on line " + b
    }
    w(nf, Error);
    nf.prototype.Mg = h("Yf");
    function of() {
        this.oa = new gf;
        this.Oc = new kf;
        this.Ug = this.Tg = !1
    }
    var pf;
    q = of.prototype;
    q.rf = function(a) {
        this.Tg = -1 !== a.indexOf(qf);
        this.Ug = -1 !== a.indexOf(rf);
        this.Oc.rf(a);
        try {
            for (a = 0; 3 > a; a++) lf(this.Oc)
        } catch(b) {
            sf(this, tf)
        }
        if (a = this.Oc.Mg()) {
            var c, d;
            try {
                c = K(a, 0, 3),
                d = K(a, 3, 6)
            } catch(f) {
                sf(this, uf)
            }
            var g;
            for (a = 0; a < c; a++) {
                try {
                    g = lf(this.Oc)
                } catch(k) {
                    sf(this, tf)
                }
                try {
                    var l = g;
                    this.Wd(K(l, 0, 10, 0), K(l, 10, 20, 0), K(l, 20, 30, 0), l.substring(31, 34).trim(), this.Ug ? 0 : K(l, 34, 36, 0), this.Tg ? 0 : K(l, 36, 39, 0), K(l, 42, 45, 0), K(l, 45, 48, 0), K(l, 48, 51, 0))
                } catch(m) {
                    sf(this, uf)
                }
            }
            var r;
            for (g = 0; g < d; g++) {
                try {
                    r = lf(this.Oc)
                } catch(x) {
                    sf(this, tf)
                }
                try {
                    c = r,
                    this.He(K(c, 0, 3), K(c, 3, 6), K(c, 6, 9), K(c, 9, 12), K(c, 15, 18))
                } catch(n) {
                    sf(this, uf)
                }
            }
        } else sf(this, tf);
        var H;
        try {
            H = lf(this.Oc)
        } catch(ka) {
            sf(this, tf)
        }
        for (; 0 !== H.indexOf(vf);) {
            r = H;
            d = r.substring(0, 6).trim();
            if ( - 1 !== wf.indexOf(d)) for (g = K(r, 6, 9), c = 0; c < g; c++) switch (l = 8 * c, a = K(r, 10 + l, 13 + l), l = K(r, 14 + l, 17 + l), d) {
            case qf:
                this.oa.ed(a - 1).sc(l);
                break;
            case rf:
                Te(this.oa.ed(a - 1), l)
            }
            try {
                H = lf(this.Oc)
            } catch(be) {
                sf(this, tf)
            }
        }
    };
    q.print = function(a) {
        a = a || new jf;
        a.append(Vb);
        a.append("\n");
        a.append("  ");
        a.append("CWRITER3");
        var b = new Date;
        a.append(("0" + (b.getMonth() + 1)).slice( - 2));
        a.append(("0" + b.getDate()).slice( - 2));
        a.append(b.getFullYear().toString().slice( - 2));
        a.append(("0" + b.getHours()).slice( - 2));
        a.append(("0" + b.getMinutes()).slice( - 2));
        a.append("2D");
        a.append("                              ");
        a.append("\n");
        a.append("Created with ChemWriter - http://chemwriter.com");
        a.append("\n");
        L(this.oa.ra.length, a, 3);
        L(this.oa.ja.length, a, 3);
        L("0", a, 3);
        L("0", a, 3);
        a: {
            for (var b = this.oa.ja,
            c = 0; c < b.length; c++) {
                var d = b[c];
                if (d.nb() === Nd || d.nb() === Od) {
                    b = !0;
                    break a
                }
            }
            b = !1
        }
        L(b ? 1 : 0, a, 3);
        L("0", a, 3);
        L("0", a, 3);
        L("0", a, 3);
        L("0", a, 3);
        L("0", a, 3);
        a.append(999);
        a.append(" V2000");
        a.append("\n");
        d = this.oa.ra;
        c = this.oa.ja;
        for (b = 0; b < d.length; b++) {
            var f = a,
            g = this.oa.ed(b),
            k = g.ca;
            L(k.x, f, 10, 4);
            L(k.y, f, 10, 4);
            L(k.z, f, 10, 4);
            f.append(" ");
            k = f;
            g = Dd(g.aa());
            Xb(g, k, 3, 1, void 0);
            L("0", k, 2);
            for (g = 0; 11 > g; g++) L("0", k, 3);
            f.append("\n")
        }
        for (b = 0; b < c.length; b++) {
            d = a;
            k = this.oa.Kg(b);
            L(k.ka.Pf() + 1, d, 3);
            L(k.pa.Pf() + 1, d, 3);
            f = d;
            L(k.cb / 2, f, 3);
            switch (k.nb()) {
            case Nd:
                L(xf, f, 3);
                break;
            case Od:
                L(yf, f, 3);
                break;
            case Pd:
                L(zf, f, 3);
                break;
            case Fd:
                L(Af, f, 3)
            }
            for (k = 0; 3 > k; k++) f.append("  0");
            d.append("\n")
        }
        c = this.oa.ra;
        b = [];
        for (d = 0; d < c.length; d++) f = c[d].fe(),
        0 !== f && b.push([d + 1, f]);
        if (0 !== b.length) {
            a.append(qf);
            L(b.length, a, 3);
            for (c = 0; c < b.length; c++) a.append(" "),
            d = b[c],
            L(d[0], a, 3),
            a.append(" "),
            L(d[1], a, 3);
            a.append("\n")
        }
        a.append(vf);
        return a.toString()
    };
    q.Wd = function(a, b, c, d, f, g) {
        d = this.oa.Wd(d);
        d.move(a, b, c);
        d.sc(g);
        if (f) {
            a: {
                a = d.aa();
                if (0 !== a.tc.length) for (b = 0; b < a.tc.length; b++) if (c = a.tc[b], c.kj) {
                    a = c;
                    break a
                }
                a = void 0
            }
            Te(d, a.Gd + f)
        }
        return d
    };
    q.He = function(a, b, c, d) {
        if (1 !== c && 2 !== c && 3 !== c) throw Error(Bf);
        a = this.oa.He(this.oa.ed(a - 1), this.oa.ed(b - 1), 2 * c);
        switch (d) {
        case xf:
            Ee(a, Nd);
            break;
        case yf:
            Ee(a, Od);
            break;
        case zf:
            Ee(a, Pd);
            break;
        case Af:
            Ee(a, Fd)
        }
        return a
    };
    q.na = h("oa");
    function sf(a, b) {
        throw new nf(b, a.Oc.ta + 1);
    }
    var xf = 1,
    Af = 3,
    zf = 4,
    yf = 6,
    qf = "M  CHG",
    rf = "M  ISO",
    vf = "M  END",
    wf = ["M  CHG", "M  ISO", "M  END"],
    tf = "[Molfile] unexpected end of file",
    Bf = "[Molfile] invalid bond type",
    uf = "[Molfile] syntax error";
    function Cf() {
        M.call(this);
        this.oa = new gf;
        this.yj = void 0;
        this.bi = 1;
        this.fd = this.$c = 0;
        this.Ka = this.La = void 0;
        this.oa.Ta(this)
    }
    w(Cf, M);
    var Df = new of;
    q = Cf.prototype;
    q.$b = function(a) {
        this.La !== a && (this.La && this.La.Ob(!1), this.Ka && (this.Ka.Ob(!1), this.Ka = void 0), a && a.Ob(!0), this.La = a)
    };
    q.ge = h("La");
    q.zc = function(a) {
        this.Ka !== a && (this.Ka && this.Ka.Ob(!1), this.La && (this.La.Ob(!1), this.La = void 0), a && a.Ob(!0), this.Ka = a)
    };
    q.Of = h("Ka");
    q.clear = function() {
        for (var a = Aa(this.oa.ra), b = 0; b < a.length; b++) a[b].wa(!1),
        a[b].Ob(!1),
        this.oa.removeNode(a[b])
    };
    q.Ma = function() {
        return 0 === this.oa.ra.length
    };
    function Ef(a) {
        a.$c += 1;
        1 === a.$c && a.Qa(Ff, a)
    }
    function Gf(a) {
        if (0 === a.$c) throw Error(Hf);
        a.$c -= 1;
        0 === a.$c && (a.Qa(If, a), a.Qa(Jf, a))
    }
    q.removeNode = function(a) {
        var b = a.ua.slice(0);
        a.wa(!1);
        a.Ob(!1);
        this.na().removeNode(a);
        for (a = 0; a < b.length; a++) {
            var c = b[a];
            void 0 !== c.getParent() && 0 === c.ua.length && (c.wa(!1), c.Ob(!1), this.na().removeNode(c))
        }
    };
    q.kb = function(a) {
        var b = a.ka,
        c = a.pa;
        a.wa(!1);
        a.Ob(!1);
        this.na().kb(a);
        void 0 !== b.getParent() && 0 === b.ua.length && (b.wa(!1), b.Ob(!1), this.na().removeNode(b));
        void 0 !== c.getParent() && 0 === c.ua.length && (c.wa(!1), c.Ob(!1), this.na().removeNode(c))
    };
    function Kf(a, b, c, d, f, g) {
        var k = a.qc(b, f);
        k || (k = a.oa.Wd("C", b.x, b.y)); (b = a.qc(c, f)) || (b = a.oa.Wd("C", c.x, c.y));
        a: {
            c = a.oa;
            for (f = 0; f < c.ja.length; f++) {
                var l = c.ja[f];
                if (l.ka === k && l.pa === b || l.ka === b && l.pa === k) {
                    c = l;
                    break a
                }
            }
            c = void 0
        }
        c ? d && c.sc( - 2) : c = a.oa.He(k, b, 2, g);
        return c
    }
    q.zf = function(a) {
        a.Ta(this);
        this.Qa(Lf, a)
    };
    q.na = h("oa");
    q.qc = function(a, b) {
        for (var c = b || 0,
        d = this.oa.ra,
        f = 0; f < d.length; f++) {
            var g = d[f];
            if (oc(a, g.ca) < c) return g
        }
    };
    function Mf(a, b) {
        a.oa.Ta(null);
        a.oa.clear();
        Df.oa = a.oa;
        Df.rf(b);
        a.oa.Ta(a)
    }
    function Nf(a) {
        Df.oa = a.oa;
        return Df.print()
    }
    q.Qa = function(a, b) {
        this.dispatchEvent(new G(a, b))
    };
    var Lf = "document-lasso-added",
    Ff = "document-composite-edit-entered",
    If = "document-composite-edit-exited",
    Jf = "document-edited",
    Hf = "document-not-in-composite-edit";
    function Of() {
        N.call(this);
        this.Fj = !1
    }
    w(Of, N);
    Of.prototype.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-error-image"
        })
    };
    Of.prototype.ma = function() {
        Of.da.ma.call(this);
        var a = this.getParent().aa(),
        b = a.offsetWidth,
        c = a.offsetHeight,
        a = b > c ? c: b,
        c = (c - a) / 2,
        b = (b - a) / 2;
        this.aa().style.width = a + "px";
        this.aa().style.height = a + "px";
        this.aa().style.top = c + "px";
        this.aa().style.left = b + "px";
        this.aa().style["font-size"] = this.aa().offsetHeight + "px"
    };
    function Pf() {
        N.call(this);
        this.sa = new Oc;
        this.yb = new Cd(this.sa);
        this.la = new pe(this.sa);
        this.ba = new Cf;
        O(this, this.la);
        this.yd = void 0
    }
    w(Pf, N);
    q = Pf.prototype;
    q.Pb = function(a) {
        try {
            Mf(this.ba, a)
        } catch(b) {
            throw this.yd || (this.yd = new Of, O(this, this.yd), this.Xc()),
            b;
        }
        this.yd && (this.removeChild(this.yd, !0), delete this.yd);
        a = nc(this.ba.na());
        this.sa.Qd(a);
        this.Xc();
        this.Me()
    };
    q.Dc = function() {
        return Nf(this.ba)
    };
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-image"
        })
    };
    q.Xc = function() {
        this.la.clear();
        this.yb.clear()
    };
    q.ma = function() {
        Pf.da.ma.call(this)
    };
    q.Me = function() {
        for (var a = this.ba.na(), b = a.ra, c = 0; c < b.length; c++) this.la.xd(this.yb, b[c]);
        a = a.ja;
        for (c = 0; c < a.length; c++) this.la.wd(this.yb, a[c]);
        ue(this.la, Ld(this.yb))
    };
    function Qf() {
        this.Pa = [];
        this.ta = 0
    }
    Qf.prototype.clear = function() {
        ya(this.Pa);
        this.ta = 0
    };
    function Rf(a) {
        return void 0 !== a.Pa[a.ta - 1]
    }
    Qf.prototype.Xd = function(a) {
        0 !== this.Pa.length && this.ta !== this.Pa.length && this.Pa.splice(this.ta, this.Pa.length - this.ta);
        this.Pa.push(a);
        this.ta += 1
    };
    function Sf() {};
    function Tf() {
        this.Pa = []
    }
    w(Tf, Sf);
    Tf.prototype.clear = function() {
        ya(this.Pa)
    };
    Tf.prototype.Xd = function(a) {
        this.Pa.push(a)
    };
    Tf.prototype.execute = function() {
        for (var a = 0; a < this.Pa.length; a++) this.Pa[a].execute()
    };
    Tf.prototype.og = function() {
        for (var a = this.Pa.length - 1; 0 <= a; a--) this.Pa[a].og()
    };
    function Uf(a) {
        if (a) {
            var b = a.Tb,
            c = {},
            d;
            for (d in b) c[d] = b[d];
            this.Tb = c;
            this.Fa = a.Fa
        } else this.Tb = {},
        this.Fa = 0
    }
    q = Uf.prototype;
    q.get = function(a) {
        a = "string" === typeof a ? a: v(a);
        return (a = this.Tb[a]) ? a.value: void 0
    };
    q.put = function(a, b) {
        var c = "string" === typeof a ? a: v(a);
        c in this.Tb || (this.Fa += 1);
        this.Tb[c] = {
            key: a,
            value: b
        }
    };
    function Vf(a, b) {
        var c = "string" === typeof b ? b: v(b);
        a.Tb[c] && (delete a.Tb[c], a.Fa -= 1)
    }
    q.clear = function() {
        this.Tb = {};
        this.Fa = 0
    };
    q.Ma = function() {
        return 0 === this.Fa
    };
    function Wf(a) {
        var b = [],
        c;
        for (c in a.Tb) b.push(a.Tb[c].key);
        return b
    }
    q.va = function() {
        var a = new Uf,
        b;
        for (b in this.Tb) {
            var c = this.Tb[b];
            a.put(c.key, c.value)
        }
        return a
    };
    function Xf() {
        M.call(this);
        this.Vg = this.Uf = !1;
        this.hb = new Qf;
        this.Zc = new Tf;
        this.ie = new Uf
    }
    w(Xf, M);
    q = Xf.prototype;
    q.ia = h("ba");
    q.Be = function(a) {
        this.ba = a;
        this.hb.clear();
        this.vb();
        this.dispatchEvent(Yf);
        this.dispatchEvent($f)
    };
    q.Ud = function() {
        if (Rf(this.hb)) {
            this.sf();
            Ef(this.ba);
            var a = this.hb,
            a = a.Pa[a.ta - 1];
            if (!a) throw Error("No previous command");
            a.og();
            Gf(this.ba);
            this.vb();
            a = this.hb;
            if (0 < a.ta) a.ta -= 1;
            else throw Error("Can not step back");
        } else throw Error(ag);
        Rf(this.hb) || this.dispatchEvent(Yf);
        this.dispatchEvent(bg)
    };
    q.Ld = function() {
        if (void 0 !== this.hb.Pa[this.hb.ta]) {
            var a;
            a = this.hb;
            a = a.Pa[a.ta];
            if (!a) throw Error("No next command");
            this.sf();
            Ef(this.ba);
            a.execute();
            Gf(this.ba);
            this.vb();
            a = this.hb;
            if (a.ta < a.Pa.length) a.ta += 1;
            else throw Error("Can not step forward");
        } else throw Error(cg);
        void 0 !== this.hb.Pa[this.hb.ta] || this.dispatchEvent($f);
        this.dispatchEvent(dg)
    };
    q.vb = function() {
        I(this.ba, Le, this.jf, !1, this);
        I(this.ba, Me, this.mf, !1, this);
        I(this.ba, Ne, this.ef, !1, this);
        I(this.ba, Oe, this.gf, !1, this);
        I(this.ba, Be, this.kf, !1, this);
        I(this.ba, Fe, this.ff, !1, this);
        I(this.ba, Ce, this.lf, !1, this);
        I(this.ba, Ff, this.df, !1, this);
        I(this.ba, If, this.Hd, !1, this);
        I(this.ba, "document-group-move-entered", this.ti, !1, this);
        I(this.ba, "document-group-move-exited", this.ui, !1, this)
    };
    q.sf = function() {
        J(this.ba, Le, this.jf, !1, this);
        J(this.ba, Me, this.mf, !1, this);
        J(this.ba, Ne, this.ef, !1, this);
        J(this.ba, Oe, this.gf, !1, this);
        J(this.ba, Be, this.kf, !1, this);
        J(this.ba, Fe, this.ff, !1, this);
        J(this.ba, Ce, this.lf, !1, this);
        J(this.ba, Ff, this.df, !1, this);
        J(this.ba, If, this.Hd, !1, this)
    };
    q.df = function() {
        this.Uf = !0
    };
    q.Hd = function() {
        this.Uf = !1;
        0 < this.Zc.Pa.length && (this.hb.Xd(this.Zc), this.Zc = new Tf, this.dispatchEvent(dg), this.dispatchEvent($f))
    };
    q.ti = function() {
        this.Vg = !0
    };
    q.ui = function() {
        if (!this.ie.Ma()) {
            for (var a = this.ie.va(), b = new Uf, c = Wf(a), d = 0; d < c.length; d++) {
                var f = c[d];
                b.put(f, f.Ia())
            }
            this.ie.clear();
            eg(this,
            function() {
                for (var a = Wf(b), c = 0; c < a.length; c++) a[c].ac(b.get(a[c]))
            },
            function() {
                for (var b = Wf(a), c = 0; c < b.length; c++) b[c].ac(a.get(b[c]))
            });
            this.dispatchEvent(dg)
        }
    };
    q.jf = function(a) {
        var b = a.target;
        eg(this,
        function() {
            this.ba.na().Vc(b)
        }.bind(this),
        function() {
            this.ba.na().removeNode(b)
        }.bind(this))
    };
    q.mf = function(a) {
        var b = a.target;
        eg(this,
        function() {
            this.ba.na().removeNode(b)
        }.bind(this),
        function() {
            this.ba.na().Vc(b)
        }.bind(this))
    };
    q.kf = function(a) {
        var b = a.target,
        c = a.oe,
        d = b.Ia();
        eg(this,
        function() {
            c && b.ac(d)
        },
        function() {
            c && b.ac(c)
        })
    };
    q.lf = function(a) {
        if (this.Vg) {
            var b = a.target;
            this.ie.get(b) || this.ie.put(b, a.oe)
        } else {
            var b = a.target,
            c = a.oe,
            d = b.Ia();
            fg(this,
            function() {
                b.move(d.td.x, d.td.y)
            },
            function() {
                b.move(c.td.x, c.td.y)
            })
        }
    };
    q.ff = function(a) {
        var b = a.target,
        c = a.oe,
        d = b.Ia();
        eg(this,
        function() {
            b.ac(d)
        },
        function() {
            b.ac(c)
        })
    };
    q.ef = function(a) {
        var b = a.target;
        eg(this,
        function() {
            this.ba.na().ub(b)
        }.bind(this),
        function() {
            this.ba.na().kb(b)
        }.bind(this))
    };
    q.gf = function(a) {
        var b = a.target;
        eg(this,
        function() {
            this.ba.na().kb(b)
        }.bind(this),
        function() {
            this.ba.na().ub(b)
        }.bind(this))
    };
    function eg(a, b, c) {
        a.Uf ? (b = gg(b, c), a.Zc.Xd(b)) : hg(a, b, c)
    }
    function fg(a, b, c) {
        hg(a, b, c)
    }
    function hg(a, b, c) {
        b = gg(b, c);
        a.Zc.Xd(b);
        a.hb.Xd(a.Zc);
        a.Zc = new Tf
    }
    function gg(a, b) {
        var c = new Sf;
        c.execute = a;
        c.og = b;
        return c
    }
    var ag = "Can not undo",
    cg = "Can not redo",
    dg = "undo-manager-undo-available",
    Yf = "undo-manager-undo-unavailable",
    bg = "undo-manager-redo-available",
    $f = "undo-manager-redo-unavailable";
    var ig = !!s.DOMTokenList,
    jg = ig ?
    function(a) {
        return a.classList
    }: function(a) {
        a = a.className;
        return t(a) && a.match(/\S+/g) || []
    },
    kg = ig ?
    function(a, b) {
        return a.classList.contains(b)
    }: function(a, b) {
        var c = jg(a);
        return 0 <= va(c, b)
    },
    lg = ig ?
    function(a, b) {
        a.classList.add(b)
    }: function(a, b) {
        kg(a, b) || (a.className += 0 < a.className.length ? " " + b: b)
    },
    mg = ig ?
    function(a, b) {
        a.classList.remove(b)
    }: function(a, b) {
        kg(a, b) && (a.className = xa(jg(a),
        function(a) {
            return a != b
        }).join(" "))
    };
    function ng() {
        N.call(this)
    }
    w(ng, N);
    function og() {
        M.call(this);
        this.Ba = new R
    }
    w(og, M);
    q = og.prototype;
    q.gb = h("Ba");
    q.clear = function() {
        this.Ba.clear()
    };
    q.Ma = function() {
        return this.Ba.Ma()
    };
    q.Ne = function() {
        var a = [];
        dd(this.Ba,
        function(b, c) {
            a.push(new E(c[0], c[1]))
        }.bind(this));
        return a
    };
    q.containsNode = function(a) {
        return 0 !== (pg(this, a.ca) & 1)
    };
    function qg(a, b) {
        a.Ba.Ma() ? a.Ba.moveTo(b.x, b.y) : a.Ba.lineTo(b.x, b.y);
        a.Qa(rg, a)
    }
    function pg(a, b) {
        var c = b.x,
        d = b.y,
        f = !1,
        g = 0,
        k = 0,
        l = 0,
        m = 0,
        r = 0;
        dd(a.Ba, ma(function(a, b) {
            if (!f) {
                switch (a) {
                case 0:
                    if (m !== k || r !== l) g += sg(m, r, k, l, c, d);
                    k = m = b[0];
                    l = r = b[1];
                    break;
                case 1:
                    for (var H = 0; H < b.length; H += 2) g += sg(m, r, b[H], b[H + 1], c, d),
                    m = b[H],
                    r = b[H + 1]
                }
                if (r !== l || m !== k) g += sg(m, r, k, l, c, d),
                m = k,
                r = l;
                c === m && d === r && (g = 0, r = l, f = !0)
            }
        },
        a));
        r !== l && (g += sg(m, r, k, l, c, d));
        return g
    }
    function sg(a, b, c, d, f, g) {
        return f < a && f < c || (f > a && f > c || g > b && g > d || a === c) || !(g < b && g < d) && (d - b) * (f - a) / (c - a) <= g - b ? 0 : f === a ? a < c ? 0 : -1 : f === c ? a < c ? 1 : 0 : a < c ? 1 : -1
    }
    q.Qa = function(a, b) {
        this.dispatchEvent(new G(a, b))
    };
    var rg = "lasso-coordinate-added";
    function tg() {
        this.ra = new Uf;
        this.ja = new Uf
    }
    function ug(a, b, c) {
        for (var d = Wf(a.ra), f = 0; f < d.length; f++) {
            var g = d[f];
            switch (a.ra.get(g)) {
            case vg:
                var k = b,
                l = c,
                m = g;
                k.xd(l, m);
                g = k;
                k = m;
                l = ne(g.qa, new $c(k.ca.x, k.ca.y, Xc(l.sa)), Ic, g.Ja.Bd);
                g.Tf[v(l)] = k;
                g.Jc[v(k)] = l;
                break;
            case wg:
                l = b;
                l.we(c, g);
                l = l.Jc[v(g)];
                k = g.ca.y;
                l.setAttribute("cx", g.ca.x);
                l.setAttribute("cy", k);
                break;
            case xg:
                l = b,
                l.xf(c, g),
                m = k = l.Jc[v(g)],
                l.Ja.Bd.aa().removeChild(m),
                delete l.Tf[v(k)],
                delete l.Jc[v(g)]
            }
        }
        d = Wf(a.ja);
        for (f = 0; f < d.length; f++) switch (l = d[f], a.ja.get(l)) {
        case vg:
            g = b;
            k = c;
            g.wd(k, l);
            k = ne(g.qa, Kd(k, l), Ic, g.Ja.Bd);
            g.Og[v(k)] = l;
            g.Bc[v(l)] = k;
            break;
        case wg:
            k = b;
            g = c;
            k.ve(g, l);
            k = k.Bc[v(l)];
            l = Kd(g, l);
            g = k;
            k = l.y;
            g.setAttribute("cx", l.x);
            g.setAttribute("cy", k);
            break;
        case xg:
            g = b,
            g.wf(c, l),
            k = g.Bc[v(l)],
            g.Ja.Bd.aa().removeChild(k),
            delete g.Bc[v(l)]
        }
        a.clear()
    }
    q = tg.prototype;
    q.xd = function(a) {
        this.ra.put(a, vg)
    };
    q.we = function(a) {
        this.ra.get(a) || this.ra.put(a, wg)
    };
    q.xf = function(a) {
        this.ra.get(a) === vg ? Vf(this.ra, a) : this.ra.put(a, xg)
    };
    q.wd = function(a) {
        this.ja.put(a, vg)
    };
    q.ve = function(a) {
        this.ja.get(a) || this.ja.put(a, wg)
    };
    q.wf = function(a) {
        this.ja.get(a) === vg ? Vf(this.ja, a) : this.ja.put(a, xg)
    };
    q.clear = function() {
        this.ra.clear();
        this.ja.clear()
    };
    var vg = "redraw-buffer-command-draw",
    xg = "redraw-buffer-command-undraw",
    wg = "redraw-buffer-command-redraw";
    function U() {
        M.call(this);
        this.ta = yg.yf;
        this.Cb = 1
    }
    w(U, M);
    q = U.prototype;
    q.getName = p("unknown");
    q.Wa = p("Unknown");
    q.Qc = aa("Cb");
    q.Lf = h("ta");
    q.Pd = function(a) {
        this.ta !== a && (this.ta = a, this.dispatchEvent(zg))
    };
    q.Ic = e();
    q.ic = e();
    q.cd = e();
    q.nc = e();
    q.Ua = aa("ba");
    q.disconnect = function() {
        delete this.ba
    };
    q.Fc = p(!1);
    q.Lb = e();
    q.pb = e();
    q.pe = e();
    q.jd = e();
    q.ec = e();
    q.Fb = e();
    q.Gf = e();
    q.bd = e();
    q.kc = e();
    q.lc = e();
    q.Cf = e();
    q.Df = e();
    q.Eb = e();
    q.reset = e();
    q.Qd = e();
    function Ag(a) {
        Ef(a.ba)
    }
    function Bg(a) {
        Gf(a.ba)
    }
    q.ia = h("ba");
    q.na = function() {
        return this.ba ? this.ba.na() : void 0
    };
    var yg = {
        yf: "chemwriter-default-cursor",
        tg: "chemwriter-move-cursor"
    },
    zg = "tool-cursor-changed";
    function Cg() {
        N.call(this);
        this.sa = new Oc;
        this.yb = new Cd(this.sa);
        this.la = new pe(this.sa);
        this.ba = new Cf;
        this.sb = new V;
        this.Tc = new Xf;
        this.jb = new tg;
        this.Tc.Be(this.ba);
        this.sb.Ua(this.ba);
        this.Tc.Ta(this);
        this.ba.Ta(this);
        O(this, this.la)
    }
    w(Cg, N);
    q = Cg.prototype;
    q.ma = function() {
        Cg.da.ma.call(this);
        this.af();
        this.vb()
    };
    q.Be = function(a) {
        this.ba.Ta(null);
        this.sb.disconnect();
        this.jb.clear();
        this.ba.Le();
        delete this.ba;
        this.Xc();
        this.ba = a;
        Dg(this);
        this.sb.Ua(a);
        this.Tc.Be(a);
        var b = nc(this.ba.na());
        this.sa.Qd(b);
        this.sb.Qc(b);
        this.Me();
        a.Ta(this);
        this.dispatchEvent(new G(Jf, a))
    };
    q.Pb = function(a) {
        Mf(this.ba, a);
        a = nc(this.ba.na());
        this.sa.Qd(a);
        this.sb.Qc(a);
        this.Xc();
        this.Me()
    };
    q.Dc = function() {
        return Nf(this.ba)
    };
    q.Xc = function() {
        this.la.clear();
        this.yb.clear()
    };
    q.Aa = function(a) {
        this.sb.Aa(a)
    };
    function Eg(a, b) {
        Je(a.ba.na());
        Fg(a.sb, b)
    }
    q.Qe = function(a) {
        return this.sb.Qe(a)
    };
    q.Jf = function() {
        return this.sb.Jf()
    };
    q.Ud = function() {
        Rf(this.Tc.hb) && this.Tc.Ud()
    };
    q.Ld = function() {
        void 0 !== this.Tc.hb.Pa[this.Tc.hb.ta] && this.Tc.Ld()
    };
    q.Od = function() {
        ue(this.la, Ld(this.yb))
    };
    q.Ea = h("sb");
    q.ia = h("ba");
    q.Pe = h("sa");
    q.translate = function(a) {
        a = a.va();
        var b = this.la.Oe();
        a.x /= b;
        a.y /= -b;
        this.la.translate(a)
    };
    q.tf = function(a, b) {
        var c = ve(this.la, a);
        this.la.tf(c, b)
    };
    q.$b = function(a) {
        this.ba.$b(a)
    };
    q.ge = function() {
        return this.ba.ge()
    };
    q.zc = function(a) {
        this.ba.zc(a)
    };
    q.Of = function() {
        return this.ba.Of()
    };
    q.qc = function(a) {
        return this.la.qc(a)
    };
    q.zd = function(a) {
        return this.la.zd(a)
    };
    q.clear = function() {
        Ef(this.ba);
        this.ba.clear();
        Gf(this.ba)
    };
    q.Ma = function() {
        return this.ba.Ma()
    };
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-canvas " + yg.yf
        })
    };
    q.Me = function() {
        for (var a = this.ba.na(), b = a.ra, c = 0; c < b.length; c++) this.jb.xd(b[c]);
        a = a.ja;
        for (c = 0; c < a.length; c++) this.jb.wd(a[c]);
        if (this.me()) ug(this.jb, this.la, this.yb),
        this.Od();
        else var d = setInterval(function() {
            this.me() && (ug(this.jb, this.la, this.yb), this.Od(), clearInterval(d))
        }.bind(this), 500)
    };
    function Dg(a) {
        I(a.ba, Le, a.jf, !1, a);
        I(a.ba, Me, a.mf, !1, a);
        I(a.ba, Ne, a.ef, !1, a);
        I(a.ba, Oe, a.gf, !1, a);
        I(a.ba, Lf, a.Ei, !1, a);
        I(a.ba, "document-lasso-removed", a.Gi, !1, a);
        I(a.ba, ze, a.Qi, !1, a);
        I(a.ba, ye, a.Pi, !1, a);
        I(a.ba, Ce, a.lf, !1, a);
        I(a.ba, Be, a.kf, !1, a);
        I(a.ba, He, a.yi, !1, a);
        I(a.ba, Ge, a.xi, !1, a);
        I(a.ba, Fe, a.ff, !1, a);
        I(a.ba, "document-rotator-added", a.Ti, !1, a);
        I(a.ba, "document-rotator-removed", a.Vi, !1, a);
        I(a.ba, Gg, a.Ui, !1, a);
        I(a.ba, rg, a.Fi, !1, a);
        I(a.ba, Ff, a.df, !1, a);
        I(a.ba, If, a.Hd, !1, a)
    }
    q.vb = function() {
        Dg(this);
        I(this.sb, zg, this.Zi, !1, this)
    };
    q.af = function() {
        var a = he();
        a.Fd ? a.Gc() || jc(this, new ng, 0) : Pb(a, ie, this.qe, !1, this)
    };
    q.me = function() {
        var a = this.aa();
        return a ? (a = a.getBoundingClientRect(), 0 !== a.width && 0 !== a.height) : !1
    };
    q.qe = function(a) {
        a.target.Gc() || jc(this, new ng, 0)
    };
    q.df = e();
    q.Zi = function(a) {
        a = a.target;
        for (var b in yg) {
            var c = yg[b];
            mg(this.aa(), c)
        }
        lg(this.aa(), a.Lf())
    };
    q.Hd = function() {
        ug(this.jb, this.la, this.yb)
    };
    q.jf = function(a) {
        this.jb.xd(a.target)
    };
    q.lf = function(a) {
        this.jb.we(a.target);
        for (var b = a.target.ja,
        c = 0; c < b.length; c++) {
            var d = b[c],
            f = Ae(d, a.target);
            this.jb.ve(d);
            this.jb.we(f)
        }
    };
    q.mf = function(a) {
        a.target === this.ba.ge() && this.ba.$b(void 0);
        this.jb.xf(a.target)
    };
    q.kf = function(a) {
        a = a.target;
        this.jb.we(a);
        a = a.ja;
        for (var b = 0; b < a.length; b++) this.jb.ve(a[b])
    };
    q.Qi = function(a) {
        var b = a.target;
        b.Nb || b.hc ? (a = this.sb.Fc(), b = this.la.Jc[v(b)], oe(b, a ? Tc: Sc)) : (a = this.la.Jc[v(b)], oe(a, Ic))
    };
    q.Pi = function(a) {
        var b = a.target;
        b.hc || b.Nb ? (a = this.sb.Fc(), b = this.la.Jc[v(b)], oe(b, a ? Tc: Sc)) : (a = this.la.Jc[v(b)], oe(a, Ic))
    };
    q.yi = function(a) {
        var b = a.target;
        b.Nb || b.hc ? (a = this.sb.Fc(), b = this.la.Bc[v(b)], oe(b, a ? Tc: Sc)) : (a = this.la.Bc[v(b)], oe(a, Ic))
    };
    q.xi = function(a) {
        var b = a.target;
        b.hc || b.Nb ? (a = this.sb.Fc(), b = this.la.Bc[v(b)], oe(b, a ? Tc: Sc)) : (a = this.la.Bc[v(b)], oe(a, Ic))
    };
    q.ff = function(a) {
        this.jb.ve(a.target)
    };
    q.ef = function(a) {
        this.jb.wd(a.target)
    };
    q.gf = function(a) {
        a.target === this.ba.Of() && this.ba.zc(void 0);
        this.jb.wf(a.target)
    };
    q.Ti = function(a) {
        qe(this.la, this.yb, a.target)
    };
    q.Vi = function(a) {
        te(this.la, a.target)
    };
    q.Ui = function(a) {
        var b = this.la,
        c = this.yb;
        a = a.target;
        te(b, a);
        qe(b, c, a)
    };
    q.Ei = function(a) {
        re(this.la, this.yb, a.target)
    };
    q.Gi = function(a) {
        se(this.la, a.target)
    };
    q.Fi = function(a) {
        var b = this.la,
        c = this.yb;
        a = a.target;
        se(b, a);
        re(b, c, a)
    };
    function Hg(a, b, c, d, f) {
        if (! (A || C && D("525"))) return ! 0;
        if (z && f) return Ig(a);
        if (f && !d) return ! 1;
        u(b) && (b = Jg(b));
        if (!c && (17 == b || 18 == b || z && 91 == b)) return ! 1;
        if (C && d && c) switch (a) {
        case 220:
        case 219:
        case 221:
        case 192:
        case 186:
        case 189:
        case 187:
        case 188:
        case 190:
        case 191:
        case 192:
        case 222:
            return ! 1
        }
        if (A && d && b == a) return ! 1;
        switch (a) {
        case 13:
            return ! (A && A && 9 <= Za);
        case 27:
            return ! C
        }
        return Ig(a)
    }
    function Ig(a) {
        if (48 <= a && 57 >= a || 96 <= a && 106 >= a || 65 <= a && 90 >= a || C && 0 == a) return ! 0;
        switch (a) {
        case 32:
        case 63:
        case 107:
        case 109:
        case 110:
        case 111:
        case 186:
        case 59:
        case 189:
        case 187:
        case 61:
        case 188:
        case 190:
        case 191:
        case 192:
        case 222:
        case 219:
        case 220:
        case 221:
            return ! 0;
        default:
            return ! 1
        }
    }
    function Jg(a) {
        if (B) a = Kg(a);
        else if (z && C) a: switch (a) {
        case 93:
            a = 91;
            break a
        }
        return a
    }
    function Kg(a) {
        switch (a) {
        case 61:
            return 187;
        case 59:
            return 186;
        case 173:
            return 189;
        case 224:
            return 91;
        case 0:
            return 224;
        default:
            return a
        }
    };
    function Lg(a, b) {
        M.call(this);
        a && Mg(this, a, b)
    }
    w(Lg, M);
    q = Lg.prototype;
    q.ea = null;
    q.Ve = null;
    q.Vf = null;
    q.We = null;
    q.Ib = -1;
    q.Hc = -1;
    q.Af = !1;
    var Ng = {
        3 : 13,
        12 : 144,
        63232 : 38,
        63233 : 40,
        63234 : 37,
        63235 : 39,
        63236 : 112,
        63237 : 113,
        63238 : 114,
        63239 : 115,
        63240 : 116,
        63241 : 117,
        63242 : 118,
        63243 : 119,
        63244 : 120,
        63245 : 121,
        63246 : 122,
        63247 : 123,
        63248 : 44,
        63272 : 46,
        63273 : 36,
        63275 : 35,
        63276 : 33,
        63277 : 34,
        63289 : 144,
        63302 : 45
    },
    Og = {
        Up: 38,
        Down: 40,
        Left: 37,
        Right: 39,
        Enter: 13,
        F1: 112,
        F2: 113,
        F3: 114,
        F4: 115,
        F5: 116,
        F6: 117,
        F7: 118,
        F8: 119,
        F9: 120,
        F10: 121,
        F11: 122,
        F12: 123,
        "U+007F": 46,
        Home: 36,
        End: 35,
        PageUp: 33,
        PageDown: 34,
        Insert: 45
    },
    Pg = A || C && D("525"),
    Qg = z && B;
    q = Lg.prototype;
    q.Ad = function(a) {
        C && (17 == this.Ib && !a.ctrlKey || 18 == this.Ib && !a.altKey || z && 91 == this.Ib && !a.metaKey) && (this.Hc = this.Ib = -1); - 1 == this.Ib && (a.ctrlKey && 17 != a.keyCode ? this.Ib = 17 : a.altKey && 18 != a.keyCode ? this.Ib = 18 : a.metaKey && 91 != a.keyCode && (this.Ib = 91));
        Pg && !Hg(a.keyCode, this.Ib, a.shiftKey, a.ctrlKey, a.altKey) ? this.handleEvent(a) : (this.Hc = Jg(a.keyCode), Qg && (this.Af = a.altKey))
    };
    q.ei = function(a) {
        this.Hc = this.Ib = -1;
        this.Af = a.altKey
    };
    q.handleEvent = function(a) {
        var b = a.Cc,
        c, d, f = b.altKey;
        A && "keypress" == a.type ? (c = this.Hc, d = 13 != c && 27 != c ? b.keyCode: 0) : C && "keypress" == a.type ? (c = this.Hc, d = 0 <= b.charCode && 63232 > b.charCode && Ig(c) ? b.charCode: 0) : Ma ? (c = this.Hc, d = Ig(c) ? b.keyCode: 0) : (c = b.keyCode || this.Hc, d = b.charCode || 0, Qg && (f = this.Af), z && (63 == d && 224 == c) && (c = 191));
        var g = c = Jg(c),
        k = b.keyIdentifier;
        c ? 63232 <= c && c in Ng ? g = Ng[c] : 25 == c && a.shiftKey && (g = 9) : k && k in Og && (g = Og[k]);
        a = g == this.Ib;
        this.Ib = g;
        b = new Rg(g, d, a, b);
        b.altKey = f;
        this.dispatchEvent(b)
    };
    q.aa = h("ea");
    function Mg(a, b, c) {
        a.We && a.detach();
        a.ea = b;
        a.Ve = I(a.ea, "keypress", a, c);
        a.Vf = I(a.ea, "keydown", a.Ad, c, a);
        a.We = I(a.ea, "keyup", a.ei, c, a)
    }
    q.detach = function() {
        this.Ve && (Qb(this.Ve), Qb(this.Vf), Qb(this.We), this.We = this.Vf = this.Ve = null);
        this.ea = null;
        this.Hc = this.Ib = -1
    };
    q.fb = function() {
        Lg.da.fb.call(this);
        this.detach()
    };
    function Rg(a, b, c, d) {
        d && this.le(d, void 0);
        this.type = "key";
        this.keyCode = a;
        this.charCode = b;
        this.repeat = c
    }
    w(Rg, xb);
    function Sg(a, b) {
        if ("function" == ca(a)) b && (a = ma(a, b));
        else if (a && "function" == typeof a.handleEvent) a = ma(a.handleEvent, a);
        else throw Error("Invalid listener argument");
        s.setTimeout(a, 400)
    };
    var Tg = {
        8 : "backspace",
        9 : "tab",
        13 : "enter",
        16 : "shift",
        17 : "ctrl",
        18 : "alt",
        19 : "pause",
        20 : "caps-lock",
        27 : "esc",
        32 : "space",
        33 : "pg-up",
        34 : "pg-down",
        35 : "end",
        36 : "home",
        37 : "left",
        38 : "up",
        39 : "right",
        40 : "down",
        45 : "insert",
        46 : "delete",
        48 : "0",
        49 : "1",
        50 : "2",
        51 : "3",
        52 : "4",
        53 : "5",
        54 : "6",
        55 : "7",
        56 : "8",
        57 : "9",
        59 : "semicolon",
        61 : "equals",
        65 : "a",
        66 : "b",
        67 : "c",
        68 : "d",
        69 : "e",
        70 : "f",
        71 : "g",
        72 : "h",
        73 : "i",
        74 : "j",
        75 : "k",
        76 : "l",
        77 : "m",
        78 : "n",
        79 : "o",
        80 : "p",
        81 : "q",
        82 : "r",
        83 : "s",
        84 : "t",
        85 : "u",
        86 : "v",
        87 : "w",
        88 : "x",
        89 : "y",
        90 : "z",
        93 : "context",
        96 : "num-0",
        97 : "num-1",
        98 : "num-2",
        99 : "num-3",
        100 : "num-4",
        101 : "num-5",
        102 : "num-6",
        103 : "num-7",
        104 : "num-8",
        105 : "num-9",
        106 : "num-multiply",
        107 : "num-plus",
        109 : "num-minus",
        110 : "num-period",
        111 : "num-division",
        112 : "f1",
        113 : "f2",
        114 : "f3",
        115 : "f4",
        116 : "f5",
        117 : "f6",
        118 : "f7",
        119 : "f8",
        120 : "f9",
        121 : "f10",
        122 : "f11",
        123 : "f12",
        186 : "semicolon",
        187 : "equals",
        189 : "dash",
        188 : ",",
        190 : ".",
        191 : "/",
        192 : "~",
        219 : "open-square-bracket",
        220 : "\\",
        221 : "close-square-bracket",
        222 : "single-quote",
        224 : "win"
    };
    function Ug(a) {
        M.call(this);
        this.Ce = {};
        this.vc = {
            nd: [],
            time: 0
        };
        this.di = ib(Vg);
        this.rj = ib(Wg);
        this.Qh = !0;
        this.Ph = this.Rh = !1;
        this.oi = !0;
        this.uc = a;
        I(this.uc, "keydown", this.Ad, !1, this);
        z && (B && D("1.8")) && I(this.uc, "keyup", this.Ng, !1, this);
        Ia && !B && (I(this.uc, "keypress", this.Pg, !1, this), I(this.uc, "keyup", this.Qg, !1, this))
    }
    var Xg;
    w(Ug, M);
    var Vg = [27, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 19],
    Wg = "color date datetime datetime-local email month number password search tel text time url week".split(" "),
    Yg = {
        Kh: "shortcut",
        Jh: "shortcut_"
    };
    q = Ug.prototype;
    q.yh = function(a, b) {
        var c = Zg,
        d = this.Ce,
        f = arguments;
        if (t(f[1])) {
            for (var f = f[1], f = f.replace(/[ +]*\+[ +]*/g, "+").replace(/[ ]+/g, " ").toLowerCase(), f = f.split(" "), g = [], k, l = 0; k = f[l]; l++) {
                var m = k.split("+"),
                r;
                k = 0;
                for (var x, n = 0; x = m[n]; n++) {
                    switch (x) {
                    case "shift":
                        k |= 1;
                        continue;
                    case "ctrl":
                        k |= 2;
                        continue;
                    case "alt":
                        k |= 4;
                        continue;
                    case "meta":
                        k |= 8;
                        continue
                    }
                    r = x;
                    if (!Xg) {
                        m = {};
                        x = void 0;
                        for (x in Tg) m[Tg[x]] = x;
                        Xg = m
                    }
                    r = Xg[r];
                    break
                }
                g.push({
                    keyCode: r,
                    kh: k
                })
            }
            f = g
        } else for (g = f, l = 1, ea(f[1]) && (g = f[1], l = 0), f = []; l < g.length; l += 2) f.push({
            keyCode: g[l],
            kh: g[l + 1]
        });
        c(d, f, a)
    };
    q.fb = function() {
        Ug.da.fb.call(this);
        this.Ce = {};
        J(this.uc, "keydown", this.Ad, !1, this);
        z && (B && D("1.8")) && J(this.uc, "keyup", this.Ng, !1, this);
        Ia && !B && (J(this.uc, "keypress", this.Pg, !1, this), J(this.uc, "keyup", this.Qg, !1, this));
        this.uc = null
    };
    q.Ng = function(a) {
        if (224 == a.keyCode) this.jh = !0,
        Sg(function() {
            this.jh = !1
        },
        this);
        else {
            var b = a.metaKey || this.jh;
            67 != a.keyCode && 88 != a.keyCode && 86 != a.keyCode || !b || (a.metaKey = b, this.Ad(a))
        }
    };
    function $g(a) {
        return Ia && !B && a.ctrlKey && a.altKey && !a.shiftKey
    }
    q.Pg = function(a) {
        32 < a.keyCode && $g(a) && (this.Yg = !0)
    };
    q.Qg = function(a) { ! this.Yg && $g(a) && this.Ad(a)
    };
    function Zg(a, b, c) {
        var d = b.shift(),
        d = d.keyCode & 255 | d.kh << 8,
        f = a[d];
        if (f && c && (0 == b.length || t(f))) throw Error("Keyboard shortcut conflicts with existing shortcut");
        b.length ? (f || (f = a[d] = {}), Zg(f, b, c)) : a[d] = c
    }
    function ah(a, b, c, d) {
        c = c || 0;
        return (d = (d || a.Ce)[b[c]]) && !t(d) && 1 < b.length - c ? ah(a, b, c + 1, d) : d
    }
    q.Ad = function(a) {
        var b;
        b = a.keyCode;
        if (16 == b || 17 == b || 18 == b) b = !1;
        else {
            var c = a.target,
            d = "TEXTAREA" == c.tagName || "INPUT" == c.tagName || "BUTTON" == c.tagName || "SELECT" == c.tagName,
            f = !d && (c.isContentEditable || c.ownerDocument && "on" == c.ownerDocument.designMode);
            b = !d && !f || this.di[b] || this.Ph ? !0 : f ? !1 : this.oi && (a.altKey || a.ctrlKey || a.metaKey) ? !0 : "INPUT" == c.tagName && this.rj[c.type] ? 13 == b: "INPUT" == c.tagName || "BUTTON" == c.tagName ? 32 != b: !1
        }
        if (b) if ("keydown" == a.type && $g(a)) this.Yg = !1;
        else {
            b = (B ? Kg(a.keyCode) : a.keyCode) & 255 | ((a.shiftKey ? 1 : 0) | (a.ctrlKey ? 2 : 0) | (a.altKey ? 4 : 0) | (a.metaKey ? 8 : 0)) << 8;
            var g, k, c = na();
            this.vc.nd.length && 1500 >= c - this.vc.time ? g = ah(this, this.vc.nd) : this.vc.nd.length = 0;
            g = g ? g[b] : this.Ce[b];
            g || (g = this.Ce[b], this.vc.nd = []);
            g && t(g) ? k = g: g ? (this.vc.nd.push(b), this.vc.time = c, B && a.preventDefault()) : this.vc.nd.length = 0;
            k && (this.Qh && a.preventDefault(), this.Rh && a.stopPropagation(), g = a.target, b = this.dispatchEvent(new bh(Yg.Kh, k, g)), (b &= this.dispatchEvent(new bh(Yg.Jh + k, k, g))) || a.preventDefault(), this.vc.nd.length = 0)
        }
    };
    function bh(a, b, c) {
        G.call(this, a, c);
        this.identifier = b
    }
    w(bh, G);
    function ch() {}
    q = ch.prototype;
    q.Ua = aa("ga");
    q.eg = function(a) {
        var b = this.ga.qc(a),
        c = b ? void 0 : this.ga.zd(a);
        b ? this.Ea().Lb(b) : c ? this.Ea().ec(c) : this.Ea().kc(ve(this.ga.la, a));
        this.qb = b;
        this.Da = c
    };
    q.fg = function(a, b) {
        var c = this.ga.qc(a),
        d = c ? void 0 : this.ga.zd(a);
        this.qb ? this.Ea().jd(this.qb) : this.Da ? this.Ea().bd(this.Da) : this.Ea().Df();
        delete this.qb;
        delete this.Da;
        c ? this.Ea().pe(c) : d ? this.Ea().Gf(d) : this.Ea().Cf(ve(this.ga.la, a));
        this.Ea().Eb(ve(this.ga.la, a), b);
        this.qb = c;
        this.Da = d
    };
    q.gg = function(a) {
        var b = this.ga.qc(a),
        c = b ? void 0 : this.ga.zd(a);
        b ? this.Ea().pb(b) : c ? this.Ea().Fb(c) : this.Ea().lc(ve(this.ga.la, a));
        this.qb = b;
        this.Da = c
    };
    q.Kc = function(a) {
        this.ga.translate(a)
    };
    q.Lc = function(a, b) {
        this.ga.tf(a, b)
    };
    q.Ea = function() {
        return this.ga.Ea()
    };
    q.ia = function() {
        return this.ga.ia()
    };
    q.na = function() {
        return this.ga.ia().na()
    };
    function dh(a, b) {
        M.call(this);
        this.ea = a;
        var c = ga(this.ea) && 1 == this.ea.nodeType ? this.ea: this.ea ? this.ea.body: null,
        d;
        if (d = !!c) {
            a: {
                d = 9 == c.nodeType ? c: c.ownerDocument || c.document;
                if (d.defaultView && d.defaultView.getComputedStyle && (d = d.defaultView.getComputedStyle(c, null))) {
                    d = d.direction || d.getPropertyValue("direction") || "";
                    break a
                }
                d = ""
            }
            d = "rtl" == (d || (c.currentStyle ? c.currentStyle.direction: null) || c.style && c.style.direction)
        }
        this.ji = d;
        this.eh = I(this.ea, B ? "DOMMouseScroll": "mousewheel", this, b)
    }
    w(dh, M);
    dh.prototype.handleEvent = function(a) {
        var b = 0,
        c = 0,
        d = 0;
        a = a.Cc;
        if ("mousewheel" == a.type) {
            c = 1;
            if (A || C && (Ia || D("532.0"))) c = 40;
            d = eh( - a.wheelDelta, c);
            da(a.wheelDeltaX) ? (b = eh( - a.wheelDeltaX, c), c = eh( - a.wheelDeltaY, c)) : c = d
        } else d = a.detail,
        100 < d ? d = 3 : -100 > d && (d = -3),
        da(a.axis) && a.axis === a.HORIZONTAL_AXIS ? b = d: c = d;
        u(this.hh) && (b = Math.min(Math.max(b, -this.hh), this.hh));
        u(this.ih) && (c = Math.min(Math.max(c, -this.ih), this.ih));
        this.ji && (b = -b);
        b = new fh(d, a, b, c);
        this.dispatchEvent(b)
    };
    function eh(a, b) {
        return C && (z || Pa) && 0 != a % b ? a: a / b
    }
    dh.prototype.fb = function() {
        dh.da.fb.call(this);
        Qb(this.eh);
        this.eh = null
    };
    function fh(a, b, c, d) {
        b && this.le(b, void 0);
        this.type = "mousewheel";
        this.detail = a;
        this.Bj = c;
        this.Xh = d
    }
    w(fh, xb);
    function gh() {
        this.vd = !1;
        this.se = this.Dj = void 0
    }
    q = gh.prototype;
    q.Ua = function(a) {
        this.Ke = a;
        this.pi = new dh(a.aa());
        a.aa().addEventListener("mousedown", this.Id.bind(this), !0);
        a.aa().addEventListener("mousemove", this.Ii.bind(this), !0);
        a.aa().addEventListener("mouseup", this.Jd.bind(this), !0);
        a.aa().addEventListener("mouseover", this.Li.bind(this), !0);
        a.aa().addEventListener("mouseout", this.hf.bind(this), !0);
        window.addEventListener("mouseup", this.Mi.bind(this), !0);
        a.aa().addEventListener("MozMousePixelScroll", this.Oi.bind(this), !0);
        I(this.pi, "mousewheel", this.Ni, !1, this)
    };
    q.eg = e();
    q.fg = e();
    q.gg = e();
    q.ph = e();
    q.Kc = e();
    q.Lc = e();
    q.rh = e();
    q.qh = e();
    q.Id = function(a) {
        0 === a.button && (a.preventDefault(), this.vd = !0, this.wi(hh(this, a)))
    };
    q.Ii = function(a) {
        a.preventDefault();
        var b = hh(this, a);
        a.shiftKey && !this.vd ? (this.se && (a = b.va(), a.x -= this.se.x, a.y -= this.se.y, this.Kc(a)), this.se = b) : (this.vd ? this.vi(b, a.shiftKey) : this.ph(b), this.se = void 0)
    };
    q.Jd = function(a) {
        a.preventDefault();
        this.vd = !1;
        this.nh(hh(this, a))
    };
    q.Mi = function(a) {
        this.vd && (this.vd = !1, this.nh(hh(this, a)))
    };
    q.Li = function(a) {
        this.rh(a)
    };
    q.hf = function(a) {
        for (var b = a.toElement || a.relatedTarget; b && b.parentNode && b.parentNode !== window;) {
            if (b.parentNode === this.Ke.aa() || b === this.Ke.aa()) return b.preventDefault && b.preventDefault(),
            !1;
            b = b.parentNode
        }
        this.qh(a)
    };
    q.Ni = function(a) {
        a.preventDefault();
        a.stopPropagation();
        this.Lc(hh(this, a.Cc), 0 < a.Xh ? 0.95 : 1.05)
    };
    q.Oi = function(a) {
        a.preventDefault()
    };
    function hh(a, b) {
        var c = b.pageX - a.Ke.aa().getBoundingClientRect().left - window.pageXOffset,
        d = b.pageY - a.Ke.aa().getBoundingClientRect().top - window.pageYOffset;
        return new E(c, d)
    };
    function ih(a, b) {
        M.call(this);
        this.Cb = b || 1;
        this.Mb = new lc(a.x, a.y);
        this.rd = this.Mb.va();
        this.rd.x -= this.Cb;
        this.ad = [];
        this.ud = []
    }
    w(ih, M);
    q = ih.prototype;
    q.pc = h("rd");
    q.Ne = h("ud");
    q.ye = function(a, b) {
        var c = wc(this.Mb, a);
        if (!b && 0 < this.ad.length) {
            for (var c = 0,
            d = wc(this.Mb, a), f = Math.abs(this.ad[0] - d), g = 1; g < this.ad.length; g++) {
                var k = Math.abs(this.ad[g] - d);
                k < f && (f = k, c = g)
            }
            c = this.ad[c]
        }
        jh(this, c);
        this.fireEvent(Gg)
    };
    function kh(a, b, c) {
        a.ad = [];
        for (var d = 2 * Math.PI / b,
        f = 0; f < b; f++) a.ad.push(xc(c + f * d));
        jh(a, c)
    }
    q.Dh = e();
    function jh(a, b) {
        var c = b - wc(a.Mb, a.pc()),
        d = a.rd,
        f = mc(d, a.Mb, c);
        d.x = f.x;
        d.y = f.y;
        a.Dh(c)
    }
    q.fireEvent = function(a) {
        this.dispatchEvent(new G(a))
    };
    var Gg = "rotator-coordinates-changed";
    function lh(a, b, c) {
        this.ja = b;
        this.bi = c;
        this.Cb = c / (2 * Math.sin(Math.PI / b));
        ih.call(this, a, this.Cb);
        for (a = 0; a < this.ja; a++) b = 2 * a * Math.PI / this.ja,
        this.ud.push(new lc(this.pc().x + this.Cb * Math.cos(b), this.pc().y + this.Cb * Math.sin(b)))
    }
    w(lh, ih);
    lh.prototype.Dh = function(a) {
        for (var b = 1; b < this.ud.length; b++) {
            var c = this.ud[b],
            d = mc(c, this.Mb, a);
            c.x = d.x;
            c.y = d.y
        }
    };
    function W(a) {
        U.call(this);
        this.Fa = a;
        this.gi = 0.3;
        this.Ff = 12
    }
    w(W, U);
    q = W.prototype;
    q.getName = function() {
        return mh[this.Fa].toLowerCase()
    };
    q.Wa = function() {
        return mh[this.Fa]
    };
    q.Ic = function(a) {
        this.ia().$b(a)
    };
    q.ic = function() {
        this.ia().$b(void 0)
    };
    q.cd = function(a) {
        this.ia().zc(a)
    };
    q.nc = function() {
        this.ia().zc(void 0)
    };
    q.Lb = function(a) {
        W.da.Lb.call(this, a);
        this.md(a.ca, Bc(a))
    };
    q.pb = function(a) {
        W.da.pb.call(this, a);
        nh(this)
    };
    q.ec = function(a) {
        W.da.ec.call(this, a);
        for (var b = zc(a), c = [], d = a.ka.ua, f = a.pa.ua, g = 0; g < d.length; g++) {
            var k = d[g];
            k !== a.ka && k !== a.pa && c.push(k)
        }
        for (g = 0; g < f.length; g++) k = f[g],
        k !== a.ka && k !== a.pa && c.push(k);
        for (f = d = 0; f < c.length; f++) d += yc(a.ka.ca, a.pa.ca, c[f].ca);
        this.md(a.ka.ca, xc(b + (0 <= d ? -1 : 1) * ((this.Fa - 2) * Math.PI / (2 * this.Fa))));
        this.mb = a
    };
    q.Fb = function(a) {
        W.da.Fb.call(this, a);
        nh(this)
    };
    q.kc = function(a) {
        W.da.kc.call(this, a);
        this.md(a, Math.PI / 6)
    };
    q.lc = function(a) {
        W.da.lc.call(this, a);
        nh(this)
    };
    q.Eb = function(a, b) {
        W.da.Eb.call(this, a);
        if (this.Na) if (this.mb) {
            var c = yc(this.mb.ka.ca, this.mb.pa.ca, this.Na.pc());
            if (c !== yc(this.mb.ka.ca, this.mb.pa.ca, a)) {
                var c = xc(zc(this.mb) + (0 <= c ? -1 : 1) * ((this.Fa - 2) * Math.PI / (2 * this.Fa))),
                d = this.mb.ka.ca,
                f = this.ia(),
                g = this.Na;
                g.Ta(null);
                f.Qa("document-rotator-removed", g);
                this.md(d, c)
            }
        } else this.Na.ye(a, b)
    };
    q.reset = function() {
        W.da.reset.call(this);
        if (this.Na) {
            var a = this.ia(),
            b = this.Na;
            b.Ta(null);
            a.Qa("document-rotator-removed", b)
        }
        delete this.Na;
        delete this.mb
    };
    q.Eg = e();
    function nh(a) {
        if (a.Na) {
            var b = a.Na.Ne();
            Ag(a);
            for (var c = a.ia(), d = a.Cb * a.gi, f = [], g, k = 1; k < b.length; k++) g = Kf(c, b[k - 1], b[k], !1, d),
            f.push(g);
            g = Kf(c, b[b.length - 1], b[0], !1, d);
            f.push(g);
            a.Eg(f);
            Bg(a)
        }
        a.reset()
    }
    q.md = function(a, b) {
        var c = new lh(a, this.Fa, this.Cb);
        kh(c, this.Ff, b);
        var d = this.ia();
        c.Ta(d);
        d.Qa("document-rotator-added", c);
        this.Na = c
    };
    var mh = {
        3 : "Cyclopropane",
        4 : "Cyclobutane",
        5 : "Cyclopentane",
        6 : "Cyclohexane",
        7 : "Cycloheptane",
        8 : "Cyclooctane"
    };
    function oh() {
        W.call(this, 6)
    }
    w(oh, W);
    oh.prototype.Eg = function(a) {
        for (var b = 0; b < a.length; b++) {
            var c = a[b],
            d;
            if (2 !== c.cb) d = !1;
            else {
                d = c.ka;
                var f = c.pa;
                d = d.gc() && !Ue(d) && f.gc() && !Ue(f)
            }
            d && (Ze(c), b++)
        }
    };
    oh.prototype.getName = p("benzene");
    oh.prototype.Wa = p("Benzene");
    function ph() {
        this.xc = new gh;
        this.$g = new Lg;
        this.Ka = this.La = this.uf = void 0
    }
    w(ph, ch);
    q = ph.prototype;
    q.Ua = function(a) {
        ph.da.Ua.call(this, a);
        this.ga = a;
        this.xc.wi = this.Ri.bind(this);
        this.xc.vi = this.fg.bind(this);
        this.xc.nh = this.gg.bind(this);
        this.xc.ph = this.Hi.bind(this);
        this.xc.rh = this.Ki.bind(this);
        this.xc.qh = this.Ji.bind(this);
        this.xc.Lc = this.Lc.bind(this);
        this.xc.Kc = this.Kc.bind(this);
        this.xc.Ua(a.la);
        Mg(this.$g, a.aa());
        this.uf = new Ug(a.getParent().aa());
        this.uf.yh("undo", z ? "meta+z": "ctrl+z");
        this.uf.yh("redo", z ? "shift+meta+z": "ctrl+y");
        I(this.$g, "key", this.Ci, !1, this);
        I(this.uf, "shortcut", this.Di, !1, this)
    };
    function qh(a) {
        if (void 0 !== a.La || void 0 !== a.Ka) a.La ? a.ga.Ea().ic(a.La) : a.Ka && a.ga.Ea().nc(a.Ka),
        a.La = void 0,
        a.Ka = void 0
    }
    q.Ki = function() {
        var a = this.ga.aa();
        a.setAttribute("tabindex", 1E3);
        a.focus()
    };
    q.Ji = function() {
        qh(this);
        var a = this.ga.aa();
        a.removeAttribute("tabindex");
        a.blur()
    };
    q.Hi = function(a) {
        var b = this.ga.qc(a);
        a = b ? void 0 : this.ga.zd(a);
        b ? this.La !== b && (this.La ? this.ga.Ea().ic(this.La) : this.Ka && this.ga.Ea().nc(this.Ka), this.ga.Ea().Ic(b), this.La = b, this.Ka = void 0) : a ? this.Ka !== a && (this.La ? this.ga.Ea().ic(this.La) : this.Ka && this.ga.Ea().nc(this.Ka), this.ga.Ea().cd(a), this.La = void 0, this.Ka = a) : qh(this)
    };
    q.Ri = function(a) {
        qh(this);
        this.eg(a)
    };
    q.Ci = function(a) {
        if (!a.ctrlKey && !a.metaKey) {
            switch (a.keyCode) {
            case 72:
                X(this, "H");
                break;
            case 66:
                X(this, "B");
                break;
            case 67:
                X(this, "C");
                break;
            case 78:
                X(this, "N");
                break;
            case 79:
                X(this, "O");
                break;
            case 70:
                X(this, "F");
                break;
            case 76:
                X(this, "Cl");
                break;
            case 82:
                X(this, "Br");
                break;
            case 73:
                X(this, "I");
                break;
            case 80:
                X(this, "P");
                break;
            case 90:
                X(this, "Si");
                break;
            case 83:
                X(this, "S");
                break;
            case 84:
                X(this, "Sn");
                break;
            case 46:
                rh(this);
                break;
            case 8:
                rh(this);
                break;
            case 65:
                var b = this.ga.ge();
                if (b) {
                    var c = new oh;
                    c.Qc(this.ga.Pe().Oe());
                    c.Ua(this.ia());
                    Ef(this.ia());
                    c.Lb(b);
                    c.pb(b);
                    Gf(this.ia())
                } else {
                    for (var b = this.ga.ia().na(), c = [], d = 0; d < b.ja.length; d++) {
                        var f = b.ja[d];
                        f.hc && c.push(f)
                    }
                    if (b = c[0]) c = new oh,
                    c.Qc(this.ga.Pe().Oe()),
                    c.Ua(this.ia()),
                    Ef(this.ia()),
                    c.ec(b),
                    c.Fb(b),
                    Gf(this.ia())
                }
                break;
            default:
                return
            }
            a.preventDefault()
        }
    };
    function X(a, b) {
        var c = a.ga.ge();
        if (c) {
            var d = ef().aa(b);
            Ef(a.ia());
            Re(c, d);
            Gf(a.ia())
        }
    }
    function rh(a) {
        Ef(a.ia());
        var b = a.ia();
        b.La ? b.removeNode(b.La) : b.Ka && b.kb(b.Ka);
        for (var b = a.ia(), c = Ke(b.oa), d = 0; d < c.length; d++) {
            var f = c[d];
            void 0 !== f.getParent() && b.removeNode(f)
        }
        Gf(a.ia())
    }
    q.Di = function(a) {
        switch (a.identifier) {
        case "undo":
            this.ga.Ud();
            break;
        case "redo":
            this.ga.Ld()
        }
    };
    function sh() {
        M.call(this);
        this.bc = th;
        this.Mc = this.ea = void 0;
        this.jg = this.te = 1
    }
    w(sh, M);
    q = sh.prototype;
    q.Ua = function(a) {
        a.addEventListener("touchstart", this.cj.bind(this), !0);
        a.addEventListener("touchmove", this.bj.bind(this), !0);
        a.addEventListener("touchend", this.aj.bind(this), !0);
        a.addEventListener("touchcancel", this.$i.bind(this), !0);
        this.ea = a
    };
    q.uh = e();
    q.vh = e();
    q.th = e();
    q.re = e();
    q.dg = e();
    q.Kc = e();
    q.Lc = e();
    q.cj = function(a) {
        switch (a.touches.length) {
        case 1:
            if ("http://www.w3.org/2000/svg" === a.target.namespaceURI) switch (a.preventDefault(), this.bc) {
            case th:
                this.bc = uh,
                this.vh(vh(this, a))
            }
            break;
        case 2:
            a:
            {
                void 0 === a.scale && (this.jg = pc(a.touches[0].clientX, a.touches[0].clientY, a.touches[1].clientX, a.touches[1].clientY), a.scale = 1);
                switch (this.bc) {
                case th:
                    var b = wh(this, a);
                    this.Mc = b.va();
                    this.te = 1;
                    this.bc = xh;
                    break;
                case uh:
                    b = wh(this, a);
                    this.Mc = b.va();
                    this.te = 1;
                    this.bc = xh;
                    this.re();
                    break;
                default:
                    break a
                }
                a.preventDefault()
            }
        }
    };
    q.bj = function(a) {
        switch (a.touches.length) {
        case 1:
            switch (this.bc) {
            case uh:
                this.uh(vh(this, a))
            }
            break;
        case 2:
            switch (void 0 === a.scale && (a.scale = pc(a.touches[0].clientX, a.touches[0].clientY, a.touches[1].clientX, a.touches[1].clientY) / this.jg), this.bc) {
            case xh:
                var b = a.scale / this.te;
                this.Kc(yh(this, a));
                this.Lc(wh(this, a), b);
                this.Mc = wh(this, a);
                this.te = a.scale
            }
        }
    };
    q.aj = function(a) {
        switch (this.bc) {
        case uh:
            this.th(zh(this, a));
            this.bc = th;
            break;
        case xh:
            this.bc = th,
            this.Mc = void 0
        }
    };
    q.$i = function() {
        this.bc = th;
        this.Mc = void 0;
        this.jg = this.te = 1;
        this.re()
    };
    function zh(a, b) {
        var c = b.changedTouches[0],
        d = c.pageX - a.ea.getBoundingClientRect().left - window.pageXOffset,
        c = c.pageY - a.ea.getBoundingClientRect().top - window.pageYOffset;
        return new E(d, c)
    }
    function vh(a, b) {
        var c = b.touches[0],
        d = c.pageX - a.ea.getBoundingClientRect().left - window.pageXOffset,
        c = c.pageY - a.ea.getBoundingClientRect().top - window.pageYOffset;
        return new E(d, c)
    }
    function wh(a, b) {
        var c = b.touches[0],
        d = b.touches[1],
        f = a.ea.getBoundingClientRect(),
        g = window.pageXOffset,
        k = window.pageYOffset;
        return new E(c.pageX - f.left - g + 0.5 * (d.pageX - f.left - g - (c.pageX - f.left - g)), c.pageY - f.top - k + 0.5 * (d.pageY - f.top - k - (c.pageY - f.top - k)))
    }
    function yh(a, b) {
        if (!a.Mc) return new E(0, 0);
        var c = wh(a, b);
        c.x -= a.Mc.x;
        c.y -= a.Mc.y;
        return c
    }
    var th = "touchscreen-state-waiting",
    uh = "touchscreen-state-touching",
    xh = "touchscreen-state-pinching";
    function Ah() {
        this.Sc = new sh
    }
    w(Ah, ch);
    Ah.prototype.Ua = function(a) {
        Ah.da.Ua.call(this, a);
        this.Sc.vh = this.eg.bind(this);
        this.Sc.uh = this.fg.bind(this);
        this.Sc.th = this.gg.bind(this);
        this.Sc.re = this.re.bind(this);
        this.Sc.dg = this.dg.bind(this);
        this.Sc.Kc = this.Kc.bind(this);
        this.Sc.Lc = this.Lc.bind(this);
        this.Sc.Ua(a.aa())
    };
    Ah.prototype.re = function() {
        this.ga.Ea().reset()
    };
    Ah.prototype.dg = function(a, b, c) {
        this.ga.tf(b, c);
        this.ga.translate(a)
    };
    function V() {
        U.call(this);
        this.De = []
    }
    w(V, U);
    q = V.prototype;
    q.getName = function() {
        return this.ha ? this.ha.getName() : V.da.getName.call(this)
    };
    q.Wa = function() {
        return this.ha ? this.ha.Wa() : V.da.Wa.call(this)
    };
    q.Ic = function(a) {
        this.ha ? this.ha.Ic(a) : V.da.Ic.call(this, a)
    };
    q.ic = function(a) {
        this.ha ? this.ha.ic(a) : V.da.ic.call(this, a)
    };
    q.cd = function(a) {
        this.ha ? this.ha.cd(a) : V.da.cd.call(this, a)
    };
    q.nc = function(a) {
        this.ha ? this.ha.nc(a) : V.da.nc.call(this, a)
    };
    q.Lf = function() {
        return this.ha ? this.ha.getName() : V.da.Lf.call(this)
    };
    q.Pd = function(a) {
        this.ha ? this.ha.Pd(a) : V.da.Pd.call(this, a)
    };
    q.Qc = function(a) {
        V.da.Qc.call(this, a);
        this.ha && this.ha.Qc(a)
    };
    q.Aa = function(a) {
        this.De.push(a);
        a.Ta(this)
    };
    q.Qe = function(a) {
        for (var b = 0; b < this.De.length; b++) {
            var c = this.De[b];
            if (c.getName() === a) return c
        }
    };
    function Fg(a, b) {
        var c = a.Qe(b);
        if (!c) throw Error(Bh);
        a.ha && a.ha.disconnect();
        a.ia() && c.Ua(a.ia());
        c.Qc(a.Cb);
        a.ha = c
    }
    q.Jf = h("ha");
    q.Ua = function(a) {
        V.da.Ua.call(this, a);
        this.ha && this.ha.Ua(a)
    };
    q.disconnect = function() {
        V.da.disconnect.call(this);
        this.ha && this.ha.disconnect()
    };
    q.Fc = function() {
        return this.ha ? this.ha.Fc() : V.da.Fc.call(this)
    };
    q.Lb = function(a) {
        this.ha && this.ha.Lb(a)
    };
    q.pb = function(a) {
        this.ha && this.ha.pb(a)
    };
    q.pe = function(a) {
        this.ha && this.ha.pe(a)
    };
    q.jd = function(a) {
        this.ha && this.ha.jd(a)
    };
    q.ec = function(a) {
        this.ha && this.ha.ec(a)
    };
    q.Fb = function(a) {
        this.ha && this.ha.Fb(a)
    };
    q.Gf = function(a) {
        this.ha && this.ha.Gf(a)
    };
    q.bd = function(a) {
        this.ha && this.ha.bd(a)
    };
    q.kc = function(a) {
        this.ha && this.ha.kc(a)
    };
    q.lc = function(a) {
        this.ha && this.ha.lc(a)
    };
    q.Cf = function(a) {
        this.ha && this.ha.Cf(a)
    };
    q.Df = function() {
        this.ha && this.ha.Df()
    };
    q.Eb = function(a, b) {
        this.ha && this.ha.Eb(a, b)
    };
    q.reset = function() {
        this.ha && this.ha.reset()
    };
    q.Qd = function(a) {
        this.ha && this.ha.Qd(a)
    };
    var Bh = "toolbox-no-such-tool";
    function Ch() {
        U.call(this);
        this.Hb = new og
    }
    w(Ch, U);
    q = Ch.prototype;
    q.Fb = function(a) {
        Ch.da.Fb.call(this, a); ! this.Hb.Ma() && this.end()
    };
    q.pb = function(a) {
        Ch.da.pb.call(this, a); ! this.Hb.Ma() && this.end()
    };
    q.kc = function(a) {
        Ch.da.kc.call(this, a);
        this.Hb.clear();
        Je(this.na());
        qg(this.Hb, a);
        this.ia().zf(this.Hb)
    };
    q.lc = function(a) {
        Ch.da.lc.call(this, a);
        this.end()
    };
    q.Eb = function(a) {
        Ch.da.Eb.call(this, a);
        if (!this.Hb.Ma()) {
            qg(this.Hb, a);
            a = this.ia().na().ra;
            for (var b = 0; b < a.length; b++) this.Hb.containsNode(a[b]) ? a[b].wa(!0) : a[b].wa(!1)
        }
    };
    q.reset = function() {
        Ch.da.reset.call(this);
        var a = this.ia(),
        b = this.Hb;
        b.Ta(null);
        a.Qa("document-lasso-removed", b);
        Je(this.na());
        this.Hb.clear()
    };
    q.end = function() {
        var a = this.ia(),
        b = this.Hb;
        b.Ta(null);
        a.Qa("document-lasso-removed", b);
        this.Hb.clear()
    };
    function Dh() {
        Ch.call(this);
        this.Td = void 0
    }
    w(Dh, Ch);
    q = Dh.prototype;
    q.getName = p("move");
    q.Wa = p("Move one or more atoms");
    q.Ic = function(a) {
        this.ia().$b(a);
        a.Nb && this.Pd(yg.tg)
    };
    q.ic = function() {
        this.ia().$b(void 0);
        this.Pd(yg.yf)
    };
    q.Lb = function(a) {
        Dh.da.Lb.call(this, a);
        a.Nb || (Je(this.na()), a.wa(!0));
        var b = this.ia();
        b.fd += 1;
        1 === b.fd && b.Qa("document-group-move-entered", b);
        this.Pd(yg.tg);
        this.Td = a
    };
    q.pb = function(a) {
        Dh.da.pb.call(this, a);
        this.Td = void 0;
        a = this.ia();
        if (0 === a.fd) throw Error("document-not-in-group-move");
        a.fd -= 1;
        0 === a.fd && a.Qa("document-group-move-exited", a)
    };
    q.Eb = function(a) {
        if (this.Td) {
            var b = a.x - this.Td.ca.x;
            a = a.y - this.Td.ca.y;
            var c = this.na().ra;
            Ag(this);
            for (var d = 0; d < c.length; d++) {
                var f = c[d];
                if (f.Nb) {
                    var g = f.ca;
                    f.move(g.x + b, g.y + a)
                }
            }
            Bg(this)
        } else Dh.da.Eb.call(this, a)
    };
    q.end = function() {
        Dh.da.end.call(this);
        this.Td = void 0
    };
    function Eh() {
        Ch.call(this);
        this.kd = this.ld = void 0
    }
    w(Eh, Ch);
    q = Eh.prototype;
    q.Wa = p("Delete atoms and bonds");
    q.getName = p("delete");
    q.Fc = p(!0);
    q.Lb = function(a) {
        Eh.da.Lb.call(this, a);
        a.wa(!0);
        this.ld = a
    };
    q.pb = function(a) {
        Eh.da.pb.call(this, a);
        a === this.ld && (Ag(this), a.wa(!1), this.ia().removeNode(a), Bg(this));
        this.ld = void 0
    };
    q.jd = function(a) {
        a.wa(!1);
        this.ld = void 0
    };
    q.ec = function(a) {
        a.wa(!0);
        this.kd = a
    };
    q.bd = function(a) {
        a.wa(!1);
        this.kd = void 0
    };
    q.Fb = function(a) {
        Eh.da.Fb.call(this, a);
        a === this.kd && (Ag(this), this.ia().kb(a), Bg(this));
        this.kd = void 0
    };
    q.Eb = function(a) {
        this.ld || this.kd || Eh.da.Eb.call(this, a)
    };
    q.reset = function() {
        Eh.da.reset.call(this);
        Je(this.na());
        this.kd = this.ld = void 0
    };
    q.end = function() {
        if (this.Hb.Ma()) Je(this.na());
        else {
            Ag(this);
            for (var a = Ke(this.na()), b = 0; b < a.length; b++) {
                var c = a[b];
                c.wa(!1);
                this.na().removeNode(c)
            }
            Bg(this)
        }
        this.ld = this.kd = void 0;
        Eh.da.end.call(this)
    };
    function Fh(a, b) {
        ih.call(this, a, b);
        this.ud.push(this.Mb);
        this.ud.push(this.pc());
        this.Cb = b || 1;
        this.cf = void 0
    }
    w(Fh, ih);
    Fh.prototype.ye = function(a, b) {
        this.cf || Fh.da.ye.call(this, a, b)
    };
    function Gh(a, b) {
        a.cf = a.pc().va();
        a.rd.x = b.x;
        a.rd.y = b.y;
        a.fireEvent(Gg)
    };
    function Y() {
        U.call(this);
        this.Ff = 12;
        this.Cj = this.Vh = this.mb = this.$f = void 0
    }
    w(Y, U);
    q = Y.prototype;
    q.getName = p("single-bond");
    q.Wa = p("Single Bond");
    q.Ic = function(a) {
        this.ia().$b(a)
    };
    q.ic = function() {
        this.ia().$b(void 0)
    };
    q.cd = function(a) {
        this.ia().zc(a)
    };
    q.nc = function() {
        this.ia().zc(void 0)
    };
    q.nb = function() {
        return Md
    };
    q.Lb = function(a) {
        Y.da.Lb.call(this, a);
        var b = Ec(a);
        this.md(a.ca, b);
        this.$f = a
    };
    q.pb = function(a) {
        Y.da.pb.call(this, a);
        this.end()
    };
    q.pe = function(a) {
        Y.da.pe.call(this, a);
        a !== this.$f && this.Na && (Gh(this.Na, a.ca), this.Ag = a)
    };
    q.jd = function(a) {
        Y.da.jd.call(this, a);
        if (this.Ag && this.Ag === a) {
            var b = this.Na;
            a = a.ca;
            if (!b.cf) throw Error("line-rotator-can-not-unclamp");
            b.cf = void 0;
            var c = b.pc(),
            d = b.Mb,
            f = d.x - a.x,
            d = d.y - a.y,
            f = b.Cb / Math.sqrt(f * f + d * d);
            c.x = b.Mb.x + f * (a.x - b.Mb.x);
            c.y = b.Mb.y + f * (a.y - b.Mb.y);
            b.ye(a)
        }
    };
    q.ec = function(a) {
        Y.da.ec.call(this, a);
        a.wa(!0);
        this.mb = a
    };
    q.Fb = function(a) {
        Y.da.Fb.call(this, a);
        if (this.mb) {
            a.wa(!1);
            Ag(this);
            if (this.nb() === Md) Ze(a);
            else if (a.nb() === this.nb()) {
                var b = a.ka,
                c = a.pa,
                d = a.Ia();
                a.ka = c;
                a.pa = b;
                a.fireEvent(Fe, a, d)
            } else Ee(a, this.nb());
            Bg(this);
            this.mb = void 0
        } else this.end()
    };
    q.bd = function(a) {
        Y.da.bd.call(this, a);
        this.mb && this.mb === a && (a.wa(!1), this.mb = void 0)
    };
    q.kc = function(a) {
        Y.da.kc.call(this, a);
        this.md(a, Math.PI / 6)
    };
    q.lc = function(a) {
        Y.da.lc.call(this, a);
        this.end()
    };
    q.Eb = function(a, b) {
        Y.da.Eb.call(this, a, b);
        b ? Gh(this.Na, a) : this.Na && this.Na.ye(a)
    };
    q.reset = function() {
        if (this.Na) {
            var a = this.ia(),
            b = this.Na;
            b.Ta(null);
            a.Qa("document-rotator-removed", b)
        }
        this.Vh = this.$f = this.Na = void 0;
        Je(this.ia().na())
    };
    q.end = function() {
        this.Na && (Ag(this), Kf(this.ia(), this.Na.Mb, this.Na.pc(), !0, 0.3 * this.Cb, this.nb()), Bg(this));
        this.reset()
    };
    q.md = function(a, b) {
        var c = new Fh(a, this.Cb);
        kh(c, this.Ff, b);
        var d = this.ia();
        c.Ta(d);
        d.Qa("document-rotator-added", c);
        this.Na = c
    };
    function Hh() {
        Y.call(this)
    }
    w(Hh, Y);
    Hh.prototype.getName = p("wedge-bond");
    Hh.prototype.Wa = p("Wedge Bond");
    Hh.prototype.nb = function() {
        return Nd
    };
    function Ih() {
        Y.call(this)
    }
    w(Ih, Y);
    Ih.prototype.getName = p("hash-bond");
    Ih.prototype.Wa = p("Hash Bond");
    Ih.prototype.nb = function() {
        return Od
    };
    function Jh() {
        Y.call(this)
    }
    w(Jh, Y);
    Jh.prototype.getName = p("wavy-bond");
    Jh.prototype.Wa = p("Wavy Bond");
    Jh.prototype.nb = function() {
        return Pd
    };
    function Kh() {
        U.call(this)
    }
    w(Kh, U);
    q = Kh.prototype;
    q.execute = e();
    q.cd = function(a) {
        this.ia().zc(a)
    };
    q.nc = function() {
        this.ia().zc(void 0)
    };
    q.ec = function(a) {
        a.wa(!0);
        this.Da = a
    };
    q.Fb = function(a) {
        this.Da === a && (this.execute(a), a.wa(!1), delete this.Da)
    };
    q.bd = function(a) {
        this.Da === a && a.wa(!1);
        delete this.Da
    };
    q.Kg = h("Da");
    function Lh() {
        U.call(this)
    }
    w(Lh, Kh);
    Lh.prototype.execute = function(a) {
        Ag(this);
        a.nb() === Fd ? Ee(a, Md) : (a.sc(a.cb - 4), Ee(a, Fd));
        Bg(this)
    };
    Lh.prototype.getName = p("crossed-bond");
    Lh.prototype.Wa = p("Cis or Trans Double Bond");
    function Mh() {
        U.call(this)
    }
    w(Mh, U);
    q = Mh.prototype;
    q.execute = e();
    q.Ic = function(a) {
        this.ia().$b(a)
    };
    q.ic = function() {
        this.ia().$b(void 0)
    };
    q.Lb = function(a) {
        a.wa(!0);
        this.qb = a
    };
    q.pb = function(a) {
        this.qb === a && (this.execute(a), a.wa(!1));
        delete this.qb
    };
    q.jd = function(a) {
        this.qb === a && a.wa(!1)
    };
    q.ed = h("qb");
    function Nh() {
        U.call(this)
    }
    w(Nh, Mh);
    Nh.prototype.getName = p("increase-charge");
    Nh.prototype.Wa = p("Increase charge");
    Nh.prototype.execute = function(a) {
        Ag(this);
        a.sc(1);
        Bg(this)
    };
    function Oh() {
        U.call(this)
    }
    w(Oh, Mh);
    Oh.prototype.getName = p("decrease-charge");
    Oh.prototype.Wa = p("Decrease charge");
    Oh.prototype.execute = function(a) {
        Ag(this);
        a.sc( - 1);
        Bg(this)
    };
    function Ph() {
        U.call(this)
    }
    w(Ph, Mh);
    Ph.prototype.getName = p("next-isotope");
    Ph.prototype.Wa = p("Set isotope to next available");
    Ph.prototype.execute = function(a) {
        Ag(this);
        var b = a.ea.tc;
        if (b.length) {
            var c = a.Ia();
            if (a.Sa) {
                var d = va(b, a.Sa);
                d === b.length - 1 ? delete a.Sa: (d++, a.Sa = b[d])
            } else a.Sa = b[0];
            a.fireEvent(Be, a, c)
        }
        Bg(this)
    };
    function Qh() {}
    function Rh(a, b) {
        var c = new V,
        d = new Dh,
        f = new Eh;
        c.Aa(d);
        c.Aa(f);
        Sh(b, c);
        c = new V;
        c.Aa(new Y);
        c.Aa(new Hh);
        c.Aa(new Ih);
        c.Aa(new Jh);
        c.Aa(new Lh);
        Sh(b, c);
        b.Aa(new oh);
        b.Aa(new W(6));
        b.Aa(new W(5));
        c = new V;
        c.Aa(new W(3));
        c.Aa(new W(4));
        c.Aa(new W(7));
        c.Aa(new W(8));
        Sh(b, c);
        c = new V;
        c.Aa(new Nh);
        c.Aa(new Oh);
        c.Aa(new Ph);
        Sh(b, c);
        Th(b, "single-bond");
        c = ef();
        d = c.aa("C");
        Uh(b, d);
        d = c.aa("N");
        Uh(b, d);
        d = c.aa("O");
        Uh(b, d);
        d = c.aa("S");
        Uh(b, d);
        d = c.aa("F");
        Uh(b, d);
        d = c.aa("Cl");
        Uh(b, d);
        d = c.aa("Br");
        Uh(b, d);
        d = c.aa("I");
        Uh(b, d);
        d = c.aa("H");
        Uh(b, d);
        Vh(b, c.aa("Si"));
        Wh(a, b)
    }
    function Wh(a, b) {
        Xh(b, "undo", "Undo [" + (z ? "\u2318Z": "Ctrl-Z") + "]",
        function(a) {
            a.Ud()
        },
        Yh);
        Xh(b, "redo", "Redo [" + (z ? "\u21e7\u2318Z": "Ctrl-Y") + "]",
        function(a) {
            a.Ld()
        },
        Zh);
        Xh(b, "new-document", "New Document",
        function(a) {
            a.ga.clear()
        },
        $h);
        Xh(b, "edit-document", "Copy & Paste",
        function(a) {
            a.Cg.show()
        });
        Xh(b, "reset-view", "Zoom to Fit",
        function(a) {
            a.Od()
        },
        ai);
        Xh(b, "full-screen", a.Jg ? "Exit Full Screen": "Full Screen",
        function(a) {
            a.Fh()
        },
        bi);
        Xh(b, "about", "About ChemWriter",
        function(a) {
            a.ug.show()
        })
    };
    var ci, di;
    di = ci = !1;
    var ei = Ja();
    ei && ( - 1 != ei.indexOf("Firefox") || -1 != ei.indexOf("Camino") || -1 != ei.indexOf("iPhone") || -1 != ei.indexOf("iPod") || -1 != ei.indexOf("iPad") || ( - 1 != ei.indexOf("Chrome") ? ci = !0 : -1 != ei.indexOf("Android") || -1 != ei.indexOf("Safari") && (di = !0)));
    var fi = ci,
    gi = di;
    function hi() {
        N.call(this)
    }
    w(hi, N);
    hi.prototype.show = function() {
        z && (gi || fi) ? bb(this.aa(), "chemwriter-show-no-transition") : bb(this.aa(), "chemwriter-show")
    };
    hi.prototype.rc = function() {
        z && (gi || fi) ? cb(this.aa(), "chemwriter-show-no-transition") : cb(this.aa(), "chemwriter-show")
    };
    hi.prototype.xa = function() {
        this.ea = F("div", {
            "class": z && (gi || fi) ? "chemwriter-overlay-no-transition": "chemwriter-overlay"
        })
    };
    function ii(a) {
        N.call(this);
        this.Wh = a
    }
    w(ii, N);
    ii.prototype.xa = function() {
        this.ea = F("div", {
            "class": this.Wh
        })
    };
    ii.prototype.Wb = function(a) {
        this.aa().appendChild(a)
    };
    function ji() {
        N.call(this)
    }
    w(ji, N);
    ji.prototype.Ga = function(a) {
        O(this, a)
    };
    ji.prototype.xa = function() {
        this.ea = F("ul", {
            "class": "chemwriter-button-row"
        })
    };
    function ki(a) {
        N.call(this);
        this.Uh = a || "#000000";
        this.xg = new ji;
        this.of = new ii("chemwriter-content");
        O(this, this.of);
        O(this, this.xg)
    }
    w(ki, hi);
    ki.prototype.Ga = function(a) {
        this.xg.Ga(a)
    };
    ki.prototype.xa = function() {
        ki.da.xa.call(this);
        this.Dg = F("div", {
            "class": "chemwriter-dialog",
            style: "background-color:" + this.Uh + ";"
        });
        this.aa().appendChild(this.Dg)
    };
    ki.prototype.Kf = h("Dg");
    function li(a) {
        N.call(this);
        this.ne = a;
        this.fc = !0
    }
    w(li, N);
    q = li.prototype;
    q.xa = function() {
        this.ea = F("li", {
            "class": "chemwriter-text-button"
        },
        this.ne)
    };
    q.ma = function() {
        li.da.ma.call(this);
        mi(this);
        this.vb()
    };
    q.vb = function() {
        I(this.aa(), "touchend", this.oh, !1, this);
        I(this.aa(), "click", this.oh, !1, this)
    };
    q.oh = function(a) {
        a.preventDefault();
        a.stopPropagation();
        this.fc && this.dispatchEvent(ni)
    };
    q.Ub = function(a) {
        this.fc !== a && (this.fc = a, mi(this))
    };
    function mi(a) {
        a.Xb && (a.fc ? (mg(a.aa(), "chemwriter-button-disabled"), lg(a.aa(), "chemwriter-button-enabled")) : (mg(a.aa(), "chemwriter-button-enabled"), lg(a.aa(), "chemwriter-button-disabled")))
    }
    var ni = "text-button-pressed";
    function oi() {
        N.call(this);
        this.Ac = this.dd = void 0
    }
    w(oi, N);
    oi.prototype.xa = function() {
        var a = F("div", {
            "class": "chemwriter-code-editor"
        }),
        b = F("textarea", {
            "class": "chemwriter-code-editor-front",
            spellcheck: !1,
            wrap: "off"
        }),
        c = F("textarea", {
            "class": "chemwriter-code-editor-back",
            spellcheck: !1,
            wrap: "off"
        });
        b.addEventListener("scroll",
        function() {
            c.scrollTop = b.scrollTop;
            c.scrollLeft = b.scrollLeft
        },
        !1);
        a.appendChild(c);
        a.appendChild(b);
        this.ea = a;
        this.dd = b;
        this.Ac = c
    };
    oi.prototype.Gc = function() {
        return "" === this.Ac.value
    };
    function pi(a, b) {
        a.dd.value = b;
        a.Ac.value = ""
    }
    function qi(a, b) {
        var c = a.dd.value.split(/[\n|\r\n]/);
        c[b] = c[b].replace(/[^\n|\r\n]/g, String.fromCharCode(9608));
        if ("" === c[b]) for (var d = 0; 80 > d; d++) c[b] += String.fromCharCode(9608);
        a.Ac.value = c.join("\n");
        a.Ac.scrollTop = a.dd.scrollTop
    }
    oi.prototype.Sf = function() {
        return this.dd.value
    };
    oi.prototype.Te = function() {
        this.dd.select();
        this.dd.scrollTop = 0;
        this.Ac.scrollTop = 0
    };
    function ri() {
        N.call(this);
        this.ah = "";
        this.rg = !1;
        this.bh = new ii("chemwriter-left-panel");
        this.Ch = new ii("chemwriter-right-panel");
        this.cc = new oi;
        this.je = new Pf;
        O(this.bh, this.cc);
        O(this.Ch, this.je);
        O(this, this.bh);
        O(this, this.Ch)
    }
    w(ri, N);
    ri.prototype.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-clipboard"
        })
    };
    ri.prototype.ma = function() {
        ri.da.ma.call(this)
    };
    function si(a, b) {
        pi(a.cc, b);
        try {
            a.je.Pb(b),
            a.cc.Ac.value = ""
        } catch(c) {
            qi(a.cc, c.Yf - 1),
            a.je.Xc()
        }
        setTimeout(function() {
            this.cc.Te()
        }.bind(a), 1)
    }
    ri.prototype.update = function() {
        var a = this.cc.Sf();
        if (this.ah !== a) {
            this.ah = a;
            try {
                this.je.Pb(a),
                this.cc.Ac.value = ""
            } catch(b) {
                qi(this.cc, b.Yf - 1),
                this.je.Xc()
            }
        }
    };
    ri.prototype.Te = function() {
        this.cc.Te()
    };
    function ti(a) {
        ki.call(this);
        this.ci = a;
        this.Xg = void 0;
        this.qg = new li("Use Molecule");
        this.Eh = new li("Select All");
        this.Bg = new li("Clear");
        this.yg = new li("Cancel");
        this.Yc = new ri;
        this.Ga(this.qg);
        this.Ga(this.Eh);
        this.Ga(this.Bg);
        this.Ga(this.yg);
        O(this.of, this.Yc)
    }
    w(ti, ki);
    q = ti.prototype;
    q.ma = function() {
        ti.da.ma.call(this);
        I(this.qg, ni, this.hj, !1, this);
        I(this.Eh, ni, this.Wi, !1, this);
        I(this.Bg, ni, this.si, !1, this);
        I(this.yg, ni, this.ri, !1, this)
    };
    q.show = function() {
        ti.da.show.call(this);
        si(this.Yc, this.getParent().Dc());
        this.Xg = setInterval(this.Bi.bind(this), 100)
    };
    q.rc = function() {
        ti.da.rc.call(this);
        clearInterval(this.Xg)
    };
    q.Bi = function() {
        this.Yc.update();
        this.qg.Ub(this.Yc.cc.Gc())
    };
    q.hj = function() {
        this.ci.Pb(this.Yc.cc.Sf());
        this.rc()
    };
    q.Wi = function() {
        this.Yc.Te()
    };
    q.si = function() {
        si(this.Yc, "")
    };
    q.ri = function() {
        this.rc()
    };
    function ui() {
        ki.call(this);
        this.ai = new li("Done");
        this.Uc = new ii("chemwriter-about-panel");
        this.Gg = new ii("chemwriter-documentation-panel");
        this.Ga(this.ai);
        O(this.of, this.Uc);
        O(this.of, this.Gg)
    }
    w(ui, ki);
    ui.prototype.ma = function() {
        ui.da.ma.call(this);
        var a = F("div", {
            "class": "chemwriter-logo"
        }),
        b = F("div", {
            "class": "chemwriter-product-name"
        }),
        c = F("div", {
            "class": "chemwriter-rev"
        },
        "Version: 3.2.2"),
        d = F("div", {
            "class": "chemwriter-rev"
        },
        "Commit: 0ecf628"),
        f = F("div", {
            "class": "chemwriter-authors"
        },
        "Authors: Richard Apodaca; Robert Apodaca; Orion Jankowski; Zachary Catlin"),
        g = F("div", {
            "class": "chemwriter-copyright"
        });
        b.innerHTML = 'ChemWriter<span class="chemwriter-super">&reg;</span>';
        g.innerHTML = '&copy; 2007-2015 <a href="http://metamolecular.com" target="_blank">Metamolecular, LLC';
        this.Uc.Wb(a);
        this.Uc.Wb(b);
        this.Uc.Wb(c);
        this.Uc.Wb(d);
        this.Uc.Wb(f);
        this.Uc.Wb(g);
        a = F("ul", {
            "class": "chemwriter-list"
        });
        b = F("li", {
            "class": "chemwriter-list-item"
        });
        c = F("li", {
            "class": "chemwriter-list-item"
        });
        d = F("li", {
            "class": "chemwriter-list-item"
        });
        b.innerHTML = '<a href="http://chemwriter.com/user-guide/" target="_blank">User Guide</a>';
        c.innerHTML = '<a href="http://chemwriter.com/developer-guide/" target="_blank">Developer Guide</a>';
        d.innerHTML = '<a href="http://chemwriter.com/support?subject=chemwriter-about-dialog" target="_blank">Questions & Comments</a>';
        a.appendChild(b);
        a.appendChild(c);
        a.appendChild(d);
        this.Gg.Wb(a);
        I(this, ni, this.cg, !1, this)
    };
    ui.prototype.cg = function() {
        this.rc()
    };
    function vi(a) {
        N.call(this);
        this.ij = a;
        this.Qb = []
    }
    w(vi, N);
    vi.prototype.xa = function() {
        var a;
        switch (this.ij) {
        case wi:
            a = "chemwriter-palette-left";
            break;
        case xi:
            a = "chemwriter-palette-bottom";
            break;
        case yi:
            a = "chemwriter-palette-right";
            break;
        case zi:
            a = "chemwriter-palette-bottom-right";
            break;
        case Ai:
            a = "chemwriter-palette-float"
        }
        this.ea = F("div", {
            "class": a + " chemwriter-palette "
        })
    };
    vi.prototype.Ga = function(a, b) {
        this.Qb.push(b);
        O(this, b)
    };
    var wi = "palette-left",
    yi = "palette-right",
    xi = "palette-bottom",
    zi = "palette-bottom-right",
    Ai = "palette-float";
    function Bi() {
        N.call(this);
        this.ig = new vi(Ai);
        this.Qb = [];
        O(this, this.ig)
    }
    w(Bi, N);
    q = Bi.prototype;
    q.Ga = function(a, b) {
        0 === this.Qb.length && Ci(b, !0);
        this.ig.Ga(a, b);
        this.Qb.push(b)
    };
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-dynamic-palette"
        })
    };
    q.ma = function() {
        Bi.da.ma.call(this);
        this.vb()
    };
    q.show = function(a) {
        bb(this.aa(), "chemwriter-appear");
        this.ig.aa().style.top = a - 4 + "px"
    };
    q.rc = function() {
        cb(this.aa(), "chemwriter-appear")
    };
    q.vb = function() {
        I(this.aa(), "touchend", this.lh, !1, this);
        I(this.aa(), "click", this.lh, !1, this)
    };
    q.lh = function() {
        this.rc()
    };
    function Di(a) {
        N.call(this);
        this.hg = a
    }
    w(Di, N);
    Di.prototype.ma = function() {
        Di.da.ma.call(this);
        this.aa().value = this.Vd;
        this.aa() && (this.aa().value = this.Vd);
        I(this.aa(), "change", this.zi, !1, this)
    };
    Di.prototype.xa = function() {
        for (var a = F("select", {
            "class": "chemwriter-select"
        }), b = 0; b < this.hg.length; b += 2) {
            var c = this.hg[b + 1],
            d = F("option", {
                value: this.hg[b]
            });
            d.textContent = c;
            a.appendChild(d)
        }
        this.ea = a
    };
    function Ei(a, b) {
        a.Vd = b;
        a.aa() && (a.aa().value = a.Vd)
    }
    Di.prototype.zi = function() {
        this.Vd = this.aa().value;
        this.dispatchEvent(Fi)
    };
    var Fi = "select-selected";
    function Gi() {
        vi.call(this, xi);
        var a;
        a = ef();
        a = Object.keys(a.mg);
        y.sort.call(a, Da);
        for (var b = [], c = 0; c < a.length; c++) {
            var d = a[c];
            b.push(d);
            b.push(d)
        }
        this.Ae = new Di(b);
        O(this, this.Ae)
    }
    w(Gi, vi);
    Gi.prototype.ma = function() {
        Gi.da.ma.call(this);
        I(this.Ae, Fi, this.Xi, !1, this)
    };
    Gi.prototype.xa = function() {
        Gi.da.xa.call(this);
        bb(this.aa(), "chemwriter-element-palette")
    };
    Gi.prototype.Xi = function() {
        var a = this.Qb[this.Qb.length - 1],
        b = this.Ae.Vd,
        c = ef().aa(b);
        a.aa() && (a.aa().textContent = b);
        a.ne = b;
        Hi(a, c.getName());
        Ci(a, !0);
        this.dispatchEvent(Ii)
    };
    var Ii = "element-palette-element-selected";
    function Ji(a) {
        U.call(this);
        this.ea = a
    }
    w(Ji, Mh);
    Ji.prototype.getName = function() {
        return "element-" + this.ea.Rd
    };
    Ji.prototype.Wa = function() {
        return this.ea.getName()
    };
    Ji.prototype.execute = function() {
        Ag(this);
        Re(this.ed(), this.ea);
        Bg(this)
    };
    function Ki() {
        N.call(this)
    }
    w(Ki, N);
    Ki.prototype.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-icon"
        })
    };
    function Li(a, b, c, d, f) {
        N.call(this);
        this.Rg = this.kg = !1;
        this.fc = !0;
        this.hi = 200;
        this.Sd = void 0;
        this.$h = c || !1;
        this.ne = d || void 0;
        this.Th = f || !1;
        this.Sg = void 0;
        Mi(this, a);
        Hi(this, b);
        d || (this.Sg = new Ki, O(this, this.Sg))
    }
    w(Li, N);
    q = Li.prototype;
    q.xa = function() {
        var a = F("div", {
            "class": Ni(this) + " chemwriter-button",
            title: this.sj
        });
        this.$h && a.appendChild(F("div", {
            "class": "chemwriter-detail-disclosure"
        }));
        this.ne && (a.textContent = this.ne);
        this.ea = a
    };
    q.ma = function() {
        Li.da.ma.call(this);
        this.fc ? (this.vb(), lg(this.aa(), Oi)) : lg(this.aa(), Pi)
    };
    q.Ub = function(a) {
        this.fc !== a && (this.aa() && (a ? (lg(this.aa(), Oi), mg(this.aa(), Pi), this.vb()) : (mg(this.aa(), Oi), lg(this.aa(), Pi), mg(this.aa(), Qi), this.sf())), this.fc = a, this.Rg = this.kg = !1)
    };
    function Ci(a, b) {
        if (a.kg !== b && a.fc) {
            if (a.aa()) {
                var c = a.aa(),
                d = Qi;
                kg(c, d) ? mg(c, d) : lg(c, d)
            }
            b && a.dispatchEvent(Ri);
            a.kg = b
        }
    }
    function Mi(a, b) {
        a.aa() && mg(a.aa(), Ni(a));
        a.bf = b;
        a.aa() && lg(a.aa(), Ni(a))
    }
    q.getName = h("bf");
    function Hi(a, b) {
        a.sj = b;
        a.aa() && (a.aa().title = b)
    }
    q.vb = function() {
        I(this.aa(), "touchstart", this.Id, !1, this);
        I(this.aa(), "touchend", this.Jd, !1, this);
        I(this.aa(), "mousedown", this.Id, !1, this);
        I(this.aa(), "mouseup", this.Jd, !1, this);
        I(this.aa(), "mouseout", this.hf, !1, this)
    };
    q.sf = function() {
        J(this.aa(), "touchstart", this.Id, !1, this);
        J(this.aa(), "touchend", this.Jd, !1, this);
        J(this.aa(), "mousedown", this.Id, !1, this);
        J(this.aa(), "mouseup", this.Jd, !1, this);
        J(this.aa(), "mouseout", this.hf, !1, this)
    };
    q.Id = function(a) {
        a.preventDefault();
        this.fc && (this.Sd = window.setTimeout(this.Yi.bind(this), this.hi))
    };
    q.Jd = function(a) {
        a.preventDefault();
        this.Sd && (Ci(this, !0), this.Th && Ci(this, !1));
        Si(this)
    };
    q.hf = function(a) {
        a.preventDefault();
        Si(this)
    };
    q.Yi = function() {
        Ci(this, !0);
        this.dispatchEvent(Ti);
        this.Rg = !0;
        this.Sd = void 0
    };
    function Si(a) {
        a.Sd && (window.clearTimeout(a.Sd), a.Sd = void 0)
    }
    function Ni(a) {
        return "chemwriter-button-" + a.bf
    }
    var Ri = "button-pressed",
    Ti = "button-held",
    Qi = "chemwriter-button-pressed",
    Oi = "chemwriter-button-enabled",
    Pi = "chemwriter-button-disabled";
    function Ui() {
        M.call(this);
        this.Qb = []
    }
    w(Ui, M);
    Ui.prototype.Ga = function(a) {
        this.Qb.push(a);
        I(a, Ri, this.cg, !1, this)
    };
    Ui.prototype.cg = function(a) {
        a = a.target;
        for (var b = 0; b < this.Qb.length; b++) {
            var c = this.Qb[b];
            c !== a && Ci(c, !1)
        }
    };
    function Vi(a) {
        M.call(this);
        this.Ge = a || window;
        this.$e = I(this.Ge, "resize", this.fi, !1, this);
        this.Fa = lb(this.Ge || window)
    }
    w(Vi, M);
    q = Vi.prototype;
    q.$e = null;
    q.Ge = null;
    q.Fa = null;
    q.fb = function() {
        Vi.da.fb.call(this);
        this.$e && (Qb(this.$e), this.$e = null);
        this.Fa = this.Ge = null
    };
    q.fi = function() {
        var a = lb(this.Ge || window);
        a == this.Fa || a && this.Fa && a.width == this.Fa.width && a.height == this.Fa.height || (this.Fa = a, this.dispatchEvent("resize"))
    };
    function Wi() {
        N.call(this);
        this.Hh = new Vi;
        this.Wc = void 0
    }
    w(Wi, N);
    q = Wi.prototype;
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-fullscreen chemwriter"
        })
    };
    q.ma = function() {
        Wi.da.ma.call(this);
        I(this.Hh, "resize", this.sh, !1, this);
        Xi(this)
    };
    q.de = function() {
        Wi.da.de.call(this);
        J(this.Hh, "resize", this.sh, !1, this)
    };
    q.show = function() {
        for (this.Wc = F("div", {
            "class": "chemwriter-hide"
        }); document.body.firstChild;) this.Wc.appendChild(document.body.firstChild);
        document.body.appendChild(this.Wc);
        hc(this, document.body)
    };
    q.rc = function() {
        for (; this.Wc.firstChild;) document.body.appendChild(this.Wc.firstChild);
        document.body.removeChild(this.Wc);
        this.Wc = void 0;
        this.fb()
    };
    q.sh = function() {
        Xi(this)
    };
    function Xi(a) {
        a.aa().style.height = document.documentElement.clientHeight + "px";
        a.aa().style.width = document.documentElement.clientWidth + "px"
    };
    function Z() {
        N.call(this);
        this.ga = new Cg;
        this.Ze = new vi(wi);
        this.pd = new Gi;
        this.Bh = new vi(yi);
        this.vg = new vi(zi);
        this.ug = new ui;
        this.Cg = new ti(this);
        this.Ie = new Ui;
        this.qj = new Ah;
        this.Zh = new ph;
        this.vj = {};
        this.yc = {};
        this.zj = {};
        O(this, this.Ze);
        O(this, this.pd);
        O(this, this.Bh);
        O(this, this.vg);
        O(this, this.ga);
        O(this, this.Cg);
        O(this, this.ug);
        this.Jg = void 0
    }
    w(Z, N);
    function Th(a, b) {
        Eg(a.ga, b);
        for (var c = a.Ze,
        d = 0; d < c.Qb.length; d++) {
            var f = c.Qb[d];
            if (f.getName() === b) {
                Ci(f, !0);
                break
            }
        }
    }
    q = Z.prototype;
    q.Ud = function() {
        this.ga.Ud()
    };
    q.Ld = function() {
        this.ga.Ld()
    };
    q.Od = function() {
        this.ga.Od()
    };
    q.Fh = function() {
        var a = new Wi,
        b = new Z,
        c = new Qh;
        c.Jg = !0;
        Rh(c, b);
        O(a, b);
        a.show();
        b.Pb(this.Dc());
        b.Fh = function() {
            var c = b.Dc();
            a.rc();
            this.Pb(c);
            b = void 0
        }.bind(this)
    };
    q.Pb = function(a) {
        var b = new Cf,
        c;
        pf || (pf = new of);
        c = pf;
        c.oa = b.na();
        c.rf(a);
        this.ga.Be(b)
    };
    q.Dc = function() {
        return this.ga.Dc()
    };
    q.Ma = function() {
        return this.ga.Ma()
    };
    q.Aa = function(a) {
        var b = new Li(a.getName(), a.Wa());
        I(b, Ri,
        function() {
            Eg(this.ga, a.getName())
        },
        !1, this);
        this.Ze.Ga(a.getName(), b);
        this.Ie.Ga(b);
        this.ga.Aa(a)
    };
    function Yi(a, b, c, d) {
        var f = new Li(b.getName(), b.Wa());
        I(f, Ri,
        function() {
            Mi(c, b.getName());
            Hi(c, b.Wa());
            Fg(d, b.getName());
            Eg(this.ga, b.getName())
        },
        !1, a);
        a.ga.Aa(b);
        return f
    }
    function Sh(a, b) {
        for (var c = b.De[0], d = new Li(c.getName(), c.Wa(), !0), f = new Bi, g = new Ui, k = b.De, l = 0; l < k.length; l++) {
            var m = k[l],
            r = Yi(a, m, d, b);
            f.Ga(m.getName(), r);
            g.Ga(r);
            a.ga.Aa(m)
        }
        O(a, f);
        a.Ze.Ga("", d);
        a.Ie.Ga(d);
        I(d, Ri,
        function() {
            Eg(this.ga, b.Jf().getName())
        },
        !1, a);
        I(d, Ti,
        function() {
            f.show(d.aa().offsetTop)
        },
        !1, a);
        Fg(b, c.getName())
    }
    function Vh(a, b) {
        var c = new Ji(b);
        c.getName = p("element-selector");
        var d = new Li(c.getName(), b.getName(), !1, b.Rd);
        I(d, Ri,
        function() {
            Th(this, c.getName())
        },
        !1, a);
        a.ga.Aa(c);
        a.pd.Ga(c.getName(), d);
        a.Ie.Ga(d);
        Ei(a.pd.Ae, b.Rd)
    }
    function Xh(a, b, c, d, f) {
        c = new Li(b, c, !1, void 0, !0);
        I(c, Ri,
        function() {
            d(this)
        },
        !1, a);
        if (f) switch (a.yc[f] = c, f) {
        case Yh:
            c.Ub(!1);
            break;
        case Zh:
            c.Ub(!1)
        }
        f === bi ? a.vg.Ga(b, c) : a.Bh.Ga(b, c)
    }
    q.Pe = function() {
        return this.ga.Pe()
    };
    q.xa = function() {
        this.ea = F("div", {
            "class": "chemwriter-editor"
        })
    };
    q.ma = function() {
        Z.da.ma.call(this);
        this.qj.Ua(this.ga);
        this.Zh.Ua(this.ga);
        this.ga.Od();
        this.vb()
    };
    q.vb = function() {
        I(this.pd, Ii, this.Ai, !1, this);
        I(this, dg, this.fj, !0, this);
        I(this, Yf, this.gj, !0, this);
        I(this, bg, this.dj, !0, this);
        I(this, $f, this.ej, !0, this);
        I(this, If, this.Hd, !1, this)
    };
    q.Ai = function() {
        var a = this.pd.Ae.Vd;
        this.ga.Qe("element-selector").ea = ef().aa(a)
    };
    function Uh(a, b) {
        var c = new Ji(b),
        d = c.getName(),
        f = new Li(d, b.getName(), !1, b.Rd);
        I(f, Ri,
        function() {
            Th(this, d)
        },
        !1, a);
        a.ga.Aa(c);
        a.pd.Ga(d, f);
        a.Ie.Ga(f)
    }
    q.fj = function() {
        var a = this.yc[Yh];
        a && a.Ub(!0)
    };
    q.gj = function() {
        var a = this.yc[Yh];
        a && a.Ub(!1)
    };
    q.dj = function() {
        var a = this.yc[Zh];
        a && a.Ub(!0)
    };
    q.ej = function() {
        var a = this.yc[Zh];
        a && a.Ub(!1)
    };
    q.Hd = function() {
        var a = this.yc[$h];
        a && a.Ub(!this.ga.ia().Ma()); (a = this.yc[ai]) && a.Ub(!this.ga.ia().Ma()); (a = this.yc[Zi]) && a.Ub(!this.ga.ia().Ma()); (a = this.yc[$i]) && a.Ub(!this.ga.ia().Ma())
    };
    var Yh = "chemwriter-editor-role-undo",
    Zh = "chemwriter-editor-role-redo",
    ai = "chemwriter-editor-reset-view",
    Zi = "chemwriter-editor-zoom-in",
    $i = "chemwriter-editor-zoom-out",
    $h = "chemwriter-editor-role-new-document",
    bi = "chemwriter-editor-role-fullscreen";
    var $ = {},
    aj = {},
    bj = [],
    cj = !1;
    $.mj = function(a) {
        cj ? a() : bj.push(a)
    };
    $.pj = function() {
        for (var a = 0; a < bj.length; a++) bj[a]();
        cj = !0
    };
    $.refresh = function() {
        $.li();
        $.ki()
    };
    $.le = function() {
        $.mi();
        $.refresh();
        $.pj()
    };
    $.mi = function() {
        var a = document.querySelector("script[data-chemwriter-style]");
        if (a) {
            var b;
            try {
                b = JSON.parse(a.textContent)
            } catch(c) {
                throw Error("Error parsing stylesheet: " + a.textContent);
            }
            Yc(b)
        }
    };
    $.li = function() {
        $.gh(document);
        for (var a = document.querySelectorAll("iframe"), b = 0; b < a.length; b++) $.gh(a[b].contentWindow.document)
    };
    $.gh = function(a) {
        a = a.querySelectorAll('div[data-chemwriter-ui="image"]');
        for (var b = 0; b < a.length; b++) $.Oh(a[b])
    };
    $.ki = function() {
        $.fh(document);
        for (var a = document.querySelectorAll("iframe"), b = 0; b < a.length; b++) $.fh(a[b].contentWindow.document)
    };
    $.fh = function(a) {
        a = a.querySelectorAll('div[data-chemwriter-ui="editor"]');
        for (var b = 0; b < a.length; b++) $.Nh(a[b])
    };
    $.Nh = function(a) {
        var b = $b(a),
        c = new Z;
        Rh(new Qh, c);
        var d = a.parentNode;
        d && d.replaceChild(b, a);
        hc(c, b);
        $.xh(c, a)
    };
    $.Oh = function(a) {
        var b = $b(a),
        c = new Pf,
        d = a.parentNode;
        d && d.replaceChild(b, a);
        hc(c, b);
        $.xh(c, a)
    };
    $.xh = function(a, b) {
        var c = b.getAttribute("data-chemwriter-data") || "",
        d = b.getAttribute("id");
        if (c) c = c.replace(Zb, "\n"),
        setTimeout(function() {
            $.Pb(a, c, d)
        },
        1);
        else {
            var f = b.getAttribute("data-chemwriter-src");
            if (f) {
                var g = new Yd(f);
                Pb(g, ae,
                function() {
                    var b = g.Zb;
                    200 === b.status ? $.Pb(a, b.body, d) : $.error({
                        message: "Error reading file at URL: " + f,
                        id: d
                    })
                },
                !1);
                g.send()
            }
        }
        d && (aj[d] = a)
    };
    $.Pb = function(a, b, c) {
        try {
            a.Pb(b)
        } catch(d) {
            $.error({
                message: "Error reading inline file: " + d.message,
                id: c
            })
        }
    };
    $.error = function(a) {
        window.console && window.console.error(JSON.stringify(a))
    };
    window.addEventListener ? window.addEventListener("load",
    function() {
        $.le()
    },
    !1) : window.attachEvent("onload",
    function() {
        for (var a = document.getElementsByTagName("div"), b = 0; b < a.length; b++) {
            var c = a[b];
            if (c.getAttribute("data-chemwriter-ui")) {
                var d = document.createElement("div");
                d.setAttribute("id", c.getAttribute("id"));
                d.setAttribute("style", "width:" + c.getAttribute("data-chemwriter-width") + "px;height:" + c.getAttribute("data-chemwriter-height") + "px;");
                d.setAttribute("class", "chemwriter-fallback-content");
                c.parentNode.replaceChild(d, c)
            }
        }
    });
    ba("chemwriter.System", $);
    $.ready = $.mj;
    ba("chemwriter.components", aj);
    ba("chemwriter.refresh", $.refresh);
    Z.prototype.getMolfile = Z.prototype.Dc;
    Z.prototype.setMolfile = Z.prototype.Pb;
    Z.prototype.addEventListener = Z.prototype.addEventListener;
    Z.prototype.isEmpty = Z.prototype.Ma;
    Pf.prototype.getMolfile = Pf.prototype.Dc;
    Pf.prototype.setMolfile = Pf.prototype.Pb;
})();