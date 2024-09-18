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
}
