using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AIBase : MonoBehaviour
{
    NavMeshAgent agent;
    public float health;
    public int warFund;
    private void Start()
    {
        if (GetComponent<NavMeshAgent>() != null) 
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else // If there is no NavmeshAgent then create one.
        {
            gameObject.AddComponent<NavMeshAgent>();
            agent = GetComponent<NavMeshAgent>();
        }
        MoveTo(GameObject.Find("Base").transform.position);
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }



}
