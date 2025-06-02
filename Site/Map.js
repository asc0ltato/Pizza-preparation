(function (lib, img, cjs, ss) {

var p; // shortcut to reference prototypes
lib.webFontTxtInst = {}; 
var loadedTypekitCount = 0;
var loadedGoogleCount = 0;
var gFontsUpdateCacheList = [];
var tFontsUpdateCacheList = [];

// library properties:
lib.properties = {
	width: 550,
	height: 440,
	fps: 24,
	color: "#FFFFFF",
	opacity: 1.00,
	webfonts: {},
	manifest: [
		{src:"images/Pizza2.jpg", id:"Pizza2"},
		{src:"sounds/Колбаска.mp3", id:"Колбаска"},
		{src:"sounds/Помидорки.mp3", id:"Помидорки"},
		{src:"sounds/Скалка.mp3", id:"Скалка"},
		{src:"sounds/Сыр.mp3", id:"Сыр"},
		{src:"sounds/Тесто.mp3", id:"Тесто"},
		{src:"sounds/Томатный соус.mp3", id:"Томатный соус"},
		{src:"sounds/Черпак.mp3", id:"Черпак"}
	]
};



lib.ssMetadata = [];



lib.updateListCache = function (cacheList) {		
	for(var i = 0; i < cacheList.length; i++) {		
		if(cacheList[i].cacheCanvas)		
			cacheList[i].updateCache();		
	}		
};		

lib.addElementsToCache = function (textInst, cacheList) {		
	var cur = textInst;		
	while(cur != exportRoot) {		
		if(cacheList.indexOf(cur) != -1)		
			break;		
		cur = cur.parent;		
	}		
	if(cur != exportRoot) {	//we have found an element in the list		
		var cur2 = textInst;		
		var index = cacheList.indexOf(cur);		
		while(cur2 != cur) { //insert all it's children just before it		
			cacheList.splice(index, 0, cur2);		
			cur2 = cur2.parent;		
			index++;		
		}		
	}		
	else {	//append element and it's parents in the array		
		cur = textInst;		
		while(cur != exportRoot) {		
			cacheList.push(cur);		
			cur = cur.parent;		
		}		
	}		
};		

lib.gfontAvailable = function(family, totalGoogleCount) {		
	lib.properties.webfonts[family] = true;		
	var txtInst = lib.webFontTxtInst && lib.webFontTxtInst[family] || [];		
	for(var f = 0; f < txtInst.length; ++f)		
		lib.addElementsToCache(txtInst[f], gFontsUpdateCacheList);		

	loadedGoogleCount++;		
	if(loadedGoogleCount == totalGoogleCount) {		
		lib.updateListCache(gFontsUpdateCacheList);		
	}		
};		

lib.tfontAvailable = function(family, totalTypekitCount) {		
	lib.properties.webfonts[family] = true;		
	var txtInst = lib.webFontTxtInst && lib.webFontTxtInst[family] || [];		
	for(var f = 0; f < txtInst.length; ++f)		
		lib.addElementsToCache(txtInst[f], tFontsUpdateCacheList);		

	loadedTypekitCount++;		
	if(loadedTypekitCount == totalTypekitCount) {		
		lib.updateListCache(tFontsUpdateCacheList);		
	}		
};
// symbols:



(lib.Pizza2 = function() {
	this.initialize(img.Pizza2);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,1920,1080);


(lib.Черпак_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Черпак");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("Ai8F3IAArtIF6AAIAALtg");
	this.shape.setTransform(-1,-1.4);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Тесто_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Тесто");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("Am3G3Qi2i1AAkCQAAkBC2i2QC3i2EAAAQEBAAC3C2QC2C2AAEBQAAECi2C1Qi3C3kBAAQkAAAi3i3g");
	this.shape.setTransform(-0.7,-0.7);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Томатныйсоус_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Томатныйсоус");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("AqsHBIAAuCIVZAAIAAOCg");
	this.shape.setTransform(0.5,-0.9);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Скалка_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Скалка");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("AjqIhIAAxAIHVAAIAARAg");
	this.shape.setTransform(-0.5,-0.5);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Колбаска_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Колбаска");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("AoaEdIAAo5IQ2AAIAAI5g");
	this.shape.setTransform(-4,0.6);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Сыр_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Сыр");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("AsfF2IAArrIY/AAIAALrg");
	this.shape.setTransform(0,-4.4);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


(lib.Помидорки_1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// timeline functions:
	this.frame_2 = function() {
		playSound("Помидорки");
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).wait(2).call(this.frame_2).wait(2));

	// Слой 1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("ApqFTIAAqlITWAAIAAKlg");
	this.shape.setTransform(0,0.1);
	this.shape._off = true;

	this.timeline.addTween(cjs.Tween.get(this.shape).wait(3).to({_off:false},0).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = null;


// stage content:



(lib.Map = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Слой 1
	this.instance = new lib.Черпак_1();
	this.instance.parent = this;
	this.instance.setTransform(200.6,182.6,1.235,0.099,0,-53.5,-77.8,0.1,0.6);
	new cjs.ButtonHelper(this.instance, 0, 1, 2, false, new lib.Черпак_1(), 3);

	this.instance_1 = new lib.Томатныйсоус_1();
	this.instance_1.parent = this;
	this.instance_1.setTransform(153.9,196.8,0.633,0.358,0,11.5,0,0.1,0);
	new cjs.ButtonHelper(this.instance_1, 0, 1, 2, false, new lib.Томатныйсоус_1(), 3);

	this.instance_2 = new lib.Скалка_1();
	this.instance_2.parent = this;
	this.instance_2.setTransform(443,280.7,0.334,0.82,0,-10.7,-20.9,0.1,0.1);
	new cjs.ButtonHelper(this.instance_2, 0, 1, 2, false, new lib.Скалка_1(), 3);

	this.instance_3 = new lib.Сыр_1();
	this.instance_3.parent = this;
	this.instance_3.setTransform(406.3,201.5,0.302,0.328,0,-6.7,0,0.1,0.3);
	new cjs.ButtonHelper(this.instance_3, 0, 1, 2, false, new lib.Сыр_1(), 3);

	this.instance_4 = new lib.Помидорки_1();
	this.instance_4.parent = this;
	this.instance_4.setTransform(328.8,197.6,0.423,0.431,0,-4.2,0,0.1,0);
	new cjs.ButtonHelper(this.instance_4, 0, 1, 2, false, new lib.Помидорки_1(), 3);

	this.instance_5 = new lib.Колбаска_1();
	this.instance_5.parent = this;
	this.instance_5.setTransform(252.3,198.5,0.469,0.473,0,2.5,0,0.2,0.2);
	new cjs.ButtonHelper(this.instance_5, 0, 1, 2, false, new lib.Колбаска_1(), 3);

	this.instance_6 = new lib.Тесто_1();
	this.instance_6.parent = this;
	this.instance_6.setTransform(138.2,272.4,0.602,0.832,0,0,0,0.6,0.1);
	new cjs.ButtonHelper(this.instance_6, 0, 1, 2, false, new lib.Тесто_1(), 3);

	this.instance_7 = new lib.Pizza2();
	this.instance_7.parent = this;
	this.instance_7.setTransform(0,0,0.29,0.407);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.instance_7},{t:this.instance_6},{t:this.instance_5},{t:this.instance_4},{t:this.instance_3},{t:this.instance_2},{t:this.instance_1},{t:this.instance}]}).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(275,220,556.3,440);

})(lib = lib||{}, images = images||{}, createjs = createjs||{}, ss = ss||{});
var lib, images, createjs, ss;