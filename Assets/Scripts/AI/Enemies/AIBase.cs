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
        Animator _anim;
        [SerializeField]
        float _deathTime = 3;
        [SerializeField]
        float _dissolveTime;
        [SerializeField]
        Renderer[] _dissolveMaterials;
        [SerializeField]
        GameObject _deathParticles;

        GameObject _intentionallyNull = null;
        [SerializeField]
        GameObject _hipRotation;

        #region yield returns
        private WaitForSeconds _deathTimer;
        private WaitForSeconds _dissolvetimer;
        #endregion
        private void Start()
        {
            _dissolvetimer = new WaitForSeconds(_dissolveTime/100);
            _deathTimer = new WaitForSeconds(_deathTime);
        }
        private void OnEnable()
        {
            InitaliseAI();
            if (_anim == null)
            {
                _anim = this.gameObject.GetComponent<Animator>();
            }

            _anim?.SetTrigger("Reset");
            EventManager.Listen("onPlayerBaseReached", (Action<GameObject, bool>)onDeath);
            EventManager.Listen("onDamageEnemy", (Action<GameObject, float, bool>)ReceiveDamage);
           
            _deathParticles.SetActive(false);
            foreach (var obj in _dissolveMaterials)
            {
                StartCoroutine(Dissolve(obj, false));
            }

            EventManager.RaiseEvent("onAiSpawn", gameObject, true);
        }
        private void OnDisable()
        {
            EventManager.RaiseEvent("onAiSpawn", gameObject, false);
            EventManager.UnsubscribeEvent("onPlayerBaseReached", (Action<GameObject, bool>)onDeath);
            EventManager.UnsubscribeEvent("onDamageEnemy", (Action<GameObject, float, bool>)ReceiveDamage);
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

        private void ReceiveDamage(GameObject enemy, float damage, bool towerDeath)
        {
            if (enemy == this.gameObject)
            {
                Health -= damage;
                if (Health <= 0)
                {
                    //onDeath bool checks to see if it died from the endZone.
                    onDeath(gameObject, !towerDeath);
                }


            }
        }
        IEnumerator Dissolve(Renderer obj, bool deathRoutine)
        {
            //fix this loop up. maybe use time.Deltatime
            //float current = 0;
            if (deathRoutine == true)
            {
                //while (current < 1)
                //{
                //    current += (Time.deltaTime/_dissolveTime);
                //    obj.material.SetFloat("_fillAmount", current);
                //}
                for (float i = 0; i < 1; i += 0.01f)
                {
                    yield return _dissolvetimer;
                    obj.material.SetFloat("_fillAmount", i);
                }
            }
            else
            {
                //current = 1;
                //while (current > 0)
                //{
                //    current -= (Time.deltaTime / _dissolveTime);
                //    obj.material.SetFloat("_fillAmount", current);
                //}

                for (float i = 1; i > 0; i -= 0.01f)
                {
                    yield return _dissolvetimer;
                    obj.material.SetFloat("_fillAmount", i);
                }
            }
        }
        public virtual void onDeath(GameObject obj, bool endZoneDeath) //make event system detect on death
        {
            if (obj == this.gameObject)
            {
                EventManager.RaiseEvent("onAiDeath", obj, _intentionallyNull, true);
                if (endZoneDeath)
                {
                    //decrease the warfund we died to endzone
                    GameManager.Instance.AdjustWarfund(-_warFund);
                }
                else
                {
                    // increase the warfund if we didn't die to the end zone.
                    GameManager.Instance.AdjustWarfund(_warFund);
                }

                StartCoroutine(DeathRoutine());

            }
        }
        IEnumerator DeathRoutine()
        {
            _anim.SetTrigger("Death");
            _agent.speed = 0;
            _deathParticles.SetActive(true);
            foreach (var obj in _dissolveMaterials)
            {
                StartCoroutine(Dissolve(obj, true));
            }
            yield return _deathTimer;
            // had odd cases where the enemy wasn't removed from the turret enemy list
            //warping the enemy out should remove it from the list if it isnt already.
            _agent.Warp(SpawnManager.Instance.StartPos);
            _deathParticles.SetActive(false); // play explosion sound here with particle
            _anim.WriteDefaultValues();
            PoolManager.Instance.ObjectsReadyToRecycle(gameObject, true, _iD, _warFund);
            SpawnManager.Instance.CreateWave(); // checks if all AI is dead then creates new wave if they are.
            gameObject.SetActive(false);
        }
    }
}