using UnityEditor;
using UnityEngine;


namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Button)]
    public class ButtonLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }


        public void DrawLayer(Layer layer, GameObject parent)
        {
            UnityEngine.UI.Button button = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Button>(PSD2UGUIConfig.ASSET_PATH_BUTTON, layer.name, parent);

            if (layer.layers != null)
            {
                for (int imageIndex = 0; imageIndex < layer.layers.Length; imageIndex++)
                {
                    PSImage image = layer.layers[imageIndex].image;
                    string lowerName = image.name.ToLower();
                    if (image.imageType != EImageType.Label && image.imageType != EImageType.Texture)
                    {
                        if (image.imageSource == EImageSource.Custom || image.imageSource == EImageSource.Common)
                        {
                            string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;
                            Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;

                            if (image.name.ToLower().Contains("normal"))
                            {
                                button.image.sprite = sprite;
                                RectTransform rectTransform = button.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);

                                adjustButtonBG(image.imageType, button);
                            }
                            else if (image.name.ToLower().Contains("pressed"))
                            {
                                button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                                UnityEngine.UI.SpriteState state = button.spriteState;
                                state.pressedSprite = sprite;
                                button.spriteState = state;
                            }
                            else if (image.name.ToLower().Contains("disabled"))
                            {
                                button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                                UnityEngine.UI.SpriteState state = button.spriteState;
                                state.disabledSprite = sprite;
                                button.spriteState = state;
                            }
                            else if (image.name.ToLower().Contains("highlighted"))
                            {
                                button.transition = UnityEngine.UI.Selectable.Transition.SpriteSwap;
                                UnityEngine.UI.SpriteState state = button.spriteState;
                                state.highlightedSprite = sprite;
                                button.spriteState = state;
                            }
                        }
                    }
                    else
                    {
                        //ctrl.DrawImage(image, button.gameObject);
                        ctrl.DrawLayer(layer.layers[imageIndex], button.gameObject);
                    }
                }
            }

        }

        //调整按钮背景
        private void adjustButtonBG(EImageType imageType, UnityEngine.UI.Button button)
        {
            if (imageType == EImageType.SliceImage)
            {
                button.image.type = UnityEngine.UI.Image.Type.Sliced;
            }
            else if (imageType == EImageType.LeftHalfImage)
            {
                var mirror = button.gameObject.AddComponent<UGUI.Effects.Mirror>();
                mirror.mirrorType = UGUI.Effects.Mirror.MirrorType.Horizontal;
                mirror.SetNativeSize();
            }
            else if (imageType == EImageType.BottomHalfImage)
            {
                var mirror = button.gameObject.AddComponent<UGUI.Effects.Mirror>();
                mirror.mirrorType = UGUI.Effects.Mirror.MirrorType.Vertical;
                mirror.SetNativeSize();
            }
            else if (imageType == EImageType.QuarterImage)
            {
                var mirror = button.gameObject.AddComponent<UGUI.Effects.Mirror>();
                mirror.mirrorType = UGUI.Effects.Mirror.MirrorType.Quarter;
                mirror.SetNativeSize();
            }
        }
    }
}