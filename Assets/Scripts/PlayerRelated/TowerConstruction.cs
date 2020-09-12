using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerConstruction : MonoBehaviour // renameto tower manager?
{
    private TowerConstruction _instance;
    public TowerConstruction Instance
    {
        get
        {
            if(_instance == null)
            {
                var t = new GameObject("TowerConstruction").AddComponent<TowerConstruction>();
                _instance = t;
            }
            return _instance;
        }
    }

    public static Action onIsPlacingTower; //Event system
    private void OnEnable()
    {
        _instance = this;
    }


    public GameObject[] Towers;
    [SerializeField]
    GameObject SelectedTower;
    [SerializeField]
    bool _canPlaceTower;
    [SerializeField]
    bool _isPlacingTower;
    public bool IsPlacingTower
    {
        get
        {
            return _isPlacingTower;
        }
    }
    void Update()
    {
        if (_isPlacingTower != true) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            TowerOutline(hit.point);
            CancelTowerCreation();
            Debug.Log(hit.transform.gameObject.name + " :: " + hit.transform.root.transform.name);
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.GetComponent<TowerLocation>()?.IsOccupied == false)
                {
                    hit.transform.GetComponent<TowerLocation>().PlaceTower(SelectedTower);
                    _isPlacingTower = false;
                }
            }

        }


    }

    public void GatlingTurret() // need to rework all of this just getting a prototype functional
    {
        SelectedTower = Towers[1];
        CreateTower();
    }
    public void MissleLauncher()
    {
        SelectedTower = Towers[3];
        CreateTower();
    }

    public void TowerOutline(Vector3 pos)
    {
        SelectedTower.transform.position = pos;
    }

    public void CreateTower() // change to pooling system
    {
        _isPlacingTower = true;
        onIsPlacingTower?.Invoke();
        var tower = Instantiate(SelectedTower);
        SelectedTower = tower;
    }

    public void CancelTowerCreation() //if we right click stop placing the tower.
    {
        if (Input.GetMouseButtonDown((1)))
        {
            _isPlacingTower = false;
            SelectedTower.SetActive(false); //prepping for pooling system
        }
    }


}
