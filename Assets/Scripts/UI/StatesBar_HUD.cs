using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatesBar_HUD : StatesBar
{
    [SerializeField] protected Text percentText;

    protected virtual void SetPencentText()
    {
        percentText.text = targetFillAmount.ToString("P0");
    }
    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPencentText();
    }

    protected override IEnumerator BufferFillingCoroutine(Image image)
    {
        SetPencentText();
        return base.BufferFillingCoroutine(image);
    }
}
