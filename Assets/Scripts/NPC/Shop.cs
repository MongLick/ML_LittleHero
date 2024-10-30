using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[SerializeField] Button closeButton;
	[SerializeField] Button hpButton;
	[SerializeField] Button mpButton;
	[SerializeField] GameObject potionPrefab;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		hpButton.onClick.AddListener(HpPotionPurchase);
		mpButton.onClick.AddListener(MpPotionPurchase);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void HpPotionPurchase()
	{
		if (Manager.Data.UserData.Gold >= 5)
		{
			Manager.Data.UserData.Gold -= 5;
			AddPotion("hpPotion");
		}
		else
		{
			return;
		}
	}

	private void MpPotionPurchase()
	{
		if (Manager.Data.UserData.Gold >= 5)
		{
			Manager.Data.UserData.Gold -= 5;
			AddPotion("mpPotion");
		}
		else
		{
			return;
		}
	}

	public void AddPotion(string potionName)
	{
		Manager.Fire.UpdateGoldInDatabase(-5);

		InventoryIcon existingPotion = FindPotionInInventory(potionName);

		if (existingPotion == null)
		{
			existingPotion = FindPotionInQuickSlots(potionName);
		}

		if (existingPotion != null)
		{
			existingPotion.UpdateQuantity(1);

			if (existingPotion.quickSlot != null)
			{
				Manager.Fire.SavePotionQuickSlot(existingPotion.quickSlot.slotIndex, new InventorySlotData(potionName, existingPotion.quantity));
			}
			else
			{
				UpdatePotionQuantity(potionName, existingPotion.quantity);
			}
		}
		else
		{
			CreateNewPotionIcon(potionName);
		}
	}

	private InventoryIcon FindPotionInQuickSlots(string potionName)
	{
		QuickSlot[] quickSlots = FindObjectsOfType<QuickSlot>();

		foreach (QuickSlot slot in quickSlots)
		{
			if (slot.currentItem != null && slot.currentItem.itemName == potionName)
			{
				return slot.currentItem as InventoryIcon;
			}
		}
		return null;
	}

	private InventoryIcon FindPotionInInventory(string potionName)
	{
		InventorySlot[] slots = Manager.Inven.InventoryUI.InventorySlots;

		foreach (InventorySlot slot in slots)
		{
			if (slot.currentItem != null && slot.currentItem.itemName == potionName)
			{
				return slot.currentItem as InventoryIcon;
			}
		}
		return null;
	}

	private void CreateNewPotionIcon(string potionName)
	{
		if (potionName == "hpPotion")
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/hpPotion");
		}
		else
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/mpPotion");
		}

		GameObject potionObject = Instantiate(potionPrefab);
		InventoryIcon potionIcon = potionObject.GetComponent<InventoryIcon>();

		potionIcon.itemName = potionName;
		potionIcon.quantity = 1;
		InventorySlot[] inventorySlots = Manager.Inven.InventoryUI.InventorySlots;
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			if (inventorySlots[i].currentItem == null)
			{
				potionObject.transform.SetParent(inventorySlots[i].transform);
				potionObject.GetComponent<RectTransform>().position = inventorySlots[i].GetComponent<RectTransform>().position;
				inventorySlots[i].currentItem = potionIcon;
				potionIcon.parentSlot = inventorySlots[i];
				InventorySlotData newPotionData = new InventorySlotData(potionName, 1);
				Manager.Fire.SavePotionToDatabase(i, newPotionData);
				return;
			}
		}
	}

	private void UpdatePotionQuantity(string potionName, int quantity)
	{
		InventorySlot[] slots = Manager.Inven.InventoryUI.InventorySlots;

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].currentItem != null && slots[i].currentItem.itemName == potionName)
			{
				InventorySlotData updatedPotionData = new InventorySlotData(potionName, quantity);
				Manager.Fire.SavePotionToDatabase(i, updatedPotionData);
				break;
			}
		}
	}

	public void LoadPotion(string potionName, int slotIndex, int itemQuantity)
	{
		if (potionName == "hpPotion")
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/hpPotion");
		}
		else
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/mpPotion");
		}

		InventorySlot[] inventorySlots = Manager.Inven.InventoryUI.InventorySlots;
		InventorySlot slot = inventorySlots[slotIndex];

		GameObject potionObject = Instantiate(potionPrefab, slot.transform);
		InventoryIcon potionIcon = potionObject.GetComponent<InventoryIcon>();

		potionIcon.itemName = potionName;
		potionIcon.quantity = itemQuantity;
		potionIcon.UpdateQuantityText();

		potionObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		slot.currentItem = potionIcon;
		potionIcon.parentSlot = slot;
	}
}
