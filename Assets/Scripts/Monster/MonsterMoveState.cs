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

	public override void Update()
	{
		monster.Agent.SetDestination(monster.Target.position);

		monster.SpawnDistance = Vector3.Distance(monster.transform.position, monster.SpawnPos.position);

		if (monster.SpawnDistance > monster.MaxDistance)
		{
			ChangeState(MonsterStateType.Return);
		}	
	}
}
