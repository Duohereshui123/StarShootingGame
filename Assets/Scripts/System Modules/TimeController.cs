using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;
    float defaultFixedDeltaTime;
    float t;//协程内线性插值的参数

    float timeScaleBeforePause;
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
    }
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)
    {
        yield return SlowInCoroutine(inDuration);
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    //先慢后快
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return SlowInCoroutine(inDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    //逐渐变慢
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }
    //逐渐恢复
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }

            yield return null;
        }
    }
}
