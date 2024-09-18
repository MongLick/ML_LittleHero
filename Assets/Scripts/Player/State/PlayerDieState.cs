using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class PlayerDieState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerDieState(PlayerController player)
	{
		this.player = player;
	}
}
