using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class EnemyController : MonoBehaviour
{
    [Header("---- Move ----")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotateAngle = 25f;
    protected float paddingX;
    protected float paddingY;
    protected Vector3 targetPosition;

    [Header("---- Fire ----")]
    [SerializeField] protected GameObject[] projectiles;//子弹
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    [SerializeField] protected Transform muzzle;//枪口
    [SerializeField] protected float minFireInterval;//开火间隔最小值
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected ParticleSystem muzzleVFX;


    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    protected virtual void OnEnable()//GameObject被启用时 调用一次这个函数
    {
        StartCoroutine(nameof(RandomMovingCoroutine));
        StartCoroutine(nameof(RandomFireCoroutine));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator RandomMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemyRespawnPosition(paddingX, paddingY);

        targetPosition = Viewport.Instance.RandomRightHalfMovingPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            //如果敌人没到目标位置，就继续前往目标位置。
            if (Vector3.Distance(transform.position, targetPosition) > moveSpeed * Time.fixedDeltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotateAngle, Vector3.right);
            }
            //如果到达了，就给他一个新的目标位置
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfMovingPosition(paddingX, paddingY);
            }
            yield return waitForFixedUpdate;
        }
    }
    protected virtual IEnumerator RandomFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));

            if (GameManager.GameState == GameState.GameOver) yield break;

            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            muzzleVFX.Play();
        }
    }
}
