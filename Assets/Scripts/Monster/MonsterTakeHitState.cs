using System.Collections;
using UnityEngine;
using static MonsterState;

public class MonsterTakeHitState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterTakeHitState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		if (monster.TakeHitRoutine != null)
		{
			monster.StopCoroutine(monster.TakeHitRoutine);
		}
		monster.TakeHitRoutine = monster.StartCoroutine(TakeHitCoroutine());
	}

	public override void Update()
	{
		if (monster.IsTakeHit)
		{
			return;
		}
		else if (monster.IsDie)
		{
			ChangeState(MonsterStateType.Die);
		}
		else if (monster.IsStunned)
		{
			ChangeState(MonsterStateType.Stunned);
		}
		else if (monster.IsAttack)
		{
			ChangeState(MonsterStateType.Attack);
		}
		else if (monster.IsMove)
		{
			ChangeState(MonsterStateType.Move);
		}
		else
		{
			ChangeState(MonsterStateType.Idle);
		}
	}

	private IEnumerator TakeHitCoroutine()
	{
		Manager.Sound.PlaySFX(Manager.Sound.MonsterTakeHit);
		monster.Attack.SetActive(false);
		monster.Animator.SetTrigger("TakeHit");
		yield return new WaitForSeconds(monster.TakeHitDelay);
		monster.IsTakeHit = false;
	}
}
