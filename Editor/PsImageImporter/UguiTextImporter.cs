using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomImage(EImageType.Label)]
    public class UguiTextImporter : IPsImageImporter
    {
        /// <summary>
        /// args[]: color,font,fontSize,text,alignment,bounds
        /// </summary>
        /// <param name="psImage"></param>
        /// <param name="parent"></param>
        /// <param name="ownObj"></param>
        public void DrawPsImage(PsImage psImage, GameObject parent, GameObject ownObj = null)
        {
            Text myText;
            if (ownObj != null)
                myText = ownObj.AddMissingComponent<Text>();
            else
                myText = PSDImportUtility.LoadAndInstant<Text>(PSD2UGUIConfig.ASSET_PATH_TEXT, psImage.name, parent);

            RectTransform rectTransform = myText.GetComponent<RectTransform>();
            rectTransform.SetAnchorMiddleCenter();

            var labelColorStr = psImage.args[0];
            var fontNameStr = psImage.args[1];
            var fontSizeStr = psImage.args[2];
            var textStr = psImage.args[3];
            var alignmentStr = psImage.args[4];
            //var bounds = psImage.args[5];

            Debug.Log($"LabelColor: {labelColorStr}\nFontName: {fontNameStr}\nFontSize: {fontSizeStr}\nText: {textStr}\nAlignment: {alignmentStr}");

            // 0.处理颜色
            Color color;
            if (ColorUtility.TryParseHtmlString(("#" + labelColorStr), out color))
            {
                if (psImage.opacity > -1)
                {
                    color.a = psImage.opacity / 100f;
                }

                myText.color = color;
            }
            else
            {
                Debug.LogError($"{psImage.name} parse Color error. arg: {labelColorStr}");
            }

            // 1.处理字体
            // 设置字体,注意unity中的字体名需要和导出的xml中的一致
            // 不再区分什么静态不静态，降低复杂度

            string fontFullName = PSD2UGUIConfig.FONT_FOLDER + psImage.args[1] + PSD2UGUIConfig.k_FONT_SUFFIX;

            var font = AssetDatabase.LoadAssetAtPath(fontFullName, typeof(Font)) as Font;
            if (font == null)
            {
                Debug.LogWarning("Load font failed : " + fontFullName);
            }
            else
            {
                myText.font = font;
            }

            // 2.处理字体大小
            float size;
            if (float.TryParse(fontSizeStr, out size))
            {
                myText.fontSize = (int)size;
            }

            // 3.处理文字内容
            myText.text = textStr;

            // 4.处理对齐
            //ps的size在unity里面太小，文本会显示不出来,暂时选择溢出
            myText.verticalOverflow = VerticalWrapMode.Overflow;
            myText.horizontalOverflow = HorizontalWrapMode.Overflow;
            myText.alignment = ParseAlignmentPS2UGUI(alignmentStr);

            // 5.处理段落文字的文本框大小
            var w = psImage.size.width;
            var h = psImage.size.height;

            //if (!string.IsNullOrEmpty(bounds))
            //{
            //    var _wh = bounds.Split(',');
            //    if (_wh.Length == 2)
            //    {
            //        if (float.TryParse(_wh[0], out var _w) && float.TryParse(_wh[1], out var _h))
            //        {
            //            w = _w;
            //            h = _h;
            //        }
            //    }
            //}

            rectTransform.sizeDelta = new Vector2(w, h);
            rectTransform.anchoredPosition = new Vector2(psImage.position.x, psImage.position.y);
        }

        /// <summary>
        /// ps的对齐转换到ugui，暂时只做水平的对齐
        /// </summary>
        /// <param name="justification"></param>
        /// <returns></returns>
        public TextAnchor ParseAlignmentPS2UGUI(string justification)
        {
            var defaut = TextAnchor.MiddleCenter;
            if (string.IsNullOrEmpty(justification))
            {
                return defaut;
            }

            string[] temp = justification.Split('.');
            if (temp.Length != 2)
            {
                Debug.LogWarning("ps exported justification is error !");
                return defaut;
            }
            Justification justi = (Justification)System.Enum.Parse(typeof(Justification), temp[1]);
            int index = (int)justi;
            defaut = (TextAnchor)System.Enum.ToObject(typeof(TextAnchor), index);

            return defaut;
        }

        // ps的对齐方式
        public enum Justification
        {
            CENTERJUSTIFIED = 0,
            LEFTJUSTIFIED = 1,
            RIGHTJUSTIFIED = 2,
            LEFT = 3,
            CENTER = 4,
            RIGHT = 5,
            FULLYJUSTIFIED = 6,
        }
    }
}
