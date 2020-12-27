using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.Slider)]
    public class SliderLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            Slider slider = PSDImportUtility.LoadAndInstant<Slider>(PSD2UGUIConfig.ASSET_PATH_SLIDER, layer.name, parent);
            RectTransform sliderRectTrans = slider.GetComponent<RectTransform>();
            sliderRectTrans.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            sliderRectTrans.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            PosLoader posloader = slider.gameObject.AddComponent<PosLoader>();
            posloader.worldPos = sliderRectTrans.position;

            string type = layer.arguments[0].ToUpper();
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


            for (int i = 0; i < layer.layers.Length; i++)
            {
                PSImage image = layer.layers[i].image;
                string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;
                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;

                if (image.name.ToLower().Contains("bg"))
                {
                    var bgRect = slider.transform.Find("Background").GetComponent<RectTransform>();
                    var bgImage = bgRect.GetComponent<Image>();
                    if (image.imageType != EImageType.SliceImage)
                    {
                        bgImage.type = Image.Type.Simple;
                    }
                    bgImage.sprite = sprite;
                    bgRect.sizeDelta = new Vector2(image.size.width, image.size.height);
                }
                else if (image.name.ToLower().Contains("fill"))
                {
                    var fillImage = slider.fillRect.GetComponent<Image>();
                    if (image.imageType != EImageType.SliceImage)
                    {
                        fillImage.type = Image.Type.Simple;
                    }
                    fillImage.sprite = sprite;

                    var fillArea = slider.transform.Find("Fill Area").GetComponent<RectTransform>();
                    fillArea.sizeDelta = new Vector2(image.size.width, image.size.height);
                }
                else if (image.name.ToLower().Contains("handle"))       //默认没有handle
                {
                    var handleRectTrans = slider.transform.Find("Handle Slide Area/Handle").GetComponent<RectTransform>();
                    var handleSprite = handleRectTrans.GetComponent<Image>();
                    slider.handleRect = handleRectTrans;
                    handleSprite.sprite = sprite;

                    // calc handle size
                    var handleSlideAreaRectTrans = handleRectTrans.parent as RectTransform;
                    if (slider.direction == Slider.Direction.LeftToRight || slider.direction == Slider.Direction.RightToLeft)
                    {
                        handleRectTrans.sizeDelta = new Vector2(image.size.width, image.size.height - sliderRectTrans.sizeDelta.y - handleSlideAreaRectTrans.sizeDelta.y);
                    }
                    else
                    {
                        handleRectTrans.sizeDelta = new Vector2(image.size.width - sliderRectTrans.sizeDelta.x - handleSlideAreaRectTrans.sizeDelta.x, image.size.height);
                    }

                    handleRectTrans.gameObject.SetActive(true);
                }
            }
        }
    }
}