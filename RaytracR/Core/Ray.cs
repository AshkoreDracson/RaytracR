using System;
public struct Ray
{
    public Vector3 origin { get; set; }
    public Vector3 direction { get; set; }

    public Ray(Vector3 position, Vector3 direction)
    {
        this.origin = position;
        this.direction = direction.normalized;
    }

    public static RaycastHit Raytrace(Ray ray, Renderer renderer)
    {
        if (renderer is SphereRenderer)
            return SphereTrace(ray, (SphereRenderer)renderer);
        else if (renderer is PlaneRenderer)
            return PlaneTrace(ray, (PlaneRenderer)renderer);

        return new RaycastHit();
    }

    public static RaycastHit SphereTrace(Ray ray, SphereRenderer sphereRenderer)
    {
        float r = sphereRenderer.radius;
        Vector3 o = sphereRenderer.transform.position - ray.origin;
        double det, b;

        b = -Vector3.Dot(o, ray.direction);
        det = b * b - Vector3.Dot(o, o) + r * r;

        if (det < 0)
            return new RaycastHit(false, -1, double.MaxValue, det, new Vector3(0, 0, 0)); // -1 to false

        det = Math.Sqrt(det);
        double t1 = b - det;
        double t2 = b + det; // Distance

        Vector3 hitPos = ray.origin + ray.direction * -(float)t1;
        Vector3 normal = (sphereRenderer.transform.position - hitPos).normalized;

        return new RaycastHit(true, t1, t2, det, normal); // t1 to true
    }
    public static RaycastHit PlaneTrace(Ray ray, PlaneRenderer planeRenderer)
    {
        float denom = Vector3.Dot(planeRenderer.normal, ray.direction);

        if (denom > 0)
        {
            Vector3 v = ray.origin - planeRenderer.transform.position;
            float t = Vector3.Dot(planeRenderer.normal, v) / denom;

            return new RaycastHit(true, t, t, denom, -planeRenderer.normal.normalized);
        }

        return new RaycastHit(false, -1, double.MaxValue, denom, -planeRenderer.normal.normalized);
    }
}