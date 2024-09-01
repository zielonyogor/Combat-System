using System;
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
	[SerializeField] float jumpHeight = 12f;
	[Tooltip("Speed")]
	[SerializeField] float moveSpeed = 4f;

	private float gravityValue = -9.81f;
	[SerializeField]private float jumpGravityScale = 2f;
	private float fallGravityScale = 1f;

	private PlayerInput playerInput;
    private CharacterController controller;

	private Vector3 playerVelocity;
	private bool groundedPlayer;

	private uint numberOfJumps = 2; 

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

		if (playerVelocity.y > 0)
		{
			playerVelocity.y += gravityValue * jumpGravityScale * Time.deltaTime;
			Debug.Log(playerVelocity.y);
		}
		else if(!groundedPlayer)
		{
			playerVelocity.y += gravityValue * fallGravityScale * Time.deltaTime;
		}

		Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
		playerVelocity = new Vector3(input.x * moveSpeed, playerVelocity.y, input.y * moveSpeed);

		controller.Move(playerVelocity * Time.deltaTime);
		playerVelocity.x = 0;
		playerVelocity.z = 0;

		if(groundedPlayer)
		{
			playerInput.actions["PlungeAttack"].performed -= PlungeAttack;

			fallGravityScale = 1f;
			playerVelocity.y = 0;

			numberOfJumps = 2;
		}
		else
		{
			playerInput.actions["Attack"].performed += Attack;
			playerInput.actions["PlungeAttack"].performed += PlungeAttack;
		}
	}

	private void Jump(InputAction.CallbackContext context)
	{
		if(groundedPlayer || numberOfJumps > 0)
		{
			numberOfJumps--;
			playerVelocity.y += 1f * jumpHeight;
		}
	}

	private void Attack(InputAction.CallbackContext context) 
	{
		Debug.Log("Attack!!!");
		StartCoroutine(PerformAttack());
	}

	private IEnumerator PerformAttack()
	{
		playerVelocity = Vector3.zero;
		gravityValue = 0f;
		float duration = 0.8f;
		float currentTime = 0f;
		while (currentTime < duration)
		{
			float progress = currentTime / duration;
			transform.rotation = Quaternion.AngleAxis(Mathf.Lerp(transform.rotation.eulerAngles.y, 360, progress), Vector3.forward);
			currentTime += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		transform.rotation = Quaternion.identity;
		gravityValue = -9.81f;
		yield return null;	
	}

	private void PlungeAttack(InputAction.CallbackContext context) 
	{
		Debug.Log("Plunging");
		//playerVelocity.y = -10f;
		fallGravityScale = 15f;
	}
}
