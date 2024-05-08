using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    static Text scoreText;
    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    //顺序 Awake -> OnEnable -> Reset -> Start -> FixedUpdata
    //Awake中对当前脚本初始化 Start里对其他脚本初始化 和 调用其他脚本
    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    public static void UpdateText(int score) => scoreText.text = score.ToString();

    public static void ScaleText(Vector3 targetScale) => scoreText.rectTransform.localScale = targetScale;
}
