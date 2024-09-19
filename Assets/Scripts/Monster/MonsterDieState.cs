using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterDieState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterDieState(MonsterController monster)
	{
		this.monster = monster;
	}
}
