using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] TMP_Text infoText;
	[SerializeField] Button closeButton;

	private void Awake()
	{
		AddCloseButtonListeners();
	}

	private void AddCloseButtonListeners()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	public void DisplayMessage(string message)
	{
		infoText.text = message;
		gameObject.SetActive(true);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
