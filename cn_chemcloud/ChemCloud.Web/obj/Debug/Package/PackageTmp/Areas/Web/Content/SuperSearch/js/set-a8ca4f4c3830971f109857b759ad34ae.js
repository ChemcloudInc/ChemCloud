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
window.util||(util={}),util.Set={empty:function(){return{}},single:function(e){var t={};return util.Set.add(t,e),t},size:function(e){var t=0;for(var n in e)e[n]!==Object.prototype[n]&&t++;return t},contains:function(e,t){return typeof e[t]!="undefined"&&e[t]!==Object.prototype[t]},subset:function(e,t){for(var n in e)if(e[n]!==Object.prototype[n]&&t[n]!==e[n])return!1;return!0},eq:function(e,t){return util.Set.subset(e,t)&&util.Set.subset(t,e)},each:function(e,t,n){for(var r in e)e[r]!==Object.prototype[r]&&t.call(n,e[r])},filter:function(e,t,n){var r={};for(var i in e)e[i]!==Object.prototype[i]&&t.call(n,e[i])&&(r[e[i]]=e[i]);return r},pick:function(e){for(var t in e)if(e[t]!==Object.prototype[t])return e[t];return null},list:function(e){var t=[];for(var n in e)e[n]!==Object.prototype[n]&&t.push(e[n]);return t},add:function(e,t){e[t]=t},mergeIn:function(e,t){util.Set.each(t,function(t){util.Set.add(e,t)})},remove:function(e,t){var n=e[t];return delete e[t],n},clone:function(e){var t={};return util.Set.mergeIn(t,e),t},fromList:function(e){var t={};for(var n=0;n<e.length;++n)t[e[n]-0]=e[n]-0;return t},keySetInt:function(e){var t={};return e.each(function(e){t[e-0]=e-0}),t},find:function(e,t,n){for(var r in e)if(e[r]!==Object.prototype[r]&&t.call(n,e[r]))return r;return null}};