using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _pooledObjects; 
        public List<GameObject> PooledObjects
        {
            get
            {
                return _pooledObjects;
            }
        }
        [SerializeField]
        private List<GameObject> _pooledTurrets;
        public List<GameObject> PooledTurrets
        {
            get
            {
                return _pooledTurrets;
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
        public void ObjectsReadyToRecycle(GameObject obj, bool enemies = true, int id = 0,int warfund = 0) // when object setActive = false (In the die method) add it to this list.
        {
            obj.transform.parent = transform;
            if(id == 0) // will switch to switch statement if I increase the id's
            {
                
            }
            else if (id == 1)
            {

            }
            if (enemies != true)
            {
                _pooledTurrets.Add(obj);
                return;
            }
            else
            {
                _pooledObjects.Add(obj);
                GameManager.Instance.AdjustWarfund(-warfund);
            }
            

        }
        public GameObject RequestEnemy(int id) //Reworking the whole pooling system as its non-functional
        #region error message
        //        ArgumentOutOfRangeException: Index was out of range.Must be non-negative and less than the size of the collection.
        //Parameter name: index
        //System.ThrowHelper.ThrowArgumentOutOfRangeException (System.ExceptionArgument argument, System.ExceptionResource resource) (at<fb001e01371b4adca20013e0ac763896>:0)
        //System.ThrowHelper.ThrowArgumentOutOfRangeException() (at<fb001e01371b4adca20013e0ac763896>:0)
        //CurtisDH.Scripts.Managers.PoolManager.RequestEnemy() (at Assets/Scripts/Managers/PoolManager.cs:76)
        //CurtisDH.Scripts.Managers.SpawnManager.SpawnEnemy(System.Int32 i) (at Assets/Scripts/Managers/SpawnManager.cs:118)
        //CurtisDH.Scripts.Managers.SpawnManager+<SpawnRoutine>d__21.MoveNext() (at Assets/Scripts/Managers/SpawnManager.cs:108)
        //UnityEngine.SetupCoroutine.InvokeMoveNext(System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at<4cc8ec075538416496e5db5d391208ac>:0)
        #endregion
        {
            int i = 0;
            if (SpawnManager.Instance.Wave.Count != 0 && _pooledObjects.Count != 0)
                while (_pooledObjects[i].name != SpawnManager.Instance.Wave[i].name)
                {
                    Debug.Log("PoolManager::While Loop " + i);
                    if (_pooledObjects[i].name == SpawnManager.Instance.Wave[i].name)
                    {
                        Debug.Log("PoolManager::Line48");
                        var enemytoreturn = _pooledObjects[i];
                        _pooledObjects.RemoveAt(i);
                        Debug.Log("PoolManager::Line51" + enemytoreturn);
                        enemytoreturn.SetActive(true);
                        return enemytoreturn;
                    }
                    i++;
                }
            //if (_pooledObjects[i].name == SpawnManager.Instance.Wave[i].name)
            //{

            //}

            return null;
        }
        public GameObject RequestTower() // currently kinda works..
                                         //it returns whatever turret is in the pool regardless of the selected one. 
                                         // ideas... Seperate the lists into different IDs (Doesn't seem modular enough to me)
                                         // Check all the pooled turrets ID's before returning (if an id matches the one we want return that)
        {
            if (_pooledTurrets.Count != 0)
            {
                var TurretToReturn = _pooledTurrets[0];
                TurretToReturn.SetActive(true);
                _pooledTurrets.RemoveAt(0);
                return TurretToReturn;
            }
            else
            {
                return null;
            }
        }
    }

}
