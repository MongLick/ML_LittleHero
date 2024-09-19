using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PlayerTakeHitState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerTakeHitState(PlayerController player)
	{
		this.player = player;
	}

	public override void Enter()
	{
		if (player.TakeHitRoutine != null)
		{
			player.StopCoroutine(player.TakeHitRoutine);
		}
		player.TakeHitRoutine = player.StartCoroutine(TakeHitCoroutine());
	}

	public override void Update()
	{
		if(player.IsTakeHit)
		{
			return;
		}
		else if (player.IsDie)
		{
			ChangeState(PlayerStateType.Die);
		}
		else if (player.IsBlock)
		{
			ChangeState(PlayerStateType.Block);
		}
		else if (player.IsAttack)
		{
			ChangeState(PlayerStateType.Attack);
		}
		else if (player.IsStunned)
		{
			ChangeState(PlayerStateType.Stunned);
		}
		else
		{
			ChangeState(PlayerStateType.Idle);
		}
	}

	public override void Exit()
	{
		player.IsTakeHit = false;
	}

	private IEnumerator TakeHitCoroutine()
	{
		if (player.IsBlock)
		{
			player.Animator.SetBool("TakeHitBlock", true);
		}
		else
		{
			player.Animator.SetTrigger("TakeHit");
		}
		yield return new WaitForSeconds(player.TakeHitDelay);
		player.Animator.SetBool("TakeHitBlock", false);
		player.IsTakeHit = false;
	}
}
