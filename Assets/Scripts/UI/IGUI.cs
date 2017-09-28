using UnityEngine;
using System.Collections.Generic;

public abstract class IGUI : MonoBehaviour
{
    [SerializeField] private UIManager.UIType uiType = UIManager.UIType.GUI;

    public GameObject gObject { get; protected set; }

    protected List<IAnimation> showUIAnim;
    protected List<IAnimation> hideUIAnim;
    
    private System.Action beforeOpenCallback, afterOpenCallback;
    private System.Action beforeCloseCallback, afterCloseCallback;
    private int animCount;
    private System.Action finishAnimCallback;

    public void SetLink()
    {
        gObject = gameObject;
    }

    public virtual void Init()
    {
    }

    public virtual void UpdateUI()
    {
    }

    public UIManager.UIType GetUIType()
    {
        return uiType;
    }

    public void ShowGUI()
    {
        if (beforeOpenCallback != null)
            beforeOpenCallback();

        UIManager.Instance.SetInputBlocker(true);
        gObject.SetActive(true);

        finishAnimCallback = () =>
        {
            UIManager.Instance.SetInputBlocker(false);

            if (afterOpenCallback != null)
                afterOpenCallback();
        };

        UpdateUI();

        if (showUIAnim != null && showUIAnim.Count > 0)
        {
            animCount = showUIAnim.Count;

            for (int i = 0; i < animCount; i++)
            {
                showUIAnim[i].StartAnimation(AfterFinishAnim);
            }
        }
        else
        {
            finishAnimCallback();
        }
    }

    public void ShowGUIImmediately()
    {
        if (beforeOpenCallback != null)
            beforeOpenCallback();

        UIManager.Instance.SetInputBlocker(false);
        gObject.SetActive(true);

        UpdateUI();

        if (afterOpenCallback != null)
            afterOpenCallback();
    }

    public void HideGUI()
    {
        if (beforeCloseCallback != null)
            beforeCloseCallback();

        UIManager.Instance.SetInputBlocker(true);

        finishAnimCallback = () =>
        {
            gObject.SetActive(false);
            UIManager.Instance.SetInputBlocker(false);

            if (afterCloseCallback != null)
                afterCloseCallback();
        };

        if (hideUIAnim != null && hideUIAnim.Count > 0)
        {
            animCount = hideUIAnim.Count;

            for (int i = 0; i < animCount; i++)
            {
                hideUIAnim[i].StartAnimation(AfterFinishAnim);
            }
        }
        else
        {
            finishAnimCallback();
        }
    }

    public void HideGUIImmediately()
    {
        if (beforeCloseCallback != null)
            beforeCloseCallback();

        UIManager.Instance.SetInputBlocker(false);
        gObject.SetActive(false);

        if (afterCloseCallback != null)
            afterCloseCallback();
    }

    public bool IsShow()
    {
        return gObject.activeSelf;
    }

    public void SetOpenCallback(System.Action beforeOpen, System.Action afterOpen)
    {
        beforeOpenCallback = beforeOpen;
        afterOpenCallback = afterOpen;
    }

    public void SetCloseCallback(System.Action beforeClose, System.Action afterClose)
    {
        beforeCloseCallback = beforeClose;
        afterCloseCallback = afterClose;
    }

    private void AfterFinishAnim()
    {
        animCount--;
        if (animCount == 0 && finishAnimCallback != null)
            finishAnimCallback();
    }
}
