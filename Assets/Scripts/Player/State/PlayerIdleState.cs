using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PlayerIdleState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerIdleState(PlayerController player)
	{
		this.player = player;
	}

	public override void Update()
	{
		if(player.IsAttack)
		{
			ChangeState(PlayerStateType.Attack);
		}
		else if(player.IsTakeHit)
		{
			ChangeState(PlayerStateType.TakeHit);
		}
		else if(player.IsBlock)
		{
			ChangeState(PlayerStateType.Block);
		}
		else if(player.IsStunned)
		{
			ChangeState(PlayerStateType.Stunned);
		}
		else if(player.IsDie)
		{
			ChangeState(PlayerStateType.Die);
		}
	}
}
