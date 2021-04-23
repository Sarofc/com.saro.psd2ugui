// **************************************************
// This file created by Brett Bibby (c) 2010-2013
// You may freely use and modify this file as you see fit
// You may not sell it
//**************************************************
// hidden object game exporter
//$.writeln("=== Starting Debugging Session ===");

// enable double clicking from the Macintosh Finder or the Windows Explorer
#target photoshop

// debug level: 0-2 (0:disable, 1:break on error, 2:break at beginning)
// $.level = 0;
// debugger; // launch debugger on next line

var xml;
var srcPsd;
var dupPsd;
var dstFolder;
var uid;
var srcPsdName;

var validNameMap = new Array({ "": 1 }, { "": 2 });

main();

function main() {

    // got a valid document?
    if (app.documents.length <= 0) {
        if (app.playbackDisplayDialogs != DialogModes.NO) {
            alert("You must have a document open to export!");
        }
        // quit, returning 'cancel' makes the actions palette not record our script
        return 'cancel';
    }

    // ask for where the exported files should go
    dstFolder = Folder.selectDialog("Choose the destination for export.");
    if (!dstFolder) {
        return;
    }

    // cache useful variables
    uid = 1;
    srcPsdName = app.activeDocument.name;
    var layerCount = app.documents[srcPsdName].layers.length;
    var layerSetsCount = app.documents[srcPsdName].layerSets.length;

    if ((layerCount <= 1) && (layerSetsCount <= 0)) {
        if (app.playbackDisplayDialogs != DialogModes.NO) {
            alert("You need a document with multiple layers to export!");
            // quit, returning 'cancel' makes the actions palette not record our script
            return 'cancel';
        }
    }

    // setup the units in case it isn't pixels
    var savedRulerUnits = app.preferences.rulerUnits;
    var savedTypeUnits = app.preferences.typeUnits;
    app.preferences.rulerUnits = Units.PIXELS;
    app.preferences.typeUnits = TypeUnits.PIXELS;

    // duplicate document so we can extract everything we need
    dupPsd = app.activeDocument.duplicate();
    dupPsd.activeLayer = dupPsd.layers[dupPsd.layers.length - 1];

    hideAllLayers(dupPsd);

    // export layers
    xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
    xml += "<PSDUI>";

    xml += "\n<psdSize>";
    xml += "<width>" + dupPsd.width.value + "</width>";
    xml += "<height>" + dupPsd.height.value + "</height>";
    xml += "</psdSize>";

    xml += "\n<layers>";
    exportAllLayers(dupPsd);
    xml += "</layers>";

    xml += "\n</PSDUI>";
    $.writeln(xml);

    dupPsd.close(SaveOptions.DONOTSAVECHANGES);

    // create export
    var xmlFile = new File(dstFolder + "/" + dstFolder.name + ".xml");
    xmlFile.encoding = "utf-8";   //写文件时指定编码，不然中文会出现乱码
    xmlFile.open('w');
    xmlFile.writeln(xml);
    xmlFile.close();

    app.preferences.rulerUnits = savedRulerUnits;
    app.preferences.typeUnits = savedTypeUnits;
}

function exportAllLayers(obj) {
    if (typeof (obj) == "undefined") {
        return;
    }

    if (typeof (obj.layers) != "undefined" && obj.layers.length > 0) {

        for (var i = obj.layers.length - 1; 0 <= i; i--) {
            exportLayer(obj.layers[i])
        }

    }
    else {
        exportLayer(obj)
    };
}

function exportLayer(obj) {
    if (typeof (obj) == "undefined") {
        return;
    }

    if (obj.typename == "LayerSet") {
        exportLayerSet(obj);
    }
    else if (obj.typename = "ArtLayer") {
        exportArtLayer(obj);
    }
}

function exportLayerSet(obj) {
    if (typeof (obj.layers) == "undefined" || obj.layers.length <= 0) {
        return
    }
    if (obj.name.search("@ScrollView") >= 0) {
        exportScrollView(obj);
    }
    else if (obj.name.search("@GridLayout") >= 0) {
        exportGridLayout(obj);
    }
    else if (obj.name.search("@Button") >= 0) {
        exportButton(obj);
    }
    else if (obj.name.search("@Toggle") >= 0) {
        exportToggle(obj);
    }
    else if (obj.name.search("@Slider") >= 0) {
        exportSlider(obj);
    }
    else if (obj.name.search("@HVLayout") >= 0) {
        exportHVLayout(obj);
    }
    else if (obj.name.search("@InputField") >= 0) {
        exportInputField(obj);
    }
    else if (obj.name.search("@Scrollbar") >= 0) {
        exportScrollBar(obj);
    }
    else if (obj.name.search("@LayoutElement") >= 0) {
        exportLayoutElement(obj)
    }
    else {
        xml += "<PsLayer>";
        xml += "<type>Normal</type>";
        xml += "<name>" + obj.name + "</name>";
        xml += "<layers>";
        exportAllLayers(obj)
        xml += "</layers>";
        xml += "</PsLayer>";
    }
}

function exportLayoutElement(obj) {
    xml += "<PsLayer>";
    xml += "<type>LayoutElement</type>";
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += "<name>" + itemName + "</name>";

    xml += "<layers>";
    exportAllLayers(obj);
    xml += "</layers>";

    obj.visible = true;
    showAllLayers(obj);

    var recSize = getLayerRec(dupPsd.duplicate());

    xml += "<position>";
    xml += "<x>" + recSize.x + "</x>";
    xml += "<y>" + recSize.y + "</y>";
    xml += "</position>";

    xml += "<size>";
    xml += "<width>" + recSize.width + "</width>";
    xml += "<height>" + recSize.height + "</height>";
    xml += "</size>";

    hideAllLayers(obj);

    xml += "</PsLayer>";
}

function exportScrollView(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>ScrollView</type>\n<name>" + itemName + "</name>\n");
    xml += ("<layers>\n");
    exportAllLayers(obj);
    xml += ("</layers>");

    var params = obj.name.split(":");

    if (params.length > 2) {
        alert(obj.name + "-------Layer's name is illegal------------");
    }

    var recSize;
    if (obj.layers[obj.layers.length - 1].name.search("@Size") < 0) {
        alert("Bottom layer's name doesn't contain '@Size'");
    }
    else {
        obj.layers[obj.layers.length - 1].visible = true;

        recSize = getLayerRec(dupPsd.duplicate());

        xml += "<position>";
        xml += "<x>" + recSize.x + "</x>";
        xml += "<y>" + recSize.y + "</y>";
        xml += "</position>";

        xml += "<size>";
        xml += "<width>" + recSize.width + "</width>";
        xml += "<height>" + recSize.height + "</height>";
        xml += "</size>";

        obj.layers[obj.layers.length - 1].visible = false;
    }

    //以下计算padding和spacing
    obj.layers[0].visible = true;
    showAllLayers(obj.layers[0]);                           //子图层组已经在上面导出过，要再次计算size需先将其显示
    var rec0 = getLayerRec(dupPsd.duplicate());
    hideAllLayers(obj.layers[0]);
    obj.layers[0].visible = false;

    obj.layers[1].visible = true;
    showAllLayers(obj.layers[1]);
    var rec1 = getLayerRec(dupPsd.duplicate());
    hideAllLayers(obj.layers[0]);
    obj.layers[1].visible = false;

    var spacing;
    var paddingx;
    var paddingy;
    if (params[1].search("H") >= 0)          //水平间距
    {
        spacing = rec1.x - rec0.x - rec0.width;
        paddingx = rec0.x - (recSize.x - recSize.width / 2) - rec0.width / 2;                                      //x方向边距，默认左右相等
        paddingy = (recSize.height - rec0.height) / 2;                                                          //暂时只考虑上下边距相等
        //paddingy = recSize.height / 2 - rec0.height / 2 - (rec0.y - recSize.y);                                                                   //上边距
        //paddingy2 = recSize.height - rec0.height - paddingy;                      //下边距
    }
    else                                                //垂直间距
    {
        spacing = rec0.y - rec1.y - rec0.height;
        paddingx = (recSize.width - rec0.width) / 2;
        paddingy = (recSize.y + recSize.height / 2) - rec0.y - rec0.height / 2;
    }

    xml += "<args>";
    xml += "<string>" + params[1] + "</string>";     //滑动方向
    xml += "<string>" + spacing + "</string>";
    xml += "<string>" + Math.floor(paddingx) + "</string>";
    xml += "<string>" + Math.floor(paddingy) + "</string>";
    xml += "</args>";

    xml += "</PsLayer>";
}

function setLayerSizeAndPos(layer) {
    layer.visible = true;

    var recSize = getLayerRec(dupPsd.duplicate());

    xml += "<position>";
    xml += "<x>" + recSize.x + "</x>";
    xml += "<y>" + recSize.y + "</y>";
    xml += "</position>";

    xml += "<size>";
    xml += "<width>" + recSize.width + "</width>";
    xml += "<height>" + recSize.height + "</height>";
    xml += "</size>";

    layer.visible = false;

    return recSize;
}

function exportGridLayout(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>GridLayout</type>\n<name>" + itemName + "</name>\n");
    xml += ("<layers>\n");
    exportAllLayers(obj);
    xml += ("</layers>");

    // var params = obj.name.split(":");

    // if (params.length != 3) {
    //     alert("Layer's name is illegal");
    // }

    var recSize;
    if (obj.layers[obj.layers.length - 1].name.search("@Size") < 0) {
        alert("Bottom layer's name doesn't contain '@Size'");
        return;
    }
    else {
        recSize = setLayerSizeAndPos(obj.layers[obj.layers.length - 1]);
    }

    // var totalContentCount = obj.layers.length - 1;

    obj.layers[0].visible = true;
    showAllLayers(obj.layers[0]);                           //子图层组已经在上面导出过，要再次计算size需先将其显示
    var rec0 = getLayerRec(dupPsd.duplicate());
    hideAllLayers(obj.layers[0]);
    obj.layers[0].visible = false;

    // var renderHorizontalGap = params[2] > 1 ? (recSize.width - rec0.width * params[2]) / (params[2] - 1) : 0;
    // var renderVerticalGap = params[1] > 1 ? (recSize.height - rec0.height * params[1]) / (params[1] - 1) : 0;

    xml += "<args>";
    xml += "<string>" + undefined + "</string>";   //TODO 预留
    xml += "<string>" + undefined + "</string>";   //TODO 预留
    xml += "<string>" + rec0.width + "</string>";   //render width
    xml += "<string>" + rec0.height + "</string>";   //render height
    xml += "<string>" + undefined + "</string>"; //TODO 预留
    xml += "<string>" + undefined + "</string>";   //TODO 预留
    // xml += "<string>" + Math.floor(renderHorizontalGap) + "</string>"; //水平间距
    // xml += "<string>" + Math.floor(renderVerticalGap) + "</string>"; //垂直间距
    xml += "</args>";

    xml += "</PsLayer>";
}

function exportHVLayout(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>HVLayout</type>\n<name>" + itemName + "</name>\n");

    exportAllLayers(obj);

    var params = obj.name.split(":");

    if (params.length != 3) {
        alert(obj.name + "-------Layer's name not equals 2------------");
    }

    var recSize;
    if (obj.layers[obj.layers.length - 1].name.search("@Size") < 0) {
        alert("Bottom layer's name doesn't contain '@Size'");
    }
    else {
        obj.layers[obj.layers.length - 1].visible = true;

        recSize = getLayerRec(dupPsd.duplicate());

        xml += "<position>";
        xml += "<x>" + recSize.x + "</x>";
        xml += "<y>" + recSize.y + "</y>";
        xml += "</position>";

        xml += "<size>";
        xml += "<width>" + recSize.width + "</width>";
        xml += "<height>" + recSize.height + "</height>";
        xml += "</size>";

        obj.layers[obj.layers.length - 1].visible = false;
    }

    xml += "<args>";
    xml += "<string>" + params[1] + "</string>";   //方向
    xml += "<string>" + params[2] + "</string>";   //span
    xml += "</args>";

    xml += "</PsLayer>";
}

function exportInputField(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>InputField</type>\n<name>" + itemName + "</name>\n");
    xml += "<layers>";

    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        exportArtLayer(obj.layers[i]);
    }

    xml += "</layers>";
    xml += "\n</PsLayer>";
}

function exportButton(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>Button</type>\n<name>" + itemName + "</name>\n");
    xml += "<layers>";

    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        exportArtLayer(obj.layers[i]);
    }
    xml += "</layers>";
    xml += "\n</PsLayer>";
}

function exportToggle(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>Toggle</type>\n<name>" + itemName + "</name>\n");
    xml += "<layers>";

    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        exportArtLayer(obj.layers[i]);
    }

    xml += "</layers>";
    xml += "\n</PsLayer>";
}

function exportSlider(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>Slider</type>\n<name>" + itemName + "</name>\n");

    var params = obj.name.split(":");

    if (params.length != 2) {
        alert(obj.name + "-------Layer's name is not 1 argument------------");
    }

    var recSize;
    if (obj.layers[obj.layers.length - 1].name.search("@Size") < 0) {
        alert("Bottom layer's name doesn't contain '@Size'");
    }
    else {
        setLayerSizeAndPos(obj.layers[obj.layers.length - 1]);
    }

    xml += "<args>";
    xml += "<string>" + params[1] + "</string>"; //滑动方向
    xml += "</args>";

    xml += "<layers>";

    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        exportArtLayer(obj.layers[i]);
    }
    xml += "</layers>";

    xml += "\n</PsLayer>";
}

function exportScrollBar(obj) {
    var itemName = obj.name.substring(0, obj.name.search("@"));
    xml += ("<PsLayer>\n<type>ScrollBar</type>\n<name>" + itemName + "</name>\n");

    var params = obj.name.split(":");

    if (params.length != 3) {
        alert(obj.name + "-------Layer's name is not 1 argument------------");
    }

    xml += "<args>";
    xml += "<string>" + params[1] + "</string>"; //滑动方向
    xml += "<string>" + params[2] + "</string>"; //比例
    xml += "</args>";

    xml += "<layers>";

    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        exportArtLayer(obj.layers[i]);
    }
    xml += "</layers>";
    xml += "\n</PsLayer>";
}

function exportArtLayer(obj) {
    if (typeof (obj) == "undefined") { return };
    if (obj.name.search("@Size") >= 0) { return };

    xml += "\n<PsLayer>";
    xml += "<type>Normal</type>";
    var validFileName = makeValidFileName(obj.name);

    // alert("psdName: " + obj.name + "  picName: " + validFileName);

    xml += "<name>" + validFileName + "</name>";
    xml += "<image>\n";

    if (LayerKind.TEXT == obj.kind) {
        exportLabel(obj, validFileName);
    }
    else if (obj.name.search("Texture") >= 0) {
        exportTexture(obj, validFileName);
    }
    else {
        exportImage(obj, validFileName);
    }
    xml += "</image>";

    xml += "\n</PsLayer>";
}

function exportLabel(obj, validFileName) {
    //有些文本如标题，按钮，美术用的是其他字体，可能还加了各种样式，需要当做图片切出来使用
    if (obj.name.search("_ArtFont") >= 0) {
        exportImage(obj, validFileName);
        return;
    }

    // TODO ? 静态字体需要特殊标记？？？

    //处理静态文本，会对应unity的静态字体
    // var StaticText = false;
    // if (obj.name.search(Label_StaticFoint) >= 0) {
    //     StaticText = true;
    // }

    xml += "<imageType>" + "Label" + "</imageType>\n";
    xml += "<name>" + validFileName + "</name>\n";
    obj.visible = true;
    saveScenePng(dupPsd.duplicate(), validFileName, false);
    obj.visible = false;

    xml += "<args>";
    xml += "<string>" + obj.textItem.color.rgb.hexValue + "</string>";
    xml += "<string>" + obj.textItem.font + "</string>";
    xml += "<string>" + obj.textItem.size.value + "</string>";
    xml += "<string>" + obj.textItem.contents + "</string>";

    //段落文本带文本框，可以取得对齐方式
    if (obj.textItem.kind == TextType.PARAGRAPHTEXT) {
        xml += "<string>" + obj.textItem.justification + "</string>";     //加对齐方式
    }
    // 点文本，默认left对齐吧
    else {
        xml += "<string>LEFT</string>";
    }

    // alert(obj.textItem.name + ": " + obj.textItem.justification);

    xml += "</args>";

    // 透明度
    xml += "<opacity>" + obj.opacity + "</opacity>";
}

function exportTexture(obj, validFileName) {
    xml += "<imageType>" + "Texture" + "</imageType>\n";
    xml += "<name>" + validFileName + "</name>\n";

    // 透明度
    xml += "<opacity>" + obj.opacity + "</opacity>";

    obj.visible = true;
    saveScenePng(dupPsd.duplicate(), validFileName, true);

    obj.visible = false;
}

function exportImage(obj, validFileName) {

    xml += "<name>" + validFileName + "</name>\n";

    if (obj.name.search("Common") >= 0) {
        xml += "<imageSource>" + "Common" + "</imageSource>\n";
    }
    else if (obj.name.search("Global") >= 0) {
        xml += "<imageSource>" + "Global" + "</imageSource>\n";
    }
    else {
        xml += "<imageSource>" + "Custom" + "</imageSource>\n";
    }

    xml += "<imageType>" + "Image" + "</imageType>\n";

    // 透明度
    xml += "<opacity>" + obj.opacity + "</opacity>";

    obj.visible = true;
    saveScenePng(dupPsd.duplicate(), validFileName, true);
    obj.visible = false;
}

function hideAllLayers(obj) {
    hideLayerSets(obj);
}

function hideLayerSets(obj) {
    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        if (obj.layers[i].typename == "LayerSet") {
            hideLayerSets(obj.layers[i]);
        }
        else {
            obj.layers[i].visible = false;
        }
    }
}

function showAllLayers(obj) {
    showLayerSets(obj);
}

function showLayerSets(obj) {
    for (var i = obj.layers.length - 1; 0 <= i; i--) {
        if (obj.layers[i].typename == "LayerSet") {
            showLayerSets(obj.layers[i]);
        }
        else {
            obj.layers[i].visible = true;
        }
    }
}

function getLayerRec(psd, notMerge) {
    // we should now have a single art layer if all went well
    if (!notMerge) {
        psd.mergeVisibleLayers();
    }

    // figure out where the top-left corner is so it can be exported into the scene file for placement in game
    // capture current size
    var height = psd.height.value;
    var width = psd.width.value;
    var top = psd.height.value;
    var left = psd.width.value;
    // trim off the top and left
    psd.trim(TrimType.TRANSPARENT, true, true, false, false);
    // the difference between original and trimmed is the amount of offset
    top -= psd.height.value;
    left -= psd.width.value;
    // trim the right and bottom
    psd.trim(TrimType.TRANSPARENT);
    // find center
    top += (psd.height.value / 2)
    left += (psd.width.value / 2)
    // unity needs center of image, not top left
    top = -(top - (height / 2));
    left -= (width / 2);

    height = psd.height.value;
    width = psd.width.value;

    psd.close(SaveOptions.DONOTSAVECHANGES);

    return {
        y: top,
        x: left,
        width: width,
        height: height
    };
}

function saveScenePng(psd, fileName, writeToDisk, notMerge) {
    // we should now have a single art layer if all went well
    if (!notMerge) {
        psd.mergeVisibleLayers();
    }

    // figure out where the top-left corner is so it can be exported into the scene file for placement in game
    // capture current size
    var height = psd.height.value;
    var width = psd.width.value;
    var top = psd.height.value;
    var left = psd.width.value;
    // trim off the top and left
    psd.trim(TrimType.TRANSPARENT, true, true, false, false);
    // the difference between original and trimmed is the amount of offset
    top -= psd.height.value;
    left -= psd.width.value;
    // trim the right and bottom
    psd.trim(TrimType.TRANSPARENT);
    // find center
    top += (psd.height.value / 2)
    left += (psd.width.value / 2)
    // unity needs center of image, not top left
    top = -(top - (height / 2));
    left -= (width / 2);

    height = psd.height.value;
    width = psd.width.value;

    var rec = {
        y: top,
        x: left,
        width: width,
        height: height
    };

    // save the scene data
    if (!notMerge) {
        xml += "<position>";
        xml += "<x>" + rec.x + "</x>";
        xml += "<y>" + rec.y + "</y>";
        xml += "</position>";

        xml += "<size>";
        xml += "<width>" + rec.width + "</width>";
        xml += "<height>" + rec.height + "</height>";
        xml += "</size>";
    }

    if (writeToDisk) {
        // save the image
        var pngFile = new File(dstFolder + "/" + fileName + ".png");
        //var pngSaveOptions = new PNGSaveOptions();
        //psd.saveAs(pngFile, pngSaveOptions, true, Extension.LOWERCASE);

        if (!pngFile.exists) {
            // alert("not exists")

            var pngSaveOptions = new ExportOptionsSaveForWeb();
            pngSaveOptions.format = SaveDocumentType.PNG;
            pngSaveOptions.PNG8 = false;
            psd.exportDocument(pngFile, ExportType.SAVEFORWEB, pngSaveOptions);
        }
    }
    psd.close(SaveOptions.DONOTSAVECHANGES);

}

function makeValidFileName(fileName) {

    var validName = fileName.replace(/^\s+|\s+$/gm, ''); // trim spaces

    validName = validName.replace(/[\\\*\/\?:"\|<>]/g, ''); // remove characters not allowed in a file name
    validName = validName.replace(/[ ]/g, '_'); // replace spaces with underscores, since some programs still may have troubles with them

    if (validName.match("Common") ||
        validName.match("Global") ||
        validName.match("CustomAtlas")) {
        validName = validName.substring(0, validName.lastIndexOf("@"));  //截取@之前的字符串作为图片的名称。
    }
    else if (!srcPsdName.match("Common") ||
        !srcPsdName.match("Global") ||
        !srcPsdName.match("CustomAtlas"))    // 判断是否为公用的PSD素材文件，如果不是，则自动为图片增加后缀，防止重名。 公用psd文件的图片层不允许重名。
    {
        // new
        if (typeof (validNameMap[validName]) != "undefined") {
            var uid = validNameMap[validName];
            validNameMap[validName]++;
            validName += "_" + uid;
        }
        else {
            validNameMap[validName] = 0;
        }
    }

    $.writeln(validName);
    return validName;
}

// 裁切 基于透明像素
function trim(doc) {
    doc.trim(TrimType.TRANSPARENT, true, true, true, true);
}