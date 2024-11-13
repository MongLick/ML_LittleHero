using Firebase.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] TMP_InputField emailInputField;
	[SerializeField] TMP_InputField passInputField;
	[SerializeField] Button signUpButton;
	[SerializeField] Button loginButton;
	[SerializeField] Button resetPasswordButton;

	private void Awake()
	{
		InitializeButtonListeners();
	}

	private void InitializeButtonListeners()
	{
		signUpButton.onClick.AddListener(SignUp);
		loginButton.onClick.AddListener(Login);
		resetPasswordButton.onClick.AddListener(ResetPassword);

		AddButtonSound(signUpButton);
		AddButtonSound(loginButton);
		AddButtonSound(resetPasswordButton);
	}

	private void AddButtonSound(Button button)
	{
		button.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	public void SignUp()
	{
		panelController.SetActivePanel(PanelController.Panel.SignUp);
	}

	private void ResetPassword()
	{
		panelController.SetActivePanel(PanelController.Panel.Reset);
	}

	public void Login()
	{
		SetInteractable(false);
		AuthenticateUser(emailInputField.text, passInputField.text);
		ConnectToPhoton(emailInputField.text);
	}

	private void AuthenticateUser(string email, string password)
	{
		Manager.Fire.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
		{
			if (task.IsCanceled)
			{
				ShowMessage("작업이 취소되었습니다.");
			}
			else if (task.IsFaulted)
			{
				ShowMessage("작업이 실패되었습니다.");
			}
			else
			{
				HandleLoginSuccess();
			}

			SetInteractable(true);
		});
	}

	private void ConnectToPhoton(string email)
	{
		if (!string.IsNullOrEmpty(email))
		{
			PhotonNetwork.LocalPlayer.NickName = email;
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	private void HandleLoginSuccess()
	{
		if (Manager.Fire.Auth.CurrentUser.IsEmailVerified)
		{
			panelController.SetActivePanel(PanelController.Panel.Main);
		}
		else
		{
			panelController.SetActivePanel(PanelController.Panel.Verify);
		}
	}

	private void ShowMessage(string message)
	{
		panelController.ShowInfo(message);
	}

	private void SetInteractable(bool interactable)
	{
		emailInputField.interactable = interactable;
		passInputField.interactable = interactable;
		signUpButton.interactable = interactable;
		loginButton.interactable = interactable;
		resetPasswordButton.interactable = interactable;
	}
}
