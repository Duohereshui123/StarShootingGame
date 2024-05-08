using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void Awake()
    {
        if (moveDirection != Vector2.left)//将不是笔直向左的子弹调整为向左
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
        }
    }
}
