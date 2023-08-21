using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BladeAttack : MeleeAttack
{
    private MeleeWeapon meleeWeapon;

    private void Awake()
    {
        meleeWeapon = GetComponentInParent<MeleeWeapon>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (meleeWeapon.Swinging)
        {
            StatsManager attackerStats = meleeWeapon.equipment.playerStats;
            Vector2 attackerPosition = attackerStats.transform.position;
            TriggerDamageKnocbackEnemy(collision, attackerStats, attackerPosition, 0, true);
        }

        if (meleeWeapon.Thrusting)
        {
            StatsManager attackerStats = meleeWeapon.equipment.playerStats;
            Vector2 attackerPosition = attackerStats.transform.position;
            TriggerDamageKnocbackEnemy(collision, attackerStats, attackerPosition, 0, true);
        }
    }
}
