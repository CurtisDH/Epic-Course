using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLocation : MonoBehaviour
{
    [SerializeField]
    GameObject TurretOccupying;
    [SerializeField]
    bool _isOccupied;
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
