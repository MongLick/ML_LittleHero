using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterState;

public class MonsterStunnedState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterStunnedState(MonsterController monster)
	{
		this.monster = monster;
	}
}
