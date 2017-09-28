using UnityEngine;
using System.Collections;

public abstract class IAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float totalAnimTime;

    private const float spf = 1f / 60f;

    private System.Action finishedEvent;
    private float invTotalAnimTime;
    private bool bAnim;

    public void StartAnimation(System.Action finishedEvent)
    {
        this.finishedEvent = finishedEvent;
        bAnim = true;
        invTotalAnimTime = 1f / totalAnimTime;

        Init();

        StartCoroutine(AnimLoop(AnimFunc));
    }

    protected virtual void Init()
    {
    }
    
    protected abstract void AnimFunc(float rate);

    private IEnumerator AnimLoop(System.Action<float> callback)
    {
        float startTime = Time.realtimeSinceStartup;

        while (bAnim)
        {
            float rate = (Time.realtimeSinceStartup - startTime) * invTotalAnimTime;
            if (rate >= 1f)
            {
                rate = 1f;
                bAnim = false;
            }

            callback(animCurve.Evaluate(rate));

            if (bAnim)
                yield return null;
            else
            {
                if (finishedEvent != null)
                    finishedEvent();

                yield break;
            }
        }
    }
}
