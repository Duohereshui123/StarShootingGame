using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMp : Singleton<PlayerMp> //泛型单例
{
    [SerializeField] MpBar mpBar;
    [SerializeField] float overdriveInterval = 0.1f;
    public const int MP_MAX = 100; //常量，命名时全大写，单词间下划线分开，声明时定义初始值。
    public const int PERCENT = 1;
    private int mp;
    bool available = true;

    WaitForSeconds waitForOverdriveInterval;
    protected override void Awake()
    {
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
        base.Awake();
    }

    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }
    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }
    private void Start()
    {
        mpBar.Initialize(mp, MP_MAX);
        Obtain(MP_MAX);
    }

    public void Obtain(int value)
    {
        if (mp == MP_MAX || !available || !gameObject.activeSelf)
        {
            return;
        }
        mp = Mathf.Clamp(mp + value, 0, MP_MAX);
        mpBar.UpdateState(mp, MP_MAX);
    }

    public void Use(int value)
    {
        mp -= value;
        mpBar.UpdateState(mp, MP_MAX);

        if (mp == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    public bool IsEnough(int value)
    {
        return mp >= value;
    }

    void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && mp > 0)
        {
            yield return waitForOverdriveInterval;//每0.1秒

            Use(PERCENT);//减少1%mp
        }
    }
}
