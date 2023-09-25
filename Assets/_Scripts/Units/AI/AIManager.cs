using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIStateMachine))]
public class AIManager : MonoBehaviour
{
    public Controller UnitController;
    public List<Targets> FriendlyTowards = new List<Targets>();
    public List<Targets> HostileTowards = new List<Targets>();

    [SerializeField] private AI _aiType;
    public System.Action AIChanged;
    public AI AIType
    {
        get => _aiType;
        set
        {
            _aiType = value;
            AIChanged.Invoke();
        }
    }



    private void Awake()
    {
        UnitController = GetComponentInParent<Controller>();
    }

    //public void UpdateAIBehaviorList()
    //{
    //    Behaviors.Clear();
    //    for(int i = 0; i < gameObject.transform.childCount; i++)
    //    {
    //        AIBehavior behavior = gameObject.transform.GetChild(i).GetComponent<AIBehavior>();
    //        Behaviors.Add(behavior);
    //    }
    //}
}

public enum Targets
{
    Players,
    Enemies
}