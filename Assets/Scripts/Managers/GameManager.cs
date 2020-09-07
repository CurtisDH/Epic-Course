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
                    Debug.LogError("GameManager not found. Attempting to Create");
                    try
                    {
                        _ = new GameObject("GameManager").AddComponent<GameManager>();
                    }
                    catch (ArgumentNullException e)
                    // I think a try,catch is unnecessary so may change it after some research later
                    {
                        Debug.LogError("Failed to create GameManager" + e);
                        return (_instance);
                    }

                    Debug.Log("GameManager Created Successfully");
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }


        public void AdjustWarfund(int amount, bool negative = false)
        {
            if (_warFund <= 0)
            {
                Debug.LogWarning("Not implemented");
            }


            if (negative == true)
            {
                _warFund -= amount;
            }
            else
            {
                _warFund += amount;
            }
        }

    }
}

