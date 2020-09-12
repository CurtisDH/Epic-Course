using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [SerializeField]
    private int _warFund;
    [SerializeField]
    private int _towerID;
    [SerializeField]
    private float _towerRadius;
    public float TowerRadius
    {
        get
        {
            return _towerRadius;
        }
    }
    public int TowerID
    {
        get
        {
            return _towerID;
        }
    }
    public int WarFund
    {
        get
        {
            return _warFund;
        }
    }
    public virtual void TargetEnemy()
    {

    }

}
