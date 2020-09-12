using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLocation : MonoBehaviour //setup event system to play particles
{
    [SerializeField]
    GameObject TurretOccupying;
    [SerializeField]
    bool _isOccupied;
    private void OnEnable()
    {
        TowerConstruction.onIsPlacingTower += EnableParticleSystem;
    }
    private void OnDisable()
    {
        TowerConstruction.onIsPlacingTower -= EnableParticleSystem;
    }

    private void OnMouseEnter()
    {
        //snap object
    }
    private void OnMouseDown()
    {
        //place object.
    }
    private void OnMouseExit()
    {
        //release object
    }
    //event system check if isPlacingTower is happening & if isoccupied is false
    public bool IsOccupied
    {
        get
        {
            return _isOccupied;
        }
    }
    public void EnableParticleSystem()
    {

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
