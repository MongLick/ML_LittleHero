using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerState;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
	[SerializeField] StateMachine<PlayerStateType> playerState;
	[SerializeField] PlayerStateType currentState;
	[SerializeField] CharacterController controller;
	[SerializeField] Animator animator;
	public Animator Animator { get { return animator; } }
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;
	[SerializeField] bool isGround;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] float ySpeedMax;

	[SerializeField] float attackDelay;
	public float AttackDelay { get { return attackDelay; } }

	private Vector3 moveDir;
	private Vector3 direction;
	private float ySpeed;

	private bool isAttack;
	public bool IsAttack { get { return isAttack; } set { isAttack = value; } }
	private bool isTakeHit;
	public bool IsTakeHit { get { return isTakeHit; } set { isTakeHit = value; } }
	private bool isBlock;
	public bool IsBlock { get { return isBlock; } set { isBlock = value; } }
	private bool isStunned;
	public bool IsStunned { get { return isStunned; } set { isStunned = value; } }
	private bool isDie;
	public bool IsDie { get { return isDie; } set { isDie = value; } }

	private Coroutine attackRoutine;
	public Coroutine AttackRoutine { get { return attackRoutine; } set { attackRoutine = value; } }

	private void Awake()
	{
		playerState = new StateMachine<PlayerStateType>();
		playerState.AddState(PlayerStateType.Idle, new PlayerIdleState(this));
		playerState.AddState(PlayerStateType.Attack, new PlayerAttackState(this));
		playerState.AddState(PlayerStateType.TakeHit, new PlayerTakeHitState(this));
		playerState.AddState(PlayerStateType.Block, new PlayerBlockState(this));
		playerState.AddState(PlayerStateType.Stunned, new PlayerStunnedState(this));
		playerState.AddState(PlayerStateType.Die, new PlayerDieState(this));
		playerState.Start(PlayerStateType.Idle);
	}

	private void Update()
	{
		playerState.Update();
		currentState = playerState.GetCurrentState();
		Move();
		JumpMove();
	}

	private void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;
	}

	private void OnJump(InputValue value)
	{
		if (isGround)
		{
			ySpeed = jumpSpeed;
			animator.SetTrigger("Jump");
		}
	}

	private void OnAttack(InputValue value)
	{
		isAttack = true;
	}

	private void Move()
	{
		controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
		controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);

		animator.SetFloat("XSpeed", moveDir.x * moveSpeed, 0.1f, Time.deltaTime);
		animator.SetFloat("YSpeed", moveDir.z * moveSpeed, 0.1f, Time.deltaTime);
	}

	private void JumpMove()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;

		if (ySpeed < ySpeedMax)
		{
			ySpeed = ySpeedMax;
		}

		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (groundLayer.Contain(other.gameObject.layer))
		{
			isGround = true;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (groundLayer.Contain(other.gameObject.layer))
		{
			isGround = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (groundLayer.Contain(other.gameObject.layer))
		{
			isGround = false;
		}
	}
}
