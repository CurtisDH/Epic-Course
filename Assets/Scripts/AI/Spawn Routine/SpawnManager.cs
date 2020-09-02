using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("SpawnManager Instance is NULL.. Attempting to lazy instantiate ");
                var SpawnManager = new GameObject("SpawnManager");
                SpawnManager.AddComponent<SpawnManager>();
                Debug.Log("Created a SpawnManager");
            }
            return _instance;
        }
    }
    [SerializeField]
    GameObject[] Enemies;
    [SerializeField]
    Vector3 StartPos, EndPos;

    [SerializeField]
    List<GameObject> Wave;
    private readonly int AmountToSpawn = 10;
    private int CurrentWave;

    float timeBetweenWave = 2f;

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
        for (int i = 0; i < Wave.Count; i++)
        {
            Debug.Log(Wave.Count);
            Debug.Log("test");
            yield return new WaitForSeconds(timeBetweenWave);
            Debug.Log("test1");
            if (PoolManager.Instance.PooledObjects.Count == 0)
            {
                var enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)]); // might change to create a spawn list of enemies which are randomly defined for that wave.
                SpawnEnemy(enemy);
            }
            else
            {
                SpawnEnemy(PoolManager.Instance.PooledObjects[0]);
                PoolManager.Instance.PooledObjects.RemoveAt(0);
            }


        }
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (GameObject.Find("EnemyContainer").transform.childCount == 0)
            {
                //Display wave complete?
                Debug.Log("EnemyContainer empty.. Spawning new Wave");
                CreateWave();
                break;
            }
        }
        void SpawnEnemy(GameObject enemy)
        {
            enemy.SetActive(true);
            enemy.transform.position = StartPos;
            if (enemy.GetComponent<AIBase>())
            {
                enemy.GetComponent<AIBase>().InitaliseAI();
                enemy.GetComponent<NavMeshAgent>().Warp(StartPos); // had issues with unity saying "NO AGENT ON NAVMESH" using Warp fixes this.
                enemy.GetComponent<AIBase>().MoveTo(EndPos);
                enemy.transform.parent = GameObject.Find("EnemyContainer").transform;
            }
            else
            {
                Debug.LogError("Couldn't find Component AIBase");
            }
        }
    }

    void CreateWave() //Randomly populate a list with enemy types. List size depends on current wave * base amount to spawn.
    {
        CurrentWave++;
        Wave.Clear();
        for (int i = 0; i < (AmountToSpawn * CurrentWave); i++)
        {
            Wave.Add(Enemies[Random.Range(0, Enemies.Length)]);
        }
        StartCoroutine(SpawnRoutine());
    }



    //TEMP

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject.Find("EnemyContainer").transform.GetChild(0).GetComponent<AIBase>().onDeath();
        }
    }

}
