using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : Controller
{
    public AIManager UnitAIManager;
    // AI CONTROLLER CAN BE ENEMY OR WHATEVER
    // SHOULD HAVE THINGS BOTH ENEMY AND ALLY AI HAS

    public void SetUnitAIManager(GameObject unit)
    {
        UnitAIManager = unit.GetComponentInChildren<AIManager>();
    }
}
