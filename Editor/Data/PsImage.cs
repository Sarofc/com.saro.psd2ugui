namespace PSDUIImporter
{
    public class PsImage
    {
        public EImageType imageType;
        public EImageSource imageSource;
        public string name;
        public Position position;
        public Size size;

        /// <summary>
        /// args for userData. see <see cref="UguiTextImporter"/> etc.
        /// </summary>
        public string[] args;

        /// <summary>
        /// 图层透明度
        /// </summary>
        public float opacity = -1;
    }
}