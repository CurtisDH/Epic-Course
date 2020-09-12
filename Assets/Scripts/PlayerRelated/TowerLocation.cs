using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLocation : MonoBehaviour //setup event system to play particles
{
    [SerializeField]
    GameObject TurretOccupying;
    [SerializeField]
    bool _isOccupied;


    [SerializeField]
    GameObject AvailableSpotPrefab;
    [SerializeField]
    GameObject _activeAvailableSpot;
    public static Action<Vector3,bool> onMouseEnter,onMouseExit;
    public static Action<Vector3> onMouseDown;

    private void OnEnable()
    {
        TowerConstruction.onIsPlacingTower += ToggleParticleSystem;
    }
    private void OnDisable()
    {
        TowerConstruction.onIsPlacingTower -= ToggleParticleSystem;
    }

    private void OnMouseEnter()
    {
        if (_isOccupied) return;
        onMouseEnter?.Invoke(this.gameObject.transform.position,true);
        Debug.Log(this.gameObject +"::TowerLocation");
    }
    private void OnMouseDown()
    {
        if (_isOccupied) return;
        onMouseDown?.Invoke(this.gameObject.transform.position);
        _isOccupied = true;
    }
    private void OnMouseExit()
    {
        onMouseExit?.Invoke(Vector3.zero, false);
    }
    //event system check if isPlacingTower is happening & if isoccupied is false
    public bool IsOccupied
    {
        get
        {
            return _isOccupied;
        }
    }
    public void ToggleParticleSystem(bool toggle)
    {
        if (_isOccupied) return;
        if(_activeAvailableSpot == null)
        {
            var T =Instantiate(AvailableSpotPrefab);
            T.transform.parent = this.transform;
            T.transform.position = this.transform.position;
            _activeAvailableSpot = T;
        }
        _activeAvailableSpot.SetActive(toggle);
        //particlesystem.setactive = toggle;
    }
    public void PlaceTower(GameObject obj)
    {
        TurretOccupying = obj;
        TurretOccupying.transform.position = this.transform.position;
        _isOccupied = true;
    }
    public void RemoveTower()
    {
        TurretOccupying = null;
        _isOccupied = false;
    }
}
