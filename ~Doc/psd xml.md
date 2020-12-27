# psd xml

Xml反序列化为PSDUI对象。

```csharp

public class PSDUI
{
    public Size psdSize;
    public Layer[] layers;
}

public class Layer
{
    public string name;
    public ELayerType type;
    public Layer[] layers;
    public string[] arguments;
    //public PSImage[] images;
}

public PSImage image;
{
    public Size size;
    public Position position;
}

public class PSImage
{
    public EImageType imageType;
    public ImageSource imageSource;
    public string name;
    public Position position;
    public Size size;
    public string[] arguments;
}

public struct Position
{
    public float x;
    public float y;
}

public struct Size
{
    public float width;
    public float height;
}

public enum ELayerType
{
    Panel,
    Normal,
    ScrollView,
    Grid,
    Button,
    Lable,
    Toggle,
    Slider,
    Group,
    InputField,
    ScrollBar,
}

```

xml大概的规则就是：

```xml

<PSDUI>
    <psdSize>
        <width></width>
        <height></height>
    </psdSize>
    <layers>
        <Layer>
            <name></name>
            <type></type>
            <layers></layers>
            <arguments></arguments>
            <image></image>
            <size></size>
            <position></position>
        </Layer>
        <Layer>
            <name></name>
            <type></type>
            <layers></layers>
            <arguments></arguments>
            <image></image>
            <size></size>
            <position></position>
        </Layer>
        ...
    </layers>
</PSDUI>

```
