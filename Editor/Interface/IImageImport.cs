using UnityEngine;


namespace PSDUIImporter
{
    public interface IImageImport
    {
        void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null);
    }
}
