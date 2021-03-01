using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomLayer(ELayerType.ScrollView)]
    public class UguiScrollViewImporter : IPsLayerImporter
    {
        public PSDImportCtrl ctrl { get; set; }
        public void DrawPsLayer(PsLayer layer, GameObject parent)
        {
            ScrollRect scrollRect = PSDImportUtility.LoadAndInstant<ScrollRect>(PSD2UGUIConfig.ASSET_PATH_SCROLLVIEW, layer.name, parent);

            RectTransform rectTransform = scrollRect.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(layer.size.width, layer.size.height);
            rectTransform.anchoredPosition = new Vector2(layer.position.x, layer.position.y);

            if (layer.layers != null)
            {
                string type = layer.args[0].ToUpper();
                switch (type)
                {
                    case "V":
                        BuildVerticalScrollView(scrollRect, layer);
                        break;
                    case "H":
                        BuildHorizonScrollView(scrollRect, layer);
                        break;
                    default:
                        break;
                }

                ctrl.DrawPsLayers(layer.layers, scrollRect.content.gameObject);
            }
        }

        /// <summary>
        /// 构建水平滑动，主要是添加自动布局
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="layer"></param>
        public void BuildHorizonScrollView(ScrollRect scrollRect, PsLayer layer)
        {
            scrollRect.vertical = false;
            scrollRect.horizontal = true;

            //水平默认从左向右滑动
            scrollRect.content.anchorMin = Vector2.zero;
            scrollRect.content.anchorMax = new Vector2(0, 1);
            scrollRect.content.pivot = new Vector2(0, 0.5f);         //锚定左侧，否则动态增长时会从两边添加cell

            var contentSizeFilter = scrollRect.content.GetComponent<ContentSizeFitter>();
            contentSizeFilter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFilter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var hLayout = scrollRect.content.gameObject.AddComponent<HorizontalLayoutGroup>();  //添加水平布局组件
            if (layer.args.Length < 4)
            {
                Debug.LogWarning("ScrollView arguments error !");
            }
            else
            {
                if (float.TryParse(layer.args[1], out float spacing))
                {
                    hLayout.spacing = spacing;
                }
                if (int.TryParse(layer.args[2], out int left) && int.TryParse(layer.args[3], out int top))
                {
                    hLayout.padding = new RectOffset(left, left, top, top);
                }
            }
        }

        /// <summary>
        /// 构建垂直滑动
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="layer"></param>
        public void BuildVerticalScrollView(ScrollRect scrollRect, PsLayer layer)
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = false;

            //垂直默认从上向下滑动
            scrollRect.content.anchorMin = new Vector2(0, 1);
            scrollRect.content.anchorMax = Vector2.one;
            scrollRect.content.pivot = new Vector2(0.5f, 1);         //锚定上侧，否则动态增长时会从两边添加cell

            var contentSizeFilter = scrollRect.content.GetComponent<ContentSizeFitter>();
            contentSizeFilter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFilter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var vLayout = scrollRect.content.gameObject.AddComponent<VerticalLayoutGroup>();  //添加水平布局组件
            if (layer.args.Length < 4)
            {
                Debug.LogWarning("ScrollView arguments error !");
            }
            else
            {
                if (float.TryParse(layer.args[1], out var spacing))
                {
                    vLayout.spacing = spacing;
                }
                if (int.TryParse(layer.args[2], out var left) && int.TryParse(layer.args[3], out var top))
                {
                    vLayout.padding = new RectOffset(left, left, top, top);
                }
            }
        }
    }
}