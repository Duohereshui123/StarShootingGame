using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D))] // use C# code to set rigidbody2d
public class Player : Character
{
    [SerializeField] StatesBar_HUD statesBar_HUD;
    //回复
    [SerializeField] bool isRegenerateHp = true;
    [SerializeField, Range(0f, 1f)] float hpRegeneratePercent;
    [SerializeField] float hpRegenerateTime;
    WaitForSeconds waitHpRegenerateTime;

    [Header("---- Input ----")]
    [SerializeField] PlayerInput input;

    [Header("---- Move ----")]
    //Move
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float decelerationTime;
    [SerializeField] private float moveRotationAngle;
    private float paddingX;
    private float paddingY;

    float t;
    Vector2 moveDirection;
    Vector2 previousVelocity;
    Quaternion previousRotation;
    WaitForFixedUpdate waitForFixedUpadata;
    WaitForSeconds waitForDecelerationTime;

    [Header("---- Fire ----")]
    //Fire
    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;
    [SerializeField] private GameObject projectileOverdrive;

    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleBottom;

    [SerializeField, Range(0, 2)] private int weaponPower = 0;
    [SerializeField] private float CoolDownTime;                        //射击间隔数值
    WaitForSeconds fireCD;                                              //协程中的射击间隔参数
    WaitForSeconds overdriveFireCD;                                     //爆发中的射击间隔参数
    [SerializeField] AudioData projectileLaunchSFX;
    [SerializeField] ParticleSystem muzzleVFX;

    MissileSystem missile;

    [Header("---- Dodge ----")]
    [SerializeField] AudioData dodgeSFX;
    [SerializeField, Range(0, 100)] int dodgeMpCost = 25;
    [SerializeField] float maxRollAngle = 720f;                         //飞机翻滚最大角度
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);//缩放最小倍率

    [Header("---- Overdrive ----")]
    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;
    [SerializeField] int overdriveDodgeFactor = 2;
    readonly float slowMotionDuration = 1f;

    [Header("---- Invincible ----")]
    [SerializeField] float invincibleTime = 1f;
    WaitForSeconds waitForInvincibleTime;


    float currentRollAngle;
    float dodgeDuration;
    bool isDodging = false;
    bool isOverdriving = false;

    Rigidbody2D rb;
    Coroutine moveCoroutine;                                    //停用协程时需要的参数
    Coroutine hpRegenarateCoroutine;                            //停用协程时需要的参数
    new Collider2D collider;                                    //collider被占用 所以加new
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        dodgeDuration = maxRollAngle / rollSpeed;               //初始化翻滚持续时间
        rb.gravityScale = 0f;                                   //use C# code to set rigidbody2d gravityScale
        waitForFixedUpadata = new WaitForFixedUpdate();
        fireCD = new WaitForSeconds(CoolDownTime);
        overdriveFireCD = new WaitForSeconds(CoolDownTime / overdriveFireFactor);
        waitHpRegenerateTime = new WaitForSeconds(hpRegenerateTime);
        waitForDecelerationTime = new WaitForSeconds(decelerationTime);

        waitForInvincibleTime = new WaitForSeconds(invincibleTime);//无敌时间

        missile = GetComponent<MissileSystem>();

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }
    void Start()
    {
        input.EnableGamePlayInput();
        statesBar_HUD.Initialize(hp, hpMax);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    IEnumerator InvincibleCoroutine()
    {
        collider.isTrigger = true;
        yield return waitForInvincibleTime;
        collider.isTrigger = false;
    }
    #region MoveController
    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveDirection = moveInput.normalized;
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimiteCoroutine));//启用协程
    }

    void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, moveDirection, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    //在一段时间内update 协程计算加速减速移动
    //优化后
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rb.velocity;
        previousRotation = transform.rotation;

        while (t < time)
        {
            t += Time.fixedDeltaTime;
            rb.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);//线性插值函数
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);
            yield return waitForFixedUpadata;
        }
    }


    //用协程来取代Update计算玩家限制的位置避免每一帧都在计算
    IEnumerator MoveRangeLimiteCoroutine()// 移位限制协程
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);//将玩家的移动范围限制在viewport里

            yield return null;//挂起直到下一帧
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return waitForDecelerationTime;

        StopCoroutine(nameof(MoveRangeLimiteCoroutine));
    }
    #endregion
    #region FireController
    void Fire()
    {
        muzzleVFX.Play();
        //StartCoroutine(FireCoroutine());//won't work  (Unitiy's problem)
        //StartCoroutine("FireCoroutine");//下面的更好控制名字
        StartCoroutine(nameof(FireCoroutine));
    }
    void StopFire()
    {
        muzzleVFX.Stop();
        //StopCoroutine(FireCoroutine());
        //StopCoroutine("FireCoroutine");
        StopCoroutine(nameof(FireCoroutine));
    }

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }

    public void MissilePickUp()
    {
        missile.PickUp();
    }

    public void PowerUp()
    {
        weaponPower++;
        weaponPower = Mathf.Clamp(weaponPower, 0, 2);
    }
    public void PowerDown()
    {
        weaponPower--;
        weaponPower = Mathf.Clamp(weaponPower, 0, 2);
    }
    IEnumerator FireCoroutine()
    {
        while (true)
        {

            switch (weaponPower)//使用对象池release来发射
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);

            yield return isOverdriving ? overdriveFireCD : fireCD;
        }
    }
    #endregion
    #region DodgeController
    private void Dodge()
    {
        if (isDodging || !PlayerMp.Instance.IsEnough(dodgeMpCost)) return;
        StartCoroutine(DodgeCoroutine());
    }
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        PlayerMp.Instance.Use(dodgeMpCost);

        //利用将collider组件里的trigger打开时不会相应OnColliderEnter函数的性质，设置玩家无敌
        collider.isTrigger = true;
        currentRollAngle = 0;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);

        ////缩放方法1 缩放时间临时变量 t1缩小 t2放大
        //var t1 = 0f;
        //var t2 = 0f;
        while (currentRollAngle < maxRollAngle)
        {
            //翻滚
            currentRollAngle += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRollAngle, Vector3.right);
            ////缩放方法1  线性插值函数
            //if (currentRollAngle < maxRollAngle / 2f)
            //{
            //    t1 += Time.deltaTime / dodgeDuration;
            //    transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
            //}
            //else
            //{
            //    t2 += Time.deltaTime / dodgeDuration;
            //    transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
            //}


            //缩放方法2 贝塞尔曲线
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRollAngle / maxRollAngle);


            yield return null;
        }
        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion
    #region OverdriveController
    private void Overdrive()
    {
        if (!PlayerMp.Instance.IsEnough(PlayerMp.MP_MAX))
        {
            return;
        }
        PlayerOverdrive.on.Invoke();
    }
    private void OverdriveOn()
    {
        isOverdriving = true;
        dodgeMpCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    private void OverdriveOff()
    {
        isOverdriving = false;
        dodgeMpCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion
    #region HP Events
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PowerDown();//被攻击时 武器等级-1
        statesBar_HUD.UpdateState(hp, hpMax);
        TimeController.Instance.BulletTime(slowMotionDuration);

        if (gameObject.activeSelf)
        {
            Move(moveDirection);
            StartCoroutine(nameof(InvincibleCoroutine));

            if (isRegenerateHp)
            {
                if (hpRegenarateCoroutine != null)
                {
                    StopCoroutine(hpRegenarateCoroutine);
                }
                hpRegenarateCoroutine = StartCoroutine(HpRegenarateCoroutine(waitHpRegenerateTime, hpRegeneratePercent));
            }
        }
    }

    public override void RestoreHp(float value)
    {
        base.RestoreHp(value);
        statesBar_HUD.UpdateState(hp, hpMax);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statesBar_HUD.UpdateState(0f, hpMax);
        base.Die();
    }
    #endregion
    #region Properties
    public bool IsFullHp => hp == hpMax;
    public bool IsFullPower => weaponPower == 2;

    #endregion
}
