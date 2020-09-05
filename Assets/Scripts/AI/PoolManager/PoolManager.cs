using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PooledObjects; //change to private
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("PoolManager is NULL creating one..");
                var PoolManager = new GameObject("PoolManager");
                PoolManager.AddComponent<PoolManager>();
                Debug.Log("Created");
                
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public void ObjectsReadyToRecycle(GameObject obj) // when object setActive = false (In the die method) add it to this list.
    {
        PooledObjects.Add(obj);
    }

    public void SpawnEnemy()
    {
        if (PooledObjects.Count == 0)
        {
            Instantiate(SpawnManager.Instance.Enemies[Random.Range(0, SpawnManager.Instance.Enemies.Length)]); // might change to create a spawn list of enemies which are randomly defined for that wave.
            
        }
        else
        {
            //Utilites.RandomiseList(PoolManager.Instance.PooledObjects); //move this to randomise once instead of everytime -- more performant
            PooledObjects[0].SetActive(true);
            PooledObjects.RemoveAt(0); 
        }
    }


}
