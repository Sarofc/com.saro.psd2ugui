using UnityEngine;


namespace PSDUIImporter
{
    public interface IPsImageImporter
    {
        void DrawPsImage(PsImage image, GameObject parent, GameObject ownObj = null);
    }
}
