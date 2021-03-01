using UnityEngine;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Normal)]
    public class UguiNormalImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            RectTransform obj = PSDImportUtility.LoadAndInstant<RectTransform>(PSD2UGUIConfig.ASSET_PATH_EMPTY, layer.name, parent);
            obj.offsetMin = Vector2.zero;
            obj.offsetMax = Vector2.zero;
            obj.anchorMin = Vector2.zero;
            obj.anchorMax = Vector2.one;

            RectTransform rectTransform = parent.GetComponent<RectTransform>();
            obj.sizeDelta = rectTransform.sizeDelta;
            obj.anchoredPosition = rectTransform.anchoredPosition;

            if (layer.image != null)
            {
                PsImage image = layer.image;
                ctrl.DrawPsImage(image, parent, obj.gameObject);
            }

            ctrl.DrawPsLayers(layer.layers, obj.gameObject);
        }
    }
}