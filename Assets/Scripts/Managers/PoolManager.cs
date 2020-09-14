using CurtisDH.Scripts.Enemies;
using CurtisDH.Utilities;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _pooledObjects;

        private List<GameObject> _enemyType0, _enemyType1;
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
        public void ObjectsReadyToRecycle(GameObject obj, bool enemies = true, int id = 0, int warfund = 0) // when object setActive = false (In the die method) add it to this list.
        {
            obj.transform.parent = transform;
            if (enemies != true)
            {
                _pooledTurrets.Add(obj);
                return;
            }
            GameManager.Instance.AdjustWarfund(-warfund);
            if (id == 0) // will switch to switch statement if I increase the id's
            {
                _enemyType0.Add(obj);
            }
            else if (id == 1)
            {
                _enemyType1.Add(obj);
            }
        }
        public GameObject RequestEnemy(int waveID = 0) //Reworking the whole pooling system as its non-functional
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
            int id = SpawnManager.Instance.Wave[waveID].GetComponent<AIBase>().ID;
            Helper.RandomiseList(_enemyType0);
            if (id == 0)//switch to switch statement if increase ID's in size.
            {
                if (_enemyType0.Count != 0)
                {
                    Helper.RandomiseList(_enemyType0);
                    Debug.Log("PoolManager::etype0 count > 1");
                    var enemy = _enemyType0[0];
                    enemy.SetActive(true);
                    _enemyType0.RemoveAt(0);
                    return enemy;
                }
            }
            else if (id == 1)
            {
                if (_enemyType1.Count != 0)
                {
                    Helper.RandomiseList(_enemyType1);
                    Debug.Log("PoolManager::etype1 count > 1");
                    var enemy = _enemyType1[0];
                    enemy.SetActive(true);
                    _enemyType1.RemoveAt(0);
                    return enemy;
                }
            }
            Debug.Log("PoolManager::No list available creating object");
            var e = Instantiate(SpawnManager.Instance.Wave[waveID]);
            return e;

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
