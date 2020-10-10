using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyWave // need to change this up still
{
    public enum WaveType
    {
        Mech1,
        Mech2
    }

    public WaveType Type;
    public GameObject Prefab;
    public int Count;
    public int WaveID;
    public float TimeBetweenSpawns;
}

