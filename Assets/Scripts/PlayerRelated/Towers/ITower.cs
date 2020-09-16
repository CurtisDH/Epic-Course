using UnityEngine;
public interface ITower 
{
    [SerializeField]
    int WarFund { get; set; }
    [SerializeField]
    int TowerID { get; set; }
    [SerializeField]
    float TowerRadius { get; set; }
    void TargetEnemy();
}
