using FastBitmapLib;
public struct Color
{
    public float r { get; set; }
    public float g { get; set; }
    public float b { get; set; }

    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public static Color operator +(Color a, Color b)
    {
        return new Color(a.r + b.r, a.g + b.g, a.b + b.b);
    }
    public static Color operator -(Color a, Color b)
    {
        return new Color(a.r - b.r, a.g - b.g, a.b - b.b);
    }
    public static Color operator *(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b);
    }
    public static Color operator /(Color a, Color b)
    {
        return new Color(a.r / b.r, a.g / b.g, a.b / b.b);
    }

    public static Color operator +(Color a, float b)
    {
        return new Color(a.r + b, a.g + b, a.b + b);
    }
    public static Color operator -(Color a, float b)
    {
        return new Color(a.r - b, a.g - b, a.b - b);
    }
    public static Color operator *(Color a, float b)
    {
        return new Color(a.r * b, a.g * b, a.b * b);
    }
    public static Color operator /(Color a, float b)
    {
        return new Color(a.r / b, a.g / b, a.b / b);
    }

    public static implicit operator FastColor(Color a)
    {
        return new FastColor((byte)(Mathf.Clamp01(a.r) * 255), (byte)(Mathf.Clamp01(a.g) * 255), (byte)(Mathf.Clamp01(a.b) * 255));
    }
    public static implicit operator Color(FastColor a)
    {
        return new Color(a.R / 255f, a.G / 255f, a.B / 255f);
    }

    public override string ToString()
    {
        return $"[R:{r}, G:{g}, B:{b}]";
    }
}