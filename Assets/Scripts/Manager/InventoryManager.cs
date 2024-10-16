using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
	[SerializeField] InventoryUI inventoryUI;
	public InventoryUI InventoryUI { get { return inventoryUI; } set { inventoryUI = value; } }
}
