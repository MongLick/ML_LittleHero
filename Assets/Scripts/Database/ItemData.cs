using System;
using UnityEngine;

[Serializable]
public class ItemData
{
	public string itemName;
	public ItemType itemType;
	public Sprite itemIcon;

	public ItemData(string name, ItemType type, Sprite icon)
	{
		itemName = name;
		itemType = type;
		itemIcon = icon;
	}
}

public enum ItemType { Weapon, Shield, Cloak, PotionHP, PotionMP}
