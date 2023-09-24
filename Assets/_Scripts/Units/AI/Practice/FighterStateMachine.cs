using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateMachine : StateMachine
{
    [SerializeField] private UnitManager target;

    public State CurrentState 
    {
        get => _currentState;
    }

    private void Awake()
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
