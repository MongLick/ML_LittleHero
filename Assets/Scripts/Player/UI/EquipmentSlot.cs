using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
	public enum SlotType { Weapon, Shield, Cloak }

	public SlotType slotType;

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();

		if (draggedItem != null)
		{
			if (draggedItem.slotType == slotType)
			{
				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

				PlayerEquipment playerEquipment = GetComponentInParent<PlayerEquipment>();
				if (playerEquipment != null)
				{
					string weapon = (slotType == SlotType.Weapon) ? draggedItem.itemName : "";
					string shield = (slotType == SlotType.Shield) ? draggedItem.itemName : "";
					string cloak = (slotType == SlotType.Cloak) ? draggedItem.itemName : "";
					playerEquipment.EquipItem(weapon, shield, cloak);
				}
			}
			else
			{
				draggedItem.ReturnToInventory();
			}
		}
	}
}
