using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
	[Tooltip("Height of first jump")]
	[SerializeField] float jumpHeight = 3f;
	[Tooltip("Speed")]
	[SerializeField] float moveSpeed = 4f;
	private const float gravityValue = -9.81f;

    private PlayerInput playerInput;
    private CharacterController controller;

	private Vector3 playerVelocity;
	private bool groundedPlayer;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
	}

	private void OnEnable()
	{
		playerInput.actions["Jump"].performed += Jump;
	}

	void Start()
    {
        controller = GetComponent<CharacterController>();

		playerVelocity = Vector3.zero;
    }

	private void Update()
	{
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
			playerInput.SwitchCurrentActionMap("Ground");
			playerInput.actions["Jump"].performed += Jump;
		}

		Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
		playerVelocity = new Vector3(input.x, playerVelocity.y, input.y);
		controller.Move(playerVelocity);

		if (playerVelocity.y != 0 && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -1f * gravityValue);
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}

	private void Jump(InputAction.CallbackContext context)
	{
		playerInput.SwitchCurrentActionMap("Air");
		Debug.Log("jump");
		playerVelocity.y = 1f;
		playerInput.actions["Jump"].performed -= Jump;
		playerInput.actions["Attack"].performed += Attack;
	}

	private void Attack(InputAction.CallbackContext context) 
	{
		Debug.Log("Attack!!!");
	}
}
