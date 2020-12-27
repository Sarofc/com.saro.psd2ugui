using UnityEditor;
using UnityEngine;
namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Panel)]
    public class PanelLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            //UnityEngine.UI.Image temp = AssetDatabase.LoadAssetAtPath(PSDImporterConst.PREFAB_PATH_IMAGE, typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image;
            UnityEngine.UI.Image panel = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Image>(PSD2UGUIConfig.ASSET_PATH_IMAGE, layer.name, parent);//GameObject.Instantiate(temp) as UnityEngine.UI.Image;

            panel.name = layer.name;

            ctrl.DrawLayers(layer.layers, panel.gameObject);//子节点

            //for (int i = 0; i < layer.images.Length; i++)
            //{
            PSImage image = layer.image;

            if (image.name.ToLower().Contains("background"))
            {
                string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;
                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
                panel.sprite = sprite;

                RectTransform rectTransform = panel.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);

                //panel.transform.SetParent(parent.transform, false); //parent = parent.transform;
            }
            else
            {
                ctrl.DrawImage(image, panel.gameObject);
            }
            //}

        }
    }
}