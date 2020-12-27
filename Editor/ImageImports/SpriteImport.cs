using UnityEditor;
using UnityEngine;



namespace PSDUIImporter
{
    [CustomImage(EImageType.Image)]
    public class SpriteImport : IImageImport
    {
        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null)
        {
            if (image.imageSource == EImageSource.Common || image.imageSource == EImageSource.Custom)
            {
                UnityEngine.UI.Image pic;
                if (ownObj != null)
                    pic = ownObj.AddMissingComponent<UnityEngine.UI.Image>();
                else
                    pic = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Image>(PSD2UGUIConfig.ASSET_PATH_IMAGE, image.name, parent);

                RectTransform rectTransform = pic.GetComponent<RectTransform>();
                rectTransform.offsetMin = new Vector2(0.5f, 0.5f);
                rectTransform.offsetMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                string assetPath = PSDImportUtility.baseDirectory + image.name + PSD2UGUIConfig.PNG_SUFFIX;
                Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;

                if (sprite == null)
                {
                    Debug.Log("loading asset at path: " + PSDImportUtility.baseDirectory + image.name);
                }

                pic.sprite = sprite;

                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
            }
            else if (image.imageSource == EImageSource.Global)
            {
                UnityEngine.UI.Image pic;
                if (ownObj != null)
                    pic = ownObj.AddMissingComponent<UnityEngine.UI.Image>();
                else
                    pic = PSDImportUtility.LoadAndInstant<UnityEngine.UI.Image>(PSD2UGUIConfig.ASSET_PATH_IMAGE, image.name, parent);

                RectTransform rectTransform = pic.GetComponent<RectTransform>();
                rectTransform.offsetMin = new Vector2(0.5f, 0.5f);
                rectTransform.offsetMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                string commonImagePath = PSD2UGUIConfig.Globle_BASE_FOLDER + image.name.Replace(".", "/") + PSD2UGUIConfig.PNG_SUFFIX;
                Debug.Log("==  CommonImagePath  ====" + commonImagePath);
                Sprite sprite = AssetDatabase.LoadAssetAtPath(commonImagePath, typeof(Sprite)) as Sprite;
                pic.sprite = sprite;

                pic.name = image.name;

                if (image.imageType == EImageType.SliceImage)
                {
                    pic.type = UnityEngine.UI.Image.Type.Sliced;
                }

                //RectTransform rectTransform = pic.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
            }
        }
    }
}
