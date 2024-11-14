using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button equipmentButton;
	[SerializeField] Button inventoryButton;
	[SerializeField] Button skillButton;
	[SerializeField] Button closeButton;
	[SerializeField] EquipmentUI equipmentUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] SkillUI skillUI;
	[SerializeField] PlayerSettingUI playerSettingUI;

	private void Awake()
	{
		AddButtonListeners();
	}

	private void AddButtonListeners()
	{
		equipmentButton.onClick.AddListener(() => OpenUI(equipmentUI));
		inventoryButton.onClick.AddListener(() => OpenUI(inventoryUI));
		skillButton.onClick.AddListener(() => OpenUI(skillUI));
		closeButton.onClick.AddListener(Close);

		PlayButtonSFX();
	}

	private void PlayButtonSFX()
	{
		equipmentButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		inventoryButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		skillButton.onClick.AddListener(Manager.Sound.ButtonSFX);
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
