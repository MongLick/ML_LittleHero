using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	[SerializeField] InventoryUI inventoryUI;
	private Image image;
	private RectTransform rect;
	public InventoryIcon currentItem;

	private void Awake()
	{
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		currentItem = GetComponentInChildren<InventoryIcon>();
	}

	public void AddItem(InventoryIcon newItem)
	{
		if (currentItem == null)
		{
			currentItem = newItem;
			newItem.InventoryUI = inventoryUI;
			newItem.parentSlot = this;
			newItem.transform.SetParent(transform);
			newItem.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
			Manager.Fire.SaveItemToDatabase(Array.IndexOf(inventoryUI.InventorySlots, this), newItem.itemName);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		image.color = Color.yellow;
	}

	public void OnDrop(PointerEventData eventData)
	{
		SkillIcon draggedSkill = eventData.pointerDrag.GetComponent<SkillIcon>();

		if (draggedSkill != null)
		{
			return;
		}

		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		InventorySlot draggedSlot = draggedItem.parentSlot;
		InventoryIcon tempItem = currentItem;

		if (draggedItem == null || draggedSlot == null)
		{
			return;
		}

		int thisSlotIndex = Array.IndexOf(inventoryUI.InventorySlots, this);
		int draggedSlotIndex = Array.IndexOf(inventoryUI.InventorySlots, draggedSlot);

		draggedSlot.currentItem = null;
		Manager.Fire.SaveItemToDatabase(draggedSlotIndex, null);

		if (tempItem == null)
		{
			draggedItem.transform.SetParent(transform);
			draggedItem.GetComponent<RectTransform>().position = rect.position;

			currentItem = draggedItem;
			draggedItem.parentSlot = this;

			if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				Manager.Fire.SavePotionToDatabase(thisSlotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
			}
			else
			{
				Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.itemName);
			}
		}
		else
		{
			draggedItem.transform.SetParent(transform);
			draggedItem.GetComponent<RectTransform>().position = rect.position;

			draggedItem.parentSlot = this;

			tempItem.transform.SetParent(draggedSlot.transform);
			tempItem.GetComponent<RectTransform>().position = draggedSlot.GetComponent<RectTransform>().position;
			draggedSlot.currentItem = tempItem;
			tempItem.parentSlot = draggedSlot;

			if (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
				{
					Manager.Fire.SavePotionToDatabase(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.itemName, draggedSlot.currentItem.quantity));
					currentItem = draggedItem;
					Manager.Fire.SavePotionToDatabase(thisSlotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				}
				else
				{
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.itemName);
					currentItem = draggedItem;
					Manager.Fire.SavePotionToDatabase(thisSlotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				}
			}
			else
			{
				if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
				{
					Manager.Fire.SavePotionToDatabase(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.itemName, draggedSlot.currentItem.quantity));
					currentItem = draggedItem;
					Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.itemName);
				}
				else
				{
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.itemName);
					currentItem = draggedItem;
					Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.itemName);
				}
			}
		}
	}


	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = Color.white;
	}
}
