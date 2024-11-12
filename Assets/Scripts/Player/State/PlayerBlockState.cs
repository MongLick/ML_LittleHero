using System.Collections;
using static PlayerState;

public class PlayerBlockState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerBlockState(PlayerController player)
	{
		this.player = player;
	}

	public override void Enter()
	{
		if (player.BlockRoutine != null)
		{
			player.StopCoroutine(player.BlockRoutine);
		}
		player.BlockRoutine = player.StartCoroutine(BlockCoroutine());
	}

	public override void Update()
	{
		if (player.IsTakeHit)
		{
			ChangeState(PlayerStateType.TakeHit);
		}
		else if (player.IsStunned)
		{
			ChangeState(PlayerStateType.Stunned);
		}
		else if (player.IsAttack)
		{
			ChangeState(PlayerStateType.Attack);
		}
		else if (!player.IsBlock)
		{
			ChangeState(PlayerStateType.Idle);
		}
	}

	private IEnumerator BlockCoroutine()
	{
		while (player.IsBlock)
		{
			player.Animator.SetBool("Block", true);
			yield return null;
		}
		player.Animator.SetBool("Block", false);
		yield return null;
	}
}
