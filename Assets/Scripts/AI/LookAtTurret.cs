using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTurret : MonoBehaviour
{
    [SerializeField]
    private GameObject _rotation;
    [SerializeField]
    private GameObject _turretToLookAt;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private GameObject mech;

    private void OnEnable()
    {
        EventManager.Listen("onEnemyDetectionRadius", (Action<GameObject, GameObject, bool>)TargetTurret);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onEnemyDetectionRadius", (Action<GameObject, GameObject, bool>)TargetTurret);
    }
    void Update()
    {
        if(_turretToLookAt != null)
        {
            Vector3 direction = (_turretToLookAt.transform.position-_rotation.transform.position);
            _rotation.transform.rotation = Quaternion.LookRotation(direction);
        }

    }

    public void TargetTurret(GameObject mech, GameObject turret, bool b)
    {
        if (mech == this.mech)
        {
            _turretToLookAt = turret;
            _anim.SetTrigger("Shoot");
        }
    }


}
