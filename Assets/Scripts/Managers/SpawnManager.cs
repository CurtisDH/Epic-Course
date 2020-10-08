using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CurtisDH.Scripts.Managers
{
    using CurtisDH.Scripts.Enemies;
    using CurtisDH.Utilities;
    using System.Threading;
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

        bool _startingWave = true;

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
        [SerializeField]
        private int _currentWave;
        public int CurrentWave { get => _currentWave; set => _currentWave = value; }
        float timeBetweenWave = 2f;
        [SerializeField] // look into adding customwaves from folder to list at runtime
        #region Custom Wave related
        private List<Wave> _customWaves;
        private List<WaitForSeconds> _customWaveTimers = new List<WaitForSeconds>();
        int _customWaveCount;
        #endregion
        #region Yield return
        private WaitForSeconds _betweenWaveTimer;
        private WaitForSeconds _startTimer; // not sure if this needs to be cached
        #endregion

        private void Awake()
        {
            _instance = this;
        }
        private void Start()
        {
            foreach(var wave in _customWaves)
            {
                _customWaveTimers.Add(new WaitForSeconds(wave.TimeBetweenEnemySpawns));
            }
            _betweenWaveTimer = new WaitForSeconds(timeBetweenWave);
            _startTimer = new WaitForSeconds(3);
            CreateWave();
        }
        IEnumerator SpawnRoutine()
        {
            if(_startingWave == true)
            {
                UIManager.Instance.CountDown();
                _startingWave = false;
                yield return _startTimer;
            }
            bool customWave = false;
            foreach (var wave in _customWaves) //quick and dirty will refine it later
            {
                if (wave.WaveID == _currentWave)
                {
                    for (int i = 0; i < wave.Enemies.Count; i++)
                    {
                        var enemy = Instantiate(wave.Enemies[i]);
                        enemy.name = wave.Enemies[i].name;
                        yield return _customWaveTimers[_customWaveCount];
                    }
                    _customWaveCount++;
                    customWave = true;
                }
            }
            if (customWave == false)
            {
                //Debug.Log(Wave.Count);
                for (int i = 0; i < Wave.Count; i++)
                {
                    yield return _betweenWaveTimer;
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
                EventManager.RaiseEvent("onWaveComplete", _currentWave);
                //UIManager.Instance.UpdateWave(_currentWave);
                StartCoroutine(SpawnRoutine());
            }
        }
        public void SpawnEnemy(int id)
        {
            Instantiate(Enemies[id]);
        }

        public void SkipToWave(int wave)
        {
            Debug.Log("test");
            StopAllCoroutines();
            _currentWave = wave-1;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                go.GetComponent<AIBase>().onDeath(go, false);
            }
            CreateWave();
        }
    }



}
