using System;
public struct Vector2
{
    // VARIABLES & PROPERTIES

    public float x { get; set; }
    public float y { get; set; }

    public float sqrMagnitude
    {
        get
        {
            return (x * x + y * y);
        }
    }
    public float magnitude
    {
        get
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
    }
    public Vector2 normalized
    {
        get
        {
            return new Vector2(x / magnitude, y / magnitude);
        }
    }

    public static Vector2 zero
    {
        get
        {
            return new Vector2(0, 0);
        }
    }
    public static Vector2 one
    {
        get
        {
            return new Vector2(1, 1);
        }
    }
    public static Vector2 down
    {
        get
        {
            return new Vector2(0, 1);
        }
    }
    public static Vector2 right
    {
        get
        {
            return new Vector2(1, 0);
        }
    }

    // STRUCT INSTANCE CONSTRUCTOR

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // OPERATORS

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }
    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }
    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }
    public static Vector2 operator /(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x / b.x, a.y / b.y);
    }
    public static Vector2 operator +(Vector2 a, float b)
    {
        return new Vector2(a.x + b, a.y + b);
    }
    public static Vector2 operator -(Vector2 a, float b)
    {
        return new Vector2(a.x - b, a.y - b);
    }
    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.x * b, a.y * b);
    }
    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.x / b, a.y / b);
    }
    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.x, -a.y);
    }
    public static bool operator ==(Vector2 a, Vector2 b)
    {
        if (a.x == b.x && a.y == b.y)
            return true;
        return false;
    }
    public static bool operator !=(Vector2 a, Vector2 b)
    {
        if (a.x != b.x || a.y != b.y)
            return true;
        return false;
    }
    public static implicit operator Vector3(Vector2 a)
    {
        return new Vector3(a.x, a.y, 0);
    }

    // METHODS & PROPERTIES

    public void Normalize()
    {
        float magnitude = this.magnitude;
        if (magnitude != 0)
        {
            this.x /= magnitude;
            this.y /= magnitude;
        }
    }

    // STATIC METHODS & PROPERTIES

    public static float Dot(Vector2 a, Vector2 b)
    {
        return (a.x * b.x) + (a.y * b.y);
    }
    public static float Distance(Vector2 a, Vector2 b)
    {
        return new Vector2(b.x - a.x, b.y - a.y).magnitude;
    }
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return new Vector2(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t));
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
        return "{ x=" + this.x + ", y=" + this.y + " }";
    }
}