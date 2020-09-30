using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CurtisDH.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [Header("Whole UI Elements")]
        [SerializeField]
        GameObject _gatlingUpgrade;
        [SerializeField]
        GameObject _missileUpgrade;
        [SerializeField]
        GameObject _dismantleWeapon;
        [SerializeField]
        GameObject _levelComplete;
        private int selectedTowerID;

        [Header("Text Components")]

        [SerializeField]
        private Text _mainWarfund;
        [SerializeField]
        private Text _gatlingUpgradeCost;
        [SerializeField]
        private Text _missileUpgradeCost;
        [SerializeField]
        private Text _uiStatusChange;
        [SerializeField]
        private Text _currentWave;
        [SerializeField]
        private Text _playerHealth;

        [Header("UI Status Change")]
        [SerializeField]
        Image[] _images;
        [Header("Serialised Dictionary Workaround")]
        [SerializeField]
        private List<string> _keys = new List<string>();
        [SerializeField]
        private List<Sprite> _values = new List<Sprite>();
        [SerializeField]
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();


        bool statusGood, statusCaution, statusDangerous;


        void Awake()
        {
            Instance = this;
        }
        void OnEnable()
        {
            EventManager.Listen("onNotEnoughWarfunds", (Action<bool>)ToggleNotEnoughWarfundsUI);

            if (_keys.Count != _values.Count)
            {
                return;
            }
            else
            {
                for (int i = 0; i < _keys.Count; i++)
                {
                    _sprites.Add(_keys[i], _values[i]);
                }
            }
            UpdateWarFunds(GameManager.Instance.WarFund);
        }
        void OnDisable()
        {
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

        public void StatusSystem(int playerHealth)
        {
            string Status = "Good";
            if (playerHealth >= 0)
            {
                _playerHealth.text = "" + playerHealth;
            }
            else
            {
                _playerHealth.text = "0";
            }
                
            if (playerHealth > 60)
            {
                if (statusGood != true)
                {
                    statusCaution = false;
                    statusDangerous = false;
                    Status = "Good";
                    statusGood = true;
                    //_sprites["LivesNormal"];
                    _images[0].sprite = _sprites["LivesNormal"];
                    _images[1].sprite = _sprites["PlayBackNormal"];
                    _images[2].sprite = _sprites["RestartNormal"];
                    _images[3].sprite = _sprites["WarfundsNormal"];
                    _images[4].sprite = _sprites["ArmoryNormal"];
                }

            }
            else if (playerHealth > 20 && playerHealth < 60)
            {
                if (statusCaution != true)
                {
                    statusGood = false;
                    statusDangerous = false;
                    Status = "Caution";
                    statusCaution = true;
                    _images[0].sprite = _sprites["LivesCaution"];
                    _images[1].sprite = _sprites["PlayBackCaution"];
                    _images[2].sprite = _sprites["RestartCaution"];
                    _images[3].sprite = _sprites["WarfundsCaution"];
                    _images[4].sprite = _sprites["ArmoryCaution"];
                }
            }
            else if (playerHealth < 20)
            {
                if (statusDangerous != true)
                {
                    statusCaution = false;
                    statusGood = false;
                    Status = "Danger";
                    _images[0].sprite = _sprites["LivesWarning"];
                    _images[1].sprite = _sprites["PlayBackWarning"];
                    _images[2].sprite = _sprites["RestartWarning"];
                    _images[3].sprite = _sprites["WarfundsWarning"];
                    _images[4].sprite = _sprites["ArmoryWarning"];
                }
            }
            _uiStatusChange.text = Status;
        }

        public void UpdateWave(int currentWave)
        {
            //set to 11 not 10 so we play last wave.
            if (currentWave == 11)
            {
                _levelComplete.SetActive(true);
                //restart/loop here
            }
            _currentWave.text = currentWave + "/10";

        }

        public void PauseButton()
        {
            Time.timeScale = 0;
        }
        public void PlayButton()
        {
            Time.timeScale = 1;
        }
        public void FastForwardButton()
        {
            Time.timeScale = 2;
        }
        public void Restartbutton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
