using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[System.Serializable]
public class PoolObject
{
    public GameObject parentObj;
    public Queue<GameObject> poolQueue;

    public PoolObject(GameObject obj,GameObject poolObj)
    {
        parentObj = new GameObject(obj.name);
        parentObj.transform.SetParent(poolObj.transform);
        poolQueue = new Queue<GameObject>();
        PushObj(obj);
    }

    #region 阿严

   // public void Initialize(Transform parent)
    // {
    //     queue = new Queue<GameObject>();
    //     this.parent = parent;
    //     for (int i = 0; i < size; i++)
    //     {
    //         queue.Enqueue(Copy());
    //     }
    // }
    //
    // GameObject Copy()
    // {
    //     var copy = GameObject.Instantiate(prefab,parent);
    //     copy.SetActive(false);
    //     return copy;
    // }
    //
    // GameObject AvailableObject()
    // {
    //     GameObject availableObject = null;
    //     if (queue.Count > 0 && !queue.Peek().activeSelf)
    //         availableObject = queue.Dequeue();
    //
    //     else
    //         availableObject = Copy();
    //     queue.Enqueue(availableObject);
    //     return availableObject;
    // }
    //
    // public GameObject PrepareObject()
    // {
    //     GameObject prepareObject = AvailableObject();
    //     prepareObject.SetActive(true);
    //     return prepareObject;
    // }
    //
    // public GameObject PrepareObject(Vector3 position)
    // {
    //     GameObject prepareObject = AvailableObject();
    //     prepareObject.transform.position = position;
    //     prepareObject.SetActive(true);
    //     return prepareObject;
    // }
    //
    // public GameObject PrepareObject(Vector3 position, Quaternion rotation)
    // {
    //     GameObject prepareObject = AvailableObject();
    //     prepareObject.transform.position = position;
    //     prepareObject.transform.rotation = rotation;
    //     prepareObject.SetActive(true);
    //     return prepareObject;
    // }
    //
    // public GameObject PrepareObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    // {
    //     GameObject prepareObject = AvailableObject();
    //     prepareObject.transform.position = position;
    //     prepareObject.transform.rotation = rotation;
    //     prepareObject.transform.localScale = localScale;
    //     prepareObject.SetActive(true);
    //     return prepareObject;
    // } 

    #endregion

    public void PushObj(GameObject obj)
    {
       obj.SetActive(false);
       poolQueue.Enqueue(obj);
       obj.transform.SetParent(parentObj.transform);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolQueue.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null);
        return obj;
    }
}