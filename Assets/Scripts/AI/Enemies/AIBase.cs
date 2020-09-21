using UnityEngine;
using UnityEngine.AI;

//attribute require component
namespace CurtisDH.Scripts.Enemies
{
    using CurtisDH.Scripts.Managers;
    using CurtisDH.Scripts.PlayerRelated;
    using System;
    using System.Collections;

    public abstract class AIBase : MonoBehaviour
    {
        public static Action<GameObject,GameObject,bool> onAiDeath;


        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private float _maxHP;
        [SerializeField]
        private float _currentHealth;
        public float Health { get => _currentHealth; set => _currentHealth = value; }
        [SerializeField]
        private float _speed;
        [SerializeField]
        bool _AICollision = true; // When set to true - AI Will not be able to walk through eachother.
        [SerializeField]
        private int _iD;
        public int ID
        {
            get
            {
                return _iD;
            }
        }
        [SerializeField]
        private int _warFund;
        public int WarFund
        {
            get
            {
                return _warFund;
            }
        }
        Animator _anim;
        [SerializeField]
        float _deathTime = 3;

        // How much money is awarded for killing the enemy.         //Make this value protected??
        // Might add my own twist to the income. replacing this feature.
        // Might influence warfund based on how far the enemy has made it into the course;

        private void OnEnable()
        {
            InitaliseAI();
            if(_anim == null)
            {
                _anim = this.gameObject.GetComponent<Animator>();
            }

            _anim?.SetTrigger("Reset");            
            PlayerBase.onPlayerBaseReached += onDeath;
            Tower.onDamageEnemy += ReceiveDamage;
        }
        private void OnDisable()
        {
            PlayerBase.onPlayerBaseReached -= onDeath;
            Tower.onDamageEnemy -= ReceiveDamage;
        }
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
            if (_AICollision == true)
            {
                _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            }
            else
            {
                _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
            _agent.Warp(SpawnManager.Instance.StartPos); // had issues with unity saying "NO AGENT ON NAVMESH" using Warp fixes this.
            MoveTo(SpawnManager.Instance.EndPos);
            transform.parent = GameObject.Find("EnemyContainer").transform;
            _agent.speed = _speed;
            _currentHealth = _maxHP;

        }

        public virtual void MoveTo(Vector3 position)
        {
            if (_agent != null)
            {
                _agent.SetDestination(position);
                //Debug.Log("AIBase::Destination Acquired " + position);
            }
            else
            {
                if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
                {
                    _agent = GetComponent<NavMeshAgent>();
                }
                else // ..If there is no NavmeshAgent then create one and assign agent to it.
                {
                    Debug.Log("AIBase::No NavmeshAgent found Assigning one... ");
                    gameObject.AddComponent<NavMeshAgent>();
                    _agent = GetComponent<NavMeshAgent>();
                    Debug.Log("AIBase::NavmeshAgent Created and Assigned Successfully ");
                    _agent.SetDestination(position);
                    Debug.Log("AIBase::Destination Acquired After Creating NavMeshAgent" + position);

                }
            }
        }
        private void ReceiveDamage(GameObject enemy,float damage)
        {
            if(enemy == this.gameObject)
            {
                Health -= damage;
                if(Health <= 0)
                {
                    onDeath(gameObject); // still need to play death animation
                }
            }
        }
        public virtual void onDeath(GameObject obj) //make event system detect on death
        {
            if (obj == this.gameObject)
            {
                onAiDeath?.Invoke(obj,null,true);
                StartCoroutine(DeathRoutine());
                
            }
        }
        IEnumerator DeathRoutine()
        {
            _anim.SetTrigger("Death");
            _agent.speed = 0;
            yield return new WaitForSeconds(_deathTime);
            _anim.WriteDefaultValues();
            PoolManager.Instance.ObjectsReadyToRecycle(gameObject, true, _iD, _warFund);
            SpawnManager.Instance.CreateWave(); // checks if all AI is dead then creates new wave if they are.
            gameObject.SetActive(false);
        }
    }
}

