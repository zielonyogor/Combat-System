using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class State
{
	protected StateMachine stateMachine;
	protected Player player;

	public State(Player player, StateMachine stateMachine)
	{
		this.player = player;
		this.stateMachine = stateMachine;
	}
	public virtual void Enter() 
	{ 
		Debug.Log("entering state: " + this.ToString());
	}

	public virtual void HandleInput() {	}
	
	public virtual void UpdatePhysics() { }

	public virtual void Exit() { }
}
