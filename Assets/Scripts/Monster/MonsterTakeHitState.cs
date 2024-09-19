using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterTakeHitState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterTakeHitState(MonsterController monster)
	{
		this.monster = monster;
	}
}
