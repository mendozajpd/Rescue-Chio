using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mana : Gauge, IManaConsumeable, IRestoreMana
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void ConsumeMana(float amt)
    {
        if (CurrentValue - amt < 0)
        {
            CurrentValue = 0;
            return;
        }
        CurrentValue -= amt;
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
}
