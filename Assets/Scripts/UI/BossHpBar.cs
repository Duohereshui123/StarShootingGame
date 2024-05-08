using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpBar : StatesBar_HUD
{
    protected override void SetPencentText()
    {
        percentText.text = targetFillAmount.ToString("P2");
    }
}
