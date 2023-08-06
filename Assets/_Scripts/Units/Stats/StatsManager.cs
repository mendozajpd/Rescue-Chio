using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private float _totalAggro;
    private float _totalAttackSpeed;
    private float _totalCriticalHit;
    //private float _damage;
    private float _totalDefense;
    private float _totalHealthRegen;
    private float _totalKnockback;
    private float _totalMovementSpeed;

    //[Header("Attack Speed Variables")]

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateStats()
    {

    }

    #region Aggro Calculator
    // The more aggro a player has the more likely it will be targeted by the enemy
    // Will be used for when enemy is targeting the player
    // Will used more when the game is multiplayer


    #endregion

    #region Attack Speed Calculator
    // totalAttackSpeed = defaultAttackspeed + totalBonusattackspeed
    // totalBonusattackspeed = defaultAttackSpeed * bonusAttackSpeed * 0.01


    #endregion

    #region Critical Hit Chance Calculator
    // must be added after defense is calculated
    // critical hit is always damage * 2
    // default critical hit chance = 4%



    #endregion

    #region Damage Calculator
    // must be calculated first
    // Damage will be randomized between the -+damageDifference%

    // get base damage
    // add weapon modifier (i will probably not implement this)
    // add damage bonuses to base damage
    // totalDamage = base damage + (base damage + totalBonusDamage)
    // totalBonusDamage = baseDamage * bonusDamage * 0.01
    // Round down 
  

    #endregion

    #region Defense Calculator
    // must be calculated 2nd after damage and before adding critical hit
    // player defense is different to enemy/object defense
    
    // for players
    // net dmg = (attack damage - def * factor) factor is dependent on difficulty, the higher the factor the more damage player takes

    // for enemies
    // net dmg = (attack dmg - def * 0.5)

    #endregion

    #region Health Regen Calculator
    // idk if I am going to implement health regen normally
    #endregion

    #region Knockback Calculator
    // calculate knockback after crit calculation, or LAST
    // calculate knockback resistance

    // knockback cap

    // add extra knockback if it is critical hit
    #endregion

    #region Movement Speed Calculator
    // totalMovespeed = defaultMovespeed + bonusMovespeed - penaltyMovespeed
    #endregion




}
