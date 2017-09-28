using UnityEngine;
using System.Collections.Generic;

public class Util
{
    private static readonly float inv255 = 1f / 255f;
    
    public static Color GetColor(int r, int g, int b)
    {
        return new Color(r * inv255, g * inv255, b * inv255, 1f);
    }

    public static Color GetColor(int r, int g, int b, int a)
    {
        return new Color(r * inv255, g * inv255, b * inv255, a * inv255);
    }

    public static T GetComponent<T>(string path) where T : Component
    {
        return GetComponent<T>(GetComponent(path));
    }

    public static GameObject GetComponent(string path)
    {
        return GameObject.Instantiate(Resources.Load(path)) as GameObject;
    }

    public static T GetComponent<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t == null)
            t = obj.AddComponent<T>();
        return t;
    }

    public static GameObject FindObject(Transform root, string path)
    {
        Transform body;
        body = root.Find(path);
        if (body != null)
            return body.gameObject;

        string[] child = path.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

        int idx = 0;
        int maxIdx = child.Length;

        while (idx < maxIdx)
        {
            Transform ch = root.Find(child[idx]);
            if (ch == null)
            {
                ch = new GameObject(child[idx]).transform;
                ch.parent = root;
            }
            root = ch;
            idx++;
        }
        return root.gameObject;
    }
}
