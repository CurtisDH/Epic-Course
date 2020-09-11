using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLocation : MonoBehaviour
{
    [SerializeField]
    GameObject TurretOccupying;
    [SerializeField]
    bool _isOccupied;
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
