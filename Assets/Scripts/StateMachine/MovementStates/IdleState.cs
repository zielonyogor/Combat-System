using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : State
{

    public IdleState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter() 
	{ 
		base.Enter();
		player.canMidAirJump = false;
	}

	public override void HandleInput() 
	{
		if(player.inputManager.moveAction.triggered)
		{
			stateMachine.Change(player.states.WalkState);
			return;
		}
		bool hasAttacked = player.inputManager.attackAction.triggered;
		if (hasAttacked)
		{
			stateMachine.Change(player.states.FirstGroundAttackState);
		}
		if (player.inputManager.jumpAction.triggered)
		{
			player.canMidAirJump = true;
			stateMachine.Change(player.states.JumpState);
			return;
		}
	}

	public override void UpdatePhysics() 
	{
		if(!player.groundedPlayer)
		{
			stateMachine.Change(player.states.MidAirState);
			player.playerVelocity.y = player.playerVelocity.y + player.Gravity * player.fallGravityScale;
		}
		else
		{
			player.playerVelocity.y = -1f;
		}
		player.characterController.Move(player.playerVelocity * Time.deltaTime);
	}
}
