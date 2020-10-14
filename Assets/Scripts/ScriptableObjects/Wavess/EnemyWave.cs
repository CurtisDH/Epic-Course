using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyWave // need to change this up still
{
    public GameObject Prefab;
    public int Count;
    public int WaveID;
    public float TimeBetweenSpawns;
}

