using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryIcon;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
	[Header("Components")]
	[SerializeField] SlotType slotType;
	public SlotType SlotType { get { return slotType; } set { slotType = value; } }
	[SerializeField] InventoryIcon currentItem;
	public InventoryIcon CurrentItem { get { return currentItem; } set { currentItem = value; } }
	[SerializeField] PlayerEquipment playerEquipment;
	public PlayerEquipment PlayerEquipment { get { return playerEquipment; } set { playerEquipment = value; } }

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
		currentItem.IsEquipment = true;

		if (slotType == SlotType.Weapon)
		{
			playerEquipment.EquipWeapon(item.ItemName);
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateWeaponSlot("Left", item.ItemName);
			}
			else
			{
				Manager.Fire.UpdateWeaponSlot("Right", item.ItemName);
			}
		}
		else if (slotType == SlotType.Shield)
		{
			playerEquipment.EquipShield(item.ItemName);
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateShieldSlot("Left", item.ItemName);
			}
			else
			{
				Manager.Fire.UpdateShieldSlot("Right", item.ItemName);
			}
		}
		else if (slotType == SlotType.Cloak)
		{
			playerEquipment.EquipCloak(item.ItemName);
			if (Manager.Fire.IsLeft)
			{
				Manager.Fire.UpdateCloakSlot("Left", item.ItemName);
			}
			else
			{
				Manager.Fire.UpdateCloakSlot("Right", item.ItemName);
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
