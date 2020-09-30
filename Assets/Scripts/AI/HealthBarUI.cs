using CurtisDH.Scripts.Enemies;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    float _maxHP;
    [SerializeField]
    float _currentHP;
    [Header("Required")]
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    AIBase aiBase;
    [SerializeField]
    UnityEngine.UI.Image _healthBar;






    void OnEnable()
    {
        AssignVariables();
        EventManager.Listen("onDamageEnemy", (Action<GameObject, float, bool>)ReceiveDamage);
    }
    void OnDisable()
    {
        EventManager.UnsubscribeEvent("onDamageEnemy", (Action<GameObject, float, bool>)ReceiveDamage);
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
    void AssignVariables()
    {
        _maxHP = aiBase.Health;
        _currentHP = _maxHP;
        _healthBar.fillAmount = _currentHP / _maxHP;
    }
    public void ReceiveDamage(GameObject enemy, float damage, bool towerDeath)
    {
        if (enemy == this.enemy)
        {
            _currentHP -= damage;
            _healthBar.fillAmount = _currentHP / _maxHP;
        }

    }
}
