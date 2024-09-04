using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateList
{
    public IdleState IdleState {  get; private set; }
    public JumpState JumpState { get; private set; }
    public WalkState WalkState { get; private set; }
    public MidAirState MidAirState { get; private set; }
    public PlungeAttackState PlungeAttackState { get; private set; }
    public FirstGroundAttackState FirstGroundAttackState { get; private set; }
    public SecondGroundAttackState SecondGroundAttackState { get; private set; }

    public StateList(Player player, StateMachine stateMachine)
    {
        IdleState = new IdleState(player, stateMachine);
        JumpState = new JumpState(player, stateMachine);
        WalkState = new WalkState(player, stateMachine);
        MidAirState = new MidAirState(player, stateMachine);
        PlungeAttackState  = new PlungeAttackState(player, stateMachine);
        FirstGroundAttackState = new FirstGroundAttackState(player, stateMachine);
        SecondGroundAttackState = new SecondGroundAttackState(player, stateMachine);
    }
}
