using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateMachine
{
    private AIManager _aiManager;

    [SerializeField] private GameObject target;
    //private GameObject _temporaryTarget;
    private List<GameObject> targetList = new List<GameObject>();
    private List<float> targetDistanceList = new List<float>();

    private void Awake()
    {
        _aiManager = GetComponent<AIManager>();
        _aiManager.AIChanged += SetAIType;
        SetAIType();

    }

    private void SetAIType()
    {
        AI aiType = _aiManager.AIType;
        switch (aiType)
        {
            case AI.None:
                Debug.Log(gameObject.transform.parent.name + " has no AI.");
                return;
            case AI.Fighter:
                SetFighterAI();
                return;
            default:
                return;
        }
    }

    // foreach target make a switch statement that

    // Find Target
    // Look around to find a target
    // Filter it out by its Aggro stat and distance to the enemy

    private void FindTarget(GameObject gobject)
    {
        if (target == null)
        {
            GameObject tempTarget = gobject;
            float shortestDistance = GetComponent<CircleCollider2D>().radius;
            foreach (Targets t in _aiManager.HostileTowards)
            {
                switch (t)
                {
                    case Targets.Players:
                        PlayerController player = gobject.GetComponent<PlayerController>();
                        if(player!=null)
                        {
                            // check for aggro (temporarily nothing)
                            // check distance
                            float distanceBetweenTarget = Vector2.Distance(transform.position, player.transform.position);
                            if (distanceBetweenTarget < shortestDistance) shortestDistance = distanceBetweenTarget;
                        }
                        break;
                    case Targets.Enemies:
                        EnemyController enemy = gobject.GetComponent<EnemyController>();
                        break;
                    default:
                        break;
                }

                // Check its aggro
                // Check its distance

            }
                
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindTarget();

        // Find closest collision
        // Filter it out
        // 
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
