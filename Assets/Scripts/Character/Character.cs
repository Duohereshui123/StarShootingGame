using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioData[] deathSFX;
    [Header("---- Health ----")]
    [SerializeField] protected float hpMax;
    [SerializeField] StatesBar onHeadHpBar;
    [SerializeField] bool isShowHeadHpBar = true;

    protected float hp;

    protected virtual void OnEnable()
    {
        hp = hpMax;

        if(isShowHeadHpBar)
        {
            ShowOnHeadHpBar();
        }
        else
        {
            HideOnHeadHpBar();
        }
    }

    public void ShowOnHeadHpBar()
    {
        onHeadHpBar.gameObject.SetActive(true);
        onHeadHpBar.Initialize(hp, hpMax);
    }
    public void HideOnHeadHpBar()
    {
        onHeadHpBar.gameObject.SetActive(false);
    }

    //受伤
    public virtual void TakeDamage(float damage)
    {
        if (hp == 0f) return;
        hp -= damage;

        if(isShowHeadHpBar/* && gameObject.activeSelf*/)
        {
            onHeadHpBar.UpdateState(hp, hpMax);
        }

        if (hp <= 0)
        {
            Die();
        }
    }
    //死亡
    public virtual void Die()
    {
        hp = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    //回复
    public virtual void RestoreHp(float value)
    {
        if (hp == hpMax)
        {
            return;
        }
        hp = Mathf.Clamp(hp + value, 0f, hpMax);

        if (isShowHeadHpBar)
        {
            onHeadHpBar.UpdateState(hp, hpMax);
        }
    }


    //持续回复
    protected IEnumerator HpRegenarateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (hp < hpMax)
        {
            yield return waitTime;

            RestoreHp(hpMax * percent);
        }
    }

    //dot持续扣血
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (hp > 0)
        {
            yield return waitTime;

            TakeDamage(hpMax * percent);
        }
    }
}
