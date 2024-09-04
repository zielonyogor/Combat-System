using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
	PlayerInput playerInput;

	[HideInInspector]
	public InputAction moveAction;
	[HideInInspector]
	public InputAction jumpAction;
	[HideInInspector]
	public InputAction attackAction;
	[HideInInspector]
	public InputAction plungeAttackAction;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions["Move"];
		jumpAction = playerInput.actions["Jump"];
		attackAction = playerInput.actions["Attack"];
		plungeAttackAction = playerInput.actions["PlungeAttack"];
	}
}
