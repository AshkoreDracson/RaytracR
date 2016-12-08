using FastBitmapLib;
using System;
public static class FastBitmapExtension
{
    public static FastColor GetPixel(this FastBitmap f, float u, float v)
    {
        u = Math.Abs(u); v = Math.Abs(v);
        u = u % 1f;
        v = v % 1f;

        return f.GetPixel((int)(u * f.Width), (int)(v * f.Height));
    }
}