using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGroundAttackState : AttackStateBase
{
	private float totalZRotation = 360f;
	float rotationPerFrame;
	public SecondGroundAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) 
	{
		attackDuration = 0.8f;
	}

	public override void Enter()
	{
		base.Enter();
		rotationPerFrame = totalZRotation / attackDuration;
	}

	public override void HandleInput()
	{
		if (hasFinishedAttack)
		{
			if (player.inputManager.moveAction.IsPressed())
			{
				stateMachine.Change(player.states.WalkState);
			}
			else
			{
				stateMachine.Change(player.states.IdleState);
			}
		}
	}

	public override void UpdatePhysics()
	{
		base.UpdatePhysics();
		if (!hasFinishedAttack)
		{
			player.transform.Rotate(0, 0, rotationPerFrame * Time.deltaTime);
		}
	}

	public override void Exit()
	{
		base.Exit();
		player.transform.rotation = Quaternion.identity;
	}
}
