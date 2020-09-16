using Newtonsoft.Json.Serialization;
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
        set => _warFund = WarFund;
    }
    [SerializeField]
    protected int _towerID;
    public int TowerID
    {
        get => _towerID;
        set => _towerID = TowerID;
    }
    [SerializeField]
    protected float _towerRadius;
    public float TowerRadius
    {
        get => _towerRadius;
        set => _towerRadius = TowerRadius;
    }
    [SerializeField]
    protected Queue<GameObject> _enemiesInRange;
    public Queue<GameObject> EnemiesInRange 
    {
        get => _enemiesInRange;
        set => _enemiesInRange = value;
    }
    private void OnEnable()
    {
        EnemiesInRange = new Queue<GameObject>();
        TowerEnemyDetection.onEnemyDetectionRadius += AddEnemyToQueue;
    }
    /*
     When an enemy walks into the tower radius - collider trigger
        Add enemy to a queue list
        make the tower look at the enemy deal damage & play a firing animation
    Rotation & Firing
        set a bool to true - While true we rotate the turret to face the targeted enemy specified by the queueing system
        play the firing animation until the queue is empty.. 
        once the queue is empty we set the bool to false ending the animation sequence and then we return the rotation to normal.
        
    */
    public virtual void TargetEnemy()
    {
        throw new System.NotImplementedException();
    }
    public void AddEnemyToQueue(GameObject enemy,GameObject turret)
    {
        Debug.Log("Tower::Enemy:" + enemy + " Turret:" + turret);
        if (turret != this.gameObject) return;
        _enemiesInRange.Enqueue(enemy);


    }

    private void OnDisable()
    {
        TowerEnemyDetection.onEnemyDetectionRadius -= AddEnemyToQueue;
    }


}
