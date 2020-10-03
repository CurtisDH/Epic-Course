using System;
using System.Collections;
using UnityEngine;

namespace CurtisDH.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _warFund;
        [SerializeField]
        private int _playerHealth = 100;
        public int WarFund
        {
            get
            {
                return _warFund;
            }
        }
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("GameManager::GameManager not found. Attempting to Create");
                    try
                    {
                        _ = new GameObject("GameManager").AddComponent<GameManager>();
                    }
                    catch (ArgumentNullException e)
                    // I think a try,catch is unnecessary so may change it after some research later
                    {
                        Debug.LogError("GameManager::Failed to create GameManager" + e);
                        return (_instance);
                    }

                    Debug.Log("GameManager::GameManager Created Successfully");
                }
                return _instance;
            }
        }
        bool incrementRunning;
        private void Awake()
        {
            _instance = this;
        }
        public void AdjustWarfund(int amount)
        {
            //if(incrementRunning == false)
            //{
            //    StartCoroutine(IncrementWarfund(amount));
            //}
            _warFund += amount;
            UIManager.Instance.UpdateWarFunds(_warFund);
        }
        public void AdjustPlayerHealth(int amount)
        {
            _playerHealth += amount;
            UIManager.Instance.StatusSystem(_playerHealth);
            if (_playerHealth <= 0)
            {
                Invoke("RestartRequest", 3);

            }
        }
        //IEnumerator IncrementWarfund(int amount)
        //{
        //    incrementRunning = true;
        //    int i = 0;
        //    int increment = _warFund - amount;
        //    while(true)
        //    {
        //        i++;
        //        yield return new WaitForSeconds(0.05f);
        //        UIManager.Instance.UpdateWarFunds(increment + i);
        //        if (i == amount)
        //        {
        //            incrementRunning = false;
        //            break;
        //        }
        //    }
            
        //}
        void RestartRequest()
        {
            UIManager.Instance.Restartbutton();
        }

        void Update() // temp to speed up testing
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale++;
            }
        }
    }
}

