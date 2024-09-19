using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterIdleState : BaseState<MonsterStateType>
{
    private MonsterController monster;

    public MonsterIdleState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Update()
	{
		CheckPlayer();

		if (monster.IsMove)
		{
			ChangeState(MonsterStateType.Move);
		}
		else if (monster.IsAttack)
		{
			ChangeState(MonsterStateType.Attack);
		}
		else if (monster.IsTakeHit)
		{
			ChangeState(MonsterStateType.TakeHit);
		}
	}

	private void CheckPlayer()
	{
		Collider[] player = Physics.OverlapSphere(monster.transform.position, monster.DetectionRadius, monster.PlayerLayer);
		if (player != null)
		{
			monster.IsMove = player.Length > 0;
		}
	}
}
