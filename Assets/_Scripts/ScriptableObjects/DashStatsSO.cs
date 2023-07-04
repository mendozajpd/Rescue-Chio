using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashStats", menuName = "ScriptableObjects/DashStats")]
public class DashStatsSO : ScriptableObject
{
    public string DashName;
    public float DashCooldown;
    public float DashDuration;
    public float DashSpeed;
    public bool hasAnim;
    public float baseValue;
    public float targetValue;
    public float lerpSpeed;
}
