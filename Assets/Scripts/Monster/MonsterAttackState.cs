using System.Collections;
using UnityEngine;
using static MonsterState;

public class MonsterAttackState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterAttackState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		if (monster.IsAttackCooltime)
		{
			ChangeState(MonsterStateType.Idle);
			return;
		}

		if (monster.AttackRoutine != null)
		{
			monster.StopCoroutine(monster.AttackRoutine);
		}
		monster.AttackRoutine = monster.StartCoroutine(AttackCoroutine());
	}

	public override void Update()
	{
		if (monster.Target != null)
		{
			Vector3 direction = (monster.Target.position - monster.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, lookRotation, Time.deltaTime * 5f);
		}

		if (monster.IsAttack)
		{
			return;
		}

		if (monster.IsTakeHit)
		{
			ChangeState(MonsterStateType.TakeHit);
		}
		else if (monster.IsStunned)
		{
			ChangeState(MonsterStateType.Stunned);
		}
		else if (monster.IsReturn)
		{
			ChangeState(MonsterStateType.Return);
		}
		else if (monster.IsAttack)
		{
			ChangeState(MonsterStateType.Attack);
		}
		else if (monster.IsMove)
		{
			ChangeState(MonsterStateType.Move);
		}
		else if (monster.IsIdle)
		{
			ChangeState(MonsterStateType.Idle);
		}
	}

	private IEnumerator AttackCoroutine()
	{
		int attackIndex = Random.Range(0, 2);

		switch (attackIndex)
		{
			case 0:
				monster.Animator.SetTrigger("Attack1");
				break;
			case 1:
				monster.Animator.SetTrigger("Attack2");
				break;
		}

		yield return new WaitForSeconds(monster.AttackDelay);
		monster.IsAttackCooltime = true;
		monster.IsAttack = false;
		ChangeState(MonsterStateType.Idle);
	}
}
