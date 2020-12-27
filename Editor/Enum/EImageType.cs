
namespace PSDUIImporter
{
    [System.Flags]
    public enum EImageType
    {
        Image = 1,
        Texture = 1 << 2,
        Label = 1 << 3,
        SliceImage = 1 << 4,


        LeftHalfImage = 1 << 5,
        BottomHalfImage = 1 << 6,
        QuarterImage = 1 << 7,

        MirrorImage = LeftHalfImage | BottomHalfImage | QuarterImage,
    }
}