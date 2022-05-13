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
}