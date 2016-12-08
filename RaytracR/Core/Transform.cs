using System;

public class Transform : Component
{
    public Vector3 position { get; set; } = Vector3.zero;
    public Vector3 rotation { get; set; } = Vector3.zero;
    public Vector3 scale { get; set; } = Vector3.one;
}