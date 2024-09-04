using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SecondGroundAttackState : GroundAttackBase
{
	private float totalZRotation = 360f;
	private float rotationPerFrame;
	public SecondGroundAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) 
	{
		attackDuration = 1f;
	}

	public override void Enter()
	{
		base.Enter();
		rotationPerFrame = totalZRotation / rotationPerFrame;
	}

	public override void HandleInput()
	{
		if (hasFinishedAttack)
		{
			if (player.inputManager.moveAction.triggered)
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
