using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gauge : MonoBehaviour
{
    [SerializeField] private float maxValue;
    [SerializeField] private float currentValue;

    private void Start() => CurrentValue = MaxValue;


    public float CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = value;
            valueChanged?.Invoke();
        }
    }

    public float MaxValue => maxValue;
    public System.Action valueChanged;
    public float RatioValue => currentValue / maxValue;



}
