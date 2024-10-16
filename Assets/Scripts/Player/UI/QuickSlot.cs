using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlot : MonoBehaviour, IDropHandler
{
	public InventoryIcon currentItem;

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		if (draggedItem != null)
		{
			if (draggedItem.slotType == InventoryIcon.SlotType.hpPotion)
			{
				if (currentItem == null)
				{
					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				}
				else
				{
					InventoryIcon tempItem = currentItem;

					currentItem = draggedItem;
					currentItem.transform.SetParent(transform);
					currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

					tempItem.transform.SetParent(draggedItem.parentSlot.transform);
					tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

					tempItem.parentSlot = draggedItem.parentSlot;
				}
			}
		}
	}

	public void UsePotion()
	{
		if (currentItem != null)
		{
			currentItem.UpdateQuantity(-1);
			if (currentItem.quantity <= 0)
			{
				currentItem = null;
			}
		}
	}
}
