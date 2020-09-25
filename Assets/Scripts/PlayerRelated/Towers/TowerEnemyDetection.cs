using System;
using UnityEngine;

public class TowerEnemyDetection : MonoBehaviour
{
   public static event Action<GameObject,GameObject,bool> onEnemyDetectionRadius;
    private void OnTriggerEnter(Collider other)
    {
        onEnemyDetectionRadius?.Invoke(other.gameObject, this.transform.root.gameObject,false);
    }
    private void OnTriggerExit(Collider other)
    {
        onEnemyDetectionRadius?.Invoke(other.gameObject, this.transform.root.gameObject, true);
    }
}
