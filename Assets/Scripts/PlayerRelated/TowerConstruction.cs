using CurtisDH.Utilities;
using System;
using UnityEngine;

public class TowerConstruction : MonoBehaviour // renameto tower manager?
{
    private TowerConstruction _instance;
    public TowerConstruction Instance
    {
        get
        {
            if (_instance == null)
            {
                var t = new GameObject("TowerConstruction").AddComponent<TowerConstruction>();
                _instance = t;
            }
            return _instance;
        }
    }

    public static Action<bool> onIsPlacingTower; //Event system
    private void OnEnable()
    {
        TowerLocation.onMouseDown += PlaceTower;
        TowerLocation.onMouseEnter += SnapTower;
        TowerLocation.onMouseExit += SnapTower;
        _instance = this;
    }
    private void OnDisable()
    {
        TowerLocation.onMouseDown -= PlaceTower;
        TowerLocation.onMouseEnter -= SnapTower;
        TowerLocation.onMouseExit -= SnapTower;
    }

    [SerializeField]
    GameObject TurretShader;
    [SerializeField]
    Color InvalidPlacement=Color.red, ValidPlacement=Color.green;
    public GameObject[] Towers;
    [SerializeField]
    GameObject SelectedTower;
    [SerializeField]
    bool _canPlaceTower;
    [SerializeField]
    bool _isPlacingTower;
    [SerializeField]
    bool _snappedTower = false;
    
    public bool IsPlacingTower
    {
        get
        {
            return _isPlacingTower;
        }
    }

    private void Update()
    {
        if (_isPlacingTower != true) return;
        RightClickCancel();//If we press rightclick then cancel the towerplacement.
        if (_snappedTower == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit))
            {
                TowerOutline(hit.point);
            }
        }
        
    }

    public void PlaceTower(Vector3 pos) //receives the gameobject from towerlocation
    {
        if (_isPlacingTower)
        {
            SelectedTower.transform.position = pos;
            CancelTowerCreation();
        }

    }
    public void SnapTower(Vector3 pos, bool isSnapped) //if this runs set a bool to say hey we've snapped dont update me until i say so 
    {
        if (_isPlacingTower)
        {
            _snappedTower = isSnapped;
            SelectedTower.transform.position = pos;
        }
    }
    public void GatlingTurret() // need to rework all of this just getting a prototype functional
    {
        if(_isPlacingTower)
        {
            CancelTowerCreation();
            TowerToRecycle(SelectedTower);
        }

        SelectedTower = Towers[1];
        CreateTower();
    }
    public void MissleLauncher()
    {
        if (_isPlacingTower)
        {
            CancelTowerCreation();
            TowerToRecycle(SelectedTower);
        }
        SelectedTower = Towers[3];
        CreateTower();
    }

    public void TowerOutline(Vector3 pos)
    {
        SelectedTower.transform.position = pos;
        //set the outline active 
    }

    public void CreateTower() // pull out of the pooling system.
    {
        _isPlacingTower = true;
        onIsPlacingTower?.Invoke(_isPlacingTower);
        var tower = Instantiate(SelectedTower);
        var selectionfield = Instantiate(TurretShader);
        selectionfield.transform.parent = tower.transform;
        selectionfield.transform.position = tower.transform.position;
        selectionfield.GetComponent<Material>().color = InvalidPlacement;
        SelectedTower = tower;
    }

    public void CancelTowerCreation()
    {
        _isPlacingTower = false;
        onIsPlacingTower?.Invoke(_isPlacingTower);
    }

    void RightClickCancel()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelTowerCreation();
            TowerToRecycle(SelectedTower); //add to pooling system
        }
    }

    void TowerToRecycle(GameObject obj)
    {
        obj.SetActive(false);
    }

}
