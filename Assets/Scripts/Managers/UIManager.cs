using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public enum UIType { GUI, Dialog, Popup }

    private List<IGUI> uiList;
    [SerializeField] private bool bHideAllGUI = false;
    [SerializeField] private GameObject popupBlocker;
    [SerializeField] private List<BoxCollider> uiInputBlocker;

    public static UIManager Instance { get; private set; }
    
    private Dictionary<Type, IGUI> guiDic = new Dictionary<Type, IGUI>();
    private Dictionary<Type, IGUI> popupDic = new Dictionary<Type, IGUI>();
    private Type openedPopupType = null;

    private Vector2 screenSize;
    private Vector2 realScreenSize;
    
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
        
        Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Init()
    {
        guiDic.Clear();
        popupDic.Clear();

        Dictionary<Type, IGUI> uiDic;

        uiList = new List<IGUI>();
        foreach (IGUI ui in transform.GetComponentsInChildren<IGUI>(true))
            uiList.Add(ui);

        for (int i = 0, max = uiList.Count; i < max; i++)
        {
            Type type = uiList[i].GetType();
            UIType uiType = uiList[i].GetUIType();

            if (uiType == UIType.GUI)
                uiDic = guiDic;
            else
                uiDic = popupDic;

            if (uiDic.ContainsKey(type))
                Debug.Log("something is wrong : " + type.ToString());
            else
            {
                uiList[i].SetLink();
                uiList[i].Init();
                uiDic.Add(type, uiList[i]);
                if (bHideAllGUI)
                {
                    uiList[i].HideGUI();
                    openedPopupType = null;
                }
                else
                {
                    if (uiType == UIType.Popup && openedPopupType == null && uiList[i].IsShow())
                        openedPopupType = type;
                }   
            }
        }

        ShowPopupBlocker(openedPopupType);

        CanvasScaler root = GetComponent<CanvasScaler>();
        
        if (root)
        {
            screenSize = root.referenceResolution;
            //root.matchWidthOrHeight
            //realScreenSize.x = Mathf.RoundToInt(Screen.width / (float)Screen.height * root.activeHeight);
            //realScreenSize.y = root.activeHeight;
        }
    }

    public void ShowUI<T>() where T : IGUI
    {
        Type type = typeof(T);

        if (guiDic.ContainsKey(type))
        {
            guiDic[type].ShowGUI();
        }   
        else if (popupDic.ContainsKey(type))
        {
            if (openedPopupType == type)
                Debug.LogError("It's already opened!");
            else if (openedPopupType != null)
                Debug.LogError("There is a popup what already opened!!");
            else
            {
                popupDic[type].ShowGUI();
                ShowPopupBlocker(type);
            }
        }
        else
        {
            Debug.Log("there is no ui - " + type.ToString());
        }
    }

    public void ShowUIImmediately<T>() where T : IGUI
    {
        Type type = typeof(T);

        if (guiDic.ContainsKey(type))
        {
            guiDic[type].ShowGUIImmediately();
        }
        else if (popupDic.ContainsKey(type))
        {
            if (openedPopupType == type)
                Debug.LogError("It's already opened!");
            else if (openedPopupType != null)
                Debug.LogError("There is a popup what already opened!!");
            else
            {
                popupDic[type].ShowGUIImmediately();
                ShowPopupBlocker(type);
            }
        }
        else
        {
            Debug.Log("there is no ui - " + type.ToString());
        }
    }

    public void HideUI<T>() where T : IGUI
    {
        Type type = typeof(T);
        if (guiDic.ContainsKey(type))
            guiDic[type].HideGUI();
        else if (popupDic.ContainsKey(type))
        {
            if (openedPopupType == null)
                Debug.LogError("any popup is not opened now. check it please. : " + typeof(T).ToString());
            else if (openedPopupType != type)
                Debug.LogError("It's not the popup what is opened now");
            else
            {
                popupDic[type].HideGUI();
                ShowPopupBlocker(null);
            }
        }
        else
        {
            Debug.Log("there is no ui - " + type.ToString());
        }
    }

    public void HideUIImmediately<T>() where T : IGUI
    {
        Type type = typeof(T);
        if (guiDic.ContainsKey(type))
            guiDic[type].HideGUIImmediately();
        else if (popupDic.ContainsKey(type))
        {
            if (openedPopupType == null)
                Debug.LogError("any popup is not opened now. check it please. : " + typeof(T).ToString());
            else if (openedPopupType != type)
                Debug.LogError("It's not the popup what is opened now");
            else
            {
                popupDic[type].HideGUIImmediately();
                ShowPopupBlocker(null);
            }
        }
        else
        {
            Debug.Log("there is no ui - " + type.ToString());
        }
    }

    public T GetUI<T>() where T : IGUI
    {
        Type type = typeof(T);
        if (guiDic.ContainsKey(type))
            return guiDic[type] as T;
        else if (popupDic.ContainsKey(type))
            return popupDic[type] as T;
        return null;
    }

    public bool IsShow<T>() where T : IGUI
    {
        Type type = typeof(T);
        if (guiDic.ContainsKey(type))
            return guiDic[type].IsShow();
        else if (popupDic.ContainsKey(type))
            return popupDic[type].IsShow();

        return false;
    }

    public void AllHidePopup()
    {
        for (int i = 0, max = uiList.Count; i < max; i++)
        {
            if (uiList[i].GetUIType() == UIType.Popup && uiList[i].IsShow())
                uiList[i].HideGUI();
        }
        ShowPopupBlocker(null);
    }

    public void ShowPopupBlocker(Type openedPopupType)
    {
        this.openedPopupType = openedPopupType;

        if (popupBlocker != null)
            popupBlocker.SetActive(openedPopupType != null);
    }

    public Vector2 GetScreenSize()
    {
        return screenSize;
    }

    public Vector2 GetRealScreenSize()
    {
        return realScreenSize;
    }

    public void SetInputBlocker(bool bActive)
    {
        for (int i = 0, max = uiInputBlocker.Count; i < max; i++)
            uiInputBlocker[i].enabled = bActive;
    }
}
