using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VFXRecycle : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    // private ParticleSystem _particleSystem;

    private void Awake()
    {
        // _particleSystem = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        Recycle();
    }
    async void  Recycle()
    {
        await UniTask.Delay((int) (lifeTime * 1000 + 200));
        ObjectPoolSystem.Instance.PushObj(gameObject.name, gameObject);
    }
}
