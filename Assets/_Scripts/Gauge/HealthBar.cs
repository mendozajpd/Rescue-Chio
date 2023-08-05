using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health _health;
    [SerializeField] private Image _healthBar;
    // COLOR
    // enemy - red
    // player - undecided
    // ally - green / blue

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
        _updateHealthbar();
        //_health.hasDied += deathMessage;
    }

    private void OnEnable()
    {
        _health.valueChanged += _updateHealthbar;
    }

    private void OnDisable()
    {
        _health.valueChanged -= _updateHealthbar;
        //_health.hasDied -= deathMessage;
    }

    private void _updateHealthbar()
    {
        _healthBar.fillAmount = _health.RatioValue;
    }

    public void AssignHealthGauge(Health healthScript)
    {
        _health = healthScript;
    }

    //private void deathMessage()
    //{
    //    Debug.Log(_health.gameObject + " has died!");
    //}
}
