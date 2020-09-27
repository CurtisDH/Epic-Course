using CurtisDH.Scripts.Enemies;
using CurtisDH.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

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
    private int _upgradeCost;
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

    protected GameObject _targetedEnemy;
    bool _enemyInRange;
    [SerializeField]
    protected float _fireRate; // this will determine how quickly we deal damage to an enemy.
    [SerializeField]
    float _damage;
    [SerializeField]
    GameObject _rotation;
    [SerializeField]
    protected bool _isCoroutineRunning = false;

    [SerializeField]
    GameObject _currentUpgradedTower; //
    [SerializeField]
    GameObject _upgradedTowerPrefab;
    [SerializeField]
    GameObject _towerRadiusShader;
    [SerializeField]
    bool _isSelected;
    private GameObject _currentLocation;
    private void OnEnable()
    {
        EnemiesInRange = new List<GameObject>();

        
        gameObject.GetComponent<Collider>().enabled = false; //cache
        if (_currentUpgradedTower == null && _upgradedTowerPrefab != null)
        {
            var stagedTower = Instantiate(_upgradedTowerPrefab);
            stagedTower.SetActive(false);
            stagedTower.transform.position = this.gameObject.transform.position;
            stagedTower.transform.localScale = new Vector3(.75f, .75f, .75f);
            _currentUpgradedTower = stagedTower;

        }
        //if a prefab exists and it's currently not active in the scene
        if (_towerRadiusShader != null && !_towerRadiusShader.activeInHierarchy)
        {
            var shader = Instantiate(_towerRadiusShader);
            shader.transform.localScale = new Vector3(_towerRadius, _towerRadius, _towerRadius);
            shader.transform.position = gameObject.transform.position;
            //move the shader into prefab, just doing it like this as i need to rework the entire
            //shader radius system
            shader.transform.parent = this.transform;
            shader.GetComponent<Renderer>().enabled = false;
            shader.AddComponent<SphereCollider>().isTrigger = true;
            shader.AddComponent<Rigidbody>().isKinematic = true;
            shader.name = "TowerRadius";
            gameObject.GetComponent<Collider>().enabled = true;
            _towerRadiusShader = shader; // Removes the prefabbed radius replacing it with active.
        }
        // just incase we forget to set firerate I dont want somehow crash the application
        if (_fireRate == 0)
        {
            _fireRate = 0.25f;
        }
        EventManager.Listen("onEnemyDetectionRadius", (Action<GameObject, GameObject, bool>)AddEnemyToQueue);
        EventManager.Listen("onAiDeath", (Action<GameObject, GameObject, bool>)AddEnemyToQueue);
        EventManager.Listen("onTowerUpgrade", UpgradeTower);
        EventManager.Listen("onTowerCancel", DeselectTower);
        EventManager.Listen("onTowerSell", SellTower);
        EventManager.Listen("onPlaceTower", (Action<GameObject,GameObject>)AddTowerLocation);

    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onEnemyDetectionRadius", (Action<GameObject, GameObject, bool>)AddEnemyToQueue);
        EventManager.UnsubscribeEvent("onAiDeath", (Action<GameObject,GameObject,bool>)AddEnemyToQueue);
        EventManager.UnsubscribeEvent("onTowerUpgrade", UpgradeTower);
        EventManager.UnsubscribeEvent("onTowerCancel", DeselectTower);
        EventManager.UnsubscribeEvent("onTowerSell", SellTower);
        EventManager.UnsubscribeEvent("onPlaceTower", (Action<GameObject, GameObject>)AddTowerLocation);
    }
    private void OnMouseDown()
    {
        Debug.Log("TOWER:: ON MOUSE DOWN");
        _isSelected = !_isSelected;
        if (_towerRadiusShader != null)
            _towerRadiusShader.GetComponent<Renderer>().enabled = _isSelected;
        UIManager.Instance.ToggleUpgradeUI(_towerID,_upgradeCost);
        //open UI & create a highlight shader??
    }
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
    public void SellTower()
    {
        //give the player money for selling
        if(_isSelected)
        {
            EventManager.RaiseEvent("onSoldTower", gameObject);
            GameManager.Instance.AdjustWarfund(_warFund / 2);
            PoolManager.Instance.ObjectsReadyToRecycle(gameObject, false, _towerID);
        }
    }
    /// <summary>
    /// if turret == null turret is assigned by method. onTriggerExit:: are we exiting the trigger (Tower radius)
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="turret"></param>
    /// <param name="onTriggerExit"></param>
    public void AddEnemyToQueue(GameObject enemy, GameObject turret, bool onTriggerExit=false)
    {
        if (turret == null)
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
            //Passing in what enemy we are attacking, then the damage it needs to take.
            // by passing in true we say "we've died from the tower" 
            EventManager.RaiseEvent("onDamageEnemy",_targetedEnemy,_damage,true);
            yield return new WaitForSeconds(time);
        }
    }

    public void UpgradeTower()
    {
        if (_isSelected)
        {
            if(GameManager.Instance.WarFund >= _upgradeCost)
            {
                _currentUpgradedTower.SetActive(true);
                _currentUpgradedTower.transform.parent = null; // remove parent object so its stays active
                _currentUpgradedTower.transform.position = gameObject.transform.position;
                //we pass in the tower that we're upgrading to & the location of the old tower
                EventManager.RaiseEvent("onPlaceTower", _currentUpgradedTower, _currentLocation);
                _currentUpgradedTower = null;
                _currentLocation = null;
                _isSelected = false;
                GameManager.Instance.AdjustWarfund(-_upgradeCost);
                PoolManager.Instance.ObjectsReadyToRecycle(gameObject, false, _towerID);
            }
        }

    }
    public void DeselectTower()
    {
        _isSelected = false;
        if (_towerRadiusShader != null)
            _towerRadiusShader.GetComponent<Renderer>().enabled = _isSelected;
    }

    public void AddTowerLocation(GameObject tower,GameObject location)
    {
        if(tower == this.gameObject)
        {
            _currentLocation = location;
        }
    }

}
