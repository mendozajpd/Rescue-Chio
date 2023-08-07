using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitManager : MonoBehaviour
{
    // Health
    public Health UnitHealth;

    // Mana
    public Mana UnitMana;
    // UNIT WILL GET A MANA SCRIPT, BUT MANA WILL ONLY APPEAR IF THE MANA IS MORE THAN 0

    // Status Effects
    public StatusEffectsManager UnitStatusEffects;

    // Stats
    public StatsManager UnitStats;

    public DefaultStatsSO UnitDefaultStats;



    public void GetRequiredComponents()
    {
        UnitHealth = GetComponent<Health>();
        UnitStatusEffects = GetComponent<StatusEffectsManager>();
        UnitStats = GetComponent<StatsManager>();
        UnitMana = GetComponent<Mana>();
    }

    public void SetDefaultStats(string location)
    {
        UnitDefaultStats = Resources.Load<DefaultStatsSO>(location);

        if (UnitDefaultStats != null)
        {
            UnitStats.SetDefaultStats(UnitDefaultStats);
            UnitHealth.SetMaxValue(UnitDefaultStats);

            if(UnitDefaultStats.DebugOn) Debug.Log(gameObject.name + ": " + UnitDefaultStats.name + " : Default Stats Set!");
        } else
        {
            Debug.Log("Default Stats for " + gameObject.name + " not found!");
        }
    }
}
