using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatMode : State
{
    protected StateMachine _stateMachine;

    protected CombatMode(StateMachine machine) => _stateMachine = machine;
}

public class AttackState : CombatMode
{
    public AttackState(StateMachine machine) : base(machine) { }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine.CurrentState = this.ToString();
        Debug.Log("In Attack mode");
    }
}

public class ChaseState : CombatMode
{
    public ChaseState(StateMachine machine) : base(machine) { }
    public override void Enter()
    {
        base.Enter();
        //_stateMachine.CurrentState = this.ToString();
        Debug.Log("In Chase mode");
    }

}

public class PassiveState : CombatMode
{
    public PassiveState(StateMachine machine) : base(machine) { }

    public override void Enter()
    {
        base.Enter();
        //_stateMachine.CurrentState = this.ToString();
        Debug.Log("In Passive mode");
    }
}
