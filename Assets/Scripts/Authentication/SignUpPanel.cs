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
		cancelButton.onClick.AddListener(Cancel);
		signUpButton.onClick.AddListener(SignUp);
		cancelButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		signUpButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	public void SignUp()
	{
		SetInteractable(false);

		string email = emailInputField.text;
		string password = passInputField.text;
		string confirm = confirmInputField.text;

		if (password != confirm)
		{
			panelController.ShowInfo("Password doesn't matched");
			SetInteractable(true);
			return;
		}

		Manager.Fire.Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled)
			{
				panelController.ShowInfo("CreateUserWithEmailAndPasswordAsync canceled");
				SetInteractable(true);
				return;
			}
			else if (task.IsFaulted)
			{
				panelController.ShowInfo($"CreateUserWithEmailAndPasswordAsync failed : {task.Exception.Message}");
				SetInteractable(true);
				return;
			}

			panelController.ShowInfo("CreateUserWithEmailAndPasswordAsync success");
			panelController.SetActivePanel(PanelController.Panel.Login);
			SetInteractable(true);
		});
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
