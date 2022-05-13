using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class ObjectPoolSystem : BaseManager<ObjectPoolSystem>
{
    public Dictionary<string, PoolObject> poolDic = new Dictionary<string, PoolObject>();
    public GameObject poolObj;
    GameObject obj = new GameObject();

    public void GetObj(string name,UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolQueue.Count > 0)
            callBack(poolDic[name].GetObj());
        else
        {
            Addressables.LoadAssetAsync<GameObject>(name).Completed += (handle) =>
            {
                callBack(handle.Result);//先调用确定位置的委托
                // handle.Result.name = name;
                GameObject.Instantiate(handle.Result).name = name;
            };
        }
    }

    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        if (poolDic.ContainsKey(name))
            poolDic[name].PushObj(obj);
        else
            poolDic.Add(name, new PoolObject(obj, poolObj));
    }

    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
}