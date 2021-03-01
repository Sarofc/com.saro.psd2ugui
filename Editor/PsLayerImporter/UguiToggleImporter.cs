using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Toggle)]
    public class UguiToggleImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            Toggle toggle = PSDImportUtility.LoadAndInstant<Toggle>(PSD2UGUIConfig.ASSET_PATH_TOGGLE, layer.name, parent);

            if (layer.layers == null)
            {
                Debug.LogError("error! bad toggle layers.");
                return;
            }

            for (int i = 0, j = 0; i < layer.layers.Length; i++)
            {
                PsLayer subLayer = layer.layers[i];
                PsImage image = subLayer.image;
                if (image != null)
                {
                    // 头两张处理为 checkmark background
                    if (j == 0)
                    {
                        ctrl.DrawPsImage(image, toggle.gameObject, toggle.graphic.gameObject);
                    }
                    else if (j == 1)
                    {
                        ctrl.DrawPsImage(image, toggle.gameObject, toggle.targetGraphic.gameObject);
                    }
                    else
                    {
                        ctrl.DrawPsImage(image, toggle.gameObject);
                    }
                    j++;
                }
                else
                {
                    ctrl.DrawPsLayer(subLayer, toggle.gameObject);
                }
            }
        }
    }
}