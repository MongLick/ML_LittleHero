using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUI : MonoBehaviour
{
	[Header("Components")]
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
		AddButtonListeners();
	}

	private void AddButtonListeners()
	{
		logoutButton.onClick.AddListener(() => OpenUI(logoutUI));
		soundButton.onClick.AddListener(() => OpenUI(soundUI));
		graphicButton.onClick.AddListener(() => OpenUI(graphicsUI));
		closeButton.onClick.AddListener(Close);

		PlayButtonSFX();
	}

	private void PlayButtonSFX()
	{
		logoutButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		soundButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		graphicButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void OpenUI(MonoBehaviour ui)
	{
		ui.gameObject.SetActive(true);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
