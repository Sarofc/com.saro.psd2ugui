using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.ScrollBar)]
    public class UguiScrollBarImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            Scrollbar temp = AssetDatabase.LoadAssetAtPath<Scrollbar>(PSD2UGUIConfig.ASSET_PATH_SCROLLBAR);
            Scrollbar scrollBar = GameObject.Instantiate(temp);
            scrollBar.transform.SetParent(parent.transform, false); //parent = parent.transform;

            string type = layer.args[0].ToUpper();
            switch (type)
            {
                case "R":
                    scrollBar.direction = Scrollbar.Direction.RightToLeft;
                    break;
                case "L":
                    scrollBar.direction = Scrollbar.Direction.LeftToRight;
                    break;
                case "T":
                    scrollBar.direction = Scrollbar.Direction.TopToBottom;
                    break;
                case "B":
                    scrollBar.direction = Scrollbar.Direction.BottomToTop;
                    break;
                default:
                    break;
            }

            if (float.TryParse(layer.args[1], out float pecent))
            {
                scrollBar.size = pecent;
            }

            PsImage image = layer.image;
            Sprite sprite = image.LoadAssetAtPath<Sprite>();

            if (image.name.ToLower().Contains("background"))
            {
                scrollBar.GetComponent<Image>().sprite = sprite;
                RectTransform rectTransform = scrollBar.GetComponent<RectTransform>();

                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
            }
            else if (image.name.ToLower().Contains("handle"))
            {
                scrollBar.handleRect.GetComponent<Image>().sprite = sprite;
            }
        }
    }
}
