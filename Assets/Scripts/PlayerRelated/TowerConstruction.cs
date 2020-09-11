using CurtisDH.Utilites;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TowerConstruction : MonoBehaviour
{
    bool _canPlaceTower;
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);

        }
    }
}
