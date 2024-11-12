using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PanelController : MonoBehaviourPunCallbacks
{
	public enum Panel { Login, SignUp, Verify, Reset, Main, Edit, Hero }
	private ClientState state;

	[Header("Components")]
	[SerializeField] InfoPanel infoPanel;
	[SerializeField] LoginPanel loginPanel;
	[SerializeField] SignUpPanel signUpPanel;
	[SerializeField] ResetPanel resetPanel;
	[SerializeField] VerifyPanel verifyPanel;
	[SerializeField] MainPanel mainPanel;
	[SerializeField] EditPanel editPanel;
	[SerializeField] HeroPanel heroPanel;
	[SerializeField] ChoicePanel choicePanel;

	private void Update()
	{
		ClientState curState = PhotonNetwork.NetworkClientState;
		if (state == curState)
		{
			return;
		}
		state = curState;
	}

	private void Start()
	{
		SetActivePanel(Panel.Login);
	}

	public void SetActivePanel(Panel panel)
	{
		loginPanel.gameObject.SetActive(panel == Panel.Login);
		signUpPanel.gameObject.SetActive(panel == Panel.SignUp);
		resetPanel.gameObject.SetActive(panel == Panel.Reset);
		mainPanel.gameObject.SetActive(panel == Panel.Main);
		editPanel.gameObject.SetActive(panel == Panel.Edit);
		verifyPanel.gameObject.SetActive(panel == Panel.Verify);
		heroPanel.gameObject.SetActive(panel == Panel.Hero);
	}

	public void ShowInfo(string message)
	{
		infoPanel.DisplayMessage(message);
	}

	public void ShowChoice(bool isLeft)
	{
		choicePanel.ShowChoice(isLeft);
	}
}
