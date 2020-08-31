using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{
    NavMeshAgent _agent;
    public float health;
    [SerializeField]
    protected int warFund; // How much money is awarded for killing the enemy. // Might add my own twist to the income. 
    // Reason for protected - Allows for inherited scripts to access warFund. Not sure if required currently.
    private void Start()
    {
        if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        else // ..If there is no NavmeshAgent then create one.
        {
            gameObject.AddComponent<NavMeshAgent>();
            _agent = GetComponent<NavMeshAgent>();
        }
        MoveTo(GameObject.Find("Base").transform.position); // temp for testing movement.
    }

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }



}
