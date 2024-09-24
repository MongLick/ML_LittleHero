using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField passInputField;
    [SerializeField] TMP_InputField confirmInputField;

    [SerializeField] Button nameApplyButton;
    [SerializeField] Button passApplyButton;
    [SerializeField] Button backButton;
    [SerializeField] Button deleteButton;

    private void Awake()
    {
        nameApplyButton.onClick.AddListener(NameApply);
        passApplyButton.onClick.AddListener(PassApply);
        backButton.onClick.AddListener(Back);
        deleteButton.onClick.AddListener(Delete);
    }

    private void NameApply()
    {
        SetInteractable(false);

        UserProfile profile = new UserProfile();
        profile.DisplayName = nameInputField.text;

        Manager.Fire.Auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                panelController.ShowInfo("UpdateUserProfileAsync canceled");
				SetInteractable(true);
				return;
            }
            else if(task.IsFaulted)
            {
                panelController.ShowInfo($"UpdateUserProfileAsync failed : {task.Exception.Message}");
				SetInteractable(true);
				return;
            }

            panelController.ShowInfo("UpdateUserProfileAsync success");
			SetInteractable(true);
		});
	}

    private void PassApply()
    {
		SetInteractable(false);

        if(passInputField.text != confirmInputField.text)
        {
            panelController.ShowInfo("password doesn't matched");
			SetInteractable(true);
            return;
		}

        string newPassword = passInputField.text;

        Manager.Fire.Auth.CurrentUser.UpdatePasswordAsync(newPassword).ContinueWithOnMainThread(task =>
        {
			if (task.IsCanceled)
			{
				panelController.ShowInfo("UpdatePasswordAsync canceled");
				SetInteractable(true);
				return;
			}
			else if (task.IsFaulted)
			{
				panelController.ShowInfo($"UpdatePasswordAsync failed : {task.Exception.Message}");
				SetInteractable(true);
				return;
			}

			panelController.ShowInfo("UpdatePasswordAsync success");
			SetInteractable(true);
		});
	}

    private void Back()
    {
        panelController.SetActivePanel(PanelController.Panel.Main);
    }

    private void Delete()
    {
        SetInteractable(false);
		Manager.Fire.Auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
			if (task.IsCanceled)
			{
				panelController.ShowInfo("DeleteAsync canceled");
				SetInteractable(true);
				return;
			}
			else if (task.IsFaulted)
			{
				panelController.ShowInfo($"DeleteAsync failed : {task.Exception.Message}");
				SetInteractable(true);
				return;
			}

			panelController.ShowInfo("DeleteAsync success");
            panelController.SetActivePanel(PanelController.Panel.Login);
			SetInteractable(true);
            Manager.Fire.Auth.SignOut();
		});
    }

    private void SetInteractable(bool interactable)
    {
        nameInputField.interactable = interactable;
        passInputField.interactable = interactable;
        confirmInputField.interactable = interactable;
        nameApplyButton.interactable = interactable;
        passApplyButton.interactable = interactable;
        backButton.interactable = interactable;
        deleteButton.interactable = interactable;
    }
}
