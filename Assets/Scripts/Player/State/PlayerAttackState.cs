using System.Collections;
using UnityEngine;
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
		if (player.IsBlock)
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
		Manager.Sound.PlaySFX(Manager.Sound.PlayerAttack);
		if (!player.IsSkiilAttack)
		{
			int attackIndex = Random.Range(0, 2);
			switch (attackIndex)
			{
				case 0:
					player.Animator.SetTrigger("Attack1");
					break;
				case 1:
					player.Animator.SetTrigger("Attack2");
					break;
			}
		}
		else
		{
			player.Animator.SetTrigger("Attack3");
			Vector3 effectPosition = player.transform.position + player.transform.forward * player.SkillOffset;
			Manager.Game.PoolEffect.SpawnEffects(player.SkillName, effectPosition);
		}

		yield return new WaitForSeconds(player.AttackDelay);
		player.IsAttackCooltime = true;
		player.IsAttack = false;
		player.IsSkiilAttack = false;
		ChangeState(PlayerStateType.Idle);
	}
}
