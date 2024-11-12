using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LogoutUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button closeButton;
	[SerializeField] Button logoutButton;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		logoutButton.onClick.AddListener(Logout);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		logoutButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void Logout()
	{
		Manager.Scene.LoadScene("TitleScene");
		PhotonNetwork.Disconnect();
	}
}
