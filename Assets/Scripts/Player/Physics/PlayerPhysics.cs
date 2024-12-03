using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters
	public PlayerPhysicsData PhysicsData;

	#region COMPONENTS
	public Rigidbody2D Rig { get; private set; }
	public PlayerAnimations PlayerAnim;
	private PlayerPhysicsStateMachine playerPhysicsSM;
	#endregion

	#region STATE PARAMETERS
	//Variables that controls the various actions the player can perform at any time.
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get { return playerPhysicsSM.GetCurrentState() == PlayerPhysicsStatesEnum.JUMP; } }

	//Timers
	public float LastOnGroundTime { get; private set; }

	//Grounded
	public bool IsGrounded { get; private set; }

	//Jump
	[HideInInspector]
	public bool IsJumpCut;
	[HideInInspector]
	public bool IsJumpFalling;

	#endregion

	#region INPUT PARAMETERS
	public Vector2 MoveInput { get; private set; }

	public float LastPressedJumpTime { get; private set; }
	#endregion

	#region CHECK PARAMETERS
	[Header("Checks")]
	[SerializeField]
	private Transform groundCheckPoint;
	[SerializeField]
	private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
	#endregion

	#region LAYERS & TAGS
	[Header("Layers & Tags")]
	[SerializeField]
	private LayerMask groundLayer;
	#endregion

	private void Start()
	{
		Rig = GetComponent<Rigidbody2D>();
		PlayerAnim = GetComponent<PlayerAnimations>();
		playerPhysicsSM = GetComponent<PlayerPhysicsStateMachine>();

		SetGravityScale(PhysicsData.gravityScale);
		IsFacingRight = true;

	}

	private void Update()
	{
		#region TIMERS
		LastOnGroundTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		#endregion

		if (MoveInput.x != 0)
			CheckDirectionToFace(MoveInput.x > 0);

		PlayerAnim.ToggleRunParticles((IsGrounded && MoveInput.x != 0));

		#region COLLISION CHECKS
		IsGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
		PlayerAnim.SetBool("isGrounded", IsGrounded, false, true);

		if (!IsJumping)
		{
			//Ground Check
			if (IsGrounded)
			{
				if (LastOnGroundTime < -0.1f)
				{
					PlayerAnim.CallLandingFeedback();
				}

				LastOnGroundTime = PhysicsData.coyoteTime;
				playerPhysicsSM.ChangeState(PlayerPhysicsStatesEnum.NEUTRAL);
			}
		}
		#endregion

		#region JUMP CHECKS
		if (LastOnGroundTime > 0 && !IsJumping)
		{
			IsJumpCut = false;

			IsJumpFalling = false;
		}

		//Jump
		if (CanJump() && LastPressedJumpTime > 0)
		{
			playerPhysicsSM.ChangeState(PlayerPhysicsStatesEnum.JUMP);
			IsJumpCut = false;
			IsJumpFalling = false;
			Jump(Vector2.up);
		}
		#endregion

		#region GRAVITY
		if (IsJumpCut)
		{
			//Higher gravity if jump button released
			SetGravityScale(PhysicsData.gravityScale * PhysicsData.jumpCutGravityMult);
			Rig.velocity = new Vector2(Rig.velocity.x, Mathf.Max(Rig.velocity.y, -PhysicsData.maxFallSpeed));
		}
		else if (Rig.velocity.y < 0)
		{
			//Higher gravity if falling
			SetGravityScale(PhysicsData.gravityScale * PhysicsData.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			Rig.velocity = new Vector2(Rig.velocity.x, Mathf.Max(Rig.velocity.y, -PhysicsData.maxFallSpeed));
		}
		#endregion
	}

	private void FixedUpdate()
	{
		//Handle Run
		Run(1);
	}

	#region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
	public void OnJumpInput()
	{
		LastPressedJumpTime = PhysicsData.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut())
			IsJumpCut = true;
	}
	#endregion

	#region GENERAL METHODS
	public void SetGravityScale(float scale)
	{
		Rig.gravityScale = scale;
	}
	#endregion

	//MOVEMENT METHODS
	#region RUN METHODS
	public void OnMoveInput(Vector2 dir)
	{
		Vector2 lastMoveInput = MoveInput;

		MoveInput = dir;

		PlayerAnim.SetBool("isMoving", MoveInput.x != 0, false, true);
	}

	private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = MoveInput.x * PhysicsData.runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(Rig.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PhysicsData.runAccelAmount : PhysicsData.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PhysicsData.runAccelAmount * PhysicsData.accelInAir : PhysicsData.runDeccelAmount * PhysicsData.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping) && Mathf.Abs(Rig.velocity.y) < PhysicsData.jumpHangTimeThreshold)
		{
			accelRate *= PhysicsData.jumpHangAccelerationMult;
			targetSpeed *= PhysicsData.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if (PhysicsData.doConserveMomentum && Mathf.Abs(Rig.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(Rig.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - Rig.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		Rig.AddForce(movement * Vector2.right, ForceMode2D.Force);
	}

	private void Turn()
	{
		ForceTurn();
	}

	public void ForceTurn()
	{
		PlayerAnim.TurnPlayer(IsFacingRight);

		IsFacingRight = !IsFacingRight;
	}
	#endregion

	#region JUMP METHODS
	private void Jump(Vector2 jumpDir)
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		float force = PhysicsData.jumpForce;
		if (Rig.velocity.y < 0)
			force -= Rig.velocity.y;

		Rig.AddForce(jumpDir * force, ForceMode2D.Impulse);
		#endregion
	}
	#endregion

	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		return LastOnGroundTime > 0 && !IsJumping;
	}

	private bool CanJumpCut()
	{
		return IsJumping && Rig.velocity.y > 0;
	}
	#endregion

	#region EDITOR METHODS
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
	}
	#endregion
}
