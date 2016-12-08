using FastBitmapLib;
using Math = System.Math;
using System.Drawing;
using System.Threading.Tasks;
public static class Scene1
{
    public static readonly Color clearColor = new Color(0, 0, 0);

    public static Camera camera;
    public static GameObject cameraObject = new GameObject("Camera");
    public static GameObject plane = new GameObject("Plane");
    public static GameObject plane2 = new GameObject("Plane2");
    public static GameObject[] spheres = new GameObject[5];
    public static GameObject light1 = new GameObject("Light");
    public static GameObject light2 = new GameObject("Light2");

    static Renderer[] renderers;
    static Light[] lights;

    static FastBitmap f;
    static double[,] depthBuffer;

    static Material.Shader defaultShader = (i) =>
    {
        float diffuseDot = Mathf.Clamp01(Vector3.Dot(i.normal, i.lightDir));
        Color finalDiffuse = i.lightInfo.GetComputedLightColor(i.position) * i.material.color * diffuseDot;
        finalDiffuse *= (i.material.texture != null ? (Color)i.material.texture.GetPixel(i.uv.x, i.uv.y) : new Color(1, 1, 1));
        float glossDot = Mathf.Clamp01(Vector3.Dot(Vector3.Reflect(i.viewDir, i.normal), i.lightDir));
        glossDot = (float)Math.Pow(glossDot, (i.material.gloss * i.material.gloss) * 2048) * i.material.gloss;
        Color finalGloss = i.lightInfo.GetComputedLightColor(i.position) * i.material.color * glossDot;
        return (finalDiffuse * (1 - i.material.metallic)) + finalGloss + (i.reflection * i.material.gloss);
    };

    static Material.Shader debugShader = (i) =>
    {
        return new Color(i.uv.x, i.uv.y, 0);
    };

    public static void Init()
    {
        Material defaultMaterial = new Material(new Color(1, 1, 1), 0.5f, 0f) { texture = RaytracR.Properties.Resources.tile };

        cameraObject.AddComponent<Camera>();
        cameraObject.transform.position = new Vector3(0, -1.75f, 0);

        PlaneRenderer p = plane.AddComponent<PlaneRenderer>();
        p.material = defaultMaterial;
        p.normal = new Vector3(0, 0, -1);
        plane.transform.position = new Vector3(0, 0, 2);

        PlaneRenderer p2 = plane2.AddComponent<PlaneRenderer>();
        p2.material = defaultMaterial;
        p2.normal = new Vector3(1, 0, 0);
        plane2.transform.position = new Vector3(-30, 0, 0);

        for (int i = 0; i < spheres.Length; i++)
        {
            GameObject sphere = new GameObject("Sphere");
            SphereRenderer sphereRenderer = sphere.AddComponent<SphereRenderer>();
            sphereRenderer.material = new Material(new Color(Mathf.Random(0.5f, 1f), Mathf.Random(0.5f, 1f), Mathf.Random(0.5f, 1f)), 0.5f, 0f);
            sphereRenderer.radius = 0.5f;
            sphereRenderer.transform.position = new Vector3(-10, -i, 0);
            spheres[i] = sphere;
        }

        Light l1 = light1.AddComponent<Light>();
        l1.range = 120;
        l1.color = new Color(1, 1, 1);
        light1.transform.position = new Vector3(10, 10, -10);
        //Light l2 = light2.AddComponent<Light>();
        //l2.range = 120;
        //l2.color = new Color(0.1f, 0.5f, 1);
        //light2.transform.position = new Vector3(-20, -5, -10);

        camera = Camera.main;
        camera.fov = (float)Math.PI - (float)Math.PI * 0.33f;
    }

    public static Bitmap Render(int width, int height)
    {
        // INITIALIZATION
        Game.width = width;
        Game.height = height;

        f = new FastBitmap(width, height);
        depthBuffer = new double[width, height];

        renderers = GameObject.GetAllComponents<Renderer>();
        lights = GameObject.GetAllComponents<Light>();

        // RENDER

        Parallel.For(0, height, (y) => 
        {
            Parallel.For(0, width, (x) =>
            {
                RenderPixel(x, y);
            });
        });

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        RenderPixel(x, y);
        //    }
        //}

        return f;
    }

    public static void RenderPixel(int x, int y)
    {
        depthBuffer[x, y] = double.MaxValue; // Set depth buffer default value to the max.

        f.SetPixel(x, y, clearColor);

        foreach (Renderer renderer in renderers)
        {
            Ray ray = camera.ScreenPointToRay(x, y);
            RaycastHit hit = Ray.Raytrace(ray, renderer);

            if (hit.intersection >= 0 && hit.intersection < depthBuffer[x, y])
            {
                depthBuffer[x, y] = hit.intersection;

                Vector3 hitPos = ray.origin + ray.direction * -(float)hit.intersection;
                Vector2 uv = hitPos / 3f;

                Color finalColor = new Color(0, 0, 0);

                foreach (Light light in lights)
                {
                    Vector3 lightDir = (hitPos - light.transform.position).normalized;
                    Vector3 viewDir = (camera.transform.position - hitPos).normalized;

                    float shadow = 0f;

                    foreach (Renderer renderer2 in renderers) // SHADOW PASS
                    {
                        if (renderer == renderer2)
                            continue;

                        Ray ray2 = new Ray(hitPos, lightDir);
                        RaycastHit hit2 = Ray.Raytrace(ray2, renderer2);

                        if (hit2.intersection >= 0)
                        {
                            shadow += (float)hit2.det * 3f;
                        }
                    }

                    shadow = Mathf.Clamp01(shadow);

                    Color reflection = new Color(0, 0, 0);
                    double reflectionDepthBuffer = double.MaxValue;

                    foreach (Renderer renderer3 in renderers)
                    {
                        if (renderer == renderer3)
                            continue;

                        Ray ray3 = new Ray(hitPos, Vector3.Reflect(viewDir, hit.normal));
                        RaycastHit hit3 = Ray.Raytrace(ray3, renderer3);

                        if (hit3.intersection >= 0 && hit3.intersection < reflectionDepthBuffer)
                        {
                            reflectionDepthBuffer = hit3.intersection;

                            Vector3 hitPos2 = ray3.origin + ray3.direction * -(float)hit3.intersection;
                            Vector2 uv2 = hitPos2 / 3f;

                            Vector3 lightDir2 = (hitPos2 - light.transform.position).normalized;
                            Vector3 viewDir2 = (camera.transform.position - hitPos2).normalized;

                            reflection = defaultShader(new Material.ShaderInfo() { position = hitPos2, normal = hit3.normal, material = renderer3.material, lightInfo = light, lightDir = lightDir2, viewDir = viewDir2, uv = uv2 });
                        }
                    }

                    Color shaderContrib = defaultShader(new Material.ShaderInfo() { position = hitPos, normal = hit.normal, material = renderer.material, lightInfo = light, lightDir = lightDir, viewDir = viewDir, reflection = reflection, uv = uv });

                    finalColor += shaderContrib * (1 - shadow);
                }

                f.SetPixel(x, y, finalColor);
            }
        }
        
    }

    public static void RenderPixel(int x, int y, int antialias)
    {
        depthBuffer[x, y] = double.MaxValue; // Set depth buffer default value to the max.

        Color[] samples = new Color[antialias * antialias];

        for (int x2 = 0; x2 < antialias; x2++)
        {
            for (int y2 = 0; y2 < antialias; y2++)
            {
                foreach (Renderer renderer in renderers)
                {
                    Ray ray = camera.ScreenPointToRay(x + (1 / antialias * x2), y + (1 / antialias * y2));
                    RaycastHit hit = Ray.Raytrace(ray, renderer);

                    if (hit.intersection >= 0 && hit.intersection <= depthBuffer[x, y])
                    {
                        depthBuffer[x, y] = hit.intersection;

                        Vector3 hitPos = ray.origin + ray.direction * -(float)hit.intersection;
                        Vector2 uv = hitPos / 3f;

                        Color finalColor = new Color(0, 0, 0);

                        foreach (Light light in lights)
                        {
                            Vector3 lightDir = (hitPos - light.transform.position).normalized;
                            Vector3 viewDir = (camera.transform.position - hitPos).normalized;

                            float shadow = 0f;

                            foreach (Renderer renderer2 in renderers) // SHADOW PASS
                            {
                                if (renderer == renderer2)
                                    continue;

                                Ray ray2 = new Ray(hitPos, lightDir);
                                RaycastHit hit2 = Ray.Raytrace(ray2, renderer2);

                                if (hit2.intersection >= 0)
                                {
                                    shadow += (float)hit2.det * 3f;
                                }
                            }

                            shadow = Mathf.Clamp01(shadow);

                            Color reflection = new Color(0, 0, 0);
                            double reflectionDepthBuffer = double.MaxValue;

                            foreach (Renderer renderer3 in renderers)
                            {
                                if (renderer == renderer3)
                                    continue;

                                Ray ray3 = new Ray(hitPos, Vector3.Reflect(viewDir, hit.normal));
                                RaycastHit hit3 = Ray.Raytrace(ray3, renderer3);

                                if (hit3.intersection >= 0 && hit3.intersection < reflectionDepthBuffer)
                                {
                                    reflectionDepthBuffer = hit3.intersection;

                                    Vector3 hitPos2 = ray3.origin + ray3.direction * -(float)hit3.intersection;
                                    Vector2 uv2 = hitPos2 / 3f;

                                    Vector3 lightDir2 = (hitPos2 - light.transform.position).normalized;
                                    Vector3 viewDir2 = (camera.transform.position - hitPos2).normalized;

                                    reflection = defaultShader(new Material.ShaderInfo() { position = hitPos2, normal = hit3.normal, material = renderer3.material, lightInfo = light, lightDir = lightDir2, viewDir = viewDir2, uv = uv2 });
                                }
                            }

                            Color shaderContrib = defaultShader(new Material.ShaderInfo() { position = hitPos, normal = hit.normal, material = renderer.material, lightInfo = light, lightDir = lightDir, viewDir = viewDir, reflection = reflection, uv = uv });

                            finalColor += shaderContrib * (1 - shadow);
                        }

                        samples[x2 * antialias + y2] = finalColor;
                    }
                }
            }

            Color antialiasedColor = new Color(0, 0, 0);

            foreach (Color c in samples)
            {
                antialiasedColor += c;
            }
            antialiasedColor /= samples.Length;

            f.SetPixel(x, y, antialiasedColor);
        }
    }
}