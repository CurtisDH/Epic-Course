using CurtisDH.Scripts.PlayerRelated;
using System;
using UnityEngine;

namespace CurtisDH.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _warFund;
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
        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            PlayerBase.onPlayerBaseReached += AdjustWarfund;
            PlayerBase.onPlayerBaseReached += AdjustPlayerLives;
        }
        public void AdjustWarfund(int amount)
        {
            if (_warFund <= 0)
            {
                Debug.LogWarning("GameManager::AdjustWarfund Not implemented");
            }

            _warFund += amount;

        }
        public void AdjustPlayerLives(int amount)
        {
            amount *= -1;
            amount /= 10;

            //subtract player lives here
            Debug.LogError("GameManager::AdjustPlayerLives not Implemented: " + amount);
        }
        private void OnDisable()
        {
            PlayerBase.onPlayerBaseReached -= AdjustWarfund;
            PlayerBase.onPlayerBaseReached -= AdjustPlayerLives;
        }
    }
}

