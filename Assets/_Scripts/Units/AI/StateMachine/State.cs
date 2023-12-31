using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
	protected StateMachine _stateMachine;

	protected State(StateMachine machine) => _stateMachine = machine;
	private List<Transition> _transitions = new();

	public void AddTransition(Transition transition)
	{
		_transitions.Add(transition);
	}

	public virtual void Enter()
	{
		_stateMachine.SetCurrentState(this.ToString());
		for (int i = 0; i < _transitions.Count; i++)
		{
			_transitions[i].Enter();
		}
	}

	public virtual void Exit()
	{
		for (int i = 0; i < _transitions.Count; i++)
		{
			_transitions[i].Exit();
		}
	}

	public virtual void Update()
	{
		for (int i = 0; i < _transitions.Count; i++)
		{
			_transitions[i].Update();
		}
	}

}