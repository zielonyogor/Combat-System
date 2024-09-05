using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungeAttackState : State
{
	private const float bigPlungeThreshold = 8f;

	[SerializeField] float smallPlungeSpeed = 10f;
	[SerializeField] float bigPlungeSpeed = 20f;

	private float plungeSpeed;

	private Vector3 groundPosition;
	private Color plungeAttackRayColor;

	public PlungeAttackState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

	public override void Enter()
	{
		base.Enter();

		//check how high is player and do something different
		RaycastHit hit;
		Physics.Raycast(player.transform.position, Vector3.down, out hit);
		float distance = hit.distance;

		groundPosition = new Vector3(player.transform.position.x, player.transform.position.y - distance, player.transform.position.z);
		
		if (hit.distance < bigPlungeThreshold)
		{
			Debug.Log("Small plunge");
			plungeSpeed = smallPlungeSpeed;
			plungeAttackRayColor = Color.green;
		}
		else
		{
			Debug.Log("Big plunge");
			plungeSpeed = bigPlungeSpeed;
			plungeAttackRayColor = Color.red;
		}
	}

	public override void UpdatePhysics()
	{
		Debug.DrawLine(player.transform.position, groundPosition, plungeAttackRayColor);
		if (player.groundedPlayer)
		{
			player.playerVelocity.y = -1f;
			if (player.inputManager.moveAction.IsPressed())
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