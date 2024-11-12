using System.Collections;
using UnityEngine;
using static MonsterState;

public class MonsterStunnedState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterStunnedState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		if (monster.StunnedRoutine != null)
		{
			monster.StopCoroutine(monster.StunnedRoutine);
		}
		monster.StunnedRoutine = monster.StartCoroutine(StunnedCoroutine());
	}

	public override void Update()
	{
		if (monster.IsStunned)
		{
			return;
		}
		else if (monster.IsDie)
		{
			ChangeState(MonsterStateType.Die);
		}
		else if (monster.IsTakeHit)
		{
			ChangeState(MonsterStateType.TakeHit);
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
		else
		{
			ChangeState(MonsterStateType.Idle);
		}
	}

	private IEnumerator StunnedCoroutine()
	{
		Manager.Sound.PlaySFX(Manager.Sound.MonsterTakeHit);
		monster.Animator.SetBool("Stunned", true);
		monster.MonsterCon.enabled = false;
		yield return new WaitForSeconds(monster.StunnedDelay);
		monster.Animator.SetBool("Stunned", false);
		monster.MonsterCon.enabled = true;
		monster.IsStunned = false;
	}
}
