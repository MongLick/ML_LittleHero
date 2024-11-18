using System.Collections;
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
		monster.BoxCollider.enabled = false;
		monster.Attack.SetActive(false);

		if (monster.LastAttacker != null && monster.LastAttacker.PhotonView.IsMine)
		{
			Manager.Data.UserData.Gold += monster.Gold;
			Manager.Fire.UpdateGoldInDatabase(monster.Gold);
		}

		yield return new WaitForSeconds(monster.DieDelay);
		Manager.Fire.OnMonsterDie(monster.Type);
		monster.OnDieEvent?.Invoke(monster);
		monster.PooledObject.Release();
	}
}
