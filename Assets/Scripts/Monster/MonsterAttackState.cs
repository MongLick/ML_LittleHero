using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterAttackState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterAttackState(MonsterController monster)
	{
		this.monster = monster;
	}
}
