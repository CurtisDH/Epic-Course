using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemyDetection : MonoBehaviour
{
   public static Action<GameObject,GameObject> onEnemyDetectionRadius;
    private void OnTriggerEnter(Collider other)
    {
        onEnemyDetectionRadius?.Invoke(other.gameObject, this.transform.root.gameObject);
    }
}
