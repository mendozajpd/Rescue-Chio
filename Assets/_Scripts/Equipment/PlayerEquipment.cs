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

    private void OnEnable()
    {
        playerStats.CalculateStats += _calculateStats;
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

    private void _calculateStats()
    {
        playerStats.CalculateTotalWeaponBaseDamage(_getWeaponBaseDamage());
    }

    private float _getWeaponBaseDamage()
    {
        _totalBaseDamage = 0;

        foreach(Weapon weapon in Weapons)
        {
            _totalBaseDamage += weapon.BaseWeaponDamage;
        }
        return _totalBaseDamage;
    }

    IEnumerator getNumberOfChildren(float numOfSecondsUntilUpdate)
    {
        yield return new WaitForSeconds(numOfSecondsUntilUpdate);
        NumberOfChildren = transform.childCount;

        StartCoroutine(getNumberOfChildren(numOfSecondsUntilUpdate));
    }
}
