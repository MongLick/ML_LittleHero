using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button closeButton;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
