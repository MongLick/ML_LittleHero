using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button closeButton;
	[SerializeField] TMP_Dropdown qualityDropdown;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(PlayButtonSFX);
		qualityDropdown.onValueChanged.AddListener(QualitySetting);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void PlayButtonSFX()
	{
		Manager.Sound.ButtonSFX();
	}

	private void QualitySetting(int level)
	{
		QualitySettings.SetQualityLevel(level);
		Manager.Fire.SetQualitySetting(level);
	}
}
