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

			if (tempItem.slotType == InventoryIcon.SlotType.hpPotion || tempItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				Manager.Fire.SavePotionToDatabase(currentSlotIndex, new InventorySlotData(tempItem.itemName, tempItem.quantity));
			}
			else
			{
				Manager.Fire.SaveItemToDatabase(currentSlotIndex, tempItem.itemName);
			}

			tempItem.parentSlot.currentItem = null;
		}

		draggedItem.transform.SetParent(transform);
		draggedItem.GetComponent<RectTransform>().position = rect.position;

		currentItem = draggedItem;
		draggedItem.parentSlot = this;

		if (tempItem == null)
		{
			draggedSlot.currentItem = null; // tempItem�� null�� ���, �巡�׵� ������ ���� �������� null�� ����
			if (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				// ������ ��� ������ ����
				Manager.Fire.SavePotionToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this), new InventorySlotData(currentItem.itemName, currentItem.quantity));
			}
			else
			{
				// ������ ��� �̸��� ���� (������ �������� ����)
				Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this), currentItem.itemName);
			}
		}
		else
		{
			tempItem.transform.SetParent(draggedSlot.transform);
			tempItem.GetComponent<RectTransform>().position = draggedSlot.GetComponent<RectTransform>().position;
			draggedSlot.currentItem = tempItem;
			tempItem.parentSlot = draggedSlot;

			int draggedSlotIndex = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, draggedSlot);
			int thisSlotIndex = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, this);

			// �巡���� �������� ������ ���
			if (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				// ���� �������� ������ ��
				if (currentItem != null && (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion))
				{
					// ���࿡�� ��������
					Manager.Fire.SavePotionToDatabase(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.itemName, draggedSlot.currentItem.quantity));
					Manager.Fire.SavePotionToDatabase(thisSlotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				}
				else
				{
					// ���࿡�� �����
					Manager.Fire.SavePotionToDatabase(draggedSlotIndex, new InventorySlotData(draggedSlot.currentItem.itemName, draggedSlot.currentItem.quantity));
					Manager.Fire.SaveItemToDatabase(thisSlotIndex, currentItem.itemName); // ������ ������ �������� ����
				}
			}
			else // �巡���� �������� ������ ���
			{
				// ���� �������� ������ ��
				if (currentItem != null && (currentItem.slotType == InventoryIcon.SlotType.hpPotion || currentItem.slotType == InventoryIcon.SlotType.mpPotion))
				{
					// ���⿡�� ��������
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.itemName);
					Manager.Fire.SavePotionToDatabase(thisSlotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity)); // ������ ������ ����
				}
				else
				{
					// ���⿡�� �����
					Manager.Fire.SaveItemToDatabase(draggedSlotIndex, draggedSlot.currentItem.itemName);
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
