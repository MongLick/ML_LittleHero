using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VerifyPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] Button logoutButton;
	[SerializeField] Button sendButton;
	[SerializeField] TMP_Text sendButtonText;

	[Header("Specs")]
	[SerializeField] int sendMailCooltime;

	private void Awake()
	{
		AddButtonListeners();
	}

	private void OnEnable()
	{
		if (Manager.Fire.Auth == null)
		{
			return;
		}

		StartCoroutine(VerifyCheckCoroutine());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void AddButtonListeners()
	{
		logoutButton.onClick.AddListener(Logout);
		sendButton.onClick.AddListener(SendVerifyMail);
		logoutButton.onClick.AddListener(PlayButtonSFX);
		sendButton.onClick.AddListener(PlayButtonSFX);
	}

	private void PlayButtonSFX()
	{
		Manager.Sound.ButtonSFX();
	}

	private void Logout()
	{
		Manager.Fire.Auth.SignOut();
		panelController.SetActivePanel(PanelController.Panel.Login);
	}

	private void SendVerifyMail()
	{
		Manager.Fire.Auth.CurrentUser.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled)
			{
				ShowMessage("작업이 취소되었습니다.");
				return;
			}
			else if (task.IsFaulted)
			{
				ShowMessage("작업이 실패되었습니다.");
				return;
			}

			ShowMessage("이메일 인증 메일이 성공적으로 발송되었습니다.");
		});
	}

	private void ShowMessage(string message)
	{
		panelController.ShowInfo(message);
	}

	private IEnumerator VerifyCheckCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(3f);

			Manager.Fire.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
			{
				if (task.IsCanceled)
				{
					ShowMessage("작업이 취소되었습니다.");
					return;
				}
				else if (task.IsFaulted)
				{
					ShowMessage("작업이 실패되었습니다.");
					return;
				}

				if (Manager.Fire.Auth.CurrentUser.IsEmailVerified)
				{
					panelController.SetActivePanel(PanelController.Panel.Main);
				}
			});
		}
	}
}
