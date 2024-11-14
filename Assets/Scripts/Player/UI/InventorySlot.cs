using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	[Header("Components")]
	[SerializeField] Image image;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] RectTransform rect;
	[SerializeField] InventoryIcon currentItem;
	public InventoryIcon CurrentItem { get { return currentItem; } set { currentItem = value; } }

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
			newItem.ParentSlot = this;
			newItem.transform.SetParent(transform);
			newItem.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
			Manager.Fire.SaveItemToDatabase(Array.IndexOf(inventoryUI.InventorySlots, this), newItem.ItemName);
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
		InventorySlot draggedSlot = draggedItem.ParentSlot;
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
			draggedItem.ParentSlot = this;

			if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				Manager.Fire.SavePotionData(thisSlotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity), true);
			}
			else
			{
				Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.ItemName);
			}
		}
		else
		{
			draggedItem.transform.SetParent(transform);
			draggedItem.GetComponent<RectTransform>().position = rect.position;

			draggedItem.ParentSlot = this;

			tempItem.transform.SetParent(draggedSlot.transform);
			tempItem.GetComponent<RectTransform>().position = draggedSlot.GetComponent<RectTransform>().position;
			draggedSlot.currentItem = tempItem;
			tempItem.ParentSlot = draggedSlot;

			if (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
				{
					Manager.Fire.SavePotionData(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.ItemName, draggedSlot.currentItem.Quantity), false);
					currentItem = draggedItem;
					Manager.Fire.SavePotionData(thisSlotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity), false);
				}
				else
				{
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.ItemName);
					currentItem = draggedItem;
					Manager.Fire.SavePotionData(thisSlotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity), false);
				}
			}
			else
			{
				if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
				{
					Manager.Fire.SavePotionData(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.ItemName, draggedSlot.currentItem.Quantity), false);
					currentItem = draggedItem;
					Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.ItemName);
				}
				else
				{
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.ItemName);
					currentItem = draggedItem;
					Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.ItemName);
				}
			}
		}
	}


	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = Color.white;
	}
}
