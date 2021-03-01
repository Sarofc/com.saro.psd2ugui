using UnityEngine;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.GridLayout)]
    public class UguiGridLayoutImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        //args: undefined, undefined, cel.width, cell.height, spacing.x, spacing.y
        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            UnityEngine.UI.GridLayoutGroup gridLayoutGroup = PSDImportUtility.LoadAndInstant<UnityEngine.UI.GridLayoutGroup>(PSD2UGUIConfig.ASSET_PATH_GRID, layer.name, parent);

            RectTransform rectTransform = gridLayoutGroup.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            rectTransform.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            gridLayoutGroup.padding = new RectOffset();

            if (float.TryParse(layer.args[2], out float width) && float.TryParse(layer.args[3], out float height))
            {
                gridLayoutGroup.cellSize = new Vector2(width, height);
            }

            if (float.TryParse(layer.args[4], out float spaceX) && float.TryParse(layer.args[5], out float spaceY))
            {
                gridLayoutGroup.cellSize = new Vector2(spaceX, spaceY);
            }

            ctrl.DrawPsLayers(layer.layers, gridLayoutGroup.gameObject);
        }
    }
}
