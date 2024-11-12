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
		equipmentButton.onClick.AddListener(Equipment);
		inventoryButton.onClick.AddListener(Inventory);
		skillButton.onClick.AddListener(SkillUI);
		closeButton.onClick.AddListener(Close);
		equipmentButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		inventoryButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		skillButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Equipment()
	{
		equipmentUI.gameObject.SetActive(true);
	}

	private void Inventory()
	{
		inventoryUI.gameObject.SetActive(true);
	}

	private void SkillUI()
	{
		skillUI.gameObject.SetActive(true);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
