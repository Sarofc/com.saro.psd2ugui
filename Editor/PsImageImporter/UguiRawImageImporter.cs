using UnityEngine;
using UnityEngine.UI;

namespace PSDUIImporter
{
    [CustomImage(EImageType.Texture)]
    public class UguiRawImageImporter : IPsImageImporter
    {
        public void DrawPsImage(PsImage psImage, GameObject parent, GameObject ownObj = null)
        {
            RawImage pic;
            if (ownObj != null)
                pic = ownObj.AddMissingComponent<RawImage>();
            else
                pic = PSDImportUtility.LoadAndInstant<RawImage>(PSD2UGUIConfig.ASSET_PATH_RAWIMAGE, psImage.name, parent);

            Texture2D texture = psImage.LoadAssetAtPath<Texture2D>();

            pic.texture = texture;

            if (psImage.opacity > -1)
            {
                var color = pic.color;
                color.a = psImage.opacity / 100f;
                pic.color = color;
            }

            RectTransform rectTransform = pic.GetComponent<RectTransform>();
            PSDImportUtility.SetAnchorMiddleCenter(rectTransform);
            rectTransform.sizeDelta = new Vector2(psImage.size.width, psImage.size.height);
            rectTransform.anchoredPosition = new Vector2(psImage.position.x, psImage.position.y);
        }
    }
}
