public struct RaycastHit
{
    public bool intersects { get; set; }
    public double intersection { get; set; }
    public double distance { get; set; }
    public double det { get; set; }
    public Vector3 normal { get; set; }

    public RaycastHit(bool intersects, double intersection, double distance, double det, Vector3 normal)
    {
        this.intersects = intersects;
        this.intersection = intersection;
        this.distance = distance;
        this.det = det;
        this.normal = normal;
    }
}