using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main player class. Contains movement-related variables, components, input manager. Executes current state.
/// </summary>
[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{
    [Header("Variables")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 6f;
    public float midAirJumpSpeed = 4f;
    public float fallGravityScale = 1f;
    public float jumpGravityScale = 1f;
    public float Gravity => -9.81f * Time.deltaTime;


	public bool groundedPlayer => characterController.isGrounded;

    [HideInInspector]
    public bool canMidAirJump;
    [HideInInspector]
    public Material playerMaterial;
	[HideInInspector]
	public Vector3 playerVelocity;
	[HideInInspector]
	public InputManager inputManager;
	[HideInInspector]
	public CharacterController characterController;

    StateMachine stateMachine;
    public StateList states;


    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        playerMaterial = GetComponent<MeshRenderer>().material;

        stateMachine = new StateMachine();
        states = new StateList(this, stateMachine);

        characterController.minMoveDistance = 0;
        playerVelocity = Vector3.zero;
        canMidAirJump = false;
    }

	private void Start()
	{
        stateMachine.Init(states.IdleState);
	}

	void Update()
    {
        stateMachine.currentState.HandleInput();
        stateMachine.currentState.UpdatePhysics();
	}
}
