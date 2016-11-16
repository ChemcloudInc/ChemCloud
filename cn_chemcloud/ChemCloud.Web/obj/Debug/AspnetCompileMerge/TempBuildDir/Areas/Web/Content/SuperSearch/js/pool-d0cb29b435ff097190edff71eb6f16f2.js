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
if(!window.util||!util.Map)throw new Error("Map should be defined first");util.Pool=function(){this._map=new util.Map,this._nextId=0},util.Pool.prototype.newId=function(){return this._nextId++},util.Pool.prototype.add=function(e){var t=this._nextId++;return this._map.set(t,e),t},util.Pool.prototype.set=function(e,t){this._map.set(e,t)},util.Pool.prototype.get=function(e){return this._map.get(e)},util.Pool.prototype.has=function(e){return this._map.has(e)},util.Pool.prototype.remove=function(e){return this._map.unset(e)},util.Pool.prototype.clear=function(){this._map.clear()},util.Pool.prototype.keys=function(){return this._map.keys()},util.Pool.prototype.ikeys=function(){return this._map.ikeys()},util.Pool.prototype.each=function(e,t){this._map.each(e,t)},util.Pool.prototype.find=function(e,t){return this._map.find(e,t)},util.Pool.prototype.count=function(){return this._map.count()},util.Pool.prototype.keyOf=function(e){return this._map.keyOf(e)};