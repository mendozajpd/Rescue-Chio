using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Controller UnitController;
    public List<AIBehavior> Behaviors = new List<AIBehavior>();

    private void Awake()
    {
        UnitController = GetComponentInParent<Controller>();
    }

    void Start()
    {
        UpdateAIBehaviorList();
    }

    void Update()
    {
        
    }

    public void UpdateAIBehaviorList()
    {
        Behaviors.Clear();
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            AIBehavior behavior = gameObject.transform.GetChild(i).GetComponent<AIBehavior>();
            Behaviors.Add(behavior);
        }
    }
}
