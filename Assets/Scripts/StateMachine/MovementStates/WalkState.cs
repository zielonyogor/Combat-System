using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
	Vector2 input;
	public WalkState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
		player.canMidAirJump = false;
	}

	public override void HandleInput()
	{
		input = player.inputManager.moveAction.ReadValue<Vector2>();
		bool hasJumped = player.inputManager.jumpAction.triggered;
		if (hasJumped)
		{
			player.canMidAirJump = true;
			stateMachine.Change(player.states.JumpState);
			return;
		}
		bool hasAttacked = player.inputManager.attackAction.triggered;
		if(hasAttacked)
		{
			stateMachine.Change(player.states.FirstGroundAttackState);
		}
		if (input == Vector2.zero)
		{
			stateMachine.Change(player.states.IdleState);
			return;
		}
	}

	public override void UpdatePhysics()
	{
		if (!player.groundedPlayer)
		{
			stateMachine.Change(player.states.MidAirState);
			player.playerVelocity.y = player.playerVelocity.y + player.Gravity * player.fallGravityScale;
		}
		else
		{
			player.playerVelocity.y = -1f;
		}
		player.playerVelocity = new Vector3(input.x * player.moveSpeed, player.playerVelocity.y, input.y * player.moveSpeed);
		player.characterController.Move(player.playerVelocity * Time.deltaTime);
	}

	public override void Exit()
	{
		player.playerVelocity.x = 0f;
		player.playerVelocity.z = 0f;
	}
}