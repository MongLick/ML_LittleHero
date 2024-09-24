using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text emailText;

    [SerializeField] Button logoutButton;
    [SerializeField] Button editButton;

    private void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        editButton.onClick.AddListener(Edit);
    }

	private void OnEnable()
	{
		if(Manager.Fire.Auth == null)
        {
            return;
        }

        nameText.text = Manager.Fire.Auth.CurrentUser.DisplayName;
        emailText.text = Manager.Fire.Auth.CurrentUser.Email;
	}

	private void Logout()
    {
        Manager.Fire.Auth.SignOut();
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void Edit()
    {
        panelController.SetActivePanel(PanelController.Panel.Edit);
    }
}
