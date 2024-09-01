using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerInputManager : MonoBehaviour
{
	[Tooltip("Height of first jump")]
	[SerializeField] float jumpHeight = 3f;
	[Tooltip("Speed")]
	[SerializeField] float moveSpeed = 4f;

	private const float gravityValue = -9.81f;
	private const float jumpGravityScale = 0.5f;

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
		playerInput.actions["Move"].performed += Move;
	}

	void Start()
    {
        controller = GetComponent<CharacterController>();

		playerVelocity = Vector3.zero;
    }

	private void Update()
	{
		groundedPlayer = controller.isGrounded;

		if (playerVelocity.y > 0)
		{
			playerVelocity.y += gravityValue * jumpGravityScale * Time.deltaTime;
		}
		else if(!groundedPlayer)
		{
			playerVelocity.y += gravityValue * Time.deltaTime;
			Debug.Log(playerVelocity.y);
		}

		controller.Move(playerVelocity * Time.deltaTime);
		playerVelocity.x = 0;
		playerVelocity.z = 0;

		if(groundedPlayer)
		{
			playerInput.SwitchCurrentActionMap("Ground");
			playerVelocity.y = 0;
		}
	}

	private void Move(InputAction.CallbackContext context)
	{
		Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
		playerVelocity = new Vector3(input.x * moveSpeed, playerVelocity.y, input.y * moveSpeed);
		Debug.Log("mooove");
	}

	private void Jump(InputAction.CallbackContext context)
	{
		playerInput.SwitchCurrentActionMap("Air");
		playerVelocity.y = 1f * jumpHeight;
		playerInput.actions["Attack"].performed += Attack;
	}

	private void Attack(InputAction.CallbackContext context) 
	{
		Debug.Log("Attack!!!");
		StartCoroutine(PerformAttack());
	}

	private IEnumerator PerformAttack()
	{
		playerVelocity = Vector3.zero;
		yield return new WaitForSeconds(1f);

	}
}
