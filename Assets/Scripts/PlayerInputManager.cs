using System;
using System.Collections;
using System.Collections.Generic;
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
	[SerializeField]private float jumpGravityScale = 1f;
	private float fallGravityScale = 1f;

	private PlayerInput playerInput;
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

		if(groundedPlayer)
		{
			playerInput.actions["PlungeAttack"].performed -= PlungeAttack;

			fallGravityScale = 1f;
		}
		else
		{
			playerInput.actions["Attack"].performed += Attack;
			playerInput.actions["PlungeAttack"].performed += PlungeAttack;
		}

		//if (playerVelocity.y > 0)
		//{
		//	playerVelocity.y += gravityValue * jumpGravityScale * Time.deltaTime;
		//	//Debug.Log(playerVelocity.y);
		//}
		//else if(!groundedPlayer)
		//{
		//	playerVelocity.y += gravityValue * fallGravityScale * Time.deltaTime;
		//}
	}

	private void UpdateGravity()
	{
		float gravity = Physics.gravity.y * Time.deltaTime;
		if (playerVelocity.y > 0)
		{
			playerVelocity.y += gravity * jumpGravityScale;
		}
		else
		{
			playerVelocity.y = groundedPlayer ? -1f : playerVelocity.y + gravity;
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
		fallGravityScale = 15f;
	}
}
