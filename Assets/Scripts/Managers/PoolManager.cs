using CurtisDH.Scripts.Enemies;
using CurtisDH.Utilities;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _enemyType0, _enemyType1;
        [SerializeField]
        private List<GameObject> _turretType0, _turretType1;
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
                id = obj.GetComponent<Tower>().TowerID; //don't want to use getcomponent so may find a better way
                if (id == 0)
                {
                    _turretType0.Add(obj);
                }
                else if (id == 1)
                {
                    _turretType1.Add(obj);
                }

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
        
        {
            int id = SpawnManager.Instance.Wave[waveID].GetComponent<AIBase>().ID;
            if (id == 0)//switch to switch statement if increase ID's in size.
            {
                if (_enemyType0.Count > 0)
                {

                    var enemy = _enemyType0[0];
                    enemy.SetActive(true);
                    _enemyType0.RemoveAt(0);
                    return enemy;
                }
            }
            else if (id == 1)
            {
                if (_enemyType1.Count > 0)
                {

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
        // currently kinda works..
        //it returns whatever turret is in the pool regardless of the selected one. 
        // ideas... Seperate the lists into different IDs (Doesn't seem modular enough to me)
        // Check all the pooled turrets ID's before returning (if an id matches the one we want return that)
        public GameObject RequestTower(int id = 0)
        {
            if (id == 0)
            {
                if (_turretType0.Count > 0)
                {

                    var turret = _turretType0[0];
                    turret.SetActive(true);
                    _turretType0.RemoveAt(0);
                    return turret;
                }
            }
            else if (id == 1)
            {
                if (_turretType1.Count > 0)
                {

                    var turret = _turretType1[0];
                    turret.SetActive(true);
                    _turretType1.RemoveAt(0);
                    return turret;
                }
            }
            var tower = Instantiate(TowerManager.Instance.SelectedTower);


            return tower;
        }

    }
}
