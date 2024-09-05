using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MidAirState : State
{
	private Vector2 input;

	private const float plungeThreshold = 3f;
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
			RaycastHit hit;
			Physics.Raycast(player.transform.position, Vector3.down, out hit);
			if (hit.distance < plungeThreshold)
			{
				return;
			}
			stateMachine.Change(player.states.PlungeAttackState);
			return;
		}
		if(player.inputManager.attackAction.triggered)
		{
			stateMachine.Change(player.states.MidAirAttackState);
			return;
		}
		if (player.inputManager.jumpAction.triggered)
		{
			player.tryingToJumpTime = Time.time;
		}
	}

	public override void UpdatePhysics()
	{
		float velocityY;

		if (player.groundedPlayer)
		{
			velocityY = -1f;

			//Like in JumpState, lets player jump immediately 
			if (Time.time - player.tryingToJumpTime < player.jumpButtonBuffer)
			{
				player.canMidAirJump = true;
				stateMachine.Change(player.states.JumpState);
				return;
			}
			if (input != Vector2.zero)
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