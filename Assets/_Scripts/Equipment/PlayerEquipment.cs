using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public PlayerManager player;
    public StatsManager playerStats;

    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
        playerStats = player.UnitStats;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
