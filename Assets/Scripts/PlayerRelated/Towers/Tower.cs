using CurtisDH.Scripts.Enemies;
using CurtisDH.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

    protected GameObject _targetedEnemy;

    public static Action<GameObject, float> onDamageEnemy;

    bool _enemyInRange;
    [SerializeField]
    float _fireRate; // this will determine how quickly we deal damage to an enemy.
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

    private void OnEnable()
    {
        EnemiesInRange = new List<GameObject>();
        TowerEnemyDetection.onEnemyDetectionRadius += AddEnemyToQueue;
        AIBase.onAiDeath += AddEnemyToQueue;
        UIManager.onTowerUpgrade += UpgradeTower;
        UIManager.onTowerCancel += DeselectTower;
        gameObject.GetComponent<Collider>().enabled = false;
        if(_currentUpgradedTower == null && _upgradedTowerPrefab != null)
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
    }
    private void OnMouseDown()
    {
        Debug.Log("TOWER:: ON MOUSE DOWN");
        _isSelected = true;
        UIManager.Instance.ToggleUpgradeUI(_towerID);
        //open UI & create a highlight shader??
    }
    private void Update() //triggerstay
    {
        //TEMP
        if (Input.GetKeyDown(KeyCode.O))
        {
            UpgradeTower();
        }
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
            onDamageEnemy?.Invoke(_targetedEnemy, _damage);
            yield return new WaitForSeconds(time);
        }
    }

    public void UpgradeTower()
    {
        if (_currentUpgradedTower == null)
        {
            return;
        }
        //switch(_towerID)
        //{
        //    case 0:

        //        break;
        //    case 1:
        //        break;
        //    default:
        //        Debug.LogError("Tower::NO TOWER ID OF " + _towerID);
        //        break;
        //}
        if(_isSelected)
        {
            _currentUpgradedTower.SetActive(true);
            _currentUpgradedTower.transform.parent = null; // remove parent object so its stays active
            _currentUpgradedTower.transform.position = gameObject.transform.position;
            _currentUpgradedTower = null;
            PoolManager.Instance.ObjectsReadyToRecycle(gameObject, false, _towerID);
        }

    }
    public void DeselectTower()
    {
        _isSelected = false;
        _towerRadiusRenderer.enabled = (false);
    }

    private void OnDisable()
    {
        TowerEnemyDetection.onEnemyDetectionRadius -= AddEnemyToQueue;
        AIBase.onAiDeath -= AddEnemyToQueue;
        UIManager.onTowerUpgrade -= UpgradeTower;
        UIManager.onTowerCancel -= DeselectTower;
    }


}
