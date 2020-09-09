using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Wave",menuName ="Create Custom Wave",order =1)]
public class Wave : ScriptableObject
{
    public List<GameObject> Enemies;
    public int WaveID;
    public float TimeBetweenEnemySpawns;


}
