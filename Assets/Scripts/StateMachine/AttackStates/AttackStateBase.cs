using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateBase : State
{
	protected float attackTime;
	protected float attackDuration;
	protected bool hasFinishedAttack;

	protected const float comboAttackBufferTime = 0.5f;
	public AttackStateBase(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
		attackTime = Time.time;
	}

	public override void UpdatePhysics()
	{
		hasFinishedAttack = Time.time - attackTime >= attackDuration;
	}

	public override void Exit()
	{
		player.playerVelocity.x = 0;
		player.playerVelocity.y = -1f;
		player.playerVelocity.z = 0;
	}
}
