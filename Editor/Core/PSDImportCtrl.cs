using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

#if UNITY_5_3
using UnityEditor.SceneManagement;
#endif

namespace PSDUIImporter
{
    public class PSDImportCtrl
    {
        private PSDUI psdUI;

        private Dictionary<ELayerType, ILayerImport> m_Type2LayerImporterMap;
        private Dictionary<EImageType, IImageImport> m_Type2ImageImporterMap;

        //private IImageImport spriteImport;
        //private IImageImport textImport;
        //private IImageImport textureImport;
        //private IImageImport slicedSpriteImport;
        //private IImageImport halfSpriteImport;

        //private ILayerImport buttonImport;
        //private ILayerImport toggleImport;
        //private ILayerImport panelImport;
        //private ILayerImport scrollViewImport;
        //private ILayerImport scrollBarImport;
        //private ILayerImport sliderImport;
        //private ILayerImport gridImport;
        //private ILayerImport emptyImport;
        //private ILayerImport groupImport;
        //private ILayerImport inputFiledImport;
        //private ILayerImport layoutElemLayerImport;
        //private ILayerImport tabGroupLayerImport;

        public PSDImportCtrl(string xmlFilePath)
        {
            m_Type2LayerImporterMap = new Dictionary<ELayerType, ILayerImport>();
            m_Type2ImageImporterMap = new Dictionary<EImageType, IImageImport>();

            InitDataAndPath(xmlFilePath);
            InitCanvas();
            LoadLayers();
            MoveLayers();
            InitDrawers();
            PSDImportUtility.ParentDic.Clear();
        }

        public void RegisterLayerImporter(ELayerType type, ILayerImport importer)
        {
            if (m_Type2LayerImporterMap.ContainsKey(type))
            {
                throw new Exception("duplicate layer importer. type: " + type);
            }
            m_Type2LayerImporterMap.Add(type, importer);
            importer.ctrl = this;
        }

        public void RegisterImageImporter(EImageType type, IImageImport importer)
        {
            if (m_Type2ImageImporterMap.ContainsKey(type))
            {
                throw new Exception("duplicate layer importer. type: " + type);
            }
            m_Type2ImageImporterMap.Add(type, importer);
        }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            // new
            if (m_Type2LayerImporterMap.TryGetValue(layer.type, out var importer))
            {
                importer.DrawLayer(layer, parent);
            }

            // old
            //switch (layer.type)
            //{
            //    case LayerType.Panel:
            //        panelImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Normal:
            //        emptyImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Button:
            //        buttonImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Toggle:
            //        toggleImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Grid:
            //        gridImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.ScrollView:
            //        scrollViewImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Slider:
            //        sliderImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.Group:
            //        groupImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.InputField:
            //        inputFiledImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.ScrollBar:
            //        scrollBarImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.LayoutElement:
            //        layoutElemLayerImport.DrawLayer(layer, parent);
            //        break;
            //    case LayerType.TabGroup:
            //        tabGroupLayerImport.DrawLayer(layer, parent);
            //        break;
            //    default:
            //        break;

            //}
        }

        public void DrawLayers(Layer[] layers, GameObject parent)
        {
            if (layers != null)
            {
                for (int layerIndex = 0; layerIndex < layers.Length; layerIndex++)
                {
                    DrawLayer(layers[layerIndex], parent);
                }
            }
        }

        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null)
        {
            if (EImageType.MirrorImage.HasFlag(image.imageType))
            {
                if (m_Type2ImageImporterMap.TryGetValue(EImageType.MirrorImage, out var importer))
                {
                    importer.DrawImage(image, parent, ownObj);
                }
            }
            else
            {
                if (m_Type2ImageImporterMap.TryGetValue(image.imageType, out var importer))
                {
                    importer.DrawImage(image, parent, ownObj);
                }
            }

            //switch (image.imageType)
            //{
            //    case ImageType.Image:
            //        spriteImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.Texture:
            //        textureImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.Label:
            //        textImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.SliceImage:
            //        slicedSpriteImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.LeftHalfImage:
            //        halfSpriteImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.BottomHalfImage:
            //        halfSpriteImport.DrawImage(image, parent, ownObj);
            //        break;
            //    case ImageType.QuarterImage:
            //        halfSpriteImport.DrawImage(image, parent, ownObj);
            //        break;
            //    default:
            //        break;
            //}
        }

        private void InitDataAndPath(string xmlFilePath)
        {
            psdUI = (PSDUI)PSDImportUtility.DeserializeXml(xmlFilePath, typeof(PSDUI));
            Debug.Log(psdUI.psdSize.width + "=====psdSize======" + psdUI.psdSize.height);
            if (psdUI == null)
            {
                Debug.Log("The file " + xmlFilePath + " wasn't able to generate a PSDUI.");
                return;
            }
#if UNITY_5_2
            if (EditorApplication.SaveCurrentSceneIfUserWantsTo() == false) { return; }
#elif UNITY_5_3
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false) { return; }
#else
            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false) { return; }
#endif

            // 已改为，改为直接在unity里选择，然后通过 Assetdatabase转成直接可用的路径
            PSDImportUtility.baseFilename = Path.GetFileNameWithoutExtension(xmlFilePath);
            PSDImportUtility.baseDirectory = Path.GetDirectoryName(xmlFilePath) + @"\";

            // old
            //PSDImportUtility.baseFilename = Path.GetFileNameWithoutExtension(xmlFilePath);
            //PSDImportUtility.baseDirectory = "Assets/" + Path.GetDirectoryName(xmlFilePath.Remove(0, Application.dataPath.Length + 1)) + "/";
        }

        private void InitCanvas()
        {
#if UNITY_5_2
            EditorApplication.NewScene();
#elif UNITY_5_3
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
#else 
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
#endif
            Canvas temp = AssetDatabase.LoadAssetAtPath(PSD2UGUIConfig.ASSET_PATH_CANVAS, typeof(Canvas)) as Canvas;
            PSDImportUtility.canvas = GameObject.Instantiate(temp);
            PSDImportUtility.canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            UnityEngine.UI.CanvasScaler scaler = PSDImportUtility.canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            scaler.referenceResolution = new Vector2(psdUI.psdSize.width, psdUI.psdSize.height);

            // find 
            var _eventSystem = Object.FindObjectOfType<EventSystem>();

            if (_eventSystem != null)
            {
                PSDImportUtility.eventSys = _eventSystem.gameObject;
            }
            else
            {
                GameObject go = AssetDatabase.LoadAssetAtPath(PSD2UGUIConfig.ASSET_PATH_EVENTSYSTEM, typeof(GameObject)) as GameObject;

                PSDImportUtility.eventSys = GameObject.Instantiate(go);
            }
        }

        private void LoadLayers()
        {
            for (int layerIndex = 0; layerIndex < psdUI.layers.Length; layerIndex++)
            {
                ImportLayer(psdUI.layers[layerIndex], PSDImportUtility.baseDirectory);
            }
        }

        private void InitDrawers()
        {
            var layerImporters = PSDImportUtility.GetAllLayerImporters();
            foreach (var importer in layerImporters)
            {
                //Debug.LogError(importer.GetType());
                var att = importer.GetType().GetCustomAttribute<CustomLayerAttribute>();
                if (att != null)
                {
                    RegisterLayerImporter(att.LayerType, importer);
                }
            }

            var imageImporters = PSDImportUtility.GetAllImageLayerImporters();
            foreach (var importer in imageImporters)
            {
                var att = importer.GetType().GetCustomAttribute<CustomImageAttribute>();
                if (att != null)
                {
                    RegisterImageImporter(att.ImageType, importer);
                }
            }

            //RegisterLayerImporter(LayerType.Button, new ButtonLayerImport(this));
            //RegisterLayerImporter(LayerType.Toggle, new ToggleLayerImport(this));
            //RegisterLayerImporter(LayerType.Panel, new PanelLayerImport(this));
            //RegisterLayerImporter(LayerType.ScrollView, new ScrollViewLayerImport(this));
            //RegisterLayerImporter(LayerType.ScrollBar, new ScrollBarLayerImport(this));
            //RegisterLayerImporter(LayerType.Slider, new SliderLayerImport(this));
            //RegisterLayerImporter(LayerType.Grid, new GridLayerImport(this));
            //RegisterLayerImporter(LayerType.Normal, new NormalLayerImport(this));
            //RegisterLayerImporter(LayerType.InputField, new InputFieldLayerImport(this));
            //RegisterLayerImporter(LayerType.LayoutElement, new LayoutElementLayerImport(this));
            //RegisterLayerImporter(LayerType.TabGroup, new TabGroupLayerImport(this));


            //RegisterImageImporter(ImageType.Image, new SpriteImport());
            //RegisterImageImporter(ImageType.Label, new TextImport());
            //RegisterImageImporter(ImageType.Texture, new TextureImport());
            //RegisterImageImporter(ImageType.SliceImage, new SliceSpriteImport());
            //RegisterImageImporter(ImageType.LeftHalfImage, new HalfSpriteImport());
            //RegisterImageImporter(ImageType.BottomHalfImage, new HalfSpriteImport());
            //RegisterImageImporter(ImageType.QuarterImage, new HalfSpriteImport());


            //buttonImport = new ButtonLayerImport(this);
            //toggleImport = new ToggleLayerImport(this);
            //panelImport = new PanelLayerImport(this);
            //scrollViewImport = new ScrollViewLayerImport(this);
            //scrollBarImport = new ScrollBarLayerImport(this);
            //sliderImport = new SliderLayerImport(this);
            //gridImport = new GridLayerImport(this);
            //emptyImport = new DefultLayerImport(this);
            //groupImport = new GroupLayerImport(this);
            //inputFiledImport = new InputFieldLayerImport(this);
            //layoutElemLayerImport = new LayoutElementLayerImport(this);
            //tabGroupLayerImport = new TabGroupLayerImport(this);
        }

        public void BeginDrawUILayers()
        {
            RectTransform obj = PSDImportUtility.LoadAndInstant<RectTransform>(PSD2UGUIConfig.ASSET_PATH_EMPTY, PSDImportUtility.baseFilename, PSDImportUtility.canvas.gameObject);
            obj.offsetMin = Vector2.zero;
            obj.offsetMax = Vector2.zero;
            obj.anchorMin = Vector2.zero;
            obj.anchorMax = Vector2.one;

            for (int layerIndex = 0; layerIndex < psdUI.layers.Length; layerIndex++)
            {
                DrawLayer(psdUI.layers[layerIndex], obj.gameObject);
            }
            AssetDatabase.Refresh();
        }

        public void BeginSetUIParents()
        {
            foreach (var item in PSDImportUtility.ParentDic)
            {
                item.Key.SetParent(item.Value);
            }
        }


        private void MoveLayers()
        {
            for (int layerIndex = 0; layerIndex < psdUI.layers.Length; layerIndex++)
            {
                MoveAsset(psdUI.layers[layerIndex], PSDImportUtility.baseDirectory);
            }

            AssetDatabase.Refresh();
        }

        //--------------------------------------------------------------------------
        // private methods,按texture或image的要求导入图片到unity可加载的状态
        //-------------------------------------------------------------------------

        private void ImportLayer(Layer layer, string baseDirectory)
        {
            if (layer.image != null)
            {
                //for (int imageIndex = 0; imageIndex < layer.images.Length; imageIndex++)
                //{
                // we need to fixup all images that were exported from PS
                //PSImage image = layer.images[imageIndex];
                PSImage image = layer.image;

                if (image.imageType != EImageType.Label)
                {
                    string texturePathName = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;

                    Debug.Log(texturePathName);
                    // modify the importer settings
                    TextureImporter textureImporter = AssetImporter.GetAtPath(texturePathName) as TextureImporter;

                    if (textureImporter != null && image.imageType != EImageType.Texture)           //Texture类型不设置属性
                    {
                        textureImporter.textureType = TextureImporterType.Sprite;
                        textureImporter.spriteImportMode = SpriteImportMode.Single;
                        textureImporter.mipmapEnabled = false;          //默认关闭mipmap
                        if (image.imageSource == EImageSource.Global)
                        {
                            textureImporter.spritePackingTag = PSD2UGUIConfig.Globle_FOLDER_NAME;
                        }
                        else
                        {
                            textureImporter.spritePackingTag = PSDImportUtility.baseFilename;
                        }

                        textureImporter.maxTextureSize = 2048;

                        if (image.imageType == EImageType.SliceImage)  //slice才需要设置border,可能需要根据实际修改border值
                        {
                            setSpriteBorder(textureImporter, image.arguments);
                            //textureImporter.spriteBorder = new Vector4(3, 3, 3, 3);   // Set Default Slice type  UnityEngine.UI.Image's border to Vector4 (3, 3, 3, 3)
                        }

                        AssetDatabase.WriteImportSettingsIfDirty(texturePathName);
                        AssetDatabase.ImportAsset(texturePathName);
                    }
                }
                //}
            }

            if (layer.layers != null)
            {
                //LoadLayers();
                for (int layerIndex = 0; layerIndex < layer.layers.Length; layerIndex++)
                {
                    ImportLayer(layer.layers[layerIndex], PSDImportUtility.baseDirectory);
                }
            }
        }

        //设置九宫格
        private void setSpriteBorder(TextureImporter textureImporter, string[] args)
        {
            textureImporter.spriteBorder = new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]));
        }

        //------------------------------------------------------------------
        //when it's a common psd, then move the asset to special folder
        //------------------------------------------------------------------
        private void MoveAsset(Layer layer, string baseDirectory)
        {
            if (layer.image != null)
            {
                string newPath = PSD2UGUIConfig.Globle_BASE_FOLDER;
                if (layer.name == PSD2UGUIConfig.IMAGE)
                {
                    newPath += PSD2UGUIConfig.IMAGE + "/";
                }
                else if (layer.name == PSD2UGUIConfig.NINE_SLICE)
                {
                    newPath += PSD2UGUIConfig.NINE_SLICE + "/";
                }

                if (!System.IO.Directory.Exists(newPath))
                {
                    System.IO.Directory.CreateDirectory(newPath);
                    Debug.Log("creating new folder : " + newPath);
                }

                AssetDatabase.Refresh();

                //for (int imageIndex = 0; imageIndex < layer.images.Length; imageIndex++)
                //{
                // we need to fixup all images that were exported from PS
                PSImage image = layer.image;
                if (image.imageSource == EImageSource.Global)
                {
                    string texturePathName = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;
                    string targetPathName = newPath + image.name + PSD2UGUIConfig.PNG_SUFFIX;

                    if (File.Exists(texturePathName))
                    {
                        AssetDatabase.MoveAsset(texturePathName, targetPathName);
                        Debug.Log(texturePathName);
                        Debug.Log(targetPathName);
                    }
                }
                //}
            }

            if (layer.layers != null)
            {
                for (int layerIndex = 0; layerIndex < layer.layers.Length; layerIndex++)
                {
                    MoveAsset(layer.layers[layerIndex], PSDImportUtility.baseDirectory);
                }
            }
        }
    }
}