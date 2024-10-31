using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
	public InventoryIcon currentItem;
	public int slotIndex;
	[SerializeField] Button button;
	public GameObject currentSkillObject;

	private void Awake()
	{
		button.onClick.AddListener(Use);
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();

		if (draggedItem == null) return;

		QuickSlot originalSlot = draggedItem.quickSlot;
		InventorySlot previousSlot = draggedItem.parentSlot;

		if (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion)
		{
			InventoryIcon tempItem = currentItem;

			if (currentItem == null)
			{
				currentItem = draggedItem;
				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				if (previousSlot != null)
				{
					draggedItem.parentSlot = null;
					previousSlot.currentItem = null;
					Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, previousSlot), null);
				}
			}
			else
			{
				if (originalSlot != null)
				{
					originalSlot.currentItem = tempItem;
					if (tempItem != null)
					{
						tempItem.transform.SetParent(originalSlot.transform);
						tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempItem.itemName, tempItem.quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				}
				else if (previousSlot != null)
				{
					previousSlot.currentItem = tempItem;
					if (tempItem != null)
					{
						tempItem.transform.SetParent(previousSlot.transform);
						tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						int previousSlotIndex = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, previousSlot);
						Manager.Fire.SavePotionToDatabase(previousSlotIndex, new InventorySlotData(tempItem.itemName, tempItem.quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				}
			}

			draggedItem.quickSlot = this;
			if (tempItem != null)
			{
				tempItem.quickSlot = originalSlot;
				tempItem.parentSlot = previousSlot;
			}
		}
	}

	public void Use()
	{
		if (currentItem != null)
		{
			if (currentItem.slotType == InventoryIcon.SlotType.hpPotion)
			{
				if (Manager.Data.UserData.Health >= Manager.Data.UserData.maxHealth)
				{
					return;
				}
				currentItem.UpdateQuantity(-1);
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				Manager.Data.UserData.Health += 20;
				if (Manager.Data.UserData.Health > Manager.Data.UserData.maxHealth)
				{
					Manager.Data.UserData.Health = Manager.Data.UserData.maxHealth;
				}
				if (currentItem.quantity <= 0)
				{
					Destroy(currentItem.gameObject);
					currentItem = null;
					Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				}
			}
			else if (currentItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				if (Manager.Data.UserData.Mana >= Manager.Data.UserData.maxMana)
				{
					return;
				}
				currentItem.UpdateQuantity(-1);
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				Manager.Data.UserData.Mana += 20;
				if (Manager.Data.UserData.Mana > Manager.Data.UserData.maxMana)
				{
					Manager.Data.UserData.Mana = Manager.Data.UserData.maxMana;
				}
				if (currentItem.quantity <= 0)
				{
					Destroy(currentItem.gameObject);
					currentItem = null;
					Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				}
			}
		}
	}

	public void SetSkill(string skillName)
	{
		QuickSlot[] quickSlots = FindObjectsOfType<QuickSlot>();
		foreach (QuickSlot slot in quickSlots)
		{
			if (slot.currentSkillObject != null && slot.currentSkillObject.name == skillName)
			{
				Destroy(slot.currentSkillObject);
				slot.currentSkillObject = null;
			}
		}

		if (currentSkillObject != null)
		{
			Destroy(currentSkillObject);
			currentSkillObject = null;
		}

		GameObject skillPrefab = null;

		if (skillName == "Fire")
		{
			skillPrefab = Resources.Load<GameObject>("Prefabs/Fire");
		}
		else if (skillName == "Ice")
		{
			skillPrefab = Resources.Load<GameObject>("Prefabs/Ice");
		}

		if (skillPrefab != null)
		{
			currentSkillObject = Instantiate(skillPrefab, transform);
			currentSkillObject.name = skillName;
			RectTransform skillRect = currentSkillObject.GetComponent<RectTransform>();
			skillRect.anchoredPosition = Vector2.zero;
		}
	}
}
