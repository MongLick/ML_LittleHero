using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PlayerStunnedState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerStunnedState(PlayerController player)
	{
		this.player = player;
	}
}
