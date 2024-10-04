using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
	public Image icon;
	private ItemData item;

	public void AddItem(ItemData newItem)
	{
		item = newItem;
		icon.sprite = item.itemIcon;
		icon.enabled = true;
	}

	public void ClearSlot()
	{
		item = null;
		icon.sprite = null;
		icon.enabled = false;
	}
}
