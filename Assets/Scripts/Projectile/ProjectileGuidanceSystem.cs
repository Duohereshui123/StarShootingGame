using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile Projectile;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;
    float ballisticAngle;
    Vector3 targetDirection;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                //计算方向
                targetDirection = target.transform.position - transform.position;
                //修正子弹朝向
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                //增加射角弧度
                transform.rotation *=Quaternion.Euler(0f,0f,ballisticAngle);
                //移动
                Projectile.Move();
            }
            else
            {
                Projectile.Move();
            }
            yield return null;
        }
    }
}
