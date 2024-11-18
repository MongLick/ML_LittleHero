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
		player.Animator.SetBool("Die", true);
		player.GameOverCamera.Priority = player.CameraPriority;
		player.PlayerInput.enabled = false;
		player.IsAutoAttack = false;
		player.StartCoroutine(DieCoroutine());
	}

	private IEnumerator DieCoroutine()
	{
		yield return FadeOut();
		yield return new WaitForSeconds(player.DieDelay);
		Manager.Data.UserData.Health = Manager.Data.UserData.maxHealth;
		Manager.Data.UserData.Mana = Manager.Data.UserData.maxMana;

		player.Controller.enabled = false;
		Vector3 spawnPoint = new Vector3(-25, 4, -7);
		player.transform.position = spawnPoint;
		player.Controller.enabled = true;

		yield return new WaitForSeconds(0.1f);
		player.GameOverCamera.Priority = 0;
		player.IsDie = false;
		player.Animator.SetBool("Die", false);
		yield return FadeIn();
		player.PlayerInput.enabled = true;
		ChangeState(PlayerStateType.Idle);
	}

	IEnumerator FadeOut()
	{
		player.FadeImage.gameObject.SetActive(true);

		float rate = 0;
		Color fadeOutColor = new Color(player.FadeImage.color.r, player.FadeImage.color.g, player.FadeImage.color.b, 1f);
		Color fadeInColor = new Color(player.FadeImage.color.r, player.FadeImage.color.g, player.FadeImage.color.b, 0f);

		while (rate <= 1)
		{
			rate += Time.deltaTime / player.FadeTime;
			player.FadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
			yield return null;
		}
	}

	IEnumerator FadeIn()
	{
		float rate = 0;
		Color fadeOutColor = new Color(player.FadeImage.color.r, player.FadeImage.color.g, player.FadeImage.color.b, 1f);
		Color fadeInColor = new Color(player.FadeImage.color.r, player.FadeImage.color.g, player.FadeImage.color.b, 0f);

		while (rate <= 1)
		{
			rate += Time.deltaTime / player.FadeTime;
			player.FadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
			yield return null;
		}

		player.FadeImage.gameObject.SetActive(false);
	}
}
