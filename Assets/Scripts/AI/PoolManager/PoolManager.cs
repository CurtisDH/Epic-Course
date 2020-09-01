using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> PooledObjects;
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("PoolManager is NULL creating one..");
                var PoolManager = new GameObject("PoolManager");
                PoolManager.AddComponent<PoolManager>();
                Debug.Log("Created");
                
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public void ObjectsReadyToRecycle(GameObject obj) // when object setActive = false (In the die method) add it to this list.
    {
        PooledObjects.Add(obj);
    }


}
