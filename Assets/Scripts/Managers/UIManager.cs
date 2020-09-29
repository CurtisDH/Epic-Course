using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

namespace CurtisDH.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField]
        GameObject _gatlingUpgrade;
        [SerializeField]
        GameObject _missileUpgrade;
        [SerializeField]
        GameObject _dismantleWeapon;
        private int selectedTowerID;


        [SerializeField]
        private Text _mainWarfund;
        [SerializeField]
        private Text _gatlingUpgradeCost;
        [SerializeField]
        private Text _missileUpgradeCost;
        void Awake()
        {
            Instance = this;
        }
        void OnEnable()
        {
            EventManager.Listen("onUpdateWarFunds", (Action<int>)UpdateWarFunds);
            EventManager.Listen("onNotEnoughWarfunds",(Action<bool>)ToggleNotEnoughWarfundsUI);
        }
        void OnDisable()
        {
            EventManager.UnsubscribeEvent("onUpdateWarFunds", (Action<int>)UpdateWarFunds);
            EventManager.UnsubscribeEvent("onNotEnoughWarfunds", (Action<bool>)ToggleNotEnoughWarfundsUI);
        }
        public void ToggleUpgradeUI(int towerID, int UpgradeCost = 0, bool toggleUI = true)
        {
            selectedTowerID = towerID;
            ToggleSellUI(toggleUI);
            switch (towerID)
            {
                case 0:
                    _gatlingUpgrade.SetActive(toggleUI);
                    if (UpgradeCost == 0) break;
                    _gatlingUpgradeCost.text = "" + UpgradeCost;

                    break;
                case 1:
                    _missileUpgrade.SetActive(toggleUI);
                    if (UpgradeCost == 0) break;
                    _missileUpgradeCost.text = "" + UpgradeCost;
                    break;
                default:
                    Debug.LogError("UIManager::TowerID " + towerID + " Does NOT exist");
                    break;
            }
        }
        void ToggleSellUI(bool toggleUI)
        {
            _dismantleWeapon.SetActive(toggleUI);
        }
        public void ToggleNotEnoughWarfundsUI(bool toggleUI)
        {
            //noWarfunds.SetActive(true);
        }
        public void UpdateWarFunds(int funds)
        {
            _mainWarfund.text = "" + funds;
        }

        public void CancelTowerUpgrade()
        {
            EventManager.RaiseEvent("onTowerCancel");
            ToggleUpgradeUI(selectedTowerID, toggleUI: false);
        }
        public void UpgradeTower()
        {
            EventManager.RaiseEvent("onTowerUpgrade");
            CancelTowerUpgrade();
        }
        // if we hit sell button then raise event
        public void SellTower()
        {
            EventManager.RaiseEvent("onTowerSell");
            CancelTowerUpgrade();

        }
    }

}
