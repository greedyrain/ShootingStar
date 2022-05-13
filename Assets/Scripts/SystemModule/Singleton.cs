using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{    
    private static T instance;
    public static T Instance => instance; 

    private void Awake()
    {
        if (instance == null)
            instance = this as T;
    }
}

public class BaseManager<T> where T : new()
{
    private static T instance = new T();
    public static T Instance => instance;
    public BaseManager()
    {
        
    }
}