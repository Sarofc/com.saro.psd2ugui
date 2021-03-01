using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Slider)]
    public class UguiSliderImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            Slider slider = PSDImportUtility.LoadAndInstant<Slider>(PSD2UGUIConfig.ASSET_PATH_SLIDER, layer.name, parent);
            RectTransform sliderRect = slider.GetComponent<RectTransform>();
            sliderRect.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            sliderRect.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            string type = layer.args[0].ToUpper();
            switch (type)
            {
                case "R":
                    slider.direction = Slider.Direction.RightToLeft;
                    break;
                case "L":
                    slider.direction = Slider.Direction.LeftToRight;
                    break;
                case "T":
                    slider.direction = Slider.Direction.TopToBottom;
                    break;
                case "B":
                    slider.direction = Slider.Direction.BottomToTop;
                    break;
                default:
                    break;
            }

            for (int i = 0, j = 0; i < layer.layers.Length; i++)
            {
                var subLayer = layer.layers[i];
                PsImage image = subLayer.image;

                if (image != null)
                {
                    string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.k_PNG_SUFFIX;
                    Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;

                    // fill
                    if (j == 0)   //可以没有bg
                    {
                        var fillImage = slider.fillRect.GetComponent<Image>();
                        fillImage.sprite = sprite;

                        var fillArea = slider.transform.Find("Fill Area").GetComponent<RectTransform>();
                        fillArea.sizeDelta = new Vector2(image.size.width, image.size.height);
                    }
                    // bg
                    else if (i == 1)
                    {
                        var bgRect = slider.transform.Find("Background").GetComponent<RectTransform>();
                        var bgImage = bgRect.GetComponent<Image>();
                        bgImage.sprite = sprite;
                        bgRect.sizeDelta = new Vector2(image.size.width, image.size.height);

                        // fix position
                        bgRect.transform.parent = PSDImportUtility.canvas.transform;
                        bgRect.anchoredPosition = new Vector3(image.position.x, image.position.y, 0f);

                        bgRect.transform.parent = slider.transform;
                        bgRect.transform.SetAsFirstSibling();

                        bgRect.gameObject.SetActive(true);
                    }
                    // handle
                    else if (j == 2)       //默认没有handle
                    {
                        var handleRect = slider.transform.Find("Handle Slide Area/Handle").GetComponent<RectTransform>();
                        var handleSprite = handleRect.GetComponent<Image>();
                        slider.handleRect = handleRect;
                        handleSprite.sprite = sprite;

                        // calc handle size
                        var handleSlideAreaRectTrans = handleRect.parent as RectTransform;
                        if (slider.direction == Slider.Direction.LeftToRight || slider.direction == Slider.Direction.RightToLeft)
                        {
                            handleRect.sizeDelta = new Vector2(image.size.width, image.size.height - sliderRect.sizeDelta.y - handleSlideAreaRectTrans.sizeDelta.y);
                        }
                        else
                        {
                            handleRect.sizeDelta = new Vector2(image.size.width - sliderRect.sizeDelta.x - handleSlideAreaRectTrans.sizeDelta.x, image.size.height);
                        }

                        handleRect.gameObject.SetActive(true);
                    }
                    else
                    {
                        ctrl.DrawPsImage(image, slider.gameObject, null);
                    }

                    j++;
                }
                else
                {
                    ctrl.DrawPsLayer(subLayer, slider.gameObject);
                }
            }
        }
    }
}