using CurtisDH.Scripts.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField]
    protected int _warFund;
    public int WarFund
    {
        get => _warFund;
        set => _warFund = value;
    }
    [SerializeField]
    protected int _towerID;
    public int TowerID
    {
        get => _towerID;
        set => _towerID = value;
    }
    [SerializeField]
    protected float _towerRadius;
    public float TowerRadius
    {
        get => _towerRadius;
        set => _towerRadius = value;
    }
    [SerializeField]
    protected List<GameObject> _enemiesInRange;
    public List<GameObject> EnemiesInRange
    {
        get => _enemiesInRange;
        set => _enemiesInRange = value;
    }

    private GameObject _targetedEnemy;

    public static Action<GameObject,float> onDamageEnemy;
    private void OnEnable()
    {
        EnemiesInRange = new List<GameObject>();
        TowerEnemyDetection.onEnemyDetectionRadius += AddEnemyToQueue;
        AIBase.onAiDeath += AddEnemyToQueue;
        // just incase we forget to set firerate I dont want somehow crash the application
        if (_fireRate == 0)
        {
            _fireRate = 0.25f;
        }
    }
    bool _enemyInRange;
    [SerializeField]
    float _fireRate; // this will determine how quickly we deal damage to an enemy.
    [SerializeField]
    float _damage;
    [SerializeField]
    GameObject _rotation;
    [SerializeField]
    protected bool _isCoroutineRunning = false;

    private void Update() //triggerstay
    {
        if (_enemyInRange = true && _enemiesInRange.Count != 0)
        {
            TargetEnemy();
        }
        else
        {
            StopFiring();
        }
    }
    public virtual void TargetEnemy()
    {
        
        //we can add a shoot method in here. We need a way to detect if/when the target swaps 
        if (_enemiesInRange.Count != 0)
        {
            
            var enemy = _enemiesInRange[0];
            _targetedEnemy = enemy;
        }
        _rotation.transform.LookAt(_targetedEnemy.transform);
        if (_isCoroutineRunning == false)
        {
            StartCoroutine(DamageEnemy(_fireRate));
        }
        
    }
    public virtual void StopFiring()
    {
        _isCoroutineRunning = false;
        StopAllCoroutines();
    }
    public void AddEnemyToQueue(GameObject enemy, GameObject turret, bool onTriggerExit)
    {
        if(turret == null)
        {
            turret = this.gameObject;
        }
        if (turret != this.gameObject || enemy.CompareTag("Enemy") == false)
        {
            return;
        }
        if (onTriggerExit == true) // if the enemy is leaving
        {
            _enemiesInRange?.Remove(enemy);
        }
        else //the enemy is entering
        {
            _enemiesInRange?.Add(enemy);
        }
    }
    IEnumerator DamageEnemy(float time)
    {
        _isCoroutineRunning = true;
        while (true)
        {
            onDamageEnemy?.Invoke(_targetedEnemy,_damage);
            yield return new WaitForSeconds(time);
        }


    }

    private void OnDisable()
    {
        TowerEnemyDetection.onEnemyDetectionRadius -= AddEnemyToQueue;
    }


}
