'use strict';var $jscomp=$jscomp||{};$jscomp.scope={};$jscomp.findInternal=function(b,f,m){b instanceof String&&(b=String(b));for(var p=b.length,l=0;l<p;l++){var t=b[l];if(f.call(m,t,l,b))return{i:l,v:t}}return{i:-1,v:void 0}};$jscomp.ASSUME_ES5=!1;$jscomp.ASSUME_NO_NATIVE_MAP=!1;$jscomp.ASSUME_NO_NATIVE_SET=!1;$jscomp.defineProperty=$jscomp.ASSUME_ES5||"function"==typeof Object.defineProperties?Object.defineProperty:function(b,f,m){b!=Array.prototype&&b!=Object.prototype&&(b[f]=m.value)};
$jscomp.getGlobal=function(b){return"undefined"!=typeof window&&window===b?b:"undefined"!=typeof global&&null!=global?global:b};$jscomp.global=$jscomp.getGlobal(this);$jscomp.polyfill=function(b,f,m,p){if(f){m=$jscomp.global;b=b.split(".");for(p=0;p<b.length-1;p++){var l=b[p];l in m||(m[l]={});m=m[l]}b=b[b.length-1];p=m[b];f=f(p);f!=p&&null!=f&&$jscomp.defineProperty(m,b,{configurable:!0,writable:!0,value:f})}};
$jscomp.polyfill("Array.prototype.find",function(b){return b?b:function(b,m){return $jscomp.findInternal(this,b,m).v}},"es6","es3");
(function(b){"object"==typeof exports&&"object"==typeof module?b(require("../../lib/codemirror")):"function"==typeof define&&define.amd?define(["../../lib/codemirror"],b):b(CodeMirror)})(function(b){function f(c,a){var d=c.flags;for(var e=d=null!=d?d:(c.ignoreCase?"i":"")+(c.global?"g":"")+(c.multiline?"m":""),g=0;g<a.length;g++)-1==e.indexOf(a.charAt(g))&&(e+=a.charAt(g));return d==e?c:new RegExp(c.source,e)}function m(c,a,d){a=f(a,"g");var e=d.line,g=d.ch;for(d=c.lastLine();e<=d;e++,g=0)if(a.lastIndex=
g,g=c.getLine(e),g=a.exec(g))return{from:k(e,g.index),to:k(e,g.index+g[0].length),match:g}}function p(c,a,d){if(!/\\s|\\n|\n|\\W|\\D|\[\^/.test(a.source))return m(c,a,d);a=f(a,"gm");for(var e,g=1,b=d.line,q=c.lastLine();b<=q;){for(var h=0;h<g&&!(b>q);h++){var n=c.getLine(b++);e=null==e?n:e+"\n"+n}g*=2;a.lastIndex=d.ch;if(h=a.exec(e))return a=e.slice(0,h.index).split("\n"),c=h[0].split("\n"),d=d.line+a.length-1,a=a[a.length-1].length,{from:k(d,a),to:k(d+c.length-1,1==c.length?a+c[0].length:c[c.length-
1].length),match:h}}}function l(c,a){for(var d=0,e;;){a.lastIndex=d;d=a.exec(c);if(!d)return e;e=d;d=e.index+(e[0].length||1);if(d==c.length)return e}}function t(c,a,d){a=f(a,"g");var e=d.line,g=d.ch;for(d=c.firstLine();e>=d;e--,g=-1){var b=c.getLine(e);-1<g&&(b=b.slice(0,g));if(g=l(b,a))return{from:k(e,g.index),to:k(e,g.index+g[0].length),match:g}}}function y(c,a,d){a=f(a,"gm");for(var e,g=1,b=d.line,q=c.firstLine();b>=q;){for(var h=0;h<g;h++){var n=c.getLine(b--);e=null==e?n.slice(0,d.ch):n+"\n"+
e}g*=2;if(h=l(e,a))return a=e.slice(0,h.index).split("\n"),c=h[0].split("\n"),b+=a.length,a=a[a.length-1].length,{from:k(b,a),to:k(b+c.length-1,1==c.length?a+c[0].length:c[c.length-1].length),match:h}}}function r(c,a,d,e){if(c.length==a.length)return d;var g=0;for(a=d+Math.max(0,c.length-a.length);;){if(g==a)return g;var b=g+a>>1,k=e(c.slice(0,b)).length;if(k==d)return b;k>d?a=b:g=b+1}}function z(c,a,d,e){if(!a.length)return null;e=e?u:v;a=e(a).split(/\r|\n\r?/);var b=d.line;d=d.ch;var x=c.lastLine()+
1-a.length;a:for(;b<=x;b++,d=0){var q=c.getLine(b).slice(d),h=e(q);if(1==a.length){var n=h.indexOf(a[0]);if(-1==n)continue a;r(q,h,n,e);return{from:k(b,r(q,h,n,e)+d),to:k(b,r(q,h,n+a[0].length,e)+d)}}n=h.length-a[0].length;if(h.slice(n)!=a[0])continue a;for(var f=1;f<a.length-1;f++)if(e(c.getLine(b+f))!=a[f])continue a;f=c.getLine(b+a.length-1);var m=e(f),l=a[a.length-1];if(m.slice(0,l.length)!=l)continue a;return{from:k(b,r(q,h,n,e)+d),to:k(b+a.length-1,r(f,m,l.length,e))}}}function A(c,a,d,b){if(!a.length)return null;
b=b?u:v;a=b(a).split(/\r|\n\r?/);var e=d.line,f=d.ch,m=c.firstLine()-1+a.length;a:for(;e>=m;e--,f=-1){var h=c.getLine(e);-1<f&&(h=h.slice(0,f));f=b(h);if(1==a.length){d=f.lastIndexOf(a[0]);if(-1==d)continue a;return{from:k(e,r(h,f,d,b)),to:k(e,r(h,f,d+a[0].length,b))}}var n=a[a.length-1];if(f.slice(0,n.length)!=n)continue a;var l=1;for(d=e-a.length+1;l<a.length-1;l++)if(b(c.getLine(d+l))!=a[l])continue a;d=c.getLine(e+1-a.length);l=b(d);if(l.slice(l.length-a[0].length)!=a[0])continue a;return{from:k(e+
1-a.length,r(d,l,d.length-a[0].length,b)),to:k(e,r(h,f,n.length,b))}}}function w(c,a,b,e){this.atOccurrence=!1;this.doc=c;b=b?c.clipPos(b):k(0,0);this.pos={from:b,to:b};if("object"==typeof e)var d=e.caseFold;else d=e,e=null;"string"==typeof a?(null==d&&(d=!1),this.matches=function(b,e){return(b?A:z)(c,a,e,d)}):(a=f(a,"gm"),this.matches=e&&!1===e.multiline?function(b,d){return(b?t:m)(c,a,d)}:function(b,d){return(b?y:p)(c,a,d)})}var k=b.Pos;if(String.prototype.normalize){var u=function(b){return b.normalize("NFD").toLowerCase()};
var v=function(b){return b.normalize("NFD")}}else u=function(b){return b.toLowerCase()},v=function(b){return b};w.prototype={findNext:function(){return this.find(!1)},findPrevious:function(){return this.find(!0)},find:function(c){for(var a=this.matches(c,this.doc.clipPos(c?this.pos.from:this.pos.to));a&&0==b.cmpPos(a.from,a.to);)c?a.from.ch?a.from=k(a.from.line,a.from.ch-1):a=a.from.line==this.doc.firstLine()?null:this.matches(c,this.doc.clipPos(k(a.from.line-1))):a.to.ch<this.doc.getLine(a.to.line).length?
a.to=k(a.to.line,a.to.ch+1):a=a.to.line==this.doc.lastLine()?null:this.matches(c,k(a.to.line+1,0));if(a)return this.pos=a,this.atOccurrence=!0,this.pos.match||!0;c=k(c?this.doc.firstLine():this.doc.lastLine()+1,0);this.pos={from:c,to:c};return this.atOccurrence=!1},from:function(){if(this.atOccurrence)return this.pos.from},to:function(){if(this.atOccurrence)return this.pos.to},replace:function(c,a){this.atOccurrence&&(c=b.splitLines(c),this.doc.replaceRange(c,this.pos.from,this.pos.to,a),this.pos.to=
k(this.pos.from.line+c.length-1,c[c.length-1].length+(1==c.length?this.pos.from.ch:0)))}};b.defineExtension("getSearchCursor",function(b,a,d){return new w(this.doc,b,a,d)});b.defineDocExtension("getSearchCursor",function(b,a,d){return new w(this,b,a,d)});b.defineExtension("selectMatches",function(c,a){var d=[];for(c=this.getSearchCursor(c,this.getCursor("from"),a);c.findNext()&&!(0<b.cmpPos(c.to(),this.getCursor("to")));)d.push({anchor:c.from(),head:c.to()});d.length&&this.setSelections(d,0)})});