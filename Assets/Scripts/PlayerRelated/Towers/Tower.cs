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
    GameObject _gunToFaceEnemy;
    /*
     When an enemy walks into the tower radius - collider trigger
        Add enemy to a list
        make the tower look at the enemy deal damage & play a firing animation
    Rotation & Firing
        set a bool to true - While true we rotate the turret to face the targeted enemy specified by the queueing system
        play the firing animation until the queue is empty.. 
        once the queue is empty we set the bool to false ending the animation sequence and then we return the rotation to normal.
        
    */

    //ways to detect front enemy... 
    // if we attach a collider to the very first enemy to spawn of x wave then pass it to
    //whoever runs through it.


    bool temp = false;
    private void Update()
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
        transform.LookAt(_targetedEnemy.transform);
        if (temp == false)
        StartCoroutine(DamageEnemy());
    }
    public virtual void StopFiring()
    {

    }
    public void AddEnemyToQueue(GameObject enemy, GameObject turret, bool onTriggerExit)
    {
        if (turret != this.gameObject || enemy.CompareTag("Enemy") == false)
        {
            return;
        }
        if (onTriggerExit == true)
        {
            _enemiesInRange?.Remove(enemy);
        }
        else
        {
            _enemiesInRange?.Add(enemy);
        }
    }
    IEnumerator DamageEnemy()
    {
        temp = true;
        while (true)
        {
            onDamageEnemy?.Invoke(_targetedEnemy,_damage);
            yield return new WaitForSeconds(_fireRate);
        }


    }

    private void OnDisable()
    {
        TowerEnemyDetection.onEnemyDetectionRadius -= AddEnemyToQueue;
    }


}
