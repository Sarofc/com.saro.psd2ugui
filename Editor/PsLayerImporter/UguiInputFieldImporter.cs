using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PSDUIImporter
{
    // TODO test
    [CustomLayer(ELayerType.InputField)]
    internal class UguiInputFieldImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }

        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            InputField temp = AssetDatabase.LoadAssetAtPath<InputField>(PSD2UGUIConfig.ASSET_PATH_INPUTFIELD);
            InputField inputfield = GameObject.Instantiate(temp);
            inputfield.transform.SetParent(parent.transform, false);
            inputfield.name = layer.name;

            // 背景尺寸,用来限制文本框尺寸
            float _bgSize = 0;

            for (int i = 0; i < layer.layers.Length; ++i)
            {
                var _temp = layer.layers[i];

                if (_temp.image != null)
                {
                    PsImage image = _temp.image;

                    if (image.imageType == EImageType.Label)
                    {
                        if (image.name.ToLower().Contains("text"))
                        {
                            this.ctrl.DrawPsLayer(_temp, inputfield.gameObject);

                            var _text = PSDImportUtility.canvas.transform.Find(_temp.name).GetComponent<Text>();

                            RectTransform _rectTransform = _text.GetComponent<RectTransform>();

                            // 需要重设锚点,目前默认left
                            var _offsetMin = _rectTransform.offsetMin;
                            var _offsetMax = _rectTransform.offsetMax;

                            _rectTransform.pivot = new Vector2(0, 1);

                            _rectTransform.offsetMin = _offsetMin;
                            _rectTransform.offsetMax = _offsetMax;

                            if (Math.Abs(_bgSize) < float.Epsilon)
                            {
                                for (int j = 0; j < layer.layers.Length; ++j)
                                {
                                    var _layerLayer = layer.layers[i];

                                    if (_layerLayer.image != null)
                                    {
                                        if (_layerLayer.image.imageType != EImageType.Label &&
                                            _layerLayer.image.name.Contains("background"))
                                        {
                                            _bgSize = _layerLayer.image.size.width;

                                            break;
                                        }
                                    }
                                }
                            }

                            if (_rectTransform.sizeDelta.x < _bgSize)
                            {
                                _rectTransform.sizeDelta = new Vector2(_bgSize, _rectTransform.sizeDelta.y);
                            }

                            _text.supportRichText = false;
                            _text.text = "";

                            if (inputfield.textComponent != null)
                            {
                                Object.DestroyImmediate(inputfield.textComponent.gameObject);
                            }

                            inputfield.textComponent = _text;
                        }
                        else if (image.name.ToLower().Contains("holder"))
                        {
                            this.ctrl.DrawPsLayer(_temp, inputfield.gameObject);

                            if (inputfield.placeholder != null)
                            {
                                Object.DestroyImmediate(inputfield.placeholder.gameObject);
                            }

                            inputfield.placeholder = PSDImportUtility.canvas.transform.Find(_temp.name).GetComponent<Text>();
                            ((Text)inputfield.placeholder).supportRichText = false;
                        }
                    }
                    else
                    {
                        if (image.name.ToLower().Contains("background"))
                        {
                            if (image.imageSource == EImageSource.Common || image.imageSource == EImageSource.Custom)
                            {
                                string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.k_PNG_SUFFIX;
                                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
                                inputfield.image.sprite = sprite;

                                RectTransform rectTransform = inputfield.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                                _bgSize = image.size.width;

                                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
                            }
                        }
                    }
                }
            }
        }
    }
}