using UnityEngine;
using UnityEngine.AI;

//attribute require component
namespace CurtisDH.Scripts.Enemies
{
    using CurtisDH.Scripts.Managers;
    using CurtisDH.Scripts.PlayerRelated;
    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine.UI;

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
        [SerializeField]
        float _dissolveTime;
        [SerializeField]
        Renderer[] _dissolveMaterials;
        [SerializeField]
        GameObject _deathParticles;

        GameObject _intentionallyNull = null;
        [SerializeField]
        GameObject _hipRotation;
        GameObject _turretToLookAt;
        [SerializeField]
        bool _targetTurret;
        [SerializeField]
        TextMeshProUGUI _healthBar;

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
            EventManager.Listen("onEnemyDetectionRadius", (Action<GameObject, GameObject, bool>)TargetTurret);
            _deathParticles.SetActive(false);
            foreach (var obj in _dissolveMaterials)
            {
                StartCoroutine(Dissolve(obj, false));
            }
        }
        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onPlayerBaseReached", (Action<GameObject, bool>)onDeath);
            EventManager.UnsubscribeEvent("onEnemyDetectionRadius", (Action<GameObject, GameObject,bool>)TargetTurret);
            EventManager.UnsubscribeEvent("onDamageEnemy", (Action<GameObject, float, bool>)ReceiveDamage);
        }
        void Update()
        {
            if (_targetTurret)
            {
                Vector3 direction = (_hipRotation.transform.position -_turretToLookAt.transform.position);
                //float dir = _turretToLookAt.transform.position.z - _hipRotation.transform.position.z;
                //_hipRotation.transform.eulerAngles = new Vector3(0,0,direction.z);
                _hipRotation.transform.Rotate(0,0,direction.z, Space.Self);
                //_hipRotation.transform.LookAt(_turretToLookAt.transform);
            }
            
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
        public void TargetTurret(GameObject mech, GameObject turret,bool b)
        {
            if (mech == this.gameObject)
            {
                _turretToLookAt = turret;
                _anim.SetTrigger("Shoot");
                if (_hipRotation != null) _targetTurret = true;
            }
                
        }
        private void ReceiveDamage(GameObject enemy, float damage, bool towerDeath)
        {
            if (enemy == this.gameObject)
            {
                Health -= damage;
                if(_healthBar!=null)
                _healthBar.text = "" + _currentHealth;
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
                    yield return new WaitForSeconds(_dissolveTime / 100);
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
                    yield return new WaitForSeconds(_dissolveTime / 100);
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
            yield return new WaitForSeconds(_deathTime);
            _deathParticles.SetActive(false); // play explosion sound
            _anim.WriteDefaultValues();
            PoolManager.Instance.ObjectsReadyToRecycle(gameObject, true, _iD, _warFund);
            SpawnManager.Instance.CreateWave(); // checks if all AI is dead then creates new wave if they are.
            gameObject.SetActive(false);
        }
    }
}