if(!window.Prototype)throw new Error("Prototype.js should be loaded first");if(!window.rnd)throw new Error("rnd should be defined prior to loading this file");if(!window.ui)throw new Error("ui should be defined prior to loading this file");rnd.ReaGenericsTable=function(e,t){t=t||{},e=$(e),e.style.width="610px",e.style.height="390px",e.innerHTML="",this.onClick=function(e){this.setSelection(e)},this.elemHalfSz=new util.Vec2(22,14),this.elemSz=this.elemHalfSz.scaled(2),this.spacing=new util.Vec2(3,3),this.cornerRadius=2,this.orig=this.elemSz.scaled(0),this.viewSz=new util.Vec2(e.clientWidth||100,e.clientHeight||100),this.paper=new Raphael(e,this.viewSz.x,this.viewSz.y),this.bb=new util.Box2Abs(new util.Vec2,this.viewSz),this.bgColor=e.getStyle("background-color"),this.fillColor=t.fillColor||"#def",this.fillColorSelected=t.fillColorSelected||"#fcb",this.frameColor=t.frameColor||"#9ad",this.frameThickness=t.frameThickness||"1pt",this.fontSize=t.fontSize||18,this.fontType=t.fontType||"Arial",this.atomProps=null,this.frameAttrs={fill:this.fillColor,stroke:this.frameColor,"stroke-width":this.frameThickness},this.fontAttrs={"font-family":this.fontType,"font-size":this.fontSize},this.groupRectAttrs={stroke:"lightgray","stroke-width":"1px"},this.labelTextAttrs={"font-family":"Arial","font-size":13,fill:"gray"},this.labelRectAttrs={fill:this.bgColor,stroke:this.bgColor},this.items=[];var n=function(e,t,n,r,i,s){s=s||"start",this.paper.rect(e,t,n,r,this.cornerRadius).attr(this.groupRectAttrs);var o=this.paper.text(s=="left"?e+10:s=="right"?e+n-10:e+n/2,t,i).attr(this.labelTextAttrs).attr("text-anchor",s=="left"?"start":s=="right"?"end":"middle");this.paper.rect().attr(o.getBBox()).attr(this.labelRectAttrs),o.toFront()},r=function(e,t,n,r){this.paper.text(e,t,n).attr(this.labelTextAttrs).attr("text-anchor",r=="left"?"start":r=="right"?"end":"middle")},i=function(e,t){var n=this.paper.rect(e.x-this.elemHalfSz.x,e.y-this.elemHalfSz.y,this.elemSz.x,this.elemSz.y,this.cornerRadius).attr(this.frameAttrs),r=this.paper.text(e.x,e.y,t).attr(this.fontAttrs),i=this;n.node.onclick=function(){i.onClick(t)},r.node.onclick=function(){i.onClick(t)},this.items.push({text:t,box:n,label:r})},s=this.spacing.x+this.elemSz.x;n.apply(this,[5,5,600,75,"Atom Generics","left"]),i.apply(this,[new util.Vec2(57,30),"A"]),i.apply(this,[new util.Vec2(57+s,30),"AH"]),r.apply(this,[81,60,"any atom\n	"]),i.apply(this,[new util.Vec2(207,30),"Q"]),i.apply(this,[new util.Vec2(207+s,30),"QH"]),r.apply(this,[231,60,"any atom except\ncarbon or hydrogen"]),i.apply(this,[new util.Vec2(357,30),"M"]),i.apply(this,[new util.Vec2(357+s,30),"MH"]),r.apply(this,[381,60,"any metal\n	"]),i.apply(this,[new util.Vec2(507,30),"X"]),i.apply(this,[new util.Vec2(507+s,30),"XH"]),r.apply(this,[531,60,"any halogen\n	"]),n.apply(this,[5,90,600,300,"Group Generics","left"]),r.apply(this,[210,115,"any","right"]),i.apply(this,[new util.Vec2(286-s,115),"G"]),i.apply(this,[new util.Vec2(286,115),"GH"]),i.apply(this,[new util.Vec2(286+s,115),"G*"]),i.apply(this,[new util.Vec2(286+2*s,115),"GH*"]),n.apply(this,[10,140,235,245,"ACYCLIC"]),r.apply(this,[74,165,"acyclic","right"]),i.apply(this,[new util.Vec2(104,165),"ACY"]),i.apply(this,[new util.Vec2(104+s,165),"ACH"]),n.apply(this,[15,190,110,190,"CARB"]),i.apply(this,[new util.Vec2(46,215),"ABC"]),i.apply(this,[new util.Vec2(46+s,215),"ABH"]),r.apply(this,[68,235,"carb"]),i.apply(this,[new util.Vec2(46,260),"AYL"]),i.apply(this,[new util.Vec2(46+s,260),"AYH"]),r.apply(this,[68,280,"alkynyl"]),i.apply(this,[new util.Vec2(46,305),"ALK"]),i.apply(this,[new util.Vec2(46+s,305),"ALH"]),r.apply(this,[68,325,"alkyl"]),i.apply(this,[new util.Vec2(46,350),"AEL"]),i.apply(this,[new util.Vec2(46+s,350),"AEH"]),r.apply(this,[68,370,"alkenyl"]),n.apply(this,[130,190,110,190,"HETERO"]),i.apply(this,[new util.Vec2(161,215),"AHC"]),i.apply(this,[new util.Vec2(161+s,215),"AHH"]),r.apply(this,[183,235,"hetero"]),i.apply(this,[new util.Vec2(161,260),"AOX"]),i.apply(this,[new util.Vec2(161+s,260),"AOH"]),r.apply(this,[183,280,"alkoxy"]),n.apply(this,[250,140,350,245,"CYCLIC"]),r.apply(this,[371,165,"cyclic","right"]),i.apply(this,[new util.Vec2(401,165),"CYC"]),i.apply(this,[new util.Vec2(401+s,165),"CYH"]),n.apply(this,[255,190,110,190,"CARBO"]),i.apply(this,[new util.Vec2(286,215),"CBC"]),i.apply(this,[new util.Vec2(286+s,215),"CBH"]),r.apply(this,[308,235,"carbo"]),i.apply(this,[new util.Vec2(286,260),"ARY"]),i.apply(this,[new util.Vec2(286+s,260),"ARH"]),r.apply(this,[308,280,"aryl"]),i.apply(this,[new util.Vec2(286,305),"CAL"]),i.apply(this,[new util.Vec2(286+s,305),"CAH"]),r.apply(this,[308,325,"cycloalkyl"]),i.apply(this,[new util.Vec2(286,350),"CEL"]),i.apply(this,[new util.Vec2(286+s,350),"CEH"]),r.apply(this,[308,370,"cycloalkenyl"]),n.apply(this,[370,190,110,190,"HETERO"]),i.apply(this,[new util.Vec2(401,215),"CHC"]),i.apply(this,[new util.Vec2(401+s,215),"CHH"]),r.apply(this,[423,235,"hetero"]),i.apply(this,[new util.Vec2(401,260),"HAR"]),i.apply(this,[new util.Vec2(401+s,260),"HAH"]),r.apply(this,[423,280,"hetero aryl"]),n.apply(this,[485,190,110,190,"CYCLIC"]),i.apply(this,[new util.Vec2(516,215),"CXX"]),i.apply(this,[new util.Vec2(516+s,215),"CXH"]),r.apply(this,[538,235,"no carbon"])},rnd.ReaGenericsTable.prototype.getAtomProps=function(){return this.atomProps},rnd.ReaGenericsTable.prototype.setSelection=function(e){this.atomProps={label:e};for(var t=0;t<this.items.length;t++)this.items[t].box.attr("fill",this.items[t].text==e?this.fillColorSelected:this.fillColor);$("reagenerics_table_ok").disabled=!e||e==""},ui.showReaGenericsTable=function(e){if(!$("reagenerics_table").visible()){e=e||{},ui.showDialog("reagenerics_table"),typeof ui.reagenerics_table_obj=="undefined"&&(ui.reagenerics_table_obj=new rnd.ReaGenericsTable("reagenerics_table_area",{fillColor:"#DADADA",fillColorSelected:"#FFFFFF",frameColor:"#E8E8E8",fontSize:18,buttonHalfSize:18},!0)),e.selection&&ui.reagenerics_table_obj.setSelection(e.selection);var t=(new Event.Handler("reagenerics_table_ok","click",undefined,function(){if(ui.reagenerics_table_obj.atomProps==null)return;ui.hideDialog("reagenerics_table"),"onOk"in e&&e.onOk(ui.reagenerics_table_obj.selection),t.stop()})).start(),n=(new Event.Handler("reagenerics_table_cancel","click",undefined,function(){ui.hideDialog("reagenerics_table"),"onCancel"in e&&e.onCancel(),n.stop()})).start();$($("reagenerics_table_ok").disabled?"reagenerics_table_cancel":"reagenerics_table_ok").focus()}},ui.onClick_ReaGenericsTableButton=function(){if(this.hasClassName("buttonDisabled"))return;ui.showReaGenericsTable({onOk:function(){ui.selectMode("atom_reagenerics")}})};