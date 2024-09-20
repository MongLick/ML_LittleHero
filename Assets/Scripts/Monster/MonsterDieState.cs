using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MonsterState;

public class MonsterDieState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterDieState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		if (monster.DieRoutine != null)
		{
			monster.StopCoroutine(monster.DieRoutine);
		}
		monster.DieRoutine = monster.StartCoroutine(DieCoroutine());
	}

	private IEnumerator DieCoroutine()
	{
		monster.Animator.SetTrigger("Die");
		yield return new WaitForSeconds(monster.DieDelay);
		monster.gameObject.SetActive(false);
	}
}
