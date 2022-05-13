using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] protected Vector2 moveDirection;
    [SerializeField] public float attackCD;

    [SerializeField] private float damage;
    [SerializeField] private GameObject hitVFX;
    
    private TrailRenderer trail;
    
    protected Transform target;

    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }


    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
        Invoke("RecycleObject",lifeTime);
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveSpeed * moveDirection * Time.deltaTime);
            yield return null;
        }
    }

    protected void RecycleObject()
    {
        ObjectPoolSystem.Instance.PushObj(name, gameObject);
        if (trail != null)
        {
            trail.Clear();
        }
    }

    /// <summary>
    /// 产生碰撞后在碰撞点生成碰撞特效
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            ObjectPoolSystem.Instance.GetObj(hitVFX.name, (vfxObj) =>
            {
                var contantPoint = col.GetContact(0);
                vfxObj.transform.position = contantPoint.point;
                vfxObj.transform.rotation = Quaternion.identity;
                gameObject.SetActive(false);
            });
        }
    }
}