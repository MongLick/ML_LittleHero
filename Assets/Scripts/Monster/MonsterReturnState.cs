using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterReturnState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterReturnState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		monster.Agent.SetDestination(monster.SpawnPos.position);
	}

	public override void Update()
	{
		monster.SpawnDistance = Vector3.Distance(monster.transform.position, monster.SpawnPos.position);

		if (monster.SpawnDistance < 0.1f)
		{
			ChangeState(MonsterStateType.Idle);
		}
	}
}
