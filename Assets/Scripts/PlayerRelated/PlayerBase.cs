using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using CurtisDH.Scripts.Managers;
    using System;

    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        private void OnTriggerEnter(Collider other)
        {
            //what gameObject reached the end?? Did it die to the endzone?
            EventManager.RaiseEvent("onPlayerBaseReached",other.gameObject,true);

            GameManager.Instance.AdjustPlayerHealth(-10);
        }
    }
}


