using UnityEditor;
using UnityEngine;

namespace PSDUIImporter
{
    [CreateAssetMenu(fileName = "PSD2UGUIConfig", menuName = "Create PSD2UGUIConfig Asset")]
    public class PSD2UGUIConfig : ScriptableSingleton<PSD2UGUIConfig>
    {
        public const string k_CONFIG_PATH = "Assets/PSD2UGUI/PSD2UGUIConfig.asset";

        public static string Globle_BASE_FOLDER => instance._Globle_BASE_FOLDER;
        public static string Globle_FOLDER_NAME => instance._Globle_FOLDER_NAME;
        public static string FONT_FOLDER => instance._FONT_FOLDER;
        public static string FONT_STATIC_FOLDER => instance._FONT_STATIC_FOLDER;


        public static string ASSET_PATH_EMPTY => instance.m_ASSET_PATH_EMPTY;
        public static string ASSET_PATH_BUTTON => instance.m_ASSET_PATH_BUTTON;
        public static string ASSET_PATH_TOGGLE => instance.m_ASSET_PATH_TOGGLE;
        public static string ASSET_PATH_CANVAS => instance.m_ASSET_PATH_CANVAS;
        public static string ASSET_PATH_EVENTSYSTEM => instance.m_ASSET_PATH_EVENTSYSTEM;
        public static string ASSET_PATH_GRID => instance.m_ASSET_PATH_GRID;
        public static string ASSET_PATH_IMAGE => instance.m_ASSET_PATH_IMAGE;
        public static string ASSET_PATH_RAWIMAGE => instance.m_ASSET_PATH_RAWIMAGE;
        public static string ASSET_PATH_HALFIMAGE => instance.m_ASSET_PATH_HALFIMAGE;
        public static string ASSET_PATH_SCROLLVIEW => instance.m_ASSET_PATH_SCROLLVIEW;
        public static string ASSET_PATH_SLIDER => instance.m_ASSET_PATH_SLIDER;
        public static string ASSET_PATH_TEXT => instance.m_ASSET_PATH_TEXT;
        public static string ASSET_PATH_SCROLLBAR => instance.m_ASSET_PATH_SCROLLBAR;
        public static string ASSET_PATH_GROUP_V => instance.m_ASSET_PATH_GROUP_V;
        public static string ASSET_PATH_GROUP_H => instance.m_ASSET_PATH_GROUP_H;
        public static string ASSET_PATH_INPUTFIELD => instance.m_ASSET_PATH_INPUTFIELD;
        public static string ASSET_PATH_LAYOUTELEMENT => instance.m_ASSET_PATH_LAYOUTELEMENT;
        public static string ASSET_PATH_TAB => instance.m_ASSET_PATH_TAB;
        public static string ASSET_PATH_TABGROUP => instance.m_ASSET_PATH_TABGROUP;

        [Header("通用图集路径")]
        public string _Globle_BASE_FOLDER = "Assets/Textures/HomeCommon/";

        [Space(10)]
        [Header("图集名")]
        public string _Globle_FOLDER_NAME = "HomeCommon";

        [Header("字体资源路径")]
        public string _FONT_FOLDER = "Assets/PSD2UGUI/Font/";

        [Header("静态字体资源路径")]
        public string _FONT_STATIC_FOLDER = "Assets/PSD2UGUI/Font/StaticFont/";

        [Space(10)]
        [Header("资源模板加载路径")]
        public /*static*/ string m_ASSET_PATH_EMPTY = PSDUI_PATH + "Empty" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_BUTTON = PSDUI_PATH + "Button" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_TOGGLE = PSDUI_PATH + "Toggle" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_CANVAS = PSDUI_PATH + "Canvas" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_EVENTSYSTEM = PSDUI_PATH + "EventSystem" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_GRID = PSDUI_PATH + "Grid" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_IMAGE = PSDUI_PATH + "Image" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_RAWIMAGE = PSDUI_PATH + "RawImage" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_HALFIMAGE = PSDUI_PATH + "HalfImage" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_SCROLLVIEW = PSDUI_PATH + "ScrollView" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_SLIDER = PSDUI_PATH + "Slider" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_TEXT = PSDUI_PATH + "Text" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_SCROLLBAR = PSDUI_PATH + "Scrollbar" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_GROUP_V = PSDUI_PATH + "VerticalGroup" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_GROUP_H = PSDUI_PATH + "HorizontalGroup" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_INPUTFIELD = PSDUI_PATH + "InputField" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_LAYOUTELEMENT = PSDUI_PATH + "LayoutElement" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_TAB = PSDUI_PATH + "Tab" + PSDUI_SUFFIX;
        public /*static*/ string m_ASSET_PATH_TABGROUP = PSDUI_PATH + "TabGroup" + PSDUI_SUFFIX;



        public static string PSDUI_PATH = "Packages/com.saro.psd2ugui/Editor/Template/UI/";

        public const string PNG_SUFFIX = ".png";

        public const string NINE_SLICE = "9Slice";

        public const string IMAGE = "Image";

        public const string FONT_SUFIX = ".ttf";

        public const string PSDUI_SUFFIX = ".prefab";


        //public PSDImporterConst()
        //{
        //    LoadConfig();
        //}

        /// <summary>
        /// 读取工具
        /// </summary>
        //public static void LoadConfig()
        //{
        //    // 重读资源路径
        //    PSD2UGUIConfig _config = AssetDatabase.LoadAssetAtPath<PSD2UGUIConfig>(k_CONFIG_PATH);

        //    if (_config != null)
        //    {
        //        Globle_BASE_FOLDER = _config.m_commonAtlasPath;
        //        Globle_FOLDER_NAME = _config.m_commonAtlasName;
        //        FONT_FOLDER = _config.m_fontPath;
        //        FONT_STATIC_FOLDER = _config.m_staticFontPath;
        //        PSDUI_PATH = _config.m_psduiTemplatePath;

        //        // 重生成路径
        //        ASSET_PATH_EMPTY = PSDUI_PATH + "Empty" + PSDUI_SUFFIX;
        //        ASSET_PATH_BUTTON = PSDUI_PATH + "Button" + PSDUI_SUFFIX;
        //        ASSET_PATH_TOGGLE = PSDUI_PATH + "Toggle" + PSDUI_SUFFIX;
        //        ASSET_PATH_CANVAS = PSDUI_PATH + "Canvas" + PSDUI_SUFFIX;
        //        ASSET_PATH_EVENTSYSTEM = PSDUI_PATH + "EventSystem" + PSDUI_SUFFIX;
        //        ASSET_PATH_GRID = PSDUI_PATH + "Grid" + PSDUI_SUFFIX;
        //        ASSET_PATH_IMAGE = PSDUI_PATH + "Image" + PSDUI_SUFFIX;
        //        ASSET_PATH_RAWIMAGE = PSDUI_PATH + "RawImage" + PSDUI_SUFFIX;
        //        ASSET_PATH_HALFIMAGE = PSDUI_PATH + "HalfImage" + PSDUI_SUFFIX;
        //        ASSET_PATH_SCROLLVIEW = PSDUI_PATH + "ScrollView" + PSDUI_SUFFIX;
        //        ASSET_PATH_SLIDER = PSDUI_PATH + "Slider" + PSDUI_SUFFIX;
        //        ASSET_PATH_TEXT = PSDUI_PATH + "Text" + PSDUI_SUFFIX;
        //        ASSET_PATH_SCROLLBAR = PSDUI_PATH + "Scrollbar" + PSDUI_SUFFIX;
        //        ASSET_PATH_GROUP_V = PSDUI_PATH + "VerticalGroup" + PSDUI_SUFFIX;
        //        ASSET_PATH_GROUP_H = PSDUI_PATH + "HorizontalGroup" + PSDUI_SUFFIX;
        //        ASSET_PATH_INPUTFIELD = PSDUI_PATH + "InputField" + PSDUI_SUFFIX;
        //        ASSET_PATH_LAYOUTELEMENT = PSDUI_PATH + "LayoutElement" + PSDUI_SUFFIX;
        //        ASSET_PATH_TAB = PSDUI_PATH + "Tab" + PSDUI_SUFFIX;
        //        ASSET_PATH_TABGROUP = PSDUI_PATH + "TabGroup" + PSDUI_SUFFIX;

        //        Debug.Log("Load config.");
        //    }

        //    // Debug.LogError(_config.m_staticFontPath);
        //}
    }
}