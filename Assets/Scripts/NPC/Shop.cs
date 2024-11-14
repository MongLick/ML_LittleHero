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

			if (existingPotion.QuickSlot != null)
			{
				Manager.Fire.SavePotionQuickSlot(existingPotion.QuickSlot.SlotIndex, new InventorySlotData(potionName, existingPotion.Quantity));
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
		QuickSlot[] quickSlots = npc.QuickSlot;

		foreach (QuickSlot slot in quickSlots)
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
		InventorySlot[] slots = npc.InventorySlots;

		foreach (InventorySlot slot in slots)
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

		potionIcon.InventoryUI = npc.InventoryUI;
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
				Manager.Fire.SavePotionToDatabase(i, newPotionData);
				return;
			}
		}
	}

	private void UpdatePotionQuantity(string potionName, int quantity)
	{
		InventorySlot[] slots = npc.InventorySlots;

		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].CurrentItem != null && slots[i].CurrentItem.ItemName == potionName)
			{
				InventorySlotData updatedPotionData = new InventorySlotData(potionName, quantity);
				Manager.Fire.SavePotionToDatabase(i, updatedPotionData);
				break;
			}
		}
	}

	public void LoadPotion(string potionName, int slotIndex, int itemQuantity, InventoryUI inventoryUI)
	{
		if (potionName == "hpPotion")
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/hpPotion");
		}
		else
		{
			potionPrefab = Resources.Load<GameObject>("Prefabs/mpPotion");
		}

		InventorySlot[] inventorySlots = inventoryUI.InventorySlots;
		InventorySlot slot = inventorySlots[slotIndex];

		GameObject potionObject = Instantiate(potionPrefab, slot.transform);
		InventoryIcon potionIcon = potionObject.GetComponent<InventoryIcon>();

		potionIcon.InventoryUI = inventoryUI;
		potionIcon.ItemName = potionName;
		potionIcon.Quantity = itemQuantity;
		potionIcon.UpdateQuantityText();

		potionObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		slot.CurrentItem = potionIcon;
		potionIcon.ParentSlot = slot;
	}
}

