using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateMachine
{
    private AIManager _aiManager;
    private CircleCollider2D circleCollider;

    [SerializeField] private GameObject target;
    [SerializeField] private bool debugMode;
    private GameObject _closestTarget;
    public List<GameObject> targetList = new List<GameObject>();
    private List<float> targetDistanceList = new List<float>();

    public System.Action UpdateTargetList;
    private void Awake()
    {
        _aiManager = GetComponent<AIManager>();
        _aiManager.AIChanged += SetAIType;
        circleCollider = GetComponent<CircleCollider2D>();
        SetAIType();
        UpdateTargetList += GetClosestTarget;
    }

    private void SetAIType()
    {
        AI aiType = _aiManager.AIType;
        switch (aiType)
        {
            case AI.None:
                if (debugMode) Debug.Log(gameObject.transform.parent.name + " has no AI.");
                return;
            case AI.Fighter:
                return;
            default:
                return;
        }
    }

    // foreach target make a switch statement that

    // Find Target
    // Look around to find a target
    // Filter it out by its Aggro stat and distance to the enemy

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Adds to the target list
        foreach (Targets t in _aiManager.HostileTowards)
        {
            switch (t)
            {
                case Targets.Players:
                    PlayerManager player = collision.GetComponent<PlayerManager>();
                    if (player != null)
                    {
                        if (!targetList.Contains(collision.gameObject))
                        {
                            targetList.Add(collision.gameObject);
                            if (debugMode) Debug.Log("Added: " + collision.gameObject.name);
                            //UpdateTargetList.Invoke();
                        }
                        else
                        {
                            if (debugMode) Debug.Log(collision.gameObject.name + " is already in the list.");
                        }
                    }
                    else
                    {
                        if (debugMode) Debug.Log("Not a player target.");
                    }
                    break;
                case Targets.Enemies:
                    EnemyManager enemy = collision.GetComponent<EnemyManager>();
                    if (enemy != null)
                    {
                        if (!targetList.Contains(collision.gameObject))
                        {
                            targetList.Add(collision.gameObject);
                            if(debugMode) Debug.Log("Added: " + collision.gameObject.name);
                            //UpdateTargetList.Invoke();
                        }
                        else
                        {
                            if (debugMode) Debug.Log(collision.gameObject.name + " is already in the list.");
                        }
                    }
                    else
                    {
                        if (debugMode) Debug.Log("Not an enemy target.");
                    }
                    break;
                default:
                    break;
            }
        }



        UpdateTargetList.Invoke();
        SetFighterAI();

    }

    private void GetClosestTarget()
    {
        float closestDistance = circleCollider.radius; // radius
        _closestTarget = null;
        foreach (GameObject go in targetList)
        {
            // check distance
            float distance = Vector2.Distance(this.transform.position, go.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestTarget = go;
            }
        }
        target = null;
        target = _closestTarget;
    }

    private void FindTarget(GameObject gobject)
    {

        //  Find a means to get the target
        //  Find a means to identify the target
        //  Find a means to filter the target


    }


    private void SetFighterAI()
    {
        State attackState = new AttackState(this);
        State chaseState = new ChaseState(this);
        State passiveState = new PassiveState(this);

        Transition targetIsFar = new TargetIsFar(transform, target);
        Transition targetIsNear = new TargetIsNear(transform, target);
        Transition targetIsInRange = new TargetIsInRange(transform, target);

        Init(initialState: passiveState, states: new()
        {
            { passiveState, new() { { targetIsNear, chaseState } } },
            { chaseState, new() { { targetIsInRange, attackState }, { targetIsFar, passiveState } } },
            { attackState, new() { { targetIsNear, chaseState } } },
        });
    }
}

public enum AI
{
    None,
    Fighter,
}
