using System;
public class Camera : Component
{
    public const float DIST_PLANE = 100f;

    public static Camera main
    {
        get
        {
            Camera[] cameras = GameObject.GetAllComponents<Camera>();

            if (cameras.Length > 0)
                return cameras[0];
            else
                return null;
        }
    }

    private float computedTanFov;

    private float _fov = 75f;
    public float fov
    {
        get
        {
            return _fov;
        }
        set
        {
            _fov = value;
            computedTanFov = (float)Math.Tan(_fov);
        }
    }

    public Camera()
    {
        computedTanFov = (float)Math.Tan(_fov);
    }

    public Ray ScreenPointToRay(int x, int y)
    {
        float d = (float)(Game.width / (DIST_PLANE * computedTanFov));

        Vector3 pixel_world = new Vector3(DIST_PLANE, (x - Game.width / 2) / d, (y - Game.height / 2) / d);
        Vector3 ray = new Vector3(pixel_world.x - transform.position.x, pixel_world.y - transform.position.y, pixel_world.z - transform.position.z);

        return new Ray(transform.position, ray.normalized);
    }
    public Ray ScreenPointToRay(float x, float y)
    {
        float d = (float)(Game.width / (DIST_PLANE * computedTanFov));

        Vector3 pixel_world = new Vector3(DIST_PLANE, (x - Game.width / 2) / d, (y - Game.height / 2) / d);
        Vector3 ray = new Vector3(pixel_world.x - transform.position.x, pixel_world.y - transform.position.y, pixel_world.z - transform.position.z);

        return new Ray(transform.position, ray.normalized);
    }
}