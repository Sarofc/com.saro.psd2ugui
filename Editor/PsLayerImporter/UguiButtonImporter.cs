using UnityEditor;
using UnityEngine;


namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Button)]
    public class UguiButtonImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            UnityEngine.UI.Button button = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Button>(PSD2UGUIConfig.ASSET_PATH_BUTTON, layer.name, parent);

            if (layer.layers != null)
            {
                for (int imageIndex = 0; imageIndex < layer.layers.Length; imageIndex++)
                {
                    PsImage image = layer.layers[imageIndex].image;
                    string lowerName = image.name.ToLower();
                    if (image.imageType != EImageType.Label && image.imageType != EImageType.Texture)
                    {
                        Sprite sprite = image.LoadAssetAtPath<Sprite>();
                        if (lowerName.Contains("pressed"))
                        {
                            button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                            UnityEngine.UI.SpriteState state = button.spriteState;
                            state.pressedSprite = sprite;
                            button.spriteState = state;
                        }
                        else if (lowerName.Contains("disabled"))
                        {
                            button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                            UnityEngine.UI.SpriteState state = button.spriteState;
                            state.disabledSprite = sprite;
                            button.spriteState = state;
                        }
                        else if (lowerName.Contains("highlighted"))
                        {
                            button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                            UnityEngine.UI.SpriteState state = button.spriteState;
                            state.highlightedSprite = sprite;
                            button.spriteState = state;
                        }
                        else if (lowerName.Contains("selected"))
                        {
                            button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                            UnityEngine.UI.SpriteState state = button.spriteState;
                            state.selectedSprite = sprite;
                            button.spriteState = state;
                        }
                        else
                        {
                            button.image.sprite = sprite;
                            RectTransform rectTransform = button.GetComponent<RectTransform>();
                            rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                            rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
                        }
                    }
                    else
                    {
                        ctrl.DrawPsLayer(layer.layers[imageIndex], button.gameObject);
                    }
                }
            }

        }

    }
}