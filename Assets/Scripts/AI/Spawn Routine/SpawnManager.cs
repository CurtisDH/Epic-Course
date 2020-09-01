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


    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    IEnumerator SpawnRoutine() 
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            var enemy = Instantiate(Enemies[Random.Range(0,Enemies.Length)]); // might change to create a spawn list of enemies which are randomly defined for that wave.
            enemy.transform.position = StartPos;
            if(enemy.GetComponent<AIBase>())
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

    }


}
