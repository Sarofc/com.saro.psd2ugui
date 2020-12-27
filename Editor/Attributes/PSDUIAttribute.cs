using System;

namespace PSDUIImporter
{
    public class CustomLayerAttribute : Attribute
    {
        private ELayerType m_LayerType;

        public ELayerType LayerType { get => m_LayerType; set => m_LayerType = value; }

        public CustomLayerAttribute(ELayerType layerType)
        {
            m_LayerType = layerType;
        }
    }

    public class CustomImageAttribute : Attribute
    {
        private EImageType m_ImageType;

        public EImageType ImageType { get => m_ImageType; set => m_ImageType = value; }

        public CustomImageAttribute(EImageType imageType)
        {
            m_ImageType = imageType;
        }
    }
}
