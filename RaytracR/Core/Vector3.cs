using System;
public struct Vector3
{
    // VARIABLES & PROPERTIES

    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public float sqrMagnitude
    {
        get
        {
            return (x * x + y * y + z * z);
        }
    }
    public float magnitude
    {
        get
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
    }
    public Vector3 normalized
    {
        get
        {
            return new Vector3(x / magnitude, y / magnitude, z / magnitude);
        }
    }

    public static Vector3 zero
    {
        get
        {
            return new Vector3(0, 0, 0);
        }
    }
    public static Vector3 one
    {
        get
        {
            return new Vector3(1, 1, 1);
        }
    }
    public static Vector3 down
    {
        get
        {
            return new Vector3(0, 1, 0);
        }
    }
    public static Vector3 right
    {
        get
        {
            return new Vector3(1, 0, 0);
        }
    }
    public static Vector3 forward
    {
        get
        {
            return new Vector3(0, 0, 1);
        }
    }

    // STRUCT INSTANCE CONSTRUCTOR

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // OPERATORS

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static Vector3 operator *(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    public static Vector3 operator /(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    public static Vector3 operator +(Vector3 a, float b)
    {
        return new Vector3(a.x + b, a.y + b, a.z + b);
    }
    public static Vector3 operator -(Vector3 a, float b)
    {
        return new Vector3(a.x - b, a.y - b, a.z - b);
    }
    public static Vector3 operator *(Vector3 a, float b)
    {
        return new Vector3(a.x * b, a.y * b, a.z * b);
    }
    public static Vector3 operator /(Vector3 a, float b)
    {
        return new Vector3(a.x / b, a.y / b, a.z / b);
    }
    public static Vector3 operator -(Vector3 a)
    {
        return new Vector3(-a.x, -a.y, -a.z);
    }
    public static bool operator ==(Vector3 a, Vector3 b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return true;
        return false;
    }
    public static bool operator !=(Vector3 a, Vector3 b)
    {
        if (a.x != b.x || a.y != b.y || a.z != b.z)
            return true;
        return false;
    }
    public static implicit operator Vector2(Vector3 a)
    {
        return new Vector2(a.x, a.y);
    }

    // METHODS & PROPERTIES

    public void Normalize()
    {
        float magnitude = this.magnitude;
        this.x /= magnitude;
        this.y /= magnitude;
        this.z /= magnitude;
    }

    // STATIC METHODS & PROPERTIES

    public static float Dot(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }
    public static float Distance(Vector3 a, Vector3 b)
    {
        return new Vector3(b.x - a.x, b.y - a.y, b.z - a.z).magnitude;
    }
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
    }
    public static Vector3 Reflect(Vector3 a, Vector3 normal)
    {
        return a - (normal * (2 * Dot(a, normal)));
    }

    // OVERRIDES

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        return "{ x=" + this.x + ", y=" + this.y + ", z=" + this.z + " }";
    }
}