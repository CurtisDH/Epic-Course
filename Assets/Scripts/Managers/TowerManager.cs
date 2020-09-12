using System;
using UnityEngine;
using CurtisDH.Scripts.PlayerRelated.Tower;
namespace CurtisDH.Scripts.Managers
{
    public class TowerManager : MonoBehaviour // renameto tower manager?
    {
        private TowerManager _instance;
        public TowerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var t = new GameObject("TowerConstruction").AddComponent<TowerManager>();
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
        Color InvalidPlacement = Color.red, ValidPlacement = Color.green;
        public GameObject[] Towers;
        [SerializeField]
        GameObject SelectedTower;
        [SerializeField]
        GameObject SelectedTowerShader;
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
                if (Physics.Raycast(ray, out hit))
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
                Destroy(SelectedTowerShader); //probably should recycle this
                CancelTowerCreation();
            }

        }
        public void SnapTower(Vector3 pos, bool isSnapped) //if this runs set a bool to say hey we've snapped dont update me until i say so 
        {
            if (_isPlacingTower)
            {
                if (isSnapped == true) //need to refine and remove getcomponent if possible
                {
                    SelectedTowerShader.GetComponent<Renderer>().material.color = ValidPlacement;
                }
                else
                {
                    SelectedTowerShader.GetComponent<Renderer>().material.color = InvalidPlacement;
                }
                _snappedTower = isSnapped;
                SelectedTower.transform.position = pos;
            }
        }
        public void GatlingTurret() // need to rework all of this just getting a prototype functional
        {//need a better way to access the towers warfund..
            if (GameManager.Instance.WarFund >= Towers[0].GetComponent<TowerBase>().WarFund) //need to link this to the turrets warfund
            {
                if (_isPlacingTower)
                {
                    CancelTowerCreation();
                    TowerToRecycle(SelectedTower);
                }

                SelectedTower = Towers[0];
                CreateTower(); //change value to the cost of the tower.
            }
            else
            {
                Debug.Log("TowerConstruction::" + GameManager.Instance.WarFund);
                //tell the users that there is not enough currency
            }
        }
        public void MissleLauncher() //need a better way to access the towers warfund..
        {
            if (GameManager.Instance.WarFund >= Towers[1].GetComponent<TowerBase>().WarFund)
            {
                if (_isPlacingTower)
                {
                    CancelTowerCreation();
                    TowerToRecycle(SelectedTower);
                }
                SelectedTower = Towers[1];
                CreateTower();
            }
            else
            {
                Debug.Log("TowerConstruction::" + GameManager.Instance.WarFund);
                //tell the users that there is not enough currency
            }
        }

        public void TowerOutline(Vector3 pos)
        {
            SelectedTower.transform.position = pos;
            //set the outline active 
        }

        //need to rework this entire method... (CURRENTLY A BRUTEFORCED MESS)
        public void CreateTower() // pull out of the pooling system.
        {
            _isPlacingTower = true;
            onIsPlacingTower?.Invoke(_isPlacingTower);
            var tower = Instantiate(SelectedTower);
            var selectionfield = Instantiate(TurretShader);
            selectionfield.transform.parent = tower.transform;
            selectionfield.transform.position = tower.transform.position;
            float TowerRadius = SelectedTower.GetComponent<TowerBase>().TowerRadius;
            selectionfield.transform.localScale = new Vector3(TowerRadius, TowerRadius, TowerRadius);
            SelectedTowerShader = selectionfield;
            selectionfield.GetComponent<Renderer>().material.color = InvalidPlacement; //find a better way to change color.
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
}

