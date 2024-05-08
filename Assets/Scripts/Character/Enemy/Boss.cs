using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    BossHpBar healthBar;
    Canvas healthBarCanvas;

    
    protected override void Awake()
    {
        base.Awake();
        //healthBar = FindObjectOfType<BossHpBar>();
        healthBar = FindAnyObjectByType<BossHpBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(hp, hpMax);
        healthBarCanvas.enabled = true;
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
        }
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateState(hp, hpMax);
    }

    protected override void SetHealth()
    {
        hpMax += (int)(EnemyManager.Instance.WaveNumber * healthFactor);
    }
}
