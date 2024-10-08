using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
	public enum SlotType { Weapon, Shield, Cloak }

	public SlotType slotType;
	public InventoryIcon currentItem;
	private PlayerEquipment playerEquipment;

	private void Start()
	{
		playerEquipment = GetComponentInParent<PlayerEquipment>();
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon droppedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();

		if (droppedItem.slotType != slotType)
		{
			droppedItem.ReturnToInventory();
			return;
		}

		if (currentItem != null)
		{
			currentItem.ReturnToInventory();
			currentItem = null;
		}

		EquipItem(droppedItem);

		if (currentItem == null)
		{
			UnequipItem();
		}
	}

	private void EquipItem(InventoryIcon item)
	{
		item.transform.SetParent(transform);
		item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		currentItem = item;
		currentItem.isEquipment = true;

		if (slotType == SlotType.Weapon)
		{
			playerEquipment.EquipWeapon(item.itemName);
		}
		else if (slotType == SlotType.Shield)
		{
			playerEquipment.EquipShield(item.itemName);
		}
		else if (slotType == SlotType.Cloak)
		{
			playerEquipment.EquipCloak(item.itemName);
		}
	}

	public void UnequipItem()
	{
		if (slotType == SlotType.Weapon)
		{
			playerEquipment.EquipWeapon("");
		}
		else if (slotType == SlotType.Shield)
		{
			playerEquipment.EquipShield("");
		}
		else if (slotType == SlotType.Cloak)
		{
			playerEquipment.EquipCloak("");
		}
	}
}
