using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doppelganger : EnemyManager
{

    private void Awake()
    {
        GetRequiredComponents();
        SetDefaultStats("Player/PlayerDefaultStats");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void CalculateExtraStats()
    {
        base.CalculateExtraStats();
    }
}
