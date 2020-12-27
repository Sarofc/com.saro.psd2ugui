using System;
using UnityEngine;

namespace PSDUIImporter
{
    // TODO
    //[CustomLayer(LayerType.DropDown)]
    public class DropDownLayerImport : ILayerImport
    {
        public PSDImportCtrl ctrl { get; set; }
        public DropDownLayerImport()
        {

        }
        public void DrawLayer(Layer layer, GameObject parent)
        {
            throw new NotImplementedException();
        }
    }
}
