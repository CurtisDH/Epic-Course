using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerConstruction : MonoBehaviour
{
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
    //lots to do for this class.. Got to recycle turrets AKA pooling system
    //Particle system for showing placeable turrets

    void Update()
    {
        if (_isPlacingTower != true) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.green);

        Debug.Log("TowerConstruction::Clicked");
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("TowerConstruction::IfRaycast");
            TowerOutline(hit.point);
            Debug.Log(hit.transform.gameObject.name + " :: " + hit.transform.root.transform.name);
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.GetComponent<TowerLocation>()?.IsOccupied == false)
                {
                    Debug.Log("TowerConstruction::IsOccupied = false");
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
        var tower = Instantiate(SelectedTower);
        SelectedTower = tower;
    }

    public void CancelTowerCreation()
    {
        if (Input.GetMouseButtonDown((1)))
        {
            _isPlacingTower = false;
            SelectedTower.SetActive(false); //prepping for pooling system
        }
    }


}
