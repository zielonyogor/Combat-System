using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool canMidAirJump;

    public Vector3 playerVelocity;

    public InputManager inputManager;
    public CharacterController characterController;

    StateMachine stateMachine;
    public StateList states;


    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
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
