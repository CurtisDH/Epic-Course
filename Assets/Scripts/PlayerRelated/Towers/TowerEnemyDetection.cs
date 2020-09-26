using System;
using UnityEngine;

public class TowerEnemyDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.RaiseEvent("onEnemyDetectionRadius", other.gameObject, transform.root.gameObject, false);
    }
    private void OnTriggerExit(Collider other)
    {
        EventManager.RaiseEvent("onEnemyDetectionRadius", other.gameObject, transform.root.gameObject, true);
    }
}
