using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
	private const float plungeThreshold = 3f;
	private const float bigPlungeThreshold = 8f;

	private bool isMidAir;
	private bool isAttacking;

	private PlayerMovement playerInputManager;

	private const int numberOfComboAttacks = 3;
	private const int numberOfMidAirComboAttacks = 2;
	private const float comboAttackBufferTime = 0.5f;

	private uint currentAttackComboIndex = 0;
	private float lastAttackTime;

	private void Awake()
	{
		playerInputManager = GetComponent<PlayerMovement>();
	}

	private void OnEnable()
	{
		playerInputManager.OnBeforeMove += OnBeforeMove;
	}

	private void OnDisable()
	{
		playerInputManager.OnBeforeMove -= OnBeforeMove;
	}

	private void OnBeforeMove()
	{

		if (playerInputManager.groundedPlayer)
		{
			isMidAir = false;
		}
		else
		{
			isMidAir = true;
		}
	}

	private void OnAttack()
	{
		bool isContinuingCombo = Time.time - lastAttackTime < comboAttackBufferTime;
		if(!isAttacking)
		{
			Debug.Log("Attack!!!");
			if(isContinuingCombo && currentAttackComboIndex > 0)
			{
				if(currentAttackComboIndex == 1)
				{
					StartCoroutine(PerformSecondAttack());
				}
				else
				{
					Debug.Log("tu powinien byc 3 atak");
					StartCoroutine(PerformThirdAttack());
				}
			}
			else
			{
				currentAttackComboIndex = 0;
				StartCoroutine(PerformFirstAttack());
			}
			isAttacking = true;
		}
	}


	private IEnumerator PerformFirstAttack()
	{
		StartAttack();

		playerInputManager.fallGravityScale = 0f;
		float duration = 0.8f;
		float currentTime = 0f;
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		StopAttack();
	}
	private IEnumerator PerformSecondAttack()
	{
		StartAttack();

		playerInputManager.fallGravityScale = 0f;
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
		StopAttack();
	}

	private IEnumerator PerformThirdAttack()
	{
		StartAttack();
		yield return new WaitForSeconds(1f);
		StopAttack();
		currentAttackComboIndex = 0; //fix this aaaaaaaaaaaaaaaaaa
	}

	private void StartAttack()
	{
		playerInputManager.playerVelocity = Vector3.zero;

		playerInputManager.playerInput.DeactivateInput();
	}

	private void StopAttack() 
	{
		currentAttackComboIndex++;
		playerInputManager.fallGravityScale = 1f;
		isAttacking = false;
		lastAttackTime = Time.time;
		playerInputManager.playerInput.ActivateInput();
	}

	private void OnPlungeAttack()
	{
		Debug.Log("plunging");
		if(isMidAir && !isAttacking)
		{
			//check how high is player and do something different
			RaycastHit hit;
			Physics.Raycast(transform.position, Vector3.down, out hit);
			Debug.Log(hit.distance);
			if (hit.distance < plungeThreshold)
			{ 
				return; 
			}
			else if(hit.distance < bigPlungeThreshold)
			{
				Debug.Log("Small plunge");
				playerInputManager.fallGravityScale = 5f;
			}
			else
			{
				Debug.Log("Big plunge");
				playerInputManager.fallGravityScale = 15f;
			}
			isAttacking = true;
			playerInputManager.playerVelocity = Vector3.zero;
			StartCoroutine(WaitForGroundTouch());
		}
	}

	private IEnumerator WaitForGroundTouch()
	{
		yield return new WaitUntil(() => playerInputManager.groundedPlayer == true);
		isAttacking = false;
		playerInputManager.fallGravityScale = 1f;
	} 
}
