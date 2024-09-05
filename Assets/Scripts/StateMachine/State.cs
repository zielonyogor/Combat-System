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

	/// <summary>
	/// Handles input accepted by current state and, eventually, changes current state.
	/// </summary>
	public virtual void HandleInput() {	}
	
	/// <summary>
	/// Updates physics and other visible variables.
	/// </summary>
	public virtual void UpdatePhysics() { }

	public virtual void Exit() { }
}
