using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAirAttackState : AttackStateBase
{
	private Color playerColor = Color.magenta;
	private Color playerStartColor;
	public MidAirAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) 
	{
		attackDuration = 0.5f;
	}

	public override void Enter()
	{
		base.Enter();
		playerStartColor = player.playerMaterial.color;
		player.playerMaterial.color = playerColor;
	}

	public override void HandleInput()
	{
		if (hasFinishedAttack)
		{
			player.playerMaterial.color = playerStartColor;
			stateMachine.Change(player.states.MidAirState);
		}
	}

	public override void UpdatePhysics()
	{
		base.UpdatePhysics();
	}
}