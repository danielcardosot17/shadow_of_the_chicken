using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTextAlpha : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float fadeInDuration;
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        StartCoroutine(DoAfterTimeCoroutine(waitTime,() => {StartCoroutine(LerpFunction(1, fadeInDuration));}));
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = canvasGroup.alpha;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endValue;
    }
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
