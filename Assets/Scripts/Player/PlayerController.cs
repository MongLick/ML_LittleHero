using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerState;
using UnityEngine.Playables;
using Cinemachine;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading;

public class PlayerController : MonoBehaviour, IDamageable
{
	private Vector3 move;
	[SerializeField] MonsterController monster;
	[SerializeField] float closestDistance;
	private Vector3 playerPosition;
	[SerializeField] Collider[] hitColliders;
	[SerializeField] float detectionRange;
	[SerializeField] float monsterDistance;
	private Transform monsterTarget;
	[SerializeField] float attackCooltime;
	public float AttackCooltime { get { return attackCooltime; } }
	[SerializeField] float cooltime;
	[SerializeField] GameObject attack;
	public GameObject Attack { get { return attack; } set { attack = value; } }
	[SerializeField] PlayerInput playerInput;
	public PlayerInput PlayerInput { get { return playerInput; } set { playerInput = value; } }
	[SerializeField] CinemachineVirtualCamera gameOverCamera;
	public CinemachineVirtualCamera GameOverCamera { get { return gameOverCamera; } set { gameOverCamera = value; } }
	[SerializeField] StateMachine<PlayerStateType> playerState;
	[SerializeField] PlayerStateType currentState;
	public PlayerStateType CurrentState { get { return currentState; } set { currentState = value; } }
	[SerializeField] CharacterController controller;
	[SerializeField] Animator animator;
	public Animator Animator { get { return animator; } }
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;
	[SerializeField] bool isGround;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] LayerMask monsterLayer;
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
	private bool isAutoAttack;
	private bool isSkiilAttack = false;
	public bool IsSkiilAttack { get { return isSkiilAttack; } set { isSkiilAttack = value; } }

	private string skillName;
	public string SkillName { get { return skillName; } set { skillName = value; } }

	[SerializeField] float skillOffset;
	public float SkillOffset { get { return skillOffset; } set { skillOffset = value; } }

	private Coroutine attackRoutine;
	public Coroutine AttackRoutine { get { return attackRoutine; } set { attackRoutine = value; } }
	private Coroutine blockRoutine;
	public Coroutine BlockRoutine { get { return blockRoutine; } set { blockRoutine = value; } }
	private Coroutine stunnedRoutine;
	public Coroutine StunnedRoutine { get { return stunnedRoutine; } set { stunnedRoutine = value; } }
	private Coroutine takeHitRoutine;
	public Coroutine TakeHitRoutine { get { return takeHitRoutine; } set { takeHitRoutine = value; } }
	public InventoryUI inventoryUI;
	public QuickSlot quickSlot;
	[SerializeField] float range;
	public float Range { get { return range; } }
	[SerializeField] Collider[] colliders = new Collider[20];

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
			Manager.Sound.PlaySFX(Manager.Sound.PlayerJump);
		}
	}

	private void OnAttack(InputValue value)
	{
		if (!isAttack && !EventSystem.current.IsPointerOverGameObject())
		{
			isAttack = true;
		}
	}

	public void AttackButton()
	{
		if (!isAttack)
		{
			isAttack = true;
		}
	}

	private void OnBlock(InputValue value)
	{
		if (value.isPressed)
		{
			isBlock = true;
		}
		else
		{
			isBlock = false;
		}
	}

	private void OnInteract(InputValue value)
	{
		Interact();
	}

	private void Move()
	{
		move = new Vector3(moveDir.x, 0, moveDir.z).normalized;
		if (move != Vector3.zero)
		{
			Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			animator.SetBool("Move", true);
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

	public void AutoAttackButton()
	{
		if (isAutoAttack)
		{
			isAutoAttack = false;
			moveDir = Vector3.zero;
		}
		else
		{
			isAutoAttack = true;
			StartCoroutine(AutoAttackCoroutine());
		}
	}

	private void Interact()
	{
		int size = Physics.OverlapSphereNonAlloc(transform.position, range, colliders);

		for (int i = 0; i < size; i++)
		{
			IInteractable interactable = colliders[i].GetComponent<IInteractable>();
			if (interactable != null)
			{
				interactable.Interact(this);
				break;
			}
		}
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
		if (!isBlock)
		{
			Manager.Data.UserData.Health -= damage;
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
			Manager.Sound.PlaySFX(Manager.Sound.PlayerBlock);
			isTakeHit = true;
		}

		if (Manager.Data.UserData.Health <= 0)
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

	private IEnumerator AutoAttackCoroutine()
	{
		while (isAutoAttack)
		{
			hitColliders = Physics.OverlapSphere(transform.position, detectionRange, monsterLayer);

			monsterTarget = null;
			closestDistance = float.MaxValue;

			foreach (Collider collider in hitColliders)
			{
				monster = collider.GetComponent<MonsterController>();
				if (monster != null && monster.Hp > 0)
				{
					float distance = Vector3.Distance(transform.position, collider.transform.position);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						monsterTarget = collider.transform;
					}
				}
			}

			if (monsterTarget != null)
			{
				while (isAutoAttack)
				{
					if (monsterTarget.GetComponent<MonsterController>().Hp <= 0)
					{
						monsterTarget = null;
						break;
					}
					MoveTowardsMonster();
					yield return null;
				}
			}
			yield return null;
		}
	}

	private void MoveTowardsMonster()
	{
		if (monsterTarget != null)
		{
			Vector3 directionToMonster = (monsterTarget.position - transform.position).normalized;
			float distance = Vector3.Distance(transform.position, monsterTarget.position);

			if (distance > monsterDistance)
			{
				moveDir.x = directionToMonster.x;
				moveDir.z = directionToMonster.z;
			}
			else
			{
				isAttack = true;
				moveDir = Vector3.zero;

				Quaternion rotation = Quaternion.LookRotation(directionToMonster);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
			}
		}
	}

	public void SkillAttack(string skillName)
	{
        if (isSkiilAttack && isAttack)
        {
			return;
        }
        isAttack = true;
		isSkiilAttack = true;
		SkillName = skillName;
	}
}
