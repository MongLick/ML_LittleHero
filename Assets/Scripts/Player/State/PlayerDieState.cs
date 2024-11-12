using System.Collections;
using UnityEngine;
using static PlayerState;

public class PlayerDieState : BaseState<PlayerStateType>
{
	private PlayerController player;

	public PlayerDieState(PlayerController player)
	{
		this.player = player;
	}

	public override void Enter()
	{
		player.Animator.SetTrigger("Die");
		player.GameOverCamera.Priority = 100;
		player.PlayerInput.enabled = false;
		player.StartCoroutine(DieCoroutine());
	}

	private IEnumerator DieCoroutine()
	{
		yield return new WaitForSeconds(3f);
		Manager.Scene.LoadScene("LittleForestScene");
	}
}
