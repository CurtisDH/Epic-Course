using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using CurtisDH.Scripts.Enemies;
    using System;

    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        public static Action<int> onPlayerBaseReached;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AIBase>())
            {
                other.GetComponent<AIBase>().onDeath();
                onPlayerBaseReached?.Invoke(-other.GetComponent<AIBase>().WarFund); //temp way to make value negative.
            }
        }
    }
}


