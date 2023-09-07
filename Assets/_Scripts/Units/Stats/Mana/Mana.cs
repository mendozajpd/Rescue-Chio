using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mana : Gauge, IManaConsumeable, IRestoreMana
{
    private Controller _unitMovement;


    // Mana Regen Delay
    [SerializeField] private float _manaRegenDelayTime;
    [SerializeField] private float delayMultiplier = 240;
    [SerializeField] private float delayAdd = 45;

    private void Awake()
    {
        _unitMovement = GetComponent<Controller>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_manaRegenDelayTime > 0) _manaRegenDelayTime -= Time.deltaTime;
        if(_unitMovement != null && _manaRegenDelayTime <= 0) _manaRegenerationHandler(_unitMovement.IsMoving);
    }

    public void ConsumeMana(float amt)
    {
        float manaRegenDelayDuration = 0.7f * ((1 - currentValue / maxValue) * delayMultiplier + delayAdd);
        if (CurrentValue - amt < 0)
        {
            CurrentValue = 0;
            _manaRegenDelayTime = manaRegenDelayDuration;
            return;
        }
        CurrentValue -= amt;
        _manaRegenDelayTime = manaRegenDelayDuration;
    }

    public void RestoreMana(float amt)
    {
        if (CurrentValue + amt > MaxValue)
        {
            CurrentValue = MaxValue;
            return;
        }
        CurrentValue += amt;
    }

    private void _manaRegenerationHandler(bool isMoving)
    {
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
            return;
        }

        if (currentValue < maxValue)
        {
            float manaRegenRate = isMoving ? (maxValue / 7 + 1) * (currentValue / maxValue * 0.8f + 0.2f) * 1.15f : (maxValue / 7 + 1 + maxValue / 2) * (currentValue / maxValue * 0.8f + 0.2f) * 1.15f;
            currentValue += manaRegenRate * Time.deltaTime;
        }
    }
}
