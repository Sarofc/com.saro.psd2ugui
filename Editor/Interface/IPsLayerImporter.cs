using UnityEngine;


namespace PSDUIImporter
{
    public interface IPsLayerImporter
    {
        PSDImportCtrl ctrl { get; set; }
        void DrawPsLayer(PsLayer layer, GameObject parent);
    }
}
