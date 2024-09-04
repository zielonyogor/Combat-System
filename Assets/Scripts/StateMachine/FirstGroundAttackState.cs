using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGroundAttackState : GroundAttackBase
{
	private Vector3 moveDirection = new Vector3(0, -0.02f, 3f);
	private Vector3 moveSpeed;
	public FirstGroundAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) 
	{
		attackDuration = 0.5f;
	}

	public override void Enter()
	{
		base.Enter();
		moveSpeed = moveDirection / attackDuration;
		player.playerVelocity = moveSpeed;
	}

	public override void HandleInput()
	{
		bool hasComboEnded = Time.time - (attackTime + attackDuration) > comboAttackBufferTime;
		if (hasComboEnded)
		{
			stateMachine.Change(player.states.IdleState);
		}
		if (hasFinishedAttack)
		{
			if(player.inputManager.attackAction.triggered)
			{
				stateMachine.Change(player.states.SecondGroundAttackState);
			}
			else if (player.inputManager.moveAction.triggered)
			{
				stateMachine.Change(player.states.WalkState);
			}
		}
	}

	public override void UpdatePhysics()
	{
		base.UpdatePhysics();
		if (!hasFinishedAttack)
		{
			player.characterController.Move(player.playerVelocity * Time.deltaTime);
		}
	}
}
