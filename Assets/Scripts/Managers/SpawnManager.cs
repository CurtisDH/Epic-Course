using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    using CurtisDH.Scripts.PlayerRelated;
    using CurtisDH.Utilites;
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
        List<GameObject> Wave;
        private int _amountToSpawn = 10;// add _
        private int _currentWave;

        float timeBetweenWave = 2f;
        [SerializeField]
        private List<Wave> CustomWaves;

        private void Awake()
        {
            _instance = this;
        }
        private void OnEnable()
        {
            PlayerBase.onPlayerbaseReached += CreateWave;
        }
        private void OnDisable()
        {

            PlayerBase.onPlayerbaseReached -= CreateWave;

        }
        private void Start()
        {
            CreateWave();
        }
        IEnumerator SpawnRoutine()
        {
            foreach (var wave in CustomWaves) //quick and dirty will refine it later
            {
                if (wave.WaveID == _currentWave)
                {
                    for (int i = 0; i < wave.Enemies.Count; i++)
                    {
                        Instantiate(wave.Enemies[i]);
                        yield return new WaitForSeconds(wave.TimeBetweenEnemySpawns);
                    }
                    yield break;
                }
            }
            if (PoolManager.Instance.PooledObjects.Count > 1)
            {
                Utilites.RandomiseList(PoolManager.Instance.PooledObjects); // Changed to make it only randomise the list once
            }
            for (int i = 0; i < Wave.Count; i++)
            {
                //Debug.Log("SpawnManager::Wavecount "+Wave.Count);
                yield return new WaitForSeconds(timeBetweenWave);
                SpawnEnemy();
            }
            
        }

        void SpawnEnemy()
        {
            try
            {
                PoolManager.Instance.RequestEnemy().SetActive(true);
            }
            catch
            {
                Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
            }
        }

        void CreateWave() //Randomly populate a list with enemy types. List size depends on current wave * base amount to spawn.
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
