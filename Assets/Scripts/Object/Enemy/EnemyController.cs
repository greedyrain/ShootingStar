using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyController : MonoBehaviour
{
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveRotationAngle;
    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;
    [SerializeField] private BaseProjectile projectile;

    [SerializeField,Range(1,20)] private int MuzzleCount;
    private Transform MuzzleGroup;
    private Transform[] muzzles;


    private void Awake()
    {
        muzzles = new Transform[MuzzleCount];
        MuzzleGroup = transform.Find("Muzzles");
        SetMuzzles(MuzzleCount);
    }

    private void OnEnable()
    {
        StartCoroutine("RandomlyMovingCoroutine");
        StartCoroutine("RandomlyFireCoroutine");
    }

    //出生: 在awake中调用
    public void Spawn()
    {
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
    }

    //移动至目标点
    IEnumerator RandomlyMovingCoroutine()
    {
        Spawn();
        Vector3 targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
        while (gameObject.activeSelf)
        {
            //If has not arrive targetPosition
            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
            {
                //Keep moving to targetPosition
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                //Make rotate with X axis while moving
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle,
                        transform.right), Time.deltaTime * rotateSpeed);
            }

            //If had arrive targetPosition
            else
            {
                //Set a new targetPosition
                targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }

    //攻击
    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            foreach (var muzzle in muzzles)//需要用foreach循环，因为如果使用for循环，遇到getObj时会触发里面的异步方法，直接返回，因此会来到下一次循环，此时的i也就不等于原先的i了
            {
                ObjectPoolSystem.Instance.GetObj(projectile.name, (obj) =>
                {
                    obj.transform.position = muzzle.position;
                    obj.transform.rotation = muzzle.rotation; // * Quaternion.AngleAxis(180,muzzle.transform.up);
                }); 
            }
        }
    }
    //受伤
    //死亡

    //设置开火点
    void SetMuzzles(int count)
    {
        if (count <= 1)
        {
            GameObject muzzle = new GameObject($"Muzzle");
            muzzle.transform.SetParent(MuzzleGroup);
            muzzles[0] = muzzle.transform;
            muzzle.transform.localPosition = Vector3.zero; //计算每一个Muzzle的位置；
            muzzle.transform.localRotation = Quaternion.identity;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject muzzle = new GameObject($"Muzzle{i}");
                muzzle.transform.SetParent(MuzzleGroup);
                muzzles[i] = muzzle.transform;
                muzzle.transform.localPosition = new Vector3(0, -0.05f + 0.1f / (count - 1) * i, 0); //计算每一个Muzzle的位置；
                muzzle.transform.localRotation = Quaternion.AngleAxis((count - 1 - 2 * i) * 1.5f, muzzle.transform.forward);
            }
        }
    }
}