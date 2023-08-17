using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    [SerializeField] private int currentNumberOfSpells;
    [SerializeField] private float updateInSeconds = 0.5f;

    public System.Action UpdateCurrentSpells;

    public MagicWeapon Wand;

    private void Awake()
    {
        Wand = GetComponentInParent<MagicWeapon>();
    }
    public int CurrentNumberOfSpells 
    { 
        get => currentNumberOfSpells; 
        set => currentNumberOfSpells = value; 
    }

    void Start()
    {
        StartCoroutine(_getNumberOfChildren(updateInSeconds));
    }

    void Update()
    {
        
    }

    IEnumerator _getNumberOfChildren(float secondsUntilUpdate)
    {
        if (CurrentNumberOfSpells != transform.childCount)
        {
            CurrentNumberOfSpells = transform.childCount;
            UpdateCurrentSpells.Invoke();
        }
        yield return new WaitForSeconds(secondsUntilUpdate);
        StartCoroutine(_getNumberOfChildren(updateInSeconds));
    }
}
