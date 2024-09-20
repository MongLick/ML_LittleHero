using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterMoveState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterMoveState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		monster.Animator.SetBool("Move", true);
	}

	public override void Update()
	{
		monster.Agent.SetDestination(monster.Target.position);

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
	}

	public override void Exit()
	{
		monster.Animator.SetBool("Move", false);
	}
}
