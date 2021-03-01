using UnityEditor;
using UnityEngine;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.HVLayout)]
    internal class UguiHVLayoutImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            GameObject temp;
            string type = layer.args[0].ToUpper();
            switch (type.ToUpper())
            {
                case "V":
                    temp = AssetDatabase.LoadAssetAtPath(PSD2UGUIConfig.ASSET_PATH_GROUP_V, typeof(GameObject)) as GameObject;
                    break;
                case "H":
                    temp = AssetDatabase.LoadAssetAtPath(PSD2UGUIConfig.ASSET_PATH_GROUP_H, typeof(GameObject)) as GameObject;
                    break;
                default:
                    return;
            }

            UnityEngine.UI.HorizontalOrVerticalLayoutGroup group = GameObject.Instantiate(temp).GetComponent<UnityEngine.UI.HorizontalOrVerticalLayoutGroup>();
            group.name = layer.name;
            group.transform.SetParent(parent.transform, false);

            group.childControlWidth = false;
            group.childControlHeight = false;
            group.childForceExpandWidth = false;
            group.childForceExpandHeight = false;

            RectTransform rectTransform = group.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            rectTransform.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            if (float.TryParse(layer.args[1], out float span))
            {
                group.spacing = span;
            }

            ctrl.DrawPsLayers(layer.layers, group.gameObject);
        }
    }
}