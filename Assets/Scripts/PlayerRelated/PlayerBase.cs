using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using System;

    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        private void OnTriggerEnter(Collider other)
        {
            EventManager.RaiseEvent("onPlayerBaseReached",other.gameObject);
        }
    }
}


