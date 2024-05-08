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
                //���㷽��
                targetDirection = target.transform.position - transform.position;
                //�����ӵ�����
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                //������ǻ���
                transform.rotation *=Quaternion.Euler(0f,0f,ballisticAngle);
                //�ƶ�
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
