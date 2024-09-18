using System.Collections;
using System.Collections.Generic;
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
		if (player.AttackRoutine != null)
		{
			player.StopCoroutine(player.AttackRoutine);
		}
		player.AttackRoutine = player.StartCoroutine(AttackCoroutine());
	}

	public override void Update()
	{
		if(player.IsTakeHit)
		{
			ChangeState(PlayerStateType.TakeHit);
		}
		else if(player.IsStunned)
		{
			ChangeState(PlayerStateType.Stunned);
		}
		else if(player.IsDie)
		{
			ChangeState(PlayerStateType.Die);
		}
		else if(!player.IsAttack)
		{
			ChangeState(PlayerStateType.Idle);
		}
    }

	private IEnumerator AttackCoroutine()
	{
		Debug.Log(1);
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

		player.IsAttack = false;
		Debug.Log(2);
	}
}
