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
        GameObject _selectedTower;
        [SerializeField]
        GameObject _selectedTowerShader;
        [SerializeField]
        bool _canPlaceTower;
        [SerializeField]
        bool _isPlacingTower;
        [SerializeField]
        bool _snappedTower = false;
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
                _selectedTower.transform.position = pos;
                Destroy(_selectedTowerShader); //probably should recycle this
                CancelTowerCreation();
            }
        }
        public void SnapTower(Vector3 pos, bool isSnapped) //if this runs set a bool to say hey we've snapped dont update me until i say so 
        {
            if (_isPlacingTower)
            {
                if (isSnapped == true) //need to refine and remove getcomponent if possible
                {
                    _selectedTowerShader.GetComponent<Renderer>().material.color = ValidPlacement;
                }
                else
                {
                    _selectedTowerShader.GetComponent<Renderer>().material.color = InvalidPlacement;
                }
                _snappedTower = isSnapped;
                _selectedTower.transform.position = pos;
            }
        }
        public void GatlingTurret() // need to rework all of this just getting a prototype functional
        {//need a better way to access the towers warfund..
            if (GameManager.Instance.WarFund >= Towers[0].GetComponent<TowerBase>().WarFund) //need to link this to the turrets warfund
            {
                if (_isPlacingTower)
                {
                    CancelTowerCreation();
                    TowerToRecycle(_selectedTower);
                }

                _selectedTower = Towers?[0];
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
            if (GameManager.Instance.WarFund >= Towers?[1].GetComponent<TowerBase>().WarFund)
            {
                if (_isPlacingTower)
                {
                    CancelTowerCreation();
                    TowerToRecycle(_selectedTower);
                }
                _selectedTower = Towers[1];
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
            _selectedTower.transform.position = pos;
            //set the outline active 
        }

        //need to rework this entire method... (CURRENTLY A BRUTEFORCED MESS)
        public void CreateTower() // pull out of the pooling system.
        { //i think i should move createTower to the spawnManager..
            _isPlacingTower = true;
            if (PoolManager.Instance.RequestTower() == null)
            {
                onIsPlacingTower?.Invoke(_isPlacingTower);
                var tower = Instantiate(_selectedTower);
                var selectionfield = Instantiate(TurretShader);
                selectionfield.transform.parent = tower.transform;
                selectionfield.transform.position = tower.transform.position;
                float TowerRadius = _selectedTower.GetComponent<TowerBase>().TowerRadius; //event system
                selectionfield.transform.localScale = new Vector3(TowerRadius, TowerRadius, TowerRadius);
                _selectedTowerShader = selectionfield;
                selectionfield.GetComponent<Renderer>().material.color = InvalidPlacement; //find a better way to change color. //event
                _selectedTower = tower;
            }
            else
            {
                _selectedTower = PoolManager.Instance.RequestTower();
            }

            
            
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
                TowerToRecycle(_selectedTower); //add to pooling system
            }
        }

        void TowerToRecycle(GameObject obj)
        {
            PoolManager.Instance.ObjectsReadyToRecycle(obj, false);
            obj.SetActive(false);
        }

    }
}

