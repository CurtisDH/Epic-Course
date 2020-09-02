using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //GameManager.Instance.Warfund-=other.GetComponent<AIBase>().WarFund;
        other.GetComponent<AIBase>().onDeath(); // or move the above statement into the death script of the AI?
    }


}
