using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _pooledObjects; //change to private
        public List<GameObject> PooledObjects
        {
            get
            {
                return _pooledObjects;
            }
        }
        private static PoolManager _instance;
        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("PoolManager::PoolManager is NULL creating one..");
                    var PoolManager = new GameObject("PoolManager");
                    PoolManager.AddComponent<PoolManager>();
                    Debug.Log("PoolManager::Created");
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
        public GameObject RequestEnemy()
        {
            return _pooledObjects?[0];
        }



    }

}
