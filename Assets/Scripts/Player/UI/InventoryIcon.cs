using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum SlotType { Weapon, Shield, Cloak, hpPotion, mpPotion }
	public SlotType slotType;
	private Transform canvas;
	private Transform previousParent;
	private RectTransform rect;
	private CanvasGroup canvasGroup;
	public string itemName;
	public InventorySlot parentSlot;
	public EquipmentSlot equipmentSlot;
	public QuickSlot quickSlot;
	public bool isEquipment;
	public InventoryUI InventoryUI;
	public int quantity;
	public TMP_Text quantityText;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>().transform;
		rect = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		parentSlot = GetComponentInParent<InventorySlot>();
		UpdateQuantityText();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.SetParent(canvas);
		transform.SetAsLastSibling();
		canvasGroup.alpha = 0.5f;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		rect.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (transform.parent == canvas)
		{
			ReturnToInventory();
		}

		equipmentSlot = transform.parent.GetComponent<EquipmentSlot>();

		if (equipmentSlot != null)
		{
			if (parentSlot != null)
			{
				Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, parentSlot), null);
				parentSlot.currentItem = null;
				parentSlot = null;

				equipmentSlot.currentItem = this;
				isEquipment = true;
			}
		}
		else
		{
			isEquipment = false;
			EquipmentSlot[] equipmentSlots = FindObjectsOfType<EquipmentSlot>();

			foreach (EquipmentSlot slot in equipmentSlots)
			{
				if (slot.slotType == slotType && slot.currentItem != null && slot.currentItem.itemName == itemName)
				{
					slot.currentItem = null;
					slot.UnequipItem();
					break;
				}
			}
		}

		quickSlot = transform.parent.GetComponent<QuickSlot>();

		if (quickSlot != null)
		{
			quickSlot.currentItem = this;
		}
		else
		{
			QuickSlot[] quickSlot = FindObjectsOfType<QuickSlot>();

			foreach (QuickSlot slot in quickSlot)
			{
				if (slot.currentItem != null && slot.currentItem.itemName == itemName)
				{
					slot.currentItem = null;
					Manager.Fire.SavePotionQuickSlot(slot.slotIndex, new InventorySlotData("", 0));
					break;
				}
			}
		}

		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}

	public void ReturnToInventory()
	{
		if (InventoryUI == null)
		{
			InventoryUI = FindObjectOfType<InventoryUI>();
			if (InventoryUI == null)
			{
				return;
			}
		}

		InventorySlot[] inventorySlots = InventoryUI.InventorySlots;

		for (int i = 0; i < inventorySlots.Length; i++)
		{
			InventorySlot slot = inventorySlots[i];

			if (slot.currentItem == null)
			{
				transform.SetParent(slot.transform);
				rect.position = slot.GetComponent<RectTransform>().position;

				if (parentSlot != null)
				{
					Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, parentSlot), "");
					parentSlot.currentItem = null;
				}

				slot.currentItem = this;
				parentSlot = slot;
				int index = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, parentSlot);

				if (slotType == SlotType.hpPotion || slotType == SlotType.mpPotion)
				{
					Manager.Fire.SavePotionToDatabase(index, new InventorySlotData(this.itemName, this.quantity));
				}
				else
				{
					Manager.Fire.SaveItemToDatabase(index, this.itemName);
				}

				return;
			}
		}
	}

	public void UpdateQuantity(int amount)
	{
		quantity += amount;
		UpdateQuantityText();
	}

	public void UpdateQuantityText()
	{
		if (quantityText != null)
		{
			quantityText.text = quantity > 1 ? quantity.ToString() : "";
		}
	}
}
