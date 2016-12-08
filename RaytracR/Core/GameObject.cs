using System;
using System.Collections.Generic;
public class GameObject
{
    private static uint curID = 0;
    private static List<GameObject> gameObjects = new List<GameObject>();

    public string name { get; set; }
    public uint id { get; private set; }

    private Transform _transform;
    public Transform transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }

    private List<Component> components = new List<Component>();

    public GameObject(string name)
    {
        this.id = curID++;
        this.name = name;

        AddComponent<Transform>();

        gameObjects.Add(this);
    }

    public void Destroy()
    {
        components.Clear();
        gameObjects.Remove(this);
    }

    public T AddComponent<T>() where T : Component, new()
    {
        T newComp = new T();
        newComp.gameObject = this;
        components.Add(newComp);
        return newComp;
    }
    public T GetComponent<T>() where T : Component
    {
        foreach (Component comp in components)
        {
            if (comp is T) return (T)comp;
        }
        return null;
    }
    public void RemoveComponent<T>() where T : Component
    {
        if (typeof(T) == typeof(Transform))
            return;

        foreach (Component comp in components)
        {
            if (comp is T) components.Remove(comp);
        }
    }

    public static T[] GetAllComponents<T>() where T : Component
    {
        List<T> components = new List<T>();
        foreach (GameObject gameObject in gameObjects)
        {
            T comp = gameObject.GetComponent<T>();

            if (comp != null)
                components.Add(comp);
        }
        return components.ToArray();
    }

    public static GameObject Find(string name)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name == name) return gameObject;
        }
        return null;
    }
}