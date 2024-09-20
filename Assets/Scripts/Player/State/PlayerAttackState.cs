using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static MonsterState;
using static PlayerState;

public class PlayerAttackState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerAttackState(PlayerController player)
	{
		this.player = player;
	}

	public override void Enter()
	{
		if (player.IsAttackCooltime)
		{
			ChangeState(PlayerStateType.Idle);
			return;
		}
		if(player.IsBlock)
		{
			ChangeState(PlayerStateType.Block);
			return;
		}

		if (player.AttackRoutine != null)
		{
			player.StopCoroutine(player.AttackRoutine);
		}
		player.AttackRoutine = player.StartCoroutine(AttackCoroutine());
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
		else if (player.IsBlock)
		{
			ChangeState(PlayerStateType.Block);
		}
		else if (!player.IsAttack)
		{
			ChangeState(PlayerStateType.Idle);
		}
	}

	public override void Exit()
	{
		player.IsAttack = false;
	}

	private IEnumerator AttackCoroutine()
	{
		int attackIndex = Random.Range(0, 3);

		switch (attackIndex)
		{
			case 0:
				player.Animator.SetTrigger("Attack1");
				break;
			case 1:
				player.Animator.SetTrigger("Attack2");
				break;
			case 2:
				player.Animator.SetTrigger("Attack3");
				break;
		}

		yield return new WaitForSeconds(player.AttackDelay);
		player.IsAttackCooltime = true;
		player.IsAttack = false;
		ChangeState(PlayerStateType.Idle);
	}
}
