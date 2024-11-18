using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] TMP_InputField emailInputField;
	[SerializeField] Button sendButton;
	[SerializeField] Button cancelButton;

	private void Awake()
	{
		AddButtonListeners();
	}

	private void AddButtonListeners()
	{
		sendButton.onClick.AddListener(SendResetMail);
		cancelButton.onClick.AddListener(Cancel);
		sendButton.onClick.AddListener(PlayButtonSFX);
		cancelButton.onClick.AddListener(PlayButtonSFX);
	}

	private void PlayButtonSFX()
	{
		Manager.Sound.ButtonSFX();
	}

	private void SendResetMail()
	{
		SetInteractable(false);

		string email = emailInputField.text;
		Manager.Fire.Auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled)
			{
				ShowMessage("�۾��� ��ҵǾ����ϴ�.");
				SetInteractable(true);
				return;
			}
			else if (task.IsFaulted)
			{
				ShowMessage("�۾��� ���еǾ����ϴ�.");
				SetInteractable(true);
				return;
			}

			ShowMessage("��й�ȣ �缳���� �߼۵Ǿ����ϴ�");
			panelController.SetActivePanel(PanelController.Panel.Login);
			SetInteractable(true);
		});
	}

	private void Cancel()
	{
		panelController.SetActivePanel(PanelController.Panel.Login);
	}

	private void ShowMessage(string message)
	{
		panelController.ShowInfo(message);
	}

	private void SetInteractable(bool interactable)
	{
		emailInputField.interactable = interactable;
		sendButton.interactable = interactable;
		cancelButton.interactable = interactable;
	}
}
