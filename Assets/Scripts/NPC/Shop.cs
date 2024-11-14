using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button closeButton;
	[SerializeField] Button hpButton;
	[SerializeField] Button mpButton;
	[SerializeField] GameObject potionPrefab;
	[SerializeField] ShopkeeperNPC npc;

	[Header("Specs")]
	private const int PotionCost = 5;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		hpButton.onClick.AddListener(() => PurchasePotion("hpPotion"));
		mpButton.onClick.AddListener(() => PurchasePotion("mpPotion"));
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void PurchasePotion(string potionName)
	{
		if (Manager.Data.UserData.Gold >= PotionCost)
		{
			Manager.Data.UserData.Gold -= PotionCost;
			AddPotion(potionName);
		}
	}

	public void AddPotion(string potionName)
	{
		Manager.Fire.UpdateGoldInDatabase(-PotionCost);

		InventoryIcon existingPotion = FindPotionInInventory(potionName) ?? FindPotionInQuickSlots(potionName);

		if (existingPotion != null)
		{
			existingPotion.UpdateQuantity(1);

			if (existingPotion.QuickSlot != null)
			{
				Manager.Fire.SavePotionData(existingPotion.QuickSlot.SlotIndex, new InventorySlotData(potionName, existingPotion.Quantity), true);
			}
			else
			{
				UpdatePotionQuantity(potionName, existingPotion.Quantity);
			}
		}
		else
		{
			CreateNewPotionIcon(potionName);
		}
	}

	private InventoryIcon FindPotionInQuickSlots(string potionName)
	{
		foreach (QuickSlot slot in npc.QuickSlot)
		{
			if (slot.CurrentItem != null && slot.CurrentItem.ItemName == potionName)
			{
				return slot.CurrentItem;
			}
		}
		return null;
	}

	private InventoryIcon FindPotionInInventory(string potionName)
	{
		foreach (InventorySlot slot in npc.InventorySlots)
		{
			if (slot.CurrentItem != null && slot.CurrentItem.ItemName == potionName)
			{
				return slot.CurrentItem;
			}
		}
		return null;
	}

	private void CreateNewPotionIcon(string potionName)
	{
		potionPrefab = LoadPotionPrefab(potionName);

		GameObject potionObject = Instantiate(potionPrefab);
		InventoryIcon potionIcon = potionObject.GetComponent<InventoryIcon>();

		potionIcon.ItemName = potionName;
		potionIcon.Quantity = 1;
		InventorySlot[] inventorySlots = npc.InventorySlots;

		for (int i = 0; i < inventorySlots.Length; i++)
		{
			if (inventorySlots[i].CurrentItem == null)
			{
				potionObject.transform.SetParent(inventorySlots[i].transform);
				potionObject.GetComponent<RectTransform>().position = inventorySlots[i].GetComponent<RectTransform>().position;
				inventorySlots[i].CurrentItem = potionIcon;
				potionIcon.ParentSlot = inventorySlots[i];

				InventorySlotData newPotionData = new InventorySlotData(potionName, 1);
				Manager.Fire.SavePotionData(i, newPotionData, false);
				return;
			}
		}
	}

	private GameObject LoadPotionPrefab(string potionName)
	{
		return Resources.Load<GameObject>($"Prefabs/{potionName}");
	}

	private void UpdatePotionQuantity(string potionName, int quantity)
	{
		InventorySlot[] slots = npc.InventorySlots;

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].CurrentItem != null && slots[i].CurrentItem.ItemName == potionName)
			{
				InventorySlotData updatedPotionData = new InventorySlotData(potionName, quantity);
				Manager.Fire.SavePotionData(i, updatedPotionData, false);
				break;
			}
		}
	}

	public void LoadPotion(string potionName, int slotIndex, int itemQuantity)
	{
		potionPrefab = LoadPotionPrefab(potionName);

		InventorySlot[] inventorySlots = npc.InventorySlots;
		InventorySlot slot = inventorySlots[slotIndex];

		GameObject potionObject = Instantiate(potionPrefab, slot.transform);
		InventoryIcon potionIcon = potionObject.GetComponent<InventoryIcon>();

		potionIcon.ItemName = potionName;
		potionIcon.Quantity = itemQuantity;
		potionIcon.UpdateQuantityText();

		potionObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		slot.CurrentItem = potionIcon;
		potionIcon.ParentSlot = slot;
	}
}

