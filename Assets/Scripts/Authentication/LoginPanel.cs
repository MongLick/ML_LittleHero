using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField;

    [SerializeField] Button signUpButton;
    [SerializeField] Button loginButton;
    [SerializeField] Button resetPasswordButton;

    private void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        loginButton.onClick.AddListener(Login);
        resetPasswordButton.onClick.AddListener(ResetPassword);
		signUpButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		loginButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		resetPasswordButton.onClick.AddListener(Manager.Sound.ButtonSFX);
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

		string email = emailInputField.text;
        string password = passInputField.text;

        Manager.Fire.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                panelController.ShowInfo("SignInWithEmailAndPasswordAsync canceled");
                SetInteractable(true);
				return;
            }
            else if (task.IsFaulted)
            {
				panelController.ShowInfo($"SignInWithEmailAndPasswordAsync failed : {task.Exception.Message}");
				SetInteractable(true);
				return;
			}

            if(Manager.Fire.Auth.CurrentUser.IsEmailVerified)
            {
                panelController.SetActivePanel(PanelController.Panel.Main);
            }
            else
            {
				panelController.SetActivePanel(PanelController.Panel.Verify);
			}

			SetInteractable(true);
		});
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
