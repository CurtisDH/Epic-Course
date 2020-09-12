using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using System;

    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        public static Action<GameObject> onPlayerBaseReached;
        private void OnTriggerEnter(Collider other)
        {
            onPlayerBaseReached?.Invoke(other.gameObject);
        }
    }
}


