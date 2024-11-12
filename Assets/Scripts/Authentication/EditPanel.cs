using Firebase.Extensions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] TMP_InputField passInputField;
	[SerializeField] TMP_InputField confirmInputField;
	[SerializeField] Button passApplyButton;
	[SerializeField] Button backButton;
	[SerializeField] Button deleteButton;

	[Header("Specs")]
	[SerializeField] string newPassword;

	private void Awake()
	{
		AddButtonListeners();
	}

	private void AddButtonListeners()
	{
		passApplyButton.onClick.AddListener(PassApply);
		backButton.onClick.AddListener(Back);
		deleteButton.onClick.AddListener(Delete);

		AddSoundEffectListener(passApplyButton);
		AddSoundEffectListener(backButton);
		AddSoundEffectListener(deleteButton);
	}

	private void AddSoundEffectListener(Button button)
	{
		button.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void PassApply()
	{
		SetInteractable(false);

		if (passInputField.text != confirmInputField.text)
		{
			ShowMessage("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
			SetInteractable(true);
			return;
		}

		newPassword = passInputField.text;

		Manager.Fire.Auth.CurrentUser.UpdatePasswordAsync(newPassword)
			.ContinueWithOnMainThread(task => HandleTaskResult(task, "��й�ȣ ���� ����", "��й�ȣ ���� ����"));
	}

	private void Back()
	{
		panelController.SetActivePanel(PanelController.Panel.Main);
	}

	private void Delete()
	{
		SetInteractable(false);

		Manager.Fire.Auth.CurrentUser.DeleteAsync()
			.ContinueWithOnMainThread(task => HandleTaskResult(task, "���� ���� ����", "���� ���� ����", isDeleteOperation: true));
	}

	private void HandleTaskResult(Task task, string successMessage, string failureMessage, bool isDeleteOperation = false)
	{
		if (task.IsCanceled)
		{
			ShowMessage("�۾��� ��ҵǾ����ϴ�.");
		}
		else if (task.IsFaulted)
		{
			ShowMessage("�۾��� ���еǾ����ϴ�.");
		}
		else
		{
			ShowMessage(successMessage);
			if (isDeleteOperation)
			{
				panelController.SetActivePanel(PanelController.Panel.Login);
				Manager.Fire.Auth.SignOut();
			}
		}
		SetInteractable(true);
	}

	private void ShowMessage(string message)
	{
		panelController.ShowInfo(message);
	}

	private void SetInteractable(bool interactable)
	{
		passInputField.interactable = interactable;
		confirmInputField.interactable = interactable;
		passApplyButton.interactable = interactable;
		backButton.interactable = interactable;
		deleteButton.interactable = interactable;
	}
}
