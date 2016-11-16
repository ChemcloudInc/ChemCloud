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
window.rnd||(rnd={}),rnd.MouseEvent=function(e){if("touches"in e){this.touches=e.touches.length;if(e.touches.length==1){var t=e.touches[0];this.pageX=t.pageX,this.pageY=t.pageY}}else this.pageX=e.pageX,this.pageY=e.pageY;if(Object.isUndefined(this.pageX)||Object.isUndefined(this.pageY))this.pageX=e.x,this.pageY=e.y;this.altKey=e.altKey,this.shiftKey=e.shiftKey,this.ctrlKey=e.ctrlKey,this.metaKey=e.metaKey};