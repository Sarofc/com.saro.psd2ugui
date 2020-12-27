using UnityEngine;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Toggle)]
    public class ToggleLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawLayer(Layer layer, GameObject parent)
        {
            UnityEngine.UI.Toggle toggle = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Toggle>(PSD2UGUIConfig.ASSET_PATH_TOGGLE, layer.name, parent);

            if (layer.layers == null)
            {
                Debug.LogError("error! bad toggle layers.");
                return;
            }

            for (int index = 0; index < layer.layers.Length; index++)
            {
                Layer subLayer = layer.layers[index];
                PSImage image = subLayer.image;
                if (image != null)
                {
                    string lowerName = image.name.ToLower();
                    if (lowerName.Contains("_checkmark"))
                    {
                        ctrl.DrawImage(image, toggle.gameObject, toggle.graphic.gameObject);
                    }
                    else if (lowerName.Contains("_background"))
                    {
                        ctrl.DrawImage(image, toggle.gameObject, toggle.targetGraphic.gameObject);
                    }
                    else
                    {
                        ctrl.DrawImage(image, toggle.gameObject);
                    }
                }
                else
                {
                    ctrl.DrawLayer(subLayer, toggle.gameObject);
                }
            }
        }
    }
}