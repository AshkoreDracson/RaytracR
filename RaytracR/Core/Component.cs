public abstract class Component
{
    public GameObject gameObject { get; internal set; }
    public Transform transform
    {
        get
        {
            return gameObject.transform;
        }
    }

    public Component()
    {
        this.gameObject = gameObject;
    }
}