using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    using CurtisDH.Utilities;
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;
        public static SpawnManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("SpawnManager::SpawnManager Instance is NULL.. Attempting to lazy instantiate ");
                    var SpawnManager = new GameObject("SpawnManager");
                    SpawnManager.AddComponent<SpawnManager>();
                    Debug.Log("SpawnManager::Created a SpawnManager");
                }
                return _instance;
            }
        }
        [SerializeField]
        GameObject[] _enemies; //change to _enemies

        public GameObject[] Enemies
        {
            get
            {
                return _enemies;
            }
        }
        [SerializeField]
        private Vector3 _startPos, _endPos;
        public Vector3 StartPos
        {
            get
            {
                return _startPos;
            }
        }
        public Vector3 EndPos
        {
            get
            {
                return _endPos;
            }
        }

        [SerializeField]
        List<GameObject> _wave;
        public List<GameObject> Wave
        {
            get
            {
                return _wave;
            }

        }

        private int _amountToSpawn = 10;// add _
        private int _currentWave;

        float timeBetweenWave = 2f;
        [SerializeField]
        private List<Wave> CustomWaves;


        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {
            CreateWave();
        }
        IEnumerator SpawnRoutine()
        {
            bool customWave = false;
            foreach (var wave in CustomWaves) //quick and dirty will refine it later
            {
                if (wave.WaveID == _currentWave)
                {
                    for (int i = 0; i < wave.Enemies.Count; i++)
                    {
                        var enemy = Instantiate(wave.Enemies[i]);
                        enemy.name = wave.Enemies[i].name;
                        yield return new WaitForSeconds(wave.TimeBetweenEnemySpawns);
                    }
                    customWave = true;
                }
            }
            if (customWave == false)
            {
                Debug.Log(Wave.Count);
                if (PoolManager.Instance.PooledObjects.Count > 1)
                {
                    Helper.RandomiseList(PoolManager.Instance.PooledObjects); // Changed to make it only randomise the list once
                }
                for (int i = 0; i < Wave.Count; i++)
                {
                    Debug.Log(i);
                    yield return new WaitForSeconds(timeBetweenWave);
                    SpawnEnemy(i);
                }
            }
        }
        void SpawnEnemy(int i) // need to check if the pooled enemy name is == the one we want to spawn in the wave list
        {
            Debug.Log("SpawnManager::BeforeRequestEnemy");
            if (PoolManager.Instance.PooledObjects.Count != 0 
                && PoolManager.Instance.RequestEnemy() != null)
            {
                if (PoolManager.Instance.RequestEnemy().name == Wave[i].name)
                {
                    Debug.Log("SpawnManager::RequestEnemy");
                    PoolManager.Instance.RequestEnemy().SetActive(true);
                    return;
                }

            }
            else
            {
                var enemy = Instantiate(Wave[i]);
                enemy.name = Wave[i].name;
            }
        }
        public void CreateWave() //Randomly populate a list with enemy types. List size depends on current wave * base amount to spawn.
        {
            if (GameObject.Find("EnemyContainer").transform.childCount == 0)
            {
                _currentWave++;
                Wave.Clear();
                for (int i = 0; i < (_amountToSpawn * _currentWave); i++)
                {
                    Wave.Add(Enemies[Random.Range(0, Enemies.Length)]);
                }
                StartCoroutine(SpawnRoutine());
            }

        }
    }



}
