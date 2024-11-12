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
		monster.Animator.SetBool("Move", true);
		monster.BoxCollider.enabled = false;
		monster.Agent.SetDestination(monster.SpawnPos.position);
	}

	public override void Update()
	{
		if (monster.IsIdle)
		{
			ChangeState(MonsterStateType.Idle);
		}
	}

	public override void Exit()
	{
		monster.Animator.SetBool("Move", false);
	}
}
