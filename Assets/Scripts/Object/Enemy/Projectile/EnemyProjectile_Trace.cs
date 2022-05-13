using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class EnemyProjectile_Trace : BaseProjectile 
{
    protected override void Awake()
    {
         target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    protected async override void OnEnable()
    {
        await UniTask.DelayFrame(1);
        if (target.gameObject.activeSelf)
        {
            moveDirection = (target.position -transform.position ).normalized;
        }
        base.OnEnable();
    }

}
