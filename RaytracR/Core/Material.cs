using FastBitmapLib;
public class Material
{
    public Color color { get; set; } = new Color(1, 1, 1);
    public FastBitmap texture { get; set; }
    public float gloss { get; set; } = 0.5f;
    public float metallic { get; set; } = 0f;

    public struct ShaderInfo
    {
        public Material material { get; set; }
        public Color reflection { get; set; }
        public Color ambientLight { get; set; }
        public Vector3 position { get; set; }
        public Vector3 normal { get; set; }
        public Light lightInfo { get; set; }
        public Vector3 lightDir { get; set; }
        public Vector3 viewDir { get; set; }
        public Vector2 uv { get; set; }
    }
    public delegate Color Shader(ShaderInfo i);

    public Material(Color color, float gloss, float metallic)
    {
        this.color = color;
        this.gloss = gloss;
        this.metallic = metallic;
    }
}