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
        private void OnEnable()
        {
            EventManager.Listen("onMouseDown", (Action<Vector3>)PlaceTower);
            EventManager.Listen("onMouseEnter", (Action<Vector3,bool>)SnapTower);
            EventManager.Listen("onMouseExit", (Action<Vector3,bool>)SnapTower);
            _instance = this;
        }
        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onMouseDown", (Action<Vector3>)PlaceTower);
            EventManager.UnsubscribeEvent("onMouseEnter", (Action<Vector3, bool>)SnapTower);
            EventManager.UnsubscribeEvent("onMouseExit", (Action<Vector3, bool>)SnapTower);
        }

        [SerializeField]
        GameObject _turretShader;
        public GameObject TurretShader
        {
            get
            {
                return _turretShader;
            }
        } // need to rework the entire turret shader radius :: Not modular enough
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
                _selectedTower.transform.parent = null;
                _turretShader.GetComponent<MeshRenderer>().enabled = false;
                _turretShader.AddComponent<SphereCollider>().isTrigger = true;
                _turretShader.AddComponent<Rigidbody>().isKinematic = true;
                _turretShader = null;
                _selectedTower.GetComponent<Collider>().enabled = true;

                //Need to remove get component here if possible..
                GameManager.Instance.AdjustWarfund(-_selectedTower.GetComponent<Tower>().WarFund);

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
                    _turretShader.GetComponent<Renderer>().material.color = _validPlacement;
                }
                else
                {
                    _turretShader.GetComponent<Renderer>().material.color = _invalidPlacement;
                }
                _snappedTower = isSnapped;
                _selectedTower.transform.position = pos;
            }
        }
        public void GatlingTurret() // need to rework all of this just getting a prototype functional
        {//need a better way to access the towers warfund..
            if (GameManager.Instance.WarFund >= Towers[0].GetComponent<Tower>().WarFund) //need to link this to the turrets warfund
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
            if (GameManager.Instance.WarFund >= Towers?[1].GetComponent<Tower>().WarFund)
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
            EventManager.RaiseEvent("onIsPlacingTower", _isPlacingTower);
            //onIsPlacingTower?.Invoke(_isPlacingTower);
            _selectedTower = PoolManager.Instance.RequestTower(id);
            if(_turretShader == null)
            {
                //need to move all this to poolmanager somehow.. or perhaps turn into an event system.
                float TowerRadius = _selectedTower.GetComponent<Tower>().TowerRadius;
                var selectionfield = Instantiate(_towerShaderPrefab);
                selectionfield.transform.parent = _selectedTower.transform;
                selectionfield.transform.position = _selectedTower.transform.position;
                selectionfield.transform.localScale = new Vector3(TowerRadius, TowerRadius, TowerRadius);
                _turretShader = selectionfield;
                selectionfield.name = "TowerRadius";
                selectionfield.GetComponent<Renderer>().material.color = InvalidPlacement; //find a better way to change color. //event?
            }
            
        }

        public void CancelTowerCreation()
        {
            _isPlacingTower = false;
            //Destroy(_turretShader);
            EventManager.RaiseEvent("onIsPlacingTower", _isPlacingTower);
        }

        void RightClickCancel()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(_turretShader); // need to pool
                CancelTowerCreation();
                TowerToRecycle(_selectedTower); //add to pooling system -- currently creating a new one upon request.
            }
        }

        void TowerToRecycle(GameObject obj)
        {
            PoolManager.Instance.ObjectsReadyToRecycle(obj,false,obj.GetComponent<Tower>().TowerID);
            obj.SetActive(false);
        }

    }
}

