using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] Button menuButton;
	[SerializeField] Button settingButton;
	[SerializeField] PlayerMenuUI playerMenuUI;
	[SerializeField] PlayerSettingUI playerSettingUI;

	private void Awake()
	{
		menuButton.onClick.AddListener(Menu);
		settingButton.onClick.AddListener(Setting);
	}

	private void Menu()
	{
		playerMenuUI.gameObject.SetActive(true);
		playerSettingUI.gameObject.SetActive(false);
	}

	private void Setting()
	{
		playerSettingUI.gameObject.SetActive(true);
		playerMenuUI.gameObject.SetActive(false);
	}
}
