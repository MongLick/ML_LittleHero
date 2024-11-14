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

		HandleEquipment(item.ItemName, true);
	}

	public void UnequipItem()
	{
		HandleEquipment("", false);
	}

	private void HandleEquipment(string itemName, bool isEquipping)
	{
		string slotName = GetSlotName();

		if (isEquipping)
		{
			switch (slotType)
			{
				case SlotType.Weapon:
					playerEquipment.EquipWeapon(itemName);
					break;
				case SlotType.Shield:
					playerEquipment.EquipShield(itemName);
					break;
				case SlotType.Cloak:
					playerEquipment.EquipCloak(itemName);
					break;
			}
		}
		else
		{
			switch (slotType)
			{
				case SlotType.Weapon:
					playerEquipment.EquipWeapon("");
					break;
				case SlotType.Shield:
					playerEquipment.EquipShield("");
					break;
				case SlotType.Cloak:
					playerEquipment.EquipCloak("");
					break;
			}
		}

		UpdateEquipmentSlot(slotName, itemName);
	}

	private void UpdateEquipmentSlot(string slotName, string itemName)
	{
		string side = Manager.Fire.IsLeft ? "Left" : "Right";
		Manager.Fire.UpdateEquipmentSlot(side, $"{slotName}Slot", itemName);
	}

	private string GetSlotName()
	{
		switch (slotType)
		{
			case SlotType.Weapon:
				return "weapon";
			case SlotType.Shield:
				return "shield";
			case SlotType.Cloak:
				return "cloak";
			default:
				return "";
		}
	}
}
