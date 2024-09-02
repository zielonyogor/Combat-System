using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[Tooltip("Speed")]
	[SerializeField] float moveSpeed = 4f;

	[SerializeField]private float jumpGravityScale = 1f;
	public float fallGravityScale = 1f;

	public PlayerInput playerInput;
    private CharacterController controller;

	public Vector3 playerVelocity;
	public bool groundedPlayer => controller.isGrounded;

	public event Action OnBeforeMove;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
	}


	void Start()
    {
        controller = GetComponent<CharacterController>();

		playerVelocity = Vector3.zero;
    }

	private void Update()
	{
		UpdateGravity();

		OnBeforeMove?.Invoke();

		Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
		playerVelocity = new Vector3(input.x * moveSpeed, playerVelocity.y, input.y * moveSpeed);

		controller.Move(playerVelocity * Time.deltaTime);

	}

	private void UpdateGravity()
	{
		float gravity = Physics.gravity.y * Time.deltaTime;
		if((controller.collisionFlags & CollisionFlags.Above) != 0)
		{
			playerVelocity.y = -1;
		}
		else if (playerVelocity.y > 0)
		{
			playerVelocity.y += gravity * jumpGravityScale;
		}
		else
		{
			playerVelocity.y = groundedPlayer ? -1f : playerVelocity.y + gravity * fallGravityScale;
		}
		
	}	
}
