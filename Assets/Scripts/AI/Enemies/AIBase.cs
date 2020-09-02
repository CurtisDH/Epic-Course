using UnityEngine;
using UnityEngine.AI;

public abstract class AIBase : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent _agent;
    public float health;
    public float speed;
    [SerializeField]
    bool AICollision = true; // When set to true - AI Will not be able to walk through eachother.

    [SerializeField]
    private int _warFund;
    public int WarFund
    {
        get
        {
            return _warFund;
        }
    }


    // How much money is awarded for killing the enemy.         //Make this value protected??
    // Might add my own twist to the income. replacing this feature.
    // Might influence warfund based on how far the enemy has made it into the course;


    public void InitaliseAI()
    {
        if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        else // ..If there is no NavmeshAgent then create one and assign agent to it.
        {
            gameObject.AddComponent<NavMeshAgent>();
            _agent = GetComponent<NavMeshAgent>();
        }
        if(AICollision == true)
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        else
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        _agent.speed = speed;

    }

    public virtual void MoveTo(Vector3 position)
    {
        if(_agent !=null)
        {
            _agent.SetDestination(position);
            Debug.Log("Destination Acquired " + position);
        }
        else
        {
            if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            else // ..If there is no NavmeshAgent then create one and assign agent to it.
            {
                Debug.Log("No NavmeshAgent found Assigning one... ");
                gameObject.AddComponent<NavMeshAgent>();
                _agent = GetComponent<NavMeshAgent>();
                Debug.Log("NavmeshAgent Created and Assigned Successfully ");
                _agent.SetDestination(position);
                Debug.Log("Destination Acquired After Creating NavMeshAgent" + position);

            }
        }
    }

    public virtual void onDeath()
    {
        PoolManager.Instance.PooledObjects.Add(this.gameObject); // haven't setup animation transitions yet.
        this.gameObject.transform.parent = null;
        //play death animation and then setactive false, then recyle the gameobj;
        this.gameObject.SetActive(false);
    }
}
