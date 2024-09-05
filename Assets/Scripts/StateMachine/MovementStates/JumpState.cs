using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
	Vector2 input;
	public JumpState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
		player.playerVelocity.y = player.canMidAirJump ? player.jumpSpeed : player.midAirJumpSpeed;
	}

	public override void HandleInput()
	{
		input = player.inputManager.moveAction.ReadValue<Vector2>();
		if(player.inputManager.jumpAction.triggered)
		{
			player.tryingToJumpTime = Time.time;
		}
	}

	public override void UpdatePhysics()
	{
		float velocityY = -1;
		if ((player.characterController.collisionFlags & CollisionFlags.Above) != 0)
		{
			velocityY = -1;
			stateMachine.Change(player.states.MidAirState);
		}
		else if (player.playerVelocity.y > 0)
		{
			velocityY = player.playerVelocity.y + player.Gravity * player.jumpGravityScale;
		}
		else
		{
			//Enables player to have some more time to press jump button if they want to mid-air jump immediately
			if (Time.time - player.tryingToJumpTime < player.jumpButtonBuffer && player.canMidAirJump)
			{
				player.canMidAirJump = false;
				Enter(); //Re-enter jump state (skip mid-air state)
				return;
			}
			else
			{
				stateMachine.Change(player.states.MidAirState);
			}
		}
		player.playerVelocity = new Vector3(input.x * player.moveSpeed, velocityY, input.y * player.moveSpeed);
		player.characterController.Move(player.playerVelocity * Time.deltaTime);
	}

}
