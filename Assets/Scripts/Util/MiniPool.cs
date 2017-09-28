using UnityEngine;
using System.Collections.Generic;

public class MiniPool : MonoBehaviour
{
    private Transform root;
    private Stack<GameObject> itemStack;
    private GameObject itemPfb;

    private void SetMiniPool(Transform hideItemRoot, GameObject itemPfb)
    {
        root = hideItemRoot;
        itemStack = new Stack<GameObject>();
        this.itemPfb = itemPfb;
        root.gameObject.SetActive(false);
    }

    public static MiniPool MakeMiniPool(Transform hideItemRoot, GameObject itemPfb)
    {
        GameObject obj = new GameObject("MiniPool");
        MiniPool minipool = obj.AddComponent<MiniPool>();
        minipool.SetMiniPool(hideItemRoot, itemPfb);
        return minipool;
    }

    public GameObject GetItem()
    {
        GameObject item;
        if (itemStack.Count > 0)
            item = itemStack.Pop();
        else
        {
            item = MonoBehaviour.Instantiate(itemPfb) as GameObject;
            item.name = itemPfb.name;
        }

        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.one;

        return item;
    }

    public GameObject GetItem(Transform parent)
    {
        GameObject item = GetItem();
        item.transform.parent = parent;
        item.transform.localScale = Vector3.one;

        return item;
    }

    public GameObject GetItem(Transform parent, Vector3 localPos)
    {
        GameObject item = GetItem(parent);
        item.transform.localPosition = localPos;

        return item;
    }

    public void ReturnItem(GameObject item)
    {
        if (item != null && item.gameObject != null)
        {
            itemStack.Push(item);

            item.transform.parent = root;
            item.gameObject.SetActive(false);
        }
        else
        {
            MonoBehaviour.Destroy(item);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0, max = itemStack.Count; i < max; i++)
        {
            GameObject item = itemStack.Pop();

            if (item != null)
                MonoBehaviour.Destroy(item.gameObject);
        }
        itemStack.Clear();
    }
}

public class MiniPool<T> where T : Component
{
    private Transform root;
    private Stack<T> itemStack;
    private GameObject itemPfb;

    public static MiniPool<T> MakeMiniPool(Transform hideItemRoot, GameObject itemPfb)
    {
        MiniPool<T> miniPool = new MiniPool<T>();
        miniPool.SetMiniPool(hideItemRoot, itemPfb);
        return miniPool;
    }
    
    private void SetMiniPool(Transform hideItemRoot, GameObject itemPfb)
    {
        root = hideItemRoot;
        itemStack = new Stack<T>();
        this.itemPfb = itemPfb;
        root.gameObject.SetActive(false);
    }

    public void WarmUp(int count)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < count; i++)
            list.Add(GetItem());

        for (int i = 0; i < count; i++)
            ReturnItem(list[i]);
        list.Clear();
        list = null;
    }

    public T GetItem()
    {
        return GetItem(null);
    }

    public T GetItem(Transform parent)
    {
        T item;
        if (itemStack.Count > 0)
            item = itemStack.Pop();
        else
        {
            GameObject obj = MonoBehaviour.Instantiate(itemPfb) as GameObject;
            obj.name = itemPfb.name;
            item = obj.GetComponent<T>();
        }

        item.transform.parent = parent;
        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.one;

        return item;
    }

    public T GetItem(Transform parent, Vector3 localPos)
    {
        T item = GetItem(parent);
        item.transform.localPosition = localPos;

        return item;
    }

    public void ReturnItem(T item)
    {
        if (item != null && item.gameObject != null)
        {
            itemStack.Push(item);

            item.transform.parent = root;
            item.gameObject.SetActive(false);
        }
        else
        {
            MonoBehaviour.Destroy(item);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0, max = itemStack.Count; i < max; i++)
        {
            T item = itemStack.Pop();

            if (item != null && item.gameObject != null)
                MonoBehaviour.Destroy(item.gameObject);
        }
        itemStack.Clear();
    }
}

public class PoolItem
{
    public static PoolItem CreateItem() { return new PoolItem(); }
    public virtual void Init() { }
}

public class MiniPoolClass<T> where T : PoolItem
{
    private Stack<T> itemStack = new Stack<T>();

    public static MiniPoolClass<T> MakeMiniPool()
    {
        MiniPoolClass<T> miniPool = new MiniPoolClass<T>();
        return miniPool;
    }
    
    public void WarmUp(int count)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < count; i++)
            list.Add(GetItem());

        for (int i = 0; i < count; i++)
            ReturnItem(list[i]);
        list.Clear();
        list = null;
    }
    
    public T GetItem()
    {
        T item;
        if (itemStack.Count > 0)
            item = itemStack.Pop();
        else
            item = PoolItem.CreateItem() as T;

        return item;
    }

    public void ReturnItem(T item)
    {
        if (item != null)
            itemStack.Push(item);
    }

    private void OnDestroy()
    {
        for (int i = 0, max = itemStack.Count; i < max; i++)
        {
            itemStack.Pop();
        }
        itemStack.Clear();
    }
}