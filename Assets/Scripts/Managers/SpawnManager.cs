using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    using CurtisDH.Utilities;
    using UnityEditor;

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

        private int _amountToSpawn = 10;
        private int _currentWave;

        float timeBetweenWave = 2f;
        [SerializeField] // look into adding customwaves from folder to list at runtime
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
                //Debug.Log(Wave.Count);
                for (int i = 0; i < Wave.Count; i++)
                {
                    yield return new WaitForSeconds(timeBetweenWave);
                    PoolManager.Instance.RequestEnemy(waveID:i);
                }
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
                UIManager.Instance.UpdateWave(_currentWave);
                StartCoroutine(SpawnRoutine());
            }

        }
    }



}
