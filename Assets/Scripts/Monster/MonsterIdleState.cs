using static MonsterState;

public class MonsterIdleState : BaseState<MonsterStateType>
{
	private MonsterController monster;

	public MonsterIdleState(MonsterController monster)
	{
		this.monster = monster;
	}

	public override void Enter()
	{
		monster.IsReturn = false;
		monster.IsIdle = false;
		monster.BoxCollider.enabled = true;
	}

	public override void Update()
	{
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
		else if (monster.IsMove)
		{
			ChangeState(MonsterStateType.Move);
		}
	}
}
