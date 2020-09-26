using System;
using UnityEngine;
using CurtisDH.Scripts.Managers;
namespace CurtisDH.Scripts.PlayerRelated.Tower
{
    public class TowerLocation : MonoBehaviour //setup event system to play particles
    {
        [SerializeField]
        GameObject TurretOccupying;
        [SerializeField]
        bool _isOccupied;
        [SerializeField]
        GameObject AvailableSpotPrefab;
        [SerializeField]
        GameObject _activeAvailableSpot;

        private void OnEnable()
        {
            EventManager.Listen("onIsPlacingTower", (Action<bool>)ToggleParticleSystem);
            EventManager.Listen("SoldTower", (Action<GameObject>)RemoveTower);
            EventManager.Listen("PlaceTower", (Action<GameObject>)AddTower);
        }
        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onIsPlacingTower", (Action<bool>)ToggleParticleSystem);
        }

        private void OnMouseEnter()
        {
            if (_isOccupied) return;
            EventManager.RaiseEvent("onMouseEnter", this.gameObject.transform.position, true);
            Debug.Log(this.gameObject + "::TowerLocation");
        }
        private void OnMouseDown()
        {
            if (_isOccupied) return;
            EventManager.RaiseEvent("onMouseDown", this.gameObject.transform.position);
            _isOccupied = true;
        }
        private void OnMouseExit()
        {
            EventManager.RaiseEvent("onMouseExit", Vector3.zero, false);
        }
        //event system check if isPlacingTower is happening & if isoccupied is false
        public bool IsOccupied
        {
            get
            {
                return _isOccupied;
            }
        }
        public void ToggleParticleSystem(bool toggle)
        {
            if (_isOccupied) return;
            if (_activeAvailableSpot == null)
            {
                var T = Instantiate(AvailableSpotPrefab);
                T.transform.parent = this.transform;
                T.transform.position = this.transform.position;
                _activeAvailableSpot = T;
            }
            _activeAvailableSpot.SetActive(toggle);
            //particlesystem.setactive = toggle;
        }
        public void PlaceTower(GameObject obj)
        {
            TurretOccupying = obj;
            TurretOccupying.transform.position = this.transform.position;
            _isOccupied = true;
        }
        public void AddTower(GameObject tower)
        {
            TurretOccupying = tower;
        }
        public void RemoveTower(GameObject tower)
        {
            if(tower.gameObject == TurretOccupying.gameObject)
            {
                TurretOccupying = null;
                _isOccupied = false;
            }

        }
    }

}
