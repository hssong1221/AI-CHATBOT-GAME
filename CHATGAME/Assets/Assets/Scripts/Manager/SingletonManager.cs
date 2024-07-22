using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager _instance;
    private Dictionary<System.Type, MonoBehaviour> _singletons = new Dictionary<System.Type, MonoBehaviour>();
    public static SingletonManager Instance
    {
        get
        {
            if(_instance == null )
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<SingletonManager>();
                singletonObject.name = typeof(SingletonManager).ToString() + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as SingletonManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RegisterSingleton<T>(T instance) where T : MonoBehaviour
    {
        var type = typeof(T);
        if (!_singletons.ContainsKey(type))
        {
            _singletons[type] = instance;
        }
        else
        {
            Debug.LogWarning("Singleton of type " + type + " is already registered.");
        }
    }

    public T GetSingleton<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        if (_singletons.TryGetValue(type, out MonoBehaviour singleton))
        {
            return (T)singleton;
        }
        else
        {
            Debug.LogWarning("No singleton of type " + type + " is registered.");
            return null;
        }
    }
}
