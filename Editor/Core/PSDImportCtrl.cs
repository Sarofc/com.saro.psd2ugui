using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace PSDUIImporter
{
    public class PSDImportCtrl
    {
        private PSDUI psdUI;

        private Dictionary<ELayerType, IPsLayerImporter> m_Type2LayerImporterMap;
        private Dictionary<EImageType, IPsImageImporter> m_Type2ImageImporterMap;

        public PSDImportCtrl(string xmlFilePath)
        {
            m_Type2LayerImporterMap = new Dictionary<ELayerType, IPsLayerImporter>();
            m_Type2ImageImporterMap = new Dictionary<EImageType, IPsImageImporter>();

            InitDataAndPath(xmlFilePath);
            InitCanvas();
            LoadLayers();
            MoveLayers();
            RegisterDrawers();
            PSDImportUtility.ParentDic.Clear();
        }

        public void RegisterLayerImporter(ELayerType type, IPsLayerImporter importer)
        {
            if (m_Type2LayerImporterMap.ContainsKey(type))
            {
                throw new Exception("duplicate layer importer. type: " + type);
            }
            m_Type2LayerImporterMap.Add(type, importer);
            importer.ctrl = this;
        }

        public void RegisterImageImporter(EImageType type, IPsImageImporter importer)
        {
            if (m_Type2ImageImporterMap.ContainsKey(type))
            {
                throw new Exception("duplicate layer importer. type: " + type);
            }
            m_Type2ImageImporterMap.Add(type, importer);
        }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            // new
            if (m_Type2LayerImporterMap.TryGetValue(layer.type, out var importer))
            {
                importer.DrawPsLayer(layer, parent);
            }
        }

        public void DrawPsLayers(PsLayer[] layers, GameObject parent)
        {
            if (layers != null)
            {
                for (int layerIndex = 0; layerIndex < layers.Length; layerIndex++)
                {
                    DrawPsLayer(layers[layerIndex], parent);
                }
            }
        }

        public void DrawPsImage(PsImage image, GameObject parent, GameObject ownObj = null)
        {
            if (m_Type2ImageImporterMap.TryGetValue(image.imageType, out var importer))
            {
                importer.DrawPsImage(image, parent, ownObj);
            }
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

            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false) { return; }

            // 已改为，改为直接在unity里选择，然后通过Assetdatabase转成直接可用的路径
            PSDImportUtility.baseFilename = Path.GetFileNameWithoutExtension(xmlFilePath);
            PSDImportUtility.baseDirectory = Path.GetDirectoryName(xmlFilePath) + @"\";
        }

        private void InitCanvas()
        {
            Canvas temp = AssetDatabase.LoadAssetAtPath(PSD2UGUIConfig.ASSET_PATH_CANVAS, typeof(Canvas)) as Canvas;
            PSDImportUtility.canvas = GameObject.Instantiate(temp);
            PSDImportUtility.canvas.renderMode = RenderMode.ScreenSpaceCamera;
            PSDImportUtility.canvas.worldCamera = Camera.main;

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
                ImportPsLayer(psdUI.layers[layerIndex], PSDImportUtility.baseDirectory);
            }
        }

        private void RegisterDrawers()
        {
            var layerImporters = PSDImportUtility.GetAllLayerImporters();
            foreach (var importer in layerImporters)
            {
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
                DrawPsLayer(psdUI.layers[layerIndex], obj.gameObject);
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
                MoveAsset(psdUI.layers[layerIndex]);
            }

            AssetDatabase.Refresh();
        }

        //--------------------------------------------------------------------------
        // private methods,按texture或image的要求导入图片到unity可加载的状态
        //-------------------------------------------------------------------------
        private void ImportPsLayer(PsLayer layer, string baseDirectory)
        {
            if (layer.image != null)
            {
                PsImage image = layer.image;

                if (image.imageType != EImageType.Label)
                {
                    string texturePathName = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.k_PNG_SUFFIX;

                    Debug.Log(texturePathName);
                    // modify the importer settings
                    TextureImporter textureImporter = AssetImporter.GetAtPath(texturePathName) as TextureImporter;

                    if (textureImporter != null && image.imageType != EImageType.Texture)           //Texture类型不设置属性
                    {
                        textureImporter.textureType = TextureImporterType.Sprite;
                        textureImporter.spriteImportMode = SpriteImportMode.Single;
                        textureImporter.mipmapEnabled = false;          //默认关闭mipmap

                        textureImporter.maxTextureSize = 2048;

                        AssetDatabase.WriteImportSettingsIfDirty(texturePathName);
                        AssetDatabase.ImportAsset(texturePathName);
                    }
                }
            }

            if (layer.layers != null)
            {
                for (int layerIndex = 0; layerIndex < layer.layers.Length; layerIndex++)
                {
                    ImportPsLayer(layer.layers[layerIndex], PSDImportUtility.baseDirectory);
                }
            }
        }


        //------------------------------------------------------------------
        //when it's a common psd, then move the asset to special folder
        //------------------------------------------------------------------
        private void MoveAsset(PsLayer layer)
        {
            if (layer.image != null)
            {
                string newPath = PSD2UGUIConfig.Globle_BASE_FOLDER;

                if (!System.IO.Directory.Exists(newPath))
                {
                    System.IO.Directory.CreateDirectory(newPath);
                    Debug.Log("creating new folder : " + newPath);
                }

                AssetDatabase.Refresh();

                PsImage image = layer.image;
                if (image.imageSource == EImageSource.Global)
                {
                    string texturePathName = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.k_PNG_SUFFIX;
                    string targetPathName = newPath + image.name + PSD2UGUIConfig.k_PNG_SUFFIX;

                    //if (!File.Exists(targetPathName))
                    {
                        if (File.Exists(texturePathName))
                        {
                            var result = AssetDatabase.MoveAsset(texturePathName, targetPathName);
                            if (string.IsNullOrEmpty(result))
                            {
                                Debug.Log($"move success: \nsrc:{texturePathName}\ndst:{targetPathName}");
                            }
                            else
                            {
                                File.Delete(texturePathName);
                                Debug.LogError($"move failed, delete: {texturePathName} \n reason: {result}");
                            }
                        }
                    }
                }
            }

            if (layer.layers != null)
            {
                for (int layerIndex = 0; layerIndex < layer.layers.Length; layerIndex++)
                {
                    MoveAsset(layer.layers[layerIndex]);
                }
            }
        }
    }
}