using UnityEngine;
using UnityEngine.EventSystems;
using static EquipmentSlot;

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
	public bool isEquipment;
	public InventoryUI InventoryUI;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>().transform;
		rect = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		parentSlot = GetComponentInParent<InventorySlot>();
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
				parentSlot.currentItem = null;
				parentSlot = null;
			}
			isEquipment = true;
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
					parentSlot.currentItem = null;
				}
				slot.currentItem = this;
				parentSlot = slot;
				return;
			}
		}
	}
}
