using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
	[SerializeField] protected State _currentState;

	private void SetState(State newState)
	{
		_currentState?.Exit();
		_currentState = newState;
		_currentState.Enter();
	}

	private void Update() => _currentState?.Update();

	protected void Init(State initialState, Dictionary<State, Dictionary<Transition, State>> states)
	{
		foreach (var state in states)
		{
			foreach (var transition in state.Value)
			{
				transition.Key.Callback = () => SetState(transition.Value);
				state.Key.AddTransition(transition.Key);
			}
		}

		SetState(initialState);
	}
}
