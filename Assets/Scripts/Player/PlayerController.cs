using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerState;
using UnityEngine.Playables;
using Cinemachine;

public class PlayerController : MonoBehaviour, IDamageable
{
	[SerializeField] float attackCooltime;
	public float AttackCooltime { get { return attackCooltime; } }
	[SerializeField] float cooltime;
	[SerializeField] GameObject attack;
	public GameObject Attack { get { return attack; }  set { attack = value; } }
	[SerializeField] PlayerInput playerInput;
	public PlayerInput PlayerInput { get { return playerInput; } set { playerInput = value; } }
	[SerializeField] CinemachineVirtualCamera gameOverCamera;
	public CinemachineVirtualCamera GameOverCamera { get { return gameOverCamera; } set { gameOverCamera = value; } }
	[SerializeField] float hp;
	public float HP { get { return hp; } }
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
	[SerializeField] float rotationSpeed;

	[SerializeField] float attackDelay;
	public float AttackDelay { get { return attackDelay; } }
	[SerializeField] float takeHitDelay;
	public float TakeHitDelay { get { return takeHitDelay; } }
	[SerializeField] float stunnedDelay;
	public float StunnedDelay { get { return stunnedDelay; } }

	private Vector3 moveDir;
	private Vector3 direction;
	private float ySpeed;

	private bool isAttackCooltime;
	public bool IsAttackCooltime { get { return isAttackCooltime; } set { isAttackCooltime = value; } }
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
	private Coroutine blockRoutine;
	public Coroutine BlockRoutine { get { return blockRoutine; } set { blockRoutine = value; } }
	private Coroutine stunnedRoutine;
	public Coroutine StunnedRoutine { get { return stunnedRoutine; } set { stunnedRoutine = value; } }
	private Coroutine takeHitRoutine;
	public Coroutine TakeHitRoutine { get { return takeHitRoutine; } set { takeHitRoutine = value; } }

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
		CoolTimeCheck();
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
		if (!isAttack)
		{
			isAttack = true;
		}
	}

	private void OnBlock(InputValue value)
	{
		if(value.isPressed)
		{
			isBlock = true;
		}
		else
		{
			isBlock = false;
		}
	}

	private void Move()
	{
		Vector3 move = new Vector3(moveDir.x, 0, moveDir.z).normalized;
		if (move != Vector3.zero)
		{
			Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			animator.SetBool("Move",true);
		}
		else
		{
			animator.SetBool("Move", false);
		}

		controller.Move(move * moveSpeed * Time.deltaTime);
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

	private void CoolTimeCheck()
	{
		if (isAttackCooltime)
		{
			cooltime += Time.deltaTime;
			if (cooltime >= attackCooltime)
			{
				isAttackCooltime = false;
				cooltime = 0;
			}
		}
	}

	public void TakeDamage(int damage, bool isStunAttack)
	{
		if(!isBlock)
		{
			hp -= damage;
			isAttack = false;
			if (isStunAttack && !isStunned)
			{
				isStunned = true;
			}
			else
			{
				isTakeHit = true;
			}
		}
		else
		{
			isTakeHit = true;
		}
		
		if(hp <= 0)
		{
			isDie = true;
		}
	}

	public void Attacktrue()
	{
		attack.SetActive(true);
	}

	public void Attackfalse()
	{
		attack.SetActive(false);
	}
}
