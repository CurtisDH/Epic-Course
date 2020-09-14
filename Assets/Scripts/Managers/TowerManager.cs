using System;
using UnityEngine;
using CurtisDH.Scripts.PlayerRelated.Tower;
namespace CurtisDH.Scripts.Managers
{
    public class TowerManager : MonoBehaviour // renameto tower manager?
    {
        private static TowerManager _instance;
        public static TowerManager Instance
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
        GameObject _turretShader;
        public GameObject TurretShader
        {
            get
            {
                return _turretShader;
            }
        }
        [SerializeField]
        Color _invalidPlacement = Color.red, _validPlacement = Color.green;
        public Color InvalidPlacement
        {
            get
            {
                return _invalidPlacement;
            }
        }
        public Color ValidPlacement
        {
            get
            {
                return _validPlacement;
            }
        }
        public GameObject[] Towers;
        [SerializeField]
        GameObject _selectedTower;
        public GameObject SelectedTower
        {
            get
            {
                return _selectedTower;
            }
        }
        [SerializeField]
        GameObject _towerShaderPrefab;
        public GameObject TowerShaderPrefab
        {
            get
            {
                return _towerShaderPrefab;
            }
        }
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
                Destroy(_towerShaderPrefab);
                //_towerShaderPrefab.SetActive(false); //setup to recycle
                CancelTowerCreation();
            }
        }

        public void SnapTower(Vector3 pos, bool isSnapped) //if this runs set a bool to say hey we've snapped dont update me until i say so 
        {
            if (_isPlacingTower)
            {
                if (isSnapped == true) //need to refine and remove getcomponent if possible
                {
                    _towerShaderPrefab.GetComponent<Renderer>().material.color = _validPlacement;
                }
                else
                {
                    _towerShaderPrefab.GetComponent<Renderer>().material.color = _invalidPlacement;
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
                CreateTower(0); //hard coded -- need to change
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
                _selectedTower = Towers?[1];
                CreateTower(1);//hard coded -- need to change
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
        public void CreateTower(int id) // pull out of the pooling system.
        {
            _isPlacingTower = true;
            onIsPlacingTower?.Invoke(_isPlacingTower);
            _selectedTower = PoolManager.Instance.RequestTower(id);

            //need to move all this to poolmanager somehow.. or perhaps turn into an event system.
            float TowerRadius = _selectedTower.GetComponent<TowerBase>().TowerRadius;
            var selectionfield = Instantiate(_turretShader);
            selectionfield.transform.parent = _selectedTower.transform;
            selectionfield.transform.position = _selectedTower.transform.position;
            selectionfield.transform.localScale = new Vector3(TowerRadius, TowerRadius, TowerRadius);
            _towerShaderPrefab = selectionfield;
            selectionfield.GetComponent<Renderer>().material.color = InvalidPlacement; //find a better way to change color. //event?
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
                TowerToRecycle(_selectedTower); //add to pooling system -- currently creating a new one upon request.
            }
        }

        void TowerToRecycle(GameObject obj)
        {
            PoolManager.Instance.ObjectsReadyToRecycle(obj, false);
            obj.SetActive(false);
        }

    }
}

