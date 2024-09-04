using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlungeAttackState : State
{
	private const float plungeThreshold = 3f;
	private const float bigPlungeThreshold = 8f;

	[SerializeField] float smallPlungeSpeed = 6f;
	[SerializeField] float bigPlungeSpeed = 14f;

	private float plungeSpeed;

	private Vector2 input;
	public PlungeAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();

		//check how high is player and do something different
		RaycastHit hit;
		Physics.Raycast(player.transform.position, Vector3.down, out hit);
		if (hit.distance < plungeThreshold)
		{
			return;
		}
		else if (hit.distance < bigPlungeThreshold)
		{
			Debug.Log("Small plunge");
			plungeSpeed = smallPlungeSpeed;
		}
		else
		{
			Debug.Log("Big plunge");
			plungeSpeed = bigPlungeSpeed;
		}
	}

	public override void HandleInput()
	{
		input = player.inputManager.moveAction.ReadValue<Vector2>();
	}

	public override void UpdatePhysics()
	{
		if (player.groundedPlayer)
		{
			player.playerVelocity.y = -1f;
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
			player.playerVelocity.y += player.Gravity * plungeSpeed;
		}

		player.characterController.Move(player.playerVelocity * Time.deltaTime);
	}
}