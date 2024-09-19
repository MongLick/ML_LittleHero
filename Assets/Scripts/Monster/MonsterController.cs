using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using static MonsterState;
using static PlayerState;

public class MonsterController : MonoBehaviour, IDamageable
{
	[SerializeField] NavMeshAgent agent;
	public NavMeshAgent Agent { get { return agent; } }
	[SerializeField] Transform target;
	public Transform Target { get { return target; } }
	[SerializeField] Transform spawnPos;
	public Transform SpawnPos { get { return spawnPos; } }
	[SerializeField] StateMachine<MonsterStateType> monsterState;
	[SerializeField] MonsterStateType currentState;
	[SerializeField] int damage;
	[SerializeField] int hp;
	[SerializeField] float detectionRadius;
	public float DetectionRadius { get { return detectionRadius; } set { detectionRadius = value; } }
	[SerializeField] float maxDistance;
	public float MaxDistance { get { return maxDistance; } }
	[SerializeField] float spawnDistance;
	public float SpawnDistance { get {return spawnDistance; } set { spawnDistance = value; } }
	[SerializeField] LayerMask playerLayer;
	public LayerMask PlayerLayer { get { return playerLayer; }}

	private bool isMove;
	public bool IsMove { get { return isMove; } set { isMove = value; } }
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
	}

	private void OnTriggerEnter(Collider other)
	{
		if (playerLayer.Contain(other.gameObject.layer))
		{
			IDamageable damageable = other.GetComponent<IDamageable>();
			if(damageable != null)
			{
				damageable.TakeDamage(damage, true);
			}
		}
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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}
