using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryIcon;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
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
		}

		EquipItem(droppedItem);
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
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateWeaponSlot("Left", item.itemName);
			}
			else
			{
				Manager.Fire.UpdateWeaponSlot("Right", item.itemName);
			}
		}
		else if (slotType == SlotType.Shield)
		{
			playerEquipment.EquipShield(item.itemName);
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateShieldSlot("Left", item.itemName);
			}
			else
			{
				Manager.Fire.UpdateShieldSlot("Right", item.itemName);
			}
		}
		else if (slotType == SlotType.Cloak)
		{
			playerEquipment.EquipCloak(item.itemName);
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateCloakSlot("Left", item.itemName);
			}
			else
			{
				Manager.Fire.UpdateCloakSlot("Right", item.itemName);
			}
		}
	}

	public void UnequipItem()
	{
		if (slotType == SlotType.Weapon)
		{
			playerEquipment.EquipWeapon("");
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateWeaponSlot("Left", "");
			}
			else
			{
				Manager.Fire.UpdateWeaponSlot("Right", "");
			}
		}
		else if (slotType == SlotType.Shield)
		{
			playerEquipment.EquipShield("");
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateShieldSlot("Left", "");
			}
			else
			{
				Manager.Fire.UpdateShieldSlot("Right", "");
			}
		}
		else if (slotType == SlotType.Cloak)
		{
			playerEquipment.EquipCloak("");
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateCloakSlot("Left", "");
			}
			else
			{
				Manager.Fire.UpdateCloakSlot("Right", "");
			}
		}
	}
}
