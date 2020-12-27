using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.LayoutElement)]
    public class LayoutElementLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawLayer(Layer layer, GameObject parent)
        {
            RectTransform obj = PSDImportUtility.LoadAndInstant<RectTransform>(PSD2UGUIConfig.ASSET_PATH_LAYOUTELEMENT, layer.name, parent);
            RectTransform rectTransform = parent.GetComponent<RectTransform>();
            obj.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            obj.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            var le = obj.GetComponent<LayoutElement>();
            le.preferredWidth = layer.size.width;
            le.preferredHeight = layer.size.height;

            if (layer.image != null)
            {
                //for (int imageIndex = 0; imageIndex < layer.layers.Length; imageIndex++)
                //{
                PSImage image = layer.image;
                ctrl.DrawImage(image, obj.gameObject);
                //}
            }

            ctrl.DrawLayers(layer.layers, obj.gameObject);
            //obj.transform.SetParent(parent.transform, false); //parent.transform;
        }
    }
}
