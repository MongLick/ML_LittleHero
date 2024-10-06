using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	private Image image;
	private RectTransform rect;
	private InventoryIcon currentItem;

	private void Awake()
	{
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		currentItem = GetComponentInChildren<InventoryIcon>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		image.color = Color.yellow;
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
			if (draggedItem != null)
			{
				InventorySlot draggedSlot = draggedItem.parentSlot;

				InventoryIcon tempItem = currentItem;

				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().position = rect.position;

				if (tempItem != null)
				{
					tempItem.transform.SetParent(draggedSlot.transform);
					tempItem.GetComponent<RectTransform>().position = draggedSlot.GetComponent<RectTransform>().position;
					tempItem.parentSlot = draggedSlot;
				}

				currentItem = draggedItem;
				draggedSlot.currentItem = tempItem;
				draggedItem.parentSlot = this;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = Color.white;
	}
}
