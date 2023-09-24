using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition
{
	public System.Action Callback;

	public abstract bool CheckCondition();

	public virtual void Enter() { }
	public virtual void Exit() { }

	public void Update()
	{
		if (!CheckCondition()) return;

		if (Callback != null)
		{
			Callback.Invoke();
		}
		else
		{
			Enter();
		}
	}
}
