using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
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
			newItem.parentSlot = this;
			newItem.transform.SetParent(transform);
			newItem.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
			Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this), newItem.itemName);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		image.color = Color.yellow;
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		InventorySlot draggedSlot = draggedItem.parentSlot;
		InventoryIcon tempItem = currentItem;

		if (draggedItem == null || draggedSlot == null)
		{
			return;
		}

		if (tempItem != null)
		{
			int currentSlotIndex = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this);
			Manager.Fire.SaveItemToDatabase(currentSlotIndex, tempItem.itemName);
			tempItem.parentSlot.currentItem = null;
		}

		draggedItem.transform.SetParent(transform);
		draggedItem.GetComponent<RectTransform>().position = rect.position;

		currentItem = draggedItem;
		draggedItem.parentSlot = this;

		if (tempItem == null)
		{
			draggedSlot.currentItem = null;
		}
		else
		{
			tempItem.transform.SetParent(draggedSlot.transform);
			tempItem.GetComponent<RectTransform>().position = draggedSlot.GetComponent<RectTransform>().position;
			draggedSlot.currentItem = tempItem;
			tempItem.parentSlot = draggedSlot;
		}

		Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, draggedSlot), draggedSlot.currentItem != null ? draggedSlot.currentItem.itemName : null);
		Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this), currentItem != null ? currentItem.itemName : "");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = Color.white;
	}
}
