using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] Button menuButton;
	[SerializeField] Button settingButton;
	[SerializeField] PlayerMenuUI playerMenu;
	[SerializeField] PlayerSettingUI playerSetting;

	private void Awake()
	{
		menuButton.onClick.AddListener(Menu);
		settingButton.onClick.AddListener(Setting);
	}

	private void Menu()
	{
		playerMenu.gameObject.SetActive(true);
		playerSetting.gameObject.SetActive(false);
	}

	private void Setting()
	{
		playerSetting.gameObject.SetActive(true);
		playerMenu.gameObject.SetActive(false);
	}
}
