using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuUI : MonoBehaviour
{
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
