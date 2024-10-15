using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] Button menuButton;
	[SerializeField] Button settingButton;
	[SerializeField] PlayerMenuUI playerMenuUI;
	[SerializeField] PlayerSettingUI playerSettingUI;
	[SerializeField] TMP_Text healthText;
	[SerializeField] TMP_Text manaText;
	[SerializeField] Slider healthSlider;
	[SerializeField] Slider manaSlider;

	private void Awake()
	{
		menuButton.onClick.AddListener(Menu);
		settingButton.onClick.AddListener(Setting);
	}

	private void Start()
	{
		Manager.Data.UserData.OnHealthChanged += UpdateHealthUI;
		UpdateHealthUI(Manager.Data.UserData.Health);
		Manager.Data.UserData.OnManaChanged += UpdateManaUI;
		UpdateManaUI(Manager.Data.UserData.Mana);
		healthSlider.maxValue = Manager.Data.UserData.maxHealth;
		healthSlider.value = Manager.Data.UserData.maxHealth;
		manaSlider.maxValue = Manager.Data.UserData.maxMana;
		manaSlider.value = Manager.Data.UserData.maxMana;
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

	private void UpdateHealthUI(int newHealth)
	{
		healthText.text = $"{newHealth}/{Manager.Data.UserData.maxHealth}";
		healthSlider.value = newHealth;
	}

	private void UpdateManaUI(int newMana)
	{
		manaText.text = $"{newMana}/{Manager.Data.UserData.maxHealth}";
		manaSlider.value = newMana;
	}
}
