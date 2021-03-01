using UnityEditor;
using UnityEngine;

namespace PSDUIImporter
{
    internal class PSD2UGUIConfig : ScriptableObject
    {
        public const string k_ConfigPath = "Assets/Editor/PSD2UGUI/PSD2UGUIConfig.asset";

        public const string k_UGUI_PREFAB_PATH = "Packages/com.saro.psd2ugui/Editor/Template/UI/";

        public const string k_PNG_SUFFIX = ".png";

        public const string k_FONT_SUFFIX = ".ttf";

        public const string k_PREFAB_SUFFIX = ".prefab";

        public static PSD2UGUIConfig instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = AssetDatabase.LoadAssetAtPath<PSD2UGUIConfig>(k_ConfigPath);

                    if (s_Instance == null)
                    {
                        Debug.LogError("use PSD2UGUI/Create Config first...");
                    }
                }

                return s_Instance;
            }
        }

        private static PSD2UGUIConfig s_Instance;

        public static string Globle_BASE_FOLDER => instance._Globle_BASE_FOLDER;
        public static string FONT_FOLDER => instance._FONT_FOLDER;


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

        [Header("通用图片路径")]
        public string _Globle_BASE_FOLDER = "Assets/Art/Textures/HomeCommon/";

        [Header("字体资源路径")]
        public string _FONT_FOLDER = "Assets/Art/Font/";

        [Space(10)]
        [Header("预制体模板加载路径")]
        public string m_ASSET_PATH_EMPTY = k_UGUI_PREFAB_PATH + "Empty" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_BUTTON = k_UGUI_PREFAB_PATH + "Button" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_TOGGLE = k_UGUI_PREFAB_PATH + "Toggle" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_CANVAS = k_UGUI_PREFAB_PATH + "Canvas" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_EVENTSYSTEM = k_UGUI_PREFAB_PATH + "EventSystem" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_GRID = k_UGUI_PREFAB_PATH + "Grid" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_IMAGE = k_UGUI_PREFAB_PATH + "Image" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_RAWIMAGE = k_UGUI_PREFAB_PATH + "RawImage" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_HALFIMAGE = k_UGUI_PREFAB_PATH + "HalfImage" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_SCROLLVIEW = k_UGUI_PREFAB_PATH + "ScrollView" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_SLIDER = k_UGUI_PREFAB_PATH + "Slider" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_TEXT = k_UGUI_PREFAB_PATH + "Text" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_SCROLLBAR = k_UGUI_PREFAB_PATH + "Scrollbar" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_GROUP_V = k_UGUI_PREFAB_PATH + "VerticalGroup" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_GROUP_H = k_UGUI_PREFAB_PATH + "HorizontalGroup" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_INPUTFIELD = k_UGUI_PREFAB_PATH + "InputField" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_LAYOUTELEMENT = k_UGUI_PREFAB_PATH + "LayoutElement" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_TAB = k_UGUI_PREFAB_PATH + "Tab" + k_PREFAB_SUFFIX;
        public string m_ASSET_PATH_TABGROUP = k_UGUI_PREFAB_PATH + "TabGroup" + k_PREFAB_SUFFIX;

    }
}