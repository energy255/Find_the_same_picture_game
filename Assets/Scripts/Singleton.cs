using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    public static T _Instance
    {
        get
        {
            if (Instance == null)
                Instance = FindObjectOfType<T>();

            if (Instance == null)
                Instance = new GameObject(typeof(T).Name).AddComponent<T>();

            return Instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
