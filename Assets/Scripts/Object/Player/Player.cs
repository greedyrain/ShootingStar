using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [Header("--------------Move------------")]
    [SerializeField] private PlayerInput input;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    [SerializeField] private float accelerateTime;
    [SerializeField] private float decelerateTime;
    [SerializeField] private float moveRotationAngle;
    
    [Header("--------------Fire Action------------")]
    [SerializeField] [Range(1, 3)] private int fireType;
    [SerializeField] private BaseProjectile nowProjectile;
    
    [Header("--------------DOT & HOT------------")]
    [SerializeField]private float HOTTime;
    [SerializeField]private float HOTPercent;
    [SerializeField]private float DOTTime;
    [SerializeField]private float DOTPercent;
    private bool isHOT = false;
    private bool isDOT = false;
    
    private float inputAccelerateTime; //用来判断是否已经达到加速所需的事件的数值
    WaitForSeconds fireInternal;
    private WaitForSeconds waitForHOT;
    private Coroutine moveCoroutine;
    private Transform muzzleMiddle;
    private Transform muzzleTop;
    private Transform muzzleBottom;

    private Coroutine HOTCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        muzzleMiddle = transform.Find("MuzzleMiddle");
        muzzleTop = transform.Find("MuzzleTop");
        muzzleBottom = transform.Find("MuzzleBottom");
        fireInternal = new WaitForSeconds(nowProjectile.attackCD);
        waitForHOT = new WaitForSeconds(HOTTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;
        input.onStop += Stop;
        input.onFire += File;
        input.onFireStop += StopFire;
        ChangeFireType();
    }

    void Start()
    {
        rb.gravityScale = 0;
        input.EnableGameplay();
    }
    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStop -= Stop;
        input.onFire -= File;
        input.onFireStop -= StopFire;
    }

    #region ChangePlayerState
    public void ChangeFireType()
    {
        switch (fireType)
        {
            case 1:
                break;
            case 2:
                muzzleBottom.localRotation = Quaternion.identity;
                muzzleTop.localRotation = Quaternion.identity;
                break;
            case 3:
                muzzleTop.localRotation = Quaternion.AngleAxis(3, muzzleTop.forward);
                muzzleBottom.localRotation = Quaternion.AngleAxis(-3, muzzleTop.forward);
                break;
        }
    }

    public void SetNowProjectile(BaseProjectile projectile)
    {
        nowProjectile = projectile;
    }
    
    #endregion

    #region Fire
    private void StopFire()
    {
        StopCoroutine("FireAction");
    }
    private void File()
    {
        StartCoroutine("FireAction");
    }
    IEnumerator FireAction()
    {
        while (true)
        {
            switch (fireType)
            {
                case 1:
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleMiddle.position;
                        obj.transform.rotation = Quaternion.identity;
                    });
                    break;
                case 2:
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleTop.position;
                        obj.transform.rotation = Quaternion.identity;
                    });
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleBottom.position;
                        obj.transform.rotation = Quaternion.identity;
                    });
                    break;
                case 3:
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleMiddle.position;
                        obj.transform.rotation = muzzleMiddle.rotation;
                    });
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleTop.position;
                        obj.transform.rotation = muzzleTop.rotation;
                    });
                    ObjectPoolSystem.Instance.GetObj(nowProjectile.name, (obj) =>
                    {
                        obj.transform.position = muzzleBottom.position;
                        obj.transform.rotation = muzzleBottom.rotation;
                    });
                    break;
            }
            yield return fireInternal;
        }
    }

    #endregion

    #region Move

    private void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCoroutine(moveInput.normalized * moveSpeed, accelerateTime,
            Quaternion.AngleAxis(moveRotationAngle * moveInput.y, transform.right)));
        StartCoroutine(MovePosLimitCoroutine());
    }

    private void Stop()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.zero, decelerateTime, Quaternion.identity));
        StopCoroutine(MovePosLimitCoroutine());
    }

    IEnumerator MoveCoroutine(Vector2 moveVelocity, float time, Quaternion moveRotation)
    {
        inputAccelerateTime = 0f;
        while (inputAccelerateTime < time)
        {
            inputAccelerateTime += Time.fixedDeltaTime / time;
            rb.velocity = Vector2.Lerp(rb.velocity, moveVelocity, inputAccelerateTime / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, inputAccelerateTime / time);

            yield return null;
        }
    }

    IEnumerator MovePosLimitCoroutine()
    {
        while (true)
        {
            transform.position = ViewPort.Instance.PlayerMovealbePosition(transform.position, paddingX, paddingY);
            yield return null;
        }
    }

    #endregion

    #region Health

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (gameObject.activeSelf)
        {
            if (isHOT)
            {
                if (HOTCoroutine != null)
                {
                    StopCoroutine(HOTCoroutine);
                }
                HOTCoroutine = StartCoroutine(HealOverTime(waitForHOT, HOTPercent));
            }
        }
    }

    #endregion

}