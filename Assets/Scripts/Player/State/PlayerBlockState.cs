using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PlayerBlockState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerBlockState(PlayerController player)
	{
		this.player = player;
	}
}
