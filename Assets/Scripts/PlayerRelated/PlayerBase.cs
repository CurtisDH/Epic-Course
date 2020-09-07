using UnityEngine;

namespace CurtisDH.Scripts.PlayerRelated
{
    using CurtisDH.Scripts.Enemies;
    public class PlayerBase : MonoBehaviour //  Should probably rename this because of the similarity with AIBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AIBase>())
            {
                other.GetComponent<AIBase>().onDeath();
            }
        }
    }
}


