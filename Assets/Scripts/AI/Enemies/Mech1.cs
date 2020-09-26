using UnityEngine;
using CurtisDH.Scripts.Enemies;
public class Mech1 : AIBase
{
    public override void MoveTo(Vector3 position)
    {
        base.MoveTo(position);
    }
    public override void onDeath(GameObject obj,bool endZoneDeath)
    {
        base.onDeath(obj, endZoneDeath);
    }
}
