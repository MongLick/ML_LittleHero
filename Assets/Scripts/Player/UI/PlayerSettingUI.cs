using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUI : MonoBehaviour
{
	[SerializeField] Button logoutButton;
	[SerializeField] Button soundButton;
	[SerializeField] Button graphicButton;
	[SerializeField] Button closeButton;
	[SerializeField] LogoutUI logoutUI;
	[SerializeField] SoundUI soundUI;
	[SerializeField] GraphicsUI graphicsUI;
	[SerializeField] PlayerMenuUI playerMenuUI;

	private void Awake()
	{
		logoutButton.onClick.AddListener(Logout);
		soundButton.onClick.AddListener(Sound);
		graphicButton.onClick.AddListener(Graphics);
		closeButton.onClick.AddListener(Close);
		logoutButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		soundButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		graphicButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Logout()
	{
		logoutUI.gameObject.SetActive(true);
	}

	private void Sound()
	{
		soundUI.gameObject.SetActive(true);
	}

	private void Graphics()
	{
		graphicsUI.gameObject.SetActive(true);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
