using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button menuButton;
	[SerializeField] Button settingButton;
	[SerializeField] PlayerMenuUI playerMenuUI;
	[SerializeField] PlayerSettingUI playerSettingUI;
	[SerializeField] TMP_Text healthText;
	[SerializeField] TMP_Text manaText;
	[SerializeField] TMP_Text goldText;
	[SerializeField] Slider healthSlider;
	[SerializeField] Slider manaSlider;
	[SerializeField] PhotonView photonView;
	[SerializeField] UserData localUserData;

	private void Awake()
	{
		if (photonView.IsMine == false)
		{
			return;
		}

		menuButton.onClick.AddListener(Menu);
		settingButton.onClick.AddListener(Setting);
		menuButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		settingButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Start()
	{
		if (!GetComponentInParent<PhotonView>().IsMine)
		{
			gameObject.SetActive(false);
			return;
		}

		localUserData = Manager.Data.UserData;
		localUserData.OnHealthChanged += UpdateHealthUI;
		UpdateHealthUI(localUserData.Health);
		localUserData.OnManaChanged += UpdateManaUI;
		UpdateManaUI(localUserData.Mana);
		healthSlider.maxValue = localUserData.maxHealth;
		healthSlider.value = localUserData.maxHealth;
		manaSlider.maxValue = localUserData.maxMana;
		manaSlider.value = localUserData.maxMana;
		localUserData.OnGoldChanged += UpdateGold;
		UpdateGold();
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
		manaText.text = $"{newMana}/{Manager.Data.UserData.maxMana}";
		manaSlider.value = newMana;
	}

	private void UpdateGold()
	{
		goldText.text = $"{Manager.Data.UserData.Gold}";
	}
}
