using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FSM
{
	private Stack<Action> stack;

	public FSM()
	{
		stack = new Stack<Action>();
	}

	public void Execute()
	{
		Action currentStateFunction = GetCurrentState();

		if (currentStateFunction != null)
		{
			currentStateFunction();
		}
	}

	public Action PopState()
	{
		return stack.Pop();
	}

	public void PushState(Action state)
	{
		if (GetCurrentState() != state)
		{
			stack.Push(state);
		}
	}

	public Action GetCurrentState()
	{
		if (stack.Count > 0)
			return stack.Peek();
		return null;
	}

    public String ParseCurrentState()
    {
        string state = stack.Peek().Method.Name;
        return state;
    }
}
