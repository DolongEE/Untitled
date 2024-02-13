
using UnityEngine;

public class ResourcesManager
{
    public void Init()
    {

    }

    
    public T LoadResource<T>(string path) where T : Component
    {
        T obj = Resources.Load<T>(path);

        return obj;
    }
}
