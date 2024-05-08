using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    //普通子弹轨迹清除
    TrailRenderer trail;
    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)//将不是笔直向右的子弹调整为向右
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }
    private void OnDisable()
    {
        trail.Clear();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerMp.Instance.Obtain(PlayerMp.PERCENT);
    }
}
