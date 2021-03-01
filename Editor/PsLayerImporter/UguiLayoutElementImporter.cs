using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.LayoutElement)]
    public class UguiLayoutElementImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            RectTransform obj = PSDImportUtility.LoadAndInstant<RectTransform>(PSD2UGUIConfig.ASSET_PATH_LAYOUTELEMENT, layer.name, parent);
            obj.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            obj.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            var le = obj.GetComponent<LayoutElement>();
            le.preferredWidth = layer.size.width;
            le.preferredHeight = layer.size.height;

            if (layer.image != null)
            {
                PsImage image = layer.image;
                ctrl.DrawPsImage(image, obj.gameObject);
            }

            ctrl.DrawPsLayers(layer.layers, obj.gameObject);
            //obj.transform.SetParent(parent.transform, false); //parent.transform;
        }
    }
}
