using System;

public class Light : Component
{
    public Color color { get; set; } = new Color(1, 1, 1);
    public float range { get; set; } = 10f;

    public float GetLightAttenuation(Vector3 pos)
    {
        float linearRange = Mathf.Clamp01(1 - (Vector3.Distance(gameObject.transform.position, pos) / range));
        float expRange = linearRange * linearRange;
        return expRange;
    }
    public Color GetComputedLightColor(Vector3 pos)
    {
        return color * GetLightAttenuation(pos);
    }
}