using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
	[SerializeField] Button closeButton;
	[SerializeField] EquipmentSlot[] equipmentSlots;
	[SerializeField] InventoryUI inventoryUI;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	private void Close()
	{
		gameObject.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
	}
}
