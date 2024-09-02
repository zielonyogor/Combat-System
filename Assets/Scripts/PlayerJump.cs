using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 10f;
	[SerializeField] float midAirJumpSpeed = 6f;
	[SerializeField] float jumpButtonBuffer = 0.05f;
	[SerializeField] uint numberOfMidAirJumps = 1;

    PlayerInputManager playerInputManager;

	private bool tryJumping;
	private float lastJumpButtonTime;

	private void Awake()
	{
		playerInputManager = GetComponent<PlayerInputManager>();
	}

	private void OnEnable()
	{
		playerInputManager.OnBeforeMove += OnBeforeMove;	
	}

	private void OnDisable()
	{
		playerInputManager.OnBeforeMove -= OnBeforeMove;
	}

	public void OnJump()
	{
		Debug.Log("pressed Jump");
        tryJumping = true;
		lastJumpButtonTime = Time.time;
	}

	private void OnBeforeMove()
	{
		bool didTryJumping = Time.time - lastJumpButtonTime < jumpButtonBuffer;
		bool attemptToJump = (tryJumping || didTryJumping);
		if (attemptToJump && playerInputManager.groundedPlayer)
		{
			Jump(jumpSpeed);
		}
		else if(tryJumping && numberOfMidAirJumps > 0)
		{
			numberOfMidAirJumps -= 1;
			Jump(midAirJumpSpeed);
		}

		if(playerInputManager.groundedPlayer)
		{
			numberOfMidAirJumps = 1;
		}
		tryJumping = false;
	}

	private void Jump(float speed)
	{
		Debug.Log("jumping: " + speed);
		playerInputManager.playerVelocity.y = speed;
		lastJumpButtonTime = 0;
	}
}
