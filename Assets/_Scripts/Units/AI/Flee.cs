using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : AIBehavior
{
    public FleeType TypeOfFlee;

    void Start()
    {
        switch (TypeOfFlee)
        {
            case FleeType.Tactical:
                Debug.Log("This will flee after attacking.");
                return;
            case FleeType.Fearful:
                Debug.Log("This will flee when low health.");
                return;
            case FleeType.Cowardly:
                Debug.Log("This will flee after getting hit.");
                return;
            case FleeType.Pacifist:
                Debug.Log("This will flee from enemies no matter what.");
                return;
            default:
                Debug.Log("This does nothing");
                return;
        }
    }

    void Update()
    {
        
    }
}

public enum FleeType
{
    Tactical,
    Fearful,
    Cowardly,
    Pacifist,
}
