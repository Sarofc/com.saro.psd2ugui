using UnityEditor;
using UnityEngine;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Group)]
    internal class GroupLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            GameObject temp;
            string type = layer.arguments[0].ToUpper();
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

            UnityEngine.UI.HorizontalOrVerticalLayoutGroup group = GameObject.Instantiate(temp).GetComponent<UnityEngine.UI.HorizontalOrVerticalLayoutGroup>();//as UnityEngine.UI.HorizontalOrVerticalLayoutGroup;
            group.transform.SetParent(parent.transform, false); //parent = parent.transform;

            RectTransform rectTransform = group.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            rectTransform.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            float span;
            if (float.TryParse(layer.arguments[1], out span))
            {
                group.spacing = span;
            }

            ctrl.DrawLayers(layer.layers, group.gameObject);
        }
    }
}