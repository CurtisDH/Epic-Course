using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace CurtisDH.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static event Action onTowerUpgrade;
        public static event Action onTowerCancel;
        public static UIManager Instance { get; private set; }
        [SerializeField]
        GameObject _gatlingUpgrade;
        [SerializeField]
        GameObject _missileUpgrade;
        [SerializeField]
        GameObject _dismantleWeapon;
        private int selectedTowerID;
        void Awake()
        {
            Instance = this;
        }
        public void ToggleUpgradeUI(int towerID, bool toggleUI = true)
        {
            selectedTowerID = towerID;
            switch (towerID)
            {
                case 0:
                    _gatlingUpgrade.SetActive(toggleUI);

                    break;
                case 1:
                    _missileUpgrade.SetActive(toggleUI);

                    break;
                default:
                    Debug.LogError("UIManager::TowerID " + towerID + " Does NOT exist");
                    break;
            }
        }

        public void CancelTowerUpgrade()
        {
            onTowerCancel?.Invoke();
            ToggleUpgradeUI(selectedTowerID, false);
        }
        public void UpgradeTower()
        {
            onTowerUpgrade?.Invoke();
            CancelTowerUpgrade();
        }

    }

}
