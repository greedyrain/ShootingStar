using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
/// <summary>
/// 所有飞机共有的行为和属性
/// 如收到伤害，死亡等等
/// </summary>
public class Character : MonoBehaviour
{
    private float maxHp;
    private float hp;

    private GameObject deathVFX;

    protected virtual void OnEnable()
    {
        hp = maxHp;
    }

    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        hp = 0;
        ObjectPoolSystem.Instance.GetObj(deathVFX.name, (obj) =>
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        });
    }
    
    public virtual void RestoreHealth(float value)
    {
        if (hp == maxHp) return;
        hp = Mathf.Clamp(hp + value, 0, maxHp);
    }

    //HOT
    protected IEnumerator HealOverTime(WaitForSeconds waitTimes,float percent)
    {
        while (hp < maxHp)
        {
            yield return waitTimes;
            RestoreHealth(maxHp * percent);
        }
    }
    
    //DOT
    protected IEnumerator DamageOverTime(WaitForSeconds waitTimes,float percent)
    {
        while (hp > 0)
        {
            yield return waitTimes;
            TakeDamage(maxHp * percent);
        }
    } 
}
