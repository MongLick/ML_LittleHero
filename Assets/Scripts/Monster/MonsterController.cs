using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static MonsterState;

public class MonsterController : MonoBehaviour, IDamageable
{
	public enum MonsterType { Mushroom, Cactus }
	[SerializeField] MonsterType type;
	public MonsterType Type { get { return type; } }
	[SerializeField] StateMachine<MonsterStateType> monsterState;
	[SerializeField] MonsterStateType currentState;
	[SerializeField] LayerMask playerLayer;
	public LayerMask PlayerLayer { get { return playerLayer; } }

	[Header("UnityEvent")]
	[SerializeField] UnityEvent<MonsterController> onDieEvent;
	public UnityEvent<MonsterController> OnDieEvent { get { return onDieEvent; } set { onDieEvent = value; } }

	[Header("Components")]
	[SerializeField] PooledObject pooledObject;
	public PooledObject PooledObject { get { return pooledObject; } }
	[SerializeField] MonsterController monsterCon;
	public MonsterController MonsterCon { get { return monsterCon; } set { monsterCon = value; } }
	[SerializeField] GameObject attack;
	public GameObject Attack { get { return attack; } set { attack = value; } }
	[SerializeField] BoxCollider boxCollider;
	public BoxCollider BoxCollider { get { return boxCollider; } set { BoxCollider = value; } }
	[SerializeField] NavMeshAgent agent;
	public NavMeshAgent Agent { get { return agent; } }
	[SerializeField] Transform target;
	public Transform Target { get { return target; } }
	[SerializeField] Transform spawnPos;
	public Transform SpawnPos { get { return spawnPos; } set { spawnPos = value; } }
	[SerializeField] Animator animator;
	public Animator Animator { get { return animator; } }

	[Header("Coroutine")]
	private Coroutine attackRoutine;
	public Coroutine AttackRoutine { get { return attackRoutine; } set { attackRoutine = value; } }
	private Coroutine takeHitRoutine;
	public Coroutine TakeHitRoutine { get { return takeHitRoutine; } set { takeHitRoutine = value; } }
	private Coroutine stunnedRoutine;
	public Coroutine StunnedRoutine { get { return stunnedRoutine; } set { stunnedRoutine = value; } }
	private Coroutine dieRoutine;
	public Coroutine DieRoutine { get { return dieRoutine; } set { dieRoutine = value; } }

	[Header("Specs")]
	[SerializeField] float moveDetectionRadius;
	public float MoveDetectionRadius { get { return moveDetectionRadius; } set { moveDetectionRadius = value; } }
	[SerializeField] float attackDetectionRadius;
	public float AttackDetectionRadius { get { return attackDetectionRadius; } set { attackDetectionRadius = value; } }
	[SerializeField] float maxDistance;
	public float MaxDistance { get { return maxDistance; } }
	[SerializeField] float spawnDistance;
	public float SpawnDistance { get { return spawnDistance; } set { spawnDistance = value; } }
	[SerializeField] float attackDelay;
	public float AttackDelay { get { return attackDelay; } }
	[SerializeField] float takeHitDelay;
	public float TakeHitDelay { get { return takeHitDelay; } }
	[SerializeField] float stunnedDelay;
	public float StunnedDelay { get { return stunnedDelay; } }
	[SerializeField] float dieDelay;
	public float DieDelay { get { return dieDelay; } }
	[SerializeField] float attackCooltime;
	public float AttackCooltime { get { return attackCooltime; } }
	[SerializeField] float cooltime;
	[SerializeField] float spawnDistanceMax;
	[SerializeField] int gold;
	public int Gold { get { return gold; } }
	[SerializeField] int hp;
	public int Hp { get { return hp; } set { hp = value; } }
	[SerializeField] int maxHp;
	public int MaxHp { get { return maxHp; } }
	private bool isAttackCooltime;
	public bool IsAttackCooltime { get { return isAttackCooltime; } set { isAttackCooltime = value; } }
	private bool isIdle;
	public bool IsIdle { get { return isIdle; } set { isIdle = value; } }
	private bool isMove;
	public bool IsMove { get { return isMove; } set { isMove = value; } }
	private bool isReturn;
	public bool IsReturn { get { return isReturn; } set { isReturn = value; } }
	private bool isAttack;
	public bool IsAttack { get { return isAttack; } set { isAttack = value; } }
	private bool isTakeHit;
	public bool IsTakeHit { get { return isTakeHit; } set { isTakeHit = value; } }
	private bool isStunned;
	public bool IsStunned { get { return isStunned; } set { isStunned = value; } }
	private bool isDie;
	public bool IsDie { get { return isDie; } set { isDie = value; } }

	private void Awake()
	{
		monsterState = new StateMachine<MonsterStateType>();
		monsterState.AddState(MonsterStateType.Idle, new MonsterIdleState(this));
		monsterState.AddState(MonsterStateType.Move, new MonsterMoveState(this));
		monsterState.AddState(MonsterStateType.Return, new MonsterReturnState(this));
		monsterState.AddState(MonsterStateType.Attack, new MonsterAttackState(this));
		monsterState.AddState(MonsterStateType.TakeHit, new MonsterTakeHitState(this));
		monsterState.AddState(MonsterStateType.Stunned, new MonsterStunnedState(this));
		monsterState.AddState(MonsterStateType.Die, new MonsterDieState(this));
		monsterState.Start(MonsterStateType.Idle);
	}

	private void Update()
	{
		monsterState.Update();
		currentState = monsterState.GetCurrentState();
		MoveCheck();
		AttackCheck();
		RetrunCheck();
		IdleCheck();

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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, moveDetectionRadius);
		Gizmos.DrawWireSphere(transform.position, attackDetectionRadius);
	}

	public void TakeDamage(int damage, bool isStunAttack)
	{
		hp -= damage;

		if (isStunAttack)
		{
			isStunned = true;
		}
		else
		{
			isTakeHit = true;
		}

		if (hp <= 0)
		{
			isDie = true;
		}
	}

	private void MoveCheck()
	{
		Collider[] players = Physics.OverlapSphere(transform.position, moveDetectionRadius, playerLayer);
		bool foundAlivePlayer = false;

		foreach (Collider playerCollider in players)
		{
			PlayerController player = playerCollider.GetComponent<PlayerController>();
			if (player != null && !player.IsDie)
			{
				foundAlivePlayer = true;
				target = player.transform;
				isMove = true;
				break;
			}
		}

		if (!foundAlivePlayer)
		{
			isMove = false;
			target = null;
		}
	}

	private void AttackCheck()
	{
		Collider[] players = Physics.OverlapSphere(transform.position, attackDetectionRadius, playerLayer);
		bool foundAlivePlayer = false;

		foreach (Collider playerCollider in players)
		{
			PlayerController player = playerCollider.GetComponent<PlayerController>();
			if (player != null && !player.IsDie)
			{
				foundAlivePlayer = true;
				break;
			}
		}

		if (foundAlivePlayer)
		{
			isAttack = true;
		}
		else
		{
			isAttack = false;
		}
	}

	private void RetrunCheck()
	{
		spawnDistance = Vector3.Distance(transform.position, spawnPos.position);

		if (spawnDistance > maxDistance || target == null)
		{
			isReturn = true;
		}
	}

	private void IdleCheck()
	{
		spawnDistance = Vector3.Distance(transform.position, spawnPos.position);

		if (spawnDistance < spawnDistanceMax)
		{
			isReturn = false;
			isIdle = true;
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

	public void Initialize()
	{
		hp = maxHp;
		isDie = false;
		isMove = false;
		animator.SetBool("Move", false);
		boxCollider.enabled = true;
		monsterState.ChangeState(MonsterStateType.Idle);
	}
}
