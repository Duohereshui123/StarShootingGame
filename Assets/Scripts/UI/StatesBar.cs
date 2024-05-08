using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatesBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;//填充用的图片
    [SerializeField] Image fillImageFront;
    [SerializeField] float fillSpeed = 0.1f;//状态条填充时的速度
    [SerializeField] bool isDelayFill = true;//是否延迟填充
    [SerializeField] float fillDelay = 0.5f;//延迟时间

    float previousFillAmount;
    float currentFillAmount;//当前填充值
    protected float targetFillAmount;//目标填充值
    float t;//计算填充时间用参数
    WaitForSeconds waitForDelayFill;//延迟填充等待时间


    Coroutine bufferFillingCoroutine;//为了停止协程声明的

    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }


    public void UpdateState(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;
        if (bufferFillingCoroutine != null)
        {
            StopCoroutine(nameof(bufferFillingCoroutine));
        }

        //分两种情况，扣血时前面图快速减少后面图慢慢减少。回血时后面图快速增加前面图缓慢增加
        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageBack));
        }
        else if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferFillingCoroutine = StartCoroutine(BufferFillingCoroutine(fillImageFront));
        }
    }

    protected virtual IEnumerator BufferFillingCoroutine(Image image)
    {
        if (isDelayFill)
        {
            yield return waitForDelayFill;
        }
        previousFillAmount = currentFillAmount;
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
