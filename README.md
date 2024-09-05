# Movement & Combat System

Easily expandable movement & combat system. Based on State Machine pattern for managing player input.  
This project uses "New Input System". 

---

## Input

* **WSAD** - movement
* **Space** - jump
* **LMB** - attack
* **Ctrl + LMB** - plunge attack

---

## Implemented actions

### [Movement](/Assets/Scripts/StateMachine/MovementStates/WalkState.cs)

Simple **WSAD** movement
<p align="center"><img src="misc/movement.gif" /></p>

---

### [Jump (moveable) & double jump](/Assets/Scripts/StateMachine/MovementStates/JumpState.cs)

Player starts jumping (using **space**). After reaching its height they can start [mid-air state](/Assets/Scripts/StateMachine/MovementStates/MidAirState.cs) or make another (weaker) jump.  
_During mid-air state player can still perform weaker jump._  
There are a `jumpButtonBuffer` and `tryingToJumpTime` used for jump button actions. This makes jumping and double jumping smoother, f.e. if the player wants to immediately weak-jump after jump, they **don't have to spam** jump button. After completion of first jump, script checks if player previously tried to jump and let it count as _just in time_ input action. 
<p align="center"><img src="misc/jump_doublejump.gif" /></p>

---

### Two attack combo

Easily expandable, two ground attack combo.  
There is a **time buffer** for executing next combo attack so that the player doesn't have to spam attack.

* [First attack](/Assets/Scripts/StateMachine/AttackStates/FirstGroundAttackState.cs)
	<p align="center"><img src="misc/firstgroundattack.gif" /></p>
* [Second attack](/Assets/Scripts/StateMachine/AttackStates/SecondGroundAttackState.cs)
	<p align="center"><img src="misc/secondgroundattack.gif" /></p>

**Full combo:**

<p align="center"><img src="misc/groundcomboattack.gif" /></p>

---

### [Mid-air attack](/Assets/Scripts/StateMachine/AttackStates/MidAirAttackState.cs)

In this implementation, there is **no limit** to the amount of mid-air attacks.  
It can easily be added though, just like mid-air jump is done.
<p align="center"><img src="misc/midairattack.gif" /></p>

---

### [Plunge attack](/Assets/Scripts/StateMachine/AttackStates/PlungeAttackState.cs)

Before changing into this state, mid-air state checks if player is **high enough** for plunge attack.  
Then, on changing into this state, it checks whether to perform :red_circle: big or :green_heart: small plunge attack.
<p align="center"><img src="misc/plungeattack.gif" /></p>
