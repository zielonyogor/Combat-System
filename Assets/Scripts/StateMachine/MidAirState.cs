using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAirState : State
{
	private Vector2 input;
	public MidAirState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
	}

	public override void HandleInput()
	{
		input = player.inputManager.moveAction.ReadValue<Vector2>();
		bool hasJumped = player.inputManager.jumpAction.triggered && player.canMidAirJump;
		if (hasJumped)
		{
			player.canMidAirJump = false;
			stateMachine.Change(player.states.JumpState);
			return;
		}
		if(player.inputManager.plungeAttackAction.triggered)
		{
			stateMachine.Change(player.states.PlungeAttackState);
			return;
		}
	}

	public override void UpdatePhysics()
	{
		float velocityY;

		if (player.groundedPlayer)
		{
			velocityY = -1f;
			if(input != Vector2.zero)
			{
				stateMachine.Change(player.states.WalkState);
			}
			else
			{
				stateMachine.Change(player.states.IdleState);
			}
		}
		else
		{
			velocityY = player.playerVelocity.y + player.Gravity * player.fallGravityScale;
		}
		player.playerVelocity = new Vector3(input.x * player.moveSpeed, velocityY, input.y * player.moveSpeed);

		player.characterController.Move(player.playerVelocity * Time.deltaTime);
	}
}