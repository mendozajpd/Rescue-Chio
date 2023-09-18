using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : AIBehavior
{
    public List<Targets> TargetList = new List<Targets>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Add a switch statement that would return what kind component should the AI be finding
    // E.G. Target Player = PlayerManager
}


public enum Targets
{
    Player,
    Enemy,
}
