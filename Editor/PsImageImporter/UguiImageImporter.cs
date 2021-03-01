using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomImage(EImageType.Image)]
    public class UguiImageImporter : IPsImageImporter
    {
        public void DrawPsImage(PsImage psImage, GameObject parent, GameObject ownObj = null)
        {
            Image pic;
            if (ownObj != null)
                pic = ownObj.AddMissingComponent<Image>();
            else
                pic = PSDImportUtility.LoadAndInstant<Image>(PSD2UGUIConfig.ASSET_PATH_IMAGE, psImage.name, parent);

            RectTransform rectTransform = pic.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            pic.sprite = psImage.LoadAssetAtPath<Sprite>();
            pic.name = psImage.name;

            if (psImage.opacity > -1)
            {
                var color = pic.color;
                color.a = psImage.opacity / 100f;
                pic.color = color;
            }

            rectTransform.sizeDelta = new Vector2(psImage.size.width, psImage.size.height);
            rectTransform.anchoredPosition = new Vector2(psImage.position.x, psImage.position.y);
        }
    }
}