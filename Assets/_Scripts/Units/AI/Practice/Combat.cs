using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : State
{
    protected Combat(StateMachine machine) : base(machine) { }
}

public class AttackState : Combat
{
    public AttackState(StateMachine machine) : base(machine) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("In Attack mode");
    }
}

public class ChaseState : Combat
{
    public ChaseState(StateMachine machine) : base(machine) { }
    public override void Enter()
    {
        base.Enter();
        //Debug.Log("In Chase mode");
    }

}

public class PassiveState : Combat
{
    public PassiveState(StateMachine machine) : base(machine) { }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("In Passive mode");
    }
}
