using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
	[SerializeField] Button closeButton;
	public EquipmentSlot[] equipmentSlots;
	[SerializeField] InventoryUI inventoryUI;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
	}
}
