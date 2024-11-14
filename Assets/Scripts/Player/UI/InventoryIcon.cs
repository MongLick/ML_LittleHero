using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public enum SlotType { Weapon, Shield, Cloak, hpPotion, mpPotion }
	public SlotType slotType;

	[Header("Components")]
	[SerializeField] Transform canvas;
	[SerializeField] Transform previousParent;
	[SerializeField] RectTransform rect;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] InventorySlot parentSlot;
	public InventorySlot ParentSlot { get { return parentSlot; } set { parentSlot = value; } }
	[SerializeField] EquipmentSlot equipmentSlot;
	public EquipmentSlot EquipmentSlot { get { return equipmentSlot; } set { equipmentSlot = value; } }
	[SerializeField] QuickSlot quickSlot;
	public QuickSlot QuickSlot { get { return quickSlot; } set { quickSlot = value; } }
	[SerializeField] InventoryUI inventoryUI;
	public InventoryUI InventoryUI { get { return inventoryUI; } set { inventoryUI = value; } }
	[SerializeField] TMP_Text quantityText;
	public TMP_Text QuantityText { get { return quantityText; } set { quantityText = value; } }

	[Header("Specs")]
	[SerializeField] string itemName;
	public string ItemName { get { return itemName; } set { itemName = value; } }
	[SerializeField] int quantity;
	public int Quantity { get { return quantity; } set { quantity = value; } }
	private bool isEquipment;
	public bool IsEquipment { get { return isEquipment; } set { isEquipment = value; } }

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
				Manager.Fire.SaveItemToDatabase(Array.IndexOf(InventoryUI.InventorySlots, parentSlot), null);
				parentSlot.CurrentItem = null;
				parentSlot = null;

				equipmentSlot.CurrentItem = this;
				isEquipment = true;
			}
		}
		else
		{
			isEquipment = false;
			EquipmentSlot[] equipmentSlots = FindObjectsOfType<EquipmentSlot>();

			foreach (EquipmentSlot slot in equipmentSlots)
			{
				if (slot.SlotType == slotType && slot.CurrentItem != null && slot.CurrentItem.itemName == itemName)
				{
					slot.CurrentItem = null;
					slot.UnequipItem();
					break;
				}
			}
		}

		quickSlot = transform.parent.GetComponent<QuickSlot>();

		if (quickSlot != null)
		{
			quickSlot.CurrentItem = this;
		}
		else
		{
			QuickSlot[] quickSlot = FindObjectsOfType<QuickSlot>();

			foreach (QuickSlot slot in quickSlot)
			{
				if (slot.CurrentItem != null && slot.CurrentItem.itemName == itemName)
				{
					slot.CurrentItem = null;
					Manager.Fire.SavePotionQuickSlot(slot.SlotIndex, new InventorySlotData("", 0));
					break;
				}
			}
		}

		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}

	public void ReturnToInventory()
	{
		InventorySlot[] inventorySlots = InventoryUI.InventorySlots;

		for (int i = 0; i < inventorySlots.Length; i++)
		{
			InventorySlot slot = inventorySlots[i];

			if (slot.CurrentItem == null)
			{
				transform.SetParent(slot.transform);
				rect.position = slot.GetComponent<RectTransform>().position;

				if (parentSlot != null)
				{
					Manager.Fire.SaveItemToDatabase(Array.IndexOf(InventoryUI.InventorySlots, parentSlot), "");
					parentSlot.CurrentItem = null;
				}

				slot.CurrentItem = this;
				parentSlot = slot;
				int index = Array.IndexOf(InventoryUI.InventorySlots, parentSlot);

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
