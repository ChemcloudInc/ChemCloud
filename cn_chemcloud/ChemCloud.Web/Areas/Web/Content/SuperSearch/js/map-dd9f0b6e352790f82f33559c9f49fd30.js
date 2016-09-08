/****************************************************************************
 * Copyright (C) 2009-2010 GGA Software Services LLC
 * 
 * This file may be distributed and/or modified under the terms of the
 * GNU Affero General Public License version 3 as published by the Free
 * Software Foundation and appearing in the file LICENSE.GPL included in
 * the packaging of this file.
 * 
 * This file is provided AS IS with NO WARRANTY OF ANY KIND, INCLUDING THE
 * WARRANTY OF DESIGN, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 ***************************************************************************/
window.util||(util={}),util.Map=function(e){if(typeof e!="undefined"&&e.constructor!=Object)throw Error("Passed object is not an instance of 'Object'!");this._obj=e||{},this._count=0},util.Map.prototype.each=function(e,t){for(var n in this._obj){var r=parseInt(n),i=this._obj[n];isNaN(r)||(n=r),e.call(t,n,i)}},util.Map.prototype.find=function(e,t){for(var n in this._obj){var r=parseInt(n),i=this._obj[n];isNaN(r)||(n=r);if(e.call(t,n,i))return n}},util.Map.prototype.findAll=function(e,t){var n=[];for(var r in this._obj){var i=parseInt(r),s=this._obj[r];isNaN(i)||(r=i),e.call(t,r,s)&&n.push(r)}return n},util.Map.prototype.keys=function(){var e=[];for(var t in this._obj)e.push(t);return e},util.Map.prototype.ikeys=function(){var e=[];for(var t in this._obj)e.push(t-0);return e},util.Map.prototype.set=function(e,t){this._count+=(typeof t!="undefined"?1:0)-(typeof this._obj[e]!="undefined"?1:0);if(typeof t=="undefined"){var n=this._obj[e];return delete this._obj[e],n}return this._obj[e]=t},util.Map.prototype.get=function(e){return this._obj[e]!==Object.prototype[e]?this._obj[e]:undefined},util.Map.prototype.has=function(e){return this._obj[e]!==Object.prototype[e]},util.Map.prototype.unset=function(e){return this.set(e,undefined)},util.Map.prototype.update=function(e){for(var t in e)this.set(t,e[t])},util.Map.prototype.clear=function(){this._obj={}},util.Map.prototype.count=function(){return this._count},util.Map.prototype.idList=function(){return util.idList(this._obj)},util.Map.prototype.keyOf=function(e){for(var t in this._obj)if(this._obj[t]==e)return t};