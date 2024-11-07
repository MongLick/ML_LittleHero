using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VerifyPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] Button logoutButton;
    [SerializeField] Button sendButton;
    [SerializeField] TMP_Text sendButtonText;

    [SerializeField] int sendMailCooltime;

    private void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        sendButton.onClick.AddListener(SendVerifyMail);
		logoutButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		sendButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void OnEnable()
	{
        if(Manager.Fire.Auth == null)
        {
            return;
        }

		StartCoroutine(VerifyCheckCoroutine());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
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
            if(task.IsCanceled)
            {
                panelController.ShowInfo("SendEmailVerificationAsync canceled");
                return;
            }
            else if(task.IsFaulted)
            {
				panelController.ShowInfo($"SendEmailVerificationAsync failed : {task.Exception.Message}");
                return;
			}

            panelController.ShowInfo("SendEmailVerificationAsync succes");
        });
    }

    private IEnumerator VerifyCheckCoroutine()
	{
        while(true)
        {
            yield return new WaitForSeconds(3f);

            Manager.Fire.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled)
                {
                    panelController.ShowInfo("ReloadAsync canceled");
                    return;
                }
                else if(task.IsFaulted)
                {
                    panelController.ShowInfo($"ReloadAsync failed : {task.Exception.Message}");
                    return;
                }

                if(Manager.Fire.Auth.CurrentUser.IsEmailVerified)
                {
                    panelController.SetActivePanel(PanelController.Panel.Main);
                }
            });
        }
    }
}
