using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using System;

    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        private void OnTriggerEnter(Collider other)
        {
            //what gameObject reached the end?? Did it die to the endzone?
            EventManager.RaiseEvent("onPlayerBaseReached",other.gameObject,true);
        }
    }
}


