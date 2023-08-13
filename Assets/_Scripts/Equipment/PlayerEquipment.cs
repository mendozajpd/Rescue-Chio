using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public PlayerManager player;
    public StatsManager playerStats;

    public List<Weapon> Weapons = new List<Weapon>();
    private int _numberOfChildren;

    private float _totalBaseDamage;
    private float _totalWeaponKB;
    private float _totalCurrentWeaponDamage;
    public int NumberOfChildren 
    { 
        get => _numberOfChildren; 
        set
        {
            if (value == _numberOfChildren) return;
            _numberOfChildren = value;
            _listWeaponsInEquipment();
        }
    }

    public float TotalBaseDamage 
    { 
        get => _totalBaseDamage;
        set 
        { 
            _totalBaseDamage = value;
            _calculateCurrentStats();
        } 
    }
    public float TotalWeaponKB 
    { 
        get => _totalWeaponKB;
        set 
        { 
            _totalWeaponKB = value;
            _calculateCurrentStats();
        }
    }

    public float TotalCurrentWeaponDamage 
    { 
        get => _totalCurrentWeaponDamage;
        set 
        { 
            _totalCurrentWeaponDamage = value; 
        }
    }

    private void OnEnable()
    {
        playerStats.CalculateStats += _calculateTotalStats;
    }

    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
        playerStats = player.UnitStats;
    }
    void Start()
    {
        StartCoroutine(getNumberOfChildren(1));
    }

    void Update()
    {
        
    }

    private void _listWeaponsInEquipment()
    {
        // this code will create a list of the weapons listed in equipment
        Weapons.Clear();
        for(int i = 0; i < NumberOfChildren; i++)
        {
            var weapon = transform.GetChild(i).GetComponent<Weapon>();
            if (weapon != null) Weapons.Add(weapon);
        }
    }

    private void _calculateTotalStats()
    {
        playerStats.CalculateTotalWeaponBaseDamage(_getTotalWeaponBaseDamage());
    }

    private void _calculateCurrentStats()
    {
        //playerStats.CalculateCurrentWeaponDamage(TotalCurrentWeaponDamage);
        playerStats.CalculateTrueKnockback(TotalWeaponKB);
    }

    //public void SetCurrentWeaponBaseDamage(float baseDamage)
    //{

    //    TotalCurrentWeaponDamage = baseDamage;
    //}

    private float _getTotalWeaponBaseDamage()
    {
        TotalBaseDamage = 0;

        foreach(Weapon weapon in Weapons)
        {
            TotalBaseDamage += weapon.BaseWeaponDamage;
        }
        return TotalBaseDamage;
    }

    public void GetWeaponKnocback(float weaponKB)
    {
        TotalWeaponKB = weaponKB;
    }

    IEnumerator getNumberOfChildren(float numOfSecondsUntilUpdate)
    {
        yield return new WaitForSeconds(numOfSecondsUntilUpdate);
        NumberOfChildren = transform.childCount;

        StartCoroutine(getNumberOfChildren(numOfSecondsUntilUpdate));
    }
}
