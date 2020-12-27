using UnityEngine;


namespace PSDUIImporter
{
    public interface ILayerImport
    {
        PSDImportCtrl ctrl { get; set; }
        void DrawLayer(Layer layer, GameObject parent);
    }
}
