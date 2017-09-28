using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                DontDestroyOnLoad(instance.gameObject);
                instance.Awake();
            }
            return instance;
        }
    }

    private bool bInit = false;

    public static void Destroy()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance.Destructor();
            instance = null;
        }
    }

    private void Awake()
    {
        if (bInit)
        {
            if(instance != this)
                Destroy(gameObject);
            return;
        }

        bInit = true;
        Init();
    }

    protected virtual void Init()
    {
    }

    protected virtual void Destructor()
    {
    }
}