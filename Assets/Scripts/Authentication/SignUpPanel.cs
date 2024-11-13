using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] TMP_InputField emailInputField;
	[SerializeField] TMP_InputField passInputField;
	[SerializeField] TMP_InputField confirmInputField;
	[SerializeField] Button cancelButton;
	[SerializeField] Button signUpButton;

	private void Awake()
	{
		AddButtonListeners();
	}

	private void AddButtonListeners()
	{
		cancelButton.onClick.AddListener(Cancel);
		signUpButton.onClick.AddListener(SignUp);
		cancelButton.onClick.AddListener(PlayButtonSFX);
		signUpButton.onClick.AddListener(PlayButtonSFX);
	}

	private void PlayButtonSFX()
	{
		Manager.Sound.ButtonSFX();
	}

	public void SignUp()
	{
		SetInteractable(false);

		string email = emailInputField.text;
		string password = passInputField.text;
		string confirm = confirmInputField.text;

		if (password != confirm)
		{
			ShowMessage("��й�ȣ�� ��ġ���� �ʽ��ϴ�");
			SetInteractable(true);
			return;
		}

		Manager.Fire.Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
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

			ShowMessage("�̸��ϰ� ��й�ȣ�� ���ο� ����ڰ� ���������� �����Ǿ����ϴ�.");
			panelController.SetActivePanel(PanelController.Panel.Login);
			SetInteractable(true);
		});
	}

	private void ShowMessage(string message)
	{
		panelController.ShowInfo(message);
	}

	public void Cancel()
	{
		panelController.SetActivePanel(PanelController.Panel.Login);
	}

	private void SetInteractable(bool interactable)
	{
		emailInputField.interactable = interactable;
		passInputField.interactable = interactable;
		confirmInputField.interactable = interactable;
		cancelButton.interactable = interactable;
		signUpButton.interactable = interactable;
	}
}
