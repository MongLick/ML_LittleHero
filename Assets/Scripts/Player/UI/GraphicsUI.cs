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
		qualityDropdown.onValueChanged.AddListener(QualitySetting);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void QualitySetting(int level)
	{
		QualitySettings.SetQualityLevel(level);
		Manager.Fire.SetQualitySetting(level);
	}
}
