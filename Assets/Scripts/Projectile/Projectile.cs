using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投射物脚本
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX;
    [SerializeField] private float damage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()//每次游戏对象启用时会运行一次
    {
        StartCoroutine(MoveDirectly());
    }
   
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}
