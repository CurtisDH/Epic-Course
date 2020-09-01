using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    //TO-DO CONVERT TO SINGLETON
    [SerializeField]
    GameObject[] Enemies;
    [SerializeField]
    Vector3 StartPos, EndPos;

    [SerializeField]
    List<GameObject> Wave;
    private readonly int AmountToSpawn = 10;
    private int CurrentWave;

    float timeBetweenWave = 2f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator SpawnRoutine() 
    {
        for(int i = 0; i < Wave.Count; i++)
        {
            yield return new WaitForSeconds(timeBetweenWave);
            var enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)]); // might change to create a spawn list of enemies which are randomly defined for that wave.
            enemy.transform.position = StartPos;
            if (enemy.GetComponent<AIBase>())
            {
                enemy.GetComponent<AIBase>().InitaliseAI();
                enemy.GetComponent<NavMeshAgent>().Warp(StartPos); // had issues with the agent saying "NO AGENT ON NAVMESH" using Warp fixes this.
                enemy.GetComponent<AIBase>().MoveTo(EndPos);
                enemy.transform.parent = GameObject.Find("EnemyContainer").transform;
            }
            else
            {
                Debug.LogError("Couldn't find Component AIBase");
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

    }

    void CreateWave() //Randomly populate a list with enemy types. List size depends on current wave * base amount to spawn.
    {
        CurrentWave++;
        Wave.Clear();
        for(int i = 0; i < (AmountToSpawn * CurrentWave); i++)
        {
            Wave.Add(Enemies[Random.Range(0, Enemies.Length)]);
        }
        StartCoroutine(SpawnRoutine());
        

    }


}
