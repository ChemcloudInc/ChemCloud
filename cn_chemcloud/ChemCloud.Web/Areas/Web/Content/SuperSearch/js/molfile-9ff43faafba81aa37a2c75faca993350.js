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
if(!window.chem||!util.Vec2||!chem.Struct)throw new Error("Vec2 and Molecule should be defined first");chem.Molfile=function(){},chem.Molfile.loadRGroupFragments=!0,chem.Molfile.parseDecimalInt=function(e){var t=parseInt(e,10);return isNaN(t)?0:t},chem.Molfile.partitionLine=function(e,t,n){var r=[];for(var i=0,s=0;i<t.length;++i)r.push(e.slice(s,s+t[i])),n&&s++,s+=t[i];return r},chem.Molfile.partitionLineFixed=function(e,t,n){var r=[];for(var i=0;i<e.length;i+=t)r.push(e.slice(i,i+t)),n&&i++;return r},chem.Molfile.parseCTFile=function(e){var t=null;return e[0].search("\\$RXN")==0?t=chem.Molfile.parseRxn(e):t=chem.Molfile.parseMol(e),t.initHalfBonds(),t.initNeighbors(),t.markFragments(),t},chem.Molfile.fmtInfo={bondTypeMap:{1:chem.Struct.BOND.TYPE.SINGLE,2:chem.Struct.BOND.TYPE.DOUBLE,3:chem.Struct.BOND.TYPE.TRIPLE,4:chem.Struct.BOND.TYPE.AROMATIC,5:chem.Struct.BOND.TYPE.SINGLE_OR_DOUBLE,6:chem.Struct.BOND.TYPE.SINGLE_OR_AROMATIC,7:chem.Struct.BOND.TYPE.DOUBLE_OR_AROMATIC,8:chem.Struct.BOND.TYPE.ANY},bondStereoMap:{0:chem.Struct.BOND.STEREO.NONE,1:chem.Struct.BOND.STEREO.UP,4:chem.Struct.BOND.STEREO.EITHER,6:chem.Struct.BOND.STEREO.DOWN,3:chem.Struct.BOND.STEREO.CIS_TRANS},v30bondStereoMap:{0:chem.Struct.BOND.STEREO.NONE,1:chem.Struct.BOND.STEREO.UP,2:chem.Struct.BOND.STEREO.EITHER,3:chem.Struct.BOND.STEREO.DOWN},bondTopologyMap:{0:chem.Struct.BOND.TOPOLOGY.EITHER,1:chem.Struct.BOND.TOPOLOGY.RING,2:chem.Struct.BOND.TOPOLOGY.CHAIN},countsLinePartition:[3,3,3,3,3,3,3,3,3,3,3,6],atomLinePartition:[10,10,10,1,3,2,3,3,3,3,3,3,3,3,3,3,3],bondLinePartition:[3,3,3,3,3,3,3],atomListHeaderPartition:[3,1,1,4,1,1],atomListHeaderLength:11,atomListHeaderItemLength:4,chargeMap:[0,3,2,1,0,-1,-2,-3],valenceMap:[undefined,1,2,3,4,5,6,7,8,9,10,11,12,13,14,0],implicitHydrogenMap:[undefined,0,1,2,3,4],v30atomPropMap:{CHG:"charge",RAD:"radical",MASS:"isotope",VAL:"valence",HCOUNT:"hCount",INVRET:"invRet",SUBST:"substitutionCount",UNSAT:"unsaturatedAtom",RBCNT:"ringBondCount"},rxnItemsPartition:[3,3,3]},chem.Molfile.parseAtomLine=function(e){var t=chem.Molfile,n=t.partitionLine(e,t.fmtInfo.atomLinePartition),r={pp:new util.Vec2(parseFloat(n[0]),-parseFloat(n[1])),label:n[4].strip(),valence:t.fmtInfo.valenceMap[t.parseDecimalInt(n[10])],massDifference:t.parseDecimalInt(n[5]),charge:t.fmtInfo.chargeMap[t.parseDecimalInt(n[6])],hCount:t.parseDecimalInt(t.parseDecimalInt(n[8])),stereoCare:t.parseDecimalInt(n[9])!=0,aam:t.parseDecimalInt(n[14]),invRet:t.parseDecimalInt(n[15]),exactChangeFlag:t.parseDecimalInt(n[16])!=0};return r.explicitValence=typeof r.valence!="undefined",new chem.Struct.Atom(r)},chem.Molfile.stripV30=function(e){if(e.slice(0,7)!="M  V30 ")throw Error("Prefix invalid");return e.slice(7)},chem.Molfile.parseAtomLineV3000=function(e){var t=chem.Molfile,n,r,i,s,o;n=t.spaceparsplit(e);var u={pp:new util.Vec2(parseFloat(n[2]),-parseFloat(n[3])),aam:n[5].strip()},a=n[1].strip();a.charAt(0)=='"'&&a.charAt(a.length-1)=='"'&&(a=a.substr(1,a.length-2));if(a.charAt(a.length-1)=="]"){a=a.substr(0,a.length-1);var f={};f.notList=!1;if(a.substr(0,5)=="NOT [")f.notList=!0,a=a.substr(5);else{if(a.charAt(0)!="[")throw"Error: atom list expected, found '"+a+"'";a=a.substr(1)}f.ids=t.labelsListToIds(a.split(",")),u.atomList=new chem.Struct.AtomList(f),u.label=""}else u.label=a;n.splice(0,6);for(o=0;o<n.length;++o){r=t.splitonce(n[o],"="),i=r[0],s=r[1];if(i in t.fmtInfo.v30atomPropMap){var l=t.parseDecimalInt(s);if(i=="VAL"){if(l==0)continue;l==-1&&(l=0)}u[t.fmtInfo.v30atomPropMap[i]]=l}else if(i=="RGROUPS"){s=s.strip().substr(1,s.length-2);var c=s.split(" ").slice(1);u.rglabel=0;for(var h=0;h<c.length;++h)u.rglabel|=1<<c[h]-1}else i=="ATTCHPT"&&(u.attpnt=s.strip()-0)}return u.explicitValence=typeof u.valence!="undefined",new chem.Struct.Atom(u)},chem.Molfile.parseBondLineV3000=function(e){var t=chem.Molfile,n,r,i,s,o;n=t.spaceparsplit(e);var u={begin:t.parseDecimalInt(n[2])-1,end:t.parseDecimalInt(n[3])-1,type:t.fmtInfo.bondTypeMap[t.parseDecimalInt(n[1])]};n.splice(0,4);for(o=0;o<n.length;++o)r=t.splitonce(n[o],"="),i=r[0],s=r[1],i=="CFG"?u.stereo=t.fmtInfo.v30bondStereoMap[t.parseDecimalInt(s)]:i=="TOPO"?u.topology=t.fmtInfo.bondTopologyMap[t.parseDecimalInt(s)]:i=="RXCTR"?u.reactingCenterStatus=t.parseDecimalInt(s):i=="STBOX"&&(u.stereoCare=t.parseDecimalInt(s));return new chem.Struct.Bond(u)},chem.Molfile.parseBondLine=function(e){var t=chem.Molfile,n=t.partitionLine(e,t.fmtInfo.bondLinePartition),r={begin:t.parseDecimalInt(n[0])-1,end:t.parseDecimalInt(n[1])-1,type:t.fmtInfo.bondTypeMap[t.parseDecimalInt(n[2])],stereo:t.fmtInfo.bondStereoMap[t.parseDecimalInt(n[3])],topology:t.fmtInfo.bondTopologyMap[t.parseDecimalInt(n[5])],reactingCenterStatus:t.parseDecimalInt(n[6])};return new chem.Struct.Bond(r)},chem.Molfile.parseAtomListLine=function(e){var t=chem.Molfile,n=t.partitionLine(e,t.fmtInfo.atomListHeaderPartition),r=t.parseDecimalInt(n[0])-1,i=n[2].strip()=="T",s=t.parseDecimalInt(n[4].strip()),o=e.slice(t.fmtInfo.atomListHeaderLength),u=[],a=t.fmtInfo.atomListHeaderItemLength;for(var f=0;f<s;++f)u[f]=t.parseDecimalInt(o.slice(f*a,(f+1)*a-1));return{aid:r,atomList:new chem.Struct.AtomList({notList:i,ids:u})}},chem.Molfile.readKeyValuePairs=function(e,t){var n=chem.Molfile,r={},i=n.partitionLineFixed(e,3,!0),s=n.parseDecimalInt(i[0]);for(var o=0;o<s;++o)r[n.parseDecimalInt(i[2*o+1])-1]=t?i[2*o+2].strip():n.parseDecimalInt(i[2*o+2]);return r},chem.Molfile.readKeyMultiValuePairs=function(e,t){var n=chem.Molfile,r=[],i=n.partitionLineFixed(e,3,!0),s=n.parseDecimalInt(i[0]);for(var o=0;o<s;++o)r.push([n.parseDecimalInt(i[2*o+1])-1,t?i[2*o+2].strip():n.parseDecimalInt(i[2*o+2])]);return r},chem.Molfile.labelsListToIds=function(e){var t=[];for(var n=0;n<e.length;++n)t.push(chem.Element.getElementByLabel(e[n].strip()));return t},chem.Molfile.parsePropertyLineAtomList=function(e,t){var n=chem.Molfile,r=n.parseDecimalInt(e[1])-1,i=n.parseDecimalInt(e[2]),s=e[4].strip()=="T",o=n.labelsListToIds(t.slice(0,i)),u={};return u[r]=new chem.Struct.AtomList({notList:s,ids:o}),u},chem.Molfile.initSGroup=function(e,t){var n=chem.Molfile,r=n.readKeyValuePairs(t,!0);for(var i in r){var s=r[i];if(!(s in chem.SGroup.TYPES))throw new Error("Unsupported S-group type");var o=new chem.SGroup(s);o.number=i,e[i]=o}},chem.Molfile.applySGroupProp=function(e,t,n,r){var i=chem.Molfile,s=i.readKeyValuePairs(n,!r);for(var o in s)e[o].data[t]=s[o]},chem.Molfile.toIntArray=function(e){var t=chem.Molfile,n=[];for(var r=0;r<e.length;++r)n[r]=t.parseDecimalInt(e[r]);return n},chem.Molfile.applySGroupArrayProp=function(e,t,n,r){var i=chem.Molfile,s=i.parseDecimalInt(n.slice(1,4))-1,o=i.parseDecimalInt(n.slice(4,8)),u=i.toIntArray(i.partitionLineFixed(n.slice(8),3,!0));if(u.length!=o)throw new Error("File format invalid");r&&util.apply(u,function(e){return e+r}),e[s][t]=e[s][t].concat(u)},chem.Molfile.applyDataSGroupName=function(e,t){e.data.fieldName=t},chem.Molfile.applyDataSGroupQuery=function(e,t){e.data.query=t},chem.Molfile.applyDataSGroupQueryOp=function(e,t){e.data.queryOp=t},chem.Molfile.applyDataSGroupDesc=function(e,t){var n=chem.Molfile,r=n.partitionLine(t,[4,31,2,20,2,3],!1),i=n.parseDecimalInt(r[0])-1,s=r[1].strip(),o=r[2].strip(),u=r[3].strip(),a=r[4].strip(),f=r[5].strip(),l=e[i];l.data.fieldType=o,l.data.fieldName=s,l.data.units=u,l.data.query=a,l.data.queryOp=f},chem.Molfile.applyDataSGroupInfo=function(e,t){var n=chem.Molfile,r=n.partitionLine(t,[10,10,4,1,1,1,3,3,3,3,2,3,2],!1),i=parseFloat(r[0]),s=parseFloat(r[1]),o=r[3].strip()=="A",u=r[4].strip()=="A",a=r[5].strip()=="U",f=r[7].strip();f=f=="ALL"?-1:n.parseDecimalInt(f);var l=r[10].strip(),c=n.parseDecimalInt(r[11].strip());e.pp=new util.Vec2(i,-s),e.data.attached=o,e.data.absolute=u,e.data.showUnits=a,e.data.nCharsToDisplay=f,e.data.tagChar=l,e.data.daspPos=c},chem.Molfile.applyDataSGroupInfoLine=function(e,t){var n=chem.Molfile,r=n.parseDecimalInt(t.substr(0,4))-1,i=e[r];n.applyDataSGroupInfo(i,t.substr(5))},chem.Molfile.applyDataSGroupData=function(e,t,n){e.data.fieldValue=(e.data.fieldValue||"")+t,n&&(e.data.fieldValue=util.stripRight(e.data.fieldValue),e.data.fieldValue.startsWith('"')&&e.data.fieldValue.endsWith('"')&&(e.data.fieldValue=e.data.fieldValue.substr(1,e.data.fieldValue.length-2)))},chem.Molfile.applyDataSGroupDataLine=function(e,t,n){var r=chem.Molfile,i=r.parseDecimalInt(t.substr(0,5))-1,s=t.substr(5),o=e[i];r.applyDataSGroupData(o,s,n)},chem.Molfile.parsePropertyLines=function(e,t,n,r,i,s){var o=chem.Molfile,u=new util.Map;while(n<r){var a=t[n];if(a.charAt(0)=="A")u.get("label")||u.set("label",new util.Map),u.get("label").set(o.parseDecimalInt(a.slice(3,6))-1,t[++n]);else if(a.charAt(0)=="M"){var f=a.slice(3,6),l=a.slice(6);if(f=="END")break;if(f=="CHG")u.get("charge")||u.set("charge",new util.Map),u.get("charge").update(o.readKeyValuePairs(l));else if(f=="RAD")u.get("radical")||u.set("radical",new util.Map),u.get("radical").update(o.readKeyValuePairs(l));else if(f=="ISO")u.get("isotope")||u.set("isotope",new util.Map),u.get("isotope").update(o.readKeyValuePairs(l));else if(f=="RBC")u.get("ringBondCount")||u.set("ringBondCount",new util.Map),u.get("ringBondCount").update(o.readKeyValuePairs(l));else if(f=="SUB")u.get("substitutionCount")||u.set("substitutionCount",new util.Map),u.get("substitutionCount").update(o.readKeyValuePairs(l));else if(f=="UNS")u.get("unsaturatedAtom")||u.set("unsaturatedAtom",new util.Map),u.get("unsaturatedAtom").update(o.readKeyValuePairs(l));else if(f=="RGP"){u.get("rglabel")||u.set("rglabel",new util.Map);var c=u.get("rglabel"),h=o.readKeyMultiValuePairs(l);for(var p=0;p<h.length;p++){var d=h[p];c.set(d[0],(c.get(d[0])||0)|1<<d[1]-1)}}else if(f=="LOG"){l=l.slice(4);var v=o.parseDecimalInt(l.slice(0,3).strip()),m=o.parseDecimalInt(l.slice(4,7).strip()),g=o.parseDecimalInt(l.slice(8,11).strip()),y=l.slice(12).strip(),b={};m>0&&(b.ifthen=m),b.resth=g==1,b.range=y,s[v]=b}else if(f=="APO")u.get("attpnt")||u.set("attpnt",new util.Map),u.get("attpnt").update(o.readKeyValuePairs(l));else if(f=="ALS")u.get("atomList")||u.set("atomList",new util.Map),u.get("atomList").update(o.parsePropertyLineAtomList(o.partitionLine(l,[1,3,3,1,1,1]),o.partitionLineFixed(l.slice(10),4,!1)));else if(f=="STY")o.initSGroup(i,l);else if(f=="SST")o.applySGroupProp(i,"subtype",l);else if(f=="SLB")o.applySGroupProp(i,"label",l,!0);else if(f=="SCN")o.applySGroupProp(i,"connectivity",l);else if(f=="SAL")o.applySGroupArrayProp(i,"atoms",l,-1);else if(f=="SBL")o.applySGroupArrayProp(i,"bonds",l,-1);else if(f=="SPA")o.applySGroupArrayProp(i,"patoms",l,-1);else if(f=="SMT"){var w=o.parseDecimalInt(l.slice(0,4))-1;i[w].data.subscript=l.slice(4).strip()}else f=="SDT"?o.applyDataSGroupDesc(i,l):f=="SDD"?o.applyDataSGroupInfoLine(i,l):f=="SCD"?o.applyDataSGroupDataLine(i,l,!1):f=="SED"&&o.applyDataSGroupDataLine(i,l,!0)}++n}return u},chem.Molfile.applyAtomProp=function(e,t,n,r){t.each(function(t,r){e.get(t)[n]=r})},chem.Molfile.parseCTabV2000=function(e,t){var n=new chem.Struct,r,i=chem.Molfile,s=i.parseDecimalInt(t[0]),o=i.parseDecimalInt(t[1]),u=i.parseDecimalInt(t[2]);n.isChiral=i.parseDecimalInt(t[4])!=0;var a=i.parseDecimalInt(t[5]),f=i.parseDecimalInt(t[10]),l=0,c=e.slice(l,l+s);l+=s;var h=e.slice(l,l+o);l+=o;var p=e.slice(l,l+u);l+=u+a;var d=c.map(i.parseAtomLine);for(r=0;r<d.length;++r)n.atoms.add(d[r]);var v=h.map(i.parseBondLine);for(r=0;r<v.length;++r)n.bonds.add(v[r]);var m=p.map(i.parseAtomListLine);m.each(function(e){n.atoms.get(e.aid).atomList=e.atomList,n.atoms.get(e.aid).label=""});var g={},y={},b=i.parsePropertyLines(n,e,l,Math.min(e.length,l+f),g,y);b.each(function(e,t){i.applyAtomProp(n.atoms,t,e)});var w={},E;for(E in g)chem.SGroup.addGroup(n,g[E],w);var S=[];for(E in g)chem.SGroup.filter(n,g[E],w),g[E].atoms.length==0&&!g[E].allAtoms&&S.push(E);for(r=0;r<S.length;++r)n.sgroups.remove(S[r]);for(var x in y)n.rgroups.set(x,new chem.Struct.RGroup(y[x]));return n},chem.Molfile.spaceparsplit=function(e){var t=[],n=0,r,i,s=-1,o=e.toArray(),u=!1;for(i=0;i<e.length;++i)r=o[i],r=="("?n++:r==")"&&n--,r=='"'&&(u=!u),!u&&o[i]==" "&&n==0&&(i>s+1&&t.push(e.slice(s+1,i)),s=i);return i>s+1&&t.push(e.slice(s+1,i)),s=i,t},chem.Molfile.splitonce=function(e,t){var n=e.indexOf(t);return[e.slice(0,n),e.slice(n+1)]},chem.Molfile.splitSGroupDef=function(e){var t=[],n=0,r=!1;for(var i=0;i<e.length;++i){var s=e.charAt(i);s=='"'?r=!r:r||(s=="("?n++:s==")"?n--:s==" "&&n==0&&(t.push(e.slice(0,i)),e=e.slice(i+1).strip(),i=0))}if(n!=0)throw"Brace balance broken. S-group properies invalid!";return e.length>0&&t.push(e.strip()),t},chem.Molfile.parseBracedNumberList=function(e,t){if(!e)return null;var n=[];e=e.strip(),e=e.substr(1,e.length-2);var r=e.split(" ");t=t||0;for(var i=1;i<r.length;++i)n.push(r[i]-0+t);return n},chem.Molfile.v3000parseCollection=function(e,t,n){n++;while(t[n].strip()!="M  V30 END COLLECTION")n++;return n++,n},chem.Molfile.v3000parseSGroup=function(e,t,n,r,i){var s=chem.Molfile,o="";i++;while(i<t.length){o=s.stripV30(t[i++]).strip();if(o.strip()=="END SGROUP")return i;while(o[o.length-1]=="-")o=(o.substr(0,o.length-1)+s.stripV30(t[i++])).strip();var u=s.splitSGroupDef(o),a=u[1],f=new chem.SGroup(a);f.number=u[0]-0,f.type=a,f.label=u[2]-0,n[f.number]=f;var l={};for(var c=3;c<u.length;++c){var h=s.splitonce(u[c],"=");if(h.length!=2)throw"A record of form AAA=BBB or AAA=(...) expected, got '"+u[c]+"'";var p=h[0];p in l||(l[p]=[]),l[p].push(h[1])}f.atoms=s.parseBracedNumberList(l.ATOMS[0],-1),l.PATOMS&&(f.patoms=s.parseBracedNumberList(l.PATOMS[0],-1)),f.bonds=l.BONDS?s.parseBracedNumberList(l.BONDS[0],-1):[];var d=l.BRKXYZ;f.brkxyz=[];if(d)for(var v=0;v<d.length;++v)f.brkxyz.push(s.parseBracedNumberList(d[v]));l.MULT&&(f.data.subscript=l.MULT[0]-0),l.LABEL&&(f.data.subscript=l.LABEL[0].strip()),l.CONNECT&&(f.data.connectivity=l.CONNECT[0].toLowerCase()),l.FIELDDISP&&s.applyDataSGroupInfo(f,util.stripQuotes(l.FIELDDISP[0])),l.FIELDDATA&&s.applyDataSGroupData(f,l.FIELDDATA[0],!0),l.FIELDNAME&&s.applyDataSGroupName(f,l.FIELDNAME[0]),l.QUERYTYPE&&s.applyDataSGroupQuery(f,l.QUERYTYPE[0]),l.QUERYOP&&s.applyDataSGroupQueryOp(f,l.QUERYOP[0]),chem.SGroup.addGroup(e,f,r)}throw new Error("S-group declaration incomplete.")},chem.Molfile.parseCTabV3000=function(e,t){var n=new chem.Struct,r=chem.Molfile,i=0;if(e[i++].strip()!="M  V30 BEGIN CTAB")throw Error("CTAB V3000 invalid");if(e[i].slice(0,13)!="M  V30 COUNTS")throw Error("CTAB V3000 invalid");var s=e[i].slice(14).split(" ");n.isChiral=r.parseDecimalInt(s[4])==1,i++;if(e[i].strip()=="M  V30 BEGIN ATOM"){i++;var o;while(i<e.length){o=r.stripV30(e[i++]).strip();if(o=="END ATOM")break;while(o[o.length-1]=="-")o=(o.substring(0,o.length-1)+r.stripV30(e[i++])).strip();n.atoms.add(r.parseAtomLineV3000(o))}if(e[i].strip()=="M  V30 BEGIN BOND"){i++;while(i<e.length){o=r.stripV30(e[i++]).strip();if(o=="END BOND")break;while(o[o.length-1]=="-")o=(o.substring(0,o.length-1)+r.stripV30(e[i++])).strip();n.bonds.add(r.parseBondLineV3000(o))}}var u={},a={};while(e[i].strip()!="M  V30 END CTAB")if(e[i].strip()=="M  V30 BEGIN COLLECTION")i=r.v3000parseCollection(n,e,i);else{if(e[i].strip()!="M  V30 BEGIN SGROUP")throw Error("CTAB V3000 invalid");i=r.v3000parseSGroup(n,e,u,a,i)}}if(e[i++].strip()!="M  V30 END CTAB")throw Error("CTAB V3000 invalid");return t||r.readRGroups3000(n,e.slice(i)),n},chem.Molfile.readRGroups3000=function(e,t){var n={},r={},i=0,s=chem.Molfile;while(i<t.length&&t[i].search("M  V30 BEGIN RGROUP")==0){var o=t[i++].split(" ").pop();n[o]=[],r[o]={};for(;;){var u=t[i].strip();if(u.search("M  V30 RLOGIC")==0){u=u.slice(13);var a=u.strip().split(/\s+/g),f=s.parseDecimalInt(a[0]),l=s.parseDecimalInt(a[1]),c=a.slice(2).join(" "),h={};f>0&&(h.ifthen=f),h.resth=l==1,h.range=c,r[o]=h,i++;continue}if(u!="M  V30 BEGIN CTAB")throw Error("CTAB V3000 invalid");for(var p=0;p<t.length;++p)if(t[i+p].strip()=="M  V30 END CTAB")break;var d=t.slice(i,i+p+1),v=this.parseCTabV3000(d,!0);n[o].push(v),i=i+p+1;if(t[i].strip()=="M  V30 END RGROUP"){i++;break}}}for(var m in n)for(var g=0;g<n[m].length;++g){var y=n[m][g];y.rgroups.set(m,new chem.Struct.RGroup(r[m]));var b=y.frags.add(new chem.Struct.Fragment);y.rgroups.get(m).frags.add(b),y.atoms.each(function(e,t){t.fragment=b}),y.mergeInto(e)}},chem.Molfile.parseMol=function(e){return e[0].search("\\$MDL")==0?this.parseRg2000(e):(e=e.slice(3),this.parseCTab(e))},chem.Molfile.parseCTab=function(e){var t=chem.Molfile,n=t.partitionLine(e[0],t.fmtInfo.countsLinePartition),r=n[11].strip();e=e.slice(1);if(r=="V2000")return this.parseCTabV2000(e,n);if(r=="V3000")return this.parseCTabV3000(e,!chem.Molfile.loadRGroupFragments);throw Error("Molfile version unknown: "+r)},chem.MolfileSaver=function(e){this.molecule=null,this.molfile=null,this.v3000=e||!1},chem.MolfileSaver.prototype.prepareSGroups=function(e){var t=this.molecule,n=t.sgroups,r=[];n.each(function(n,i){try{i.prepareForSaving(t)}catch(s){if(!e||typeof s.id!="number")throw s;r.push(s.id)}}),r.length>0&&alert("WARNING: "+r.length.toString()+" invalid S-groups were detected. They will be omitted.");for(var i=0;i<r.length;++i)t.sGroupDelete(r[i]);return t},chem.MolfileSaver.getComponents=function(e){var t=e.findConnectedComponents(!0),n=[],r=[],i=null;e.rxnArrows.each(function(e,t){i=t.pp.x}),e.rxnPluses.each(function(e,t){r.push(t.pp.x)}),i!=null&&r.push(i),r.sort(function(e,t){return e-t});var s=[],o;for(o=0;o<t.length;++o){var u=e.getCoordBoundingBox(t[o]),a=util.Vec2.lc2(u.min,.5,u.max,.5),f=0;while(a.x>r[f])++f;s[f]=s[f]||{},util.Set.mergeIn(s[f],t[o])}var l=[],c=[],h=[];for(o=0;o<s.length;++o){if(!s[o]){l.push("");continue}u=e.getCoordBoundingBox(s[o]),a=util.Vec2.lc2(u.min,.5,u.max,.5),a.x<i?c.push(s[o]):h.push(s[o])}return{reactants:c,products:h}},chem.MolfileSaver.prototype.getCTab=function(e,t){return this.molecule=e.clone(),this.molfile="",this.writeCTab2000(t),this.molfile},chem.MolfileSaver.prototype.saveMolecule=function(e,t,n){this.reaction=e.rxnArrows.count()>0;if(e.rxnArrows.count()>1)throw new Error("Reaction may not contain more than one arrow");this.molfile="";if(this.reaction){e.rgroups.count()>0&&alert("Reactions with r-groups are not supported at the moment. R-fragments will be discarded in saving");var r=chem.MolfileSaver.getComponents(e),i=r.reactants,s=r.products,o=i.concat(s);this.molfile="$RXN\n\n\n\n"+util.paddedInt(i.length,3)+util.paddedInt(s.length,3)+util.paddedInt(0,3)+"\n";for(var u=0;u<o.length;++u){var a=new chem.MolfileSaver(!1),f=e.clone(o[u],null,!0),l=a.saveMolecule(f,!1,!0);this.molfile+="$MOL\n"+l}return this.molfile}if(e.rgroups.count()>0){if(!n){var c=(new chem.MolfileSaver(!1)).getCTab(e.getScaffold(),e.rgroups);return this.molfile="$MDL  REV  1\n$MOL\n$HDR\n\n\n\n$END HDR\n",this.molfile+="$CTAB\n"+c+"$END CTAB\n",e.rgroups.each(function(t,n){this.molfile+="$RGP\n",this.writePaddedNumber(t,3),this.molfile+="\n",n.frags.each(function(t,n){var r=(new chem.MolfileSaver(!1)).getCTab(e.getFragment(n));this.molfile+="$CTAB\n"+r+"$END CTAB\n"},this),this.molfile+="$END RGP\n"},this),this.molfile+="$END MOL\n",this.molfile}e=e.getScaffold()}return this.molecule=e.clone(),this.prepareSGroups(t),this.writeHeader(),this.writeCTab2000(),this.molfile},chem.MolfileSaver.prototype.writeHeader=function(){var e=new Date;this.writeCR(),this.writeWhiteSpace(2),this.write("Ketcher"),this.writeWhiteSpace(),this.writeCR((e.getMonth()+1).toPaddedString(2)+e.getDate().toPaddedString(2)+(e.getFullYear()%100).toPaddedString(2)+e.getHours().toPaddedString(2)+e.getMinutes().toPaddedString(2)+"2D 1   1.00000     0.00000     0"),this.writeCR()},chem.MolfileSaver.prototype.write=function(e){this.molfile+=e},chem.MolfileSaver.prototype.writeCR=function(e){arguments.length==0&&(e=""),this.molfile+=e+"\n"},chem.MolfileSaver.prototype.writeWhiteSpace=function(e){arguments.length==0&&(e=1),e.times(function(){this.write(" ")},this)},chem.MolfileSaver.prototype.writePadded=function(e,t){this.write(e),this.writeWhiteSpace(t-e.length)},chem.MolfileSaver.prototype.writePaddedNumber=function(e,t){var n=(e-0).toString();this.writeWhiteSpace(t-n.length),this.write(n)},chem.MolfileSaver.prototype.writePaddedFloat=function(e,t,n){this.write(util.paddedFloat(e,t,n))},chem.MolfileSaver.prototype.writeCTab2000Header=function(){this.writePaddedNumber(this.molecule.atoms.count(),3),this.writePaddedNumber(this.molecule.bonds.count(),3),this.writePaddedNumber(0,3),this.writeWhiteSpace(3),this.writePaddedNumber(this.molecule.isChiral?1:0,3),this.writePaddedNumber(0,3),this.writeWhiteSpace(12),this.writePaddedNumber(999,3),this.writeCR(" V2000")},chem.MolfileSaver.prototype.writeCTab2000=function(e){this.writeCTab2000Header(),this.mapping={};var t=1,n=[],r=[];this.molecule.atoms.each(function(e,i){this.writePaddedFloat(i.pp.x,10,4),this.writePaddedFloat(-i.pp.y,10,4),this.writePaddedFloat(0,10,4),this.writeWhiteSpace();var s=i.label;i.atomList!=null?(s="L",n.push(e)):chem.Element.getElementByLabel(s)==null&&["A","Q","X","*","R#"].indexOf(s)==-1&&(s="C",r.push(e)),this.writePadded(s,3),this.writePaddedNumber(0,2),this.writePaddedNumber(0,3),this.writePaddedNumber(0,3),Object.isUndefined(i.hCount)&&(i.hCount=0),this.writePaddedNumber(i.hCount,3),Object.isUndefined(i.stereoCare)&&(i.stereoCare=0),this.writePaddedNumber(i.stereoCare,3),this.writePaddedNumber(i.explicitValence?i.valence==0?15:i.valence:0,3),this.writePaddedNumber(0,3),this.writePaddedNumber(0,3),this.writePaddedNumber(0,3),Object.isUndefined(i.aam)&&(i.aam=0),this.writePaddedNumber(i.aam,3),Object.isUndefined(i.invRet)&&(i.invRet=0),this.writePaddedNumber(i.invRet,3),Object.isUndefined(i.exactChangeFlag)&&(i.exactChangeFlag=0),this.writePaddedNumber(i.exactChangeFlag,3),this.writeCR(),this.mapping[e]=t,t++},this),this.bondMapping={},t=1,this.molecule.bonds.each(function(e,n){this.bondMapping[e]=t++,this.writePaddedNumber(this.mapping[n.begin],3),this.writePaddedNumber(this.mapping[n.end],3),this.writePaddedNumber(n.type,3),Object.isUndefined(n.stereo)&&(n.stereo=0),this.writePaddedNumber(n.stereo,3),this.writeWhiteSpace(3),Object.isUndefined(n.topology)&&(n.topology=0),this.writePaddedNumber(n.topology,3),Object.isUndefined(n.reactingCenterStatus)&&(n.reactingCenterStatus=0),this.writePaddedNumber(n.reactingCenterStatus,3),this.writeCR()},this);while(r.length>0)this.write("A  "),this.writePaddedNumber(r[0]+1,3),this.writeCR(),this.writeCR(this.molecule.atoms.get(r[0]).label),r.splice(0,1);var i=new Array,s=new Array,o=new Array,u=new Array,a=new Array,f=new Array,l=new Array,c=new Array,h=new Array;this.molecule.atoms.each(function(e,t){t.charge!=0&&i.push([e,t.charge]),t.isotope!=0&&s.push([e,t.isotope]),t.radical!=0&&o.push([e,t.radical]);if(t.rglabel!=null&&t.label=="R#")for(var n=0;n<32;n++)t.rglabel&1<<n&&u.push([e,n+1]);t.attpnt!=null&&f.push([e,t.attpnt]),t.ringBondCount!=0&&l.push([e,t.ringBondCount]),t.substitutionCount!=0&&h.push([e,t.substitutionCount]),t.unsaturatedAtom!=0&&c.push([e,t.unsaturatedAtom])}),e&&e.each(function(e,t){if(t.resth||t.ifthen>0||t.range.length>0){var n="  1 "+util.paddedInt(e,3)+" "+util.paddedInt(t.ifthen,3)+" "+util.paddedInt(t.resth?1:0,3)+" "+t.range;a.push(n)}});var p=function(e,t){while(t.length>0){var n=new Array;while(t.length>0&&n.length<8)n.push(t[0]),t.splice(0,1);this.write(e),this.writePaddedNumber(n.length,3),n.each(function(e){this.writeWhiteSpace(),this.writePaddedNumber(this.mapping[e[0]],3),this.writeWhiteSpace(),this.writePaddedNumber(e[1],3)},this),this.writeCR()}};p.call(this,"M  CHG",i),p.call(this,"M  ISO",s),p.call(this,"M  RAD",o),p.call(this,"M  RGP",u);for(var d=0;d<a.length;++d)this.write("M  LOG"+a[d]+"\n");p.call(this,"M  APO",f),p.call(this,"M  RBC",l),p.call(this,"M  SUB",h),p.call(this,"M  UNS",c);if(n.length>0)for(d=0;d<n.length;++d){var v=n[d],m=this.molecule.atoms.get(v).atomList;this.write("M  ALS"),this.writePaddedNumber(v+1,4),this.writePaddedNumber(m.ids.length,3),this.writeWhiteSpace(),this.write(m.notList?"T":"F");var g=m.labelList();for(var y=0;y<g.length;++y)this.writeWhiteSpace(),this.writePadded(g[y],3);this.writeCR()}var b={},w=1;this.molecule.sgroups.each(function(e){b[e]=w++});if(w>1){this.write("M  STY"),this.writePaddedNumber(w-1,3),this.molecule.sgroups.each(function(e,t){this.writeWhiteSpace(1),this.writePaddedNumber(b[e],3),this.writeWhiteSpace(1),this.writePadded(t.type,3)},this),this.writeCR(),this.write("M  SLB"),this.writePaddedNumber(w-1,3),this.molecule.sgroups.each(function(e,t){this.writeWhiteSpace(1),this.writePaddedNumber(b[e],3),this.writeWhiteSpace(1),this.writePaddedNumber(b[e],3)},this),this.writeCR();var E="",S=0;this.molecule.sgroups.each(function(e,t){t.type=="SRU"&&t.data.connectivity&&(E+=" ",E+=util.stringPadded(b[e].toString(),3),E+=" ",E+=util.stringPadded(t.data.connectivity,3,!0),S++)},this),S>0&&(this.write("M  SCN"),this.writePaddedNumber(S,3),this.write(E.toUpperCase()),this.writeCR()),this.molecule.sgroups.each(function(e,t){t.type=="SRU"&&(this.write("M  SMT "),this.writePaddedNumber(b[e],3),this.writeWhiteSpace(),this.write(t.data.subscript||"n"),this.writeCR())},this),this.molecule.sgroups.each(function(e,t){this.writeCR(t.saveToMolfile(this.molecule,b,this.mapping,this.bondMapping))},this)}this.writeCR("M  END")},chem.Molfile.parseRxn=function(e){var t=chem.Molfile,n=e[0].strip().split(" ");return n.length>1&&n[1]=="V3000"?t.parseRxn3000(e):t.parseRxn2000(e)},chem.Molfile.parseRxn2000=function(e){var t=chem.Molfile;e=e.slice(4);var n=t.partitionLine(e[0],t.fmtInfo.rxnItemsPartition),r=n[0]-0,i=n[1]-0,s=n[2]-0;e=e.slice(2);var o=new chem.Struct,u=[],a=0,f;for(f=0;f<e.length;++f)e[f].substr(0,4)=="$MOL"&&(f>a&&u.push(e.slice(a,f)),a=f+1);u.push(e.slice(a));var l=[];for(var c=0;c<u.length;++c){var h=chem.Molfile.parseMol(u[c]);l.push(h)}return t.rxnMerge(l,r,i,s)},chem.Molfile.parseRxn3000=function(e){var t=chem.Molfile;e=e.slice(4);var n=e[0].split(/\s+/g).slice(3),r=n[0]-0,i=n[1]-0,s=n.length>2?n[2]-0:0,o=function(e){if(!e)throw new Error("CTab format invalid")},u=function(t){for(var n=t;n<e.length;++n)if(e[n].strip()=="M  V30 END CTAB")return n;o(!1)},a=function(t){for(var n=t;n<e.length;++n)if(e[n].strip()=="M  V30 END RGROUP")return n;o(!1)},f=[],l=[],c=null,h=[];for(var p=0;p<e.length;++p){var d=e[p].strip();if(!d.startsWith("M  V30 COUNTS")){if(d=="M  END")break;if(d=="M  V30 BEGIN PRODUCT")o(c==null),c=l;else if(d=="M  V30 END PRODUCT")o(c===l),c=null;else if(d=="M  V30 BEGIN REACTANT")o(c==null),c=f;else if(d=="M  V30 END REACTANT")o(c===f),c=null;else if(d.startsWith("M  V30 BEGIN RGROUP")){o(c==null);var v=a(p);h.push(e.slice(p,v+1)),p=v}else{if(d!="M  V30 BEGIN CTAB")throw new Error("line unrecognized: "+d);var v=u(p);c.push(e.slice(p,v+1)),p=v}}}var m=[],g=f.concat(l);for(var v=0;v<g.length;++v){var y=chem.Molfile.parseCTabV3000(g[v],n);m.push(y)}var b=t.rxnMerge(m,r,i,s);return t.readRGroups3000(b,function(e){var t=[];for(var n=0;n<e.length;++n)t=t.concat(e[n]);return t}(h)),b},chem.Molfile.rxnMerge=function(e,t,n,r){var i=chem.Molfile,s=new chem.Struct,o=[],u=[],a=[],f=[],l=[],c=[],h,p={cnt:0,totalLength:0};for(h=0;h<e.length;++h){var d=e[h],v=d.getBondLengthData();p.cnt+=v.cnt,p.totalLength+=v.totalLength}var m=1/(p.cnt==0?1:p.totalLength/p.cnt);for(h=0;h<e.length;++h)d=e[h],d.scale(m);for(h=0;h<e.length;++h){d=e[h];var g=d.getCoordBoundingBoxObj();if(!g)continue;var y=h<t?chem.Struct.FRAGMENT.REACTANT:h<t+n?chem.Struct.FRAGMENT.PRODUCT:chem.Struct.FRAGMENT.AGENT;y==chem.Struct.FRAGMENT.REACTANT?(o.push(g),f.push(d)):y==chem.Struct.FRAGMENT.AGENT?(u.push(g),l.push(d)):y==chem.Struct.FRAGMENT.PRODUCT&&(a.push(g),c.push(d)),d.atoms.each(function(e,t){t.rxnFragmentType=y})}var b=0,w=function(e,t,n,r,i){var s=new util.Vec2(r-n.min.x,i?1-n.min.y:-(n.min.y+n.max.y)/2);return t.atoms.each(function(e,t){t.pp.add_(s)}),t.sgroups.each(function(e,t){t.pp&&t.pp.add_(s)}),n.min.add_(s),n.max.add_(s),t.mergeInto(e),n.max.x-n.min.x};for(h=0;h<f.length;++h)b+=w(s,f[h],o[h],b,!1)+2;b+=2;for(h=0;h<l.length;++h)b+=w(s,l[h],u[h],b,!0)+2;b+=2;for(h=0;h<c.length;++h)b+=w(s,c[h],a[h],b,!1)+2;var E,S,x,T,N=null,C=null;for(h=0;h<o.length-1;++h)E=o[h],S=o[h+1],x=(E.max.x+S.min.x)/2,T=(E.max.y+E.min.y+S.max.y+S.min.y)/4,s.rxnPluses.add(new chem.Struct.RxnPlus({pp:new util.Vec2(x,T)}));for(h=0;h<o.length;++h)h==0?(N={},N.max=new util.Vec2(o[h].max),N.min=new util.Vec2(o[h].min)):(N.max=util.Vec2.max(N.max,o[h].max),N.min=util.Vec2.min(N.min,o[h].min));for(h=0;h<a.length-1;++h)E=a[h],S=a[h+1],x=(E.max.x+S.min.x)/2,T=(E.max.y+E.min.y+S.max.y+S.min.y)/4,s.rxnPluses.add(new chem.Struct.RxnPlus({pp:new util.Vec2(x,T)}));for(h=0;h<a.length;++h)h==0?(C={},C.max=new util.Vec2(a[h].max),C.min=new util.Vec2(a[h].min)):(C.max=util.Vec2.max(C.max,a[h].max),C.min=util.Vec2.min(C.min,a[h].min));E=N,S=C;if(!E&&!S)throw new Error("reaction must contain at least one product or reactant");var k=E?new util.Vec2(E.max.x,(E.max.y+E.min.y)/2):null,L=S?new util.Vec2(S.min.x,(S.max.y+S.min.y)/2):null,A=3;k||(k=new util.Vec2(L.x-A,L.y)),L||(L=new util.Vec2(k.x+A,k.y));var O=util.Vec2.lc2(k,.5,L,.5);return s.rxnArrows.add(new chem.Struct.RxnArrow({pp:O})),s.isReaction=!0,s},chem.Molfile.rgMerge=function(e,t){var n=new chem.Struct;e.mergeInto(n,null,null,!1,!0);for(var r in t)for(var i=0;i<t[r].length;++i){var s=t[r][i];s.rgroups.set(r,new chem.Struct.RGroup);var o=s.frags.add(new chem.Struct.Fragment);s.rgroups.get(r).frags.add(o),s.atoms.each(function(e,t){t.fragment=o}),s.mergeInto(n)}return n},chem.Molfile.parseRg2000=function(e){var t=chem.Molfile;e=e.slice(7);if(e[0].strip()!="$CTAB")throw new Error("RGFile format invalid");var n=1;while(e[n][0]!="$")n++;if(e[n].strip()!="$END CTAB")throw new Error("RGFile format invalid");var r=e.slice(1,n);e=e.slice(n+1);var i={};for(;;){if(e.length==0)throw new Error("Unexpected end of file");var s=e[0].strip();if(s=="$END MOL"){e=e.slice(1);break}if(s!="$RGP")throw new Error("RGFile format invalid");var o=e[1].strip()-0;i[o]=[],e=e.slice(2);for(;;){if(e.length==0)throw new Error("Unexpected end of file");s=e[0].strip();if(s=="$END RGP"){e=e.slice(1);break}if(s!="$CTAB")throw new Error("RGFile format invalid");n=1;while(e[n][0]!="$")n++;if(e[n].strip()!="$END CTAB")throw new Error("RGFile format invalid");i[o].push(e.slice(1,n)),e=e.slice(n+1)}}var u=chem.Molfile.parseCTab(r),a={};if(chem.Molfile.loadRGroupFragments)for(var f in i){a[f]=[];for(var l=0;l<i[f].length;++l)a[f].push(chem.Molfile.parseCTab(i[f][l]))}return t.rgMerge(u,a)};