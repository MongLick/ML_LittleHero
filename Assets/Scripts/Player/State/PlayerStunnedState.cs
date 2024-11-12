using System.Collections;
using UnityEngine;
using static PlayerState;

public class PlayerStunnedState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerStunnedState(PlayerController player)
	{
		this.player = player;
	}

	public override void Enter()
	{
		if (player.StunnedRoutine != null)
		{
			player.StopCoroutine(player.StunnedRoutine);
		}
		player.StunnedRoutine = player.StartCoroutine(StunnedCoroutine());
	}

	public override void Update()
	{
		if (player.IsStunned)
		{
			return;
		}
		else if (player.IsDie)
		{
			ChangeState(PlayerStateType.Die);
		}
		else if (player.IsAttack)
		{
			ChangeState(PlayerStateType.Attack);
		}
		else if (player.IsBlock)
		{
			ChangeState(PlayerStateType.Block);
		}
		else if (player.IsTakeHit)
		{
			ChangeState(PlayerStateType.TakeHit);
		}
		else
		{
			ChangeState(PlayerStateType.Idle);
		}
	}

	public override void Exit()
	{
		player.IsStunned = false;
	}

	private IEnumerator StunnedCoroutine()
	{
		player.IsAttack = false;
		player.Animator.SetBool("Stunned", true);

		player.PlayerInput.enabled = false;

		yield return new WaitForSeconds(player.StunnedDelay);

		player.Animator.SetBool("Stunned", false);

		player.PlayerInput.enabled = true;
		player.IsStunned = false;
	}
}
