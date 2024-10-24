using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : Singleton<DataManager>
{
	private UserData userData;
	public UserData UserData { get { return userData; }  set { userData = value; } }
	private InventorySlotData inventorySlotData;
	public InventorySlotData InventorySlotData { get {return inventorySlotData; } set { inventorySlotData = value; } }
}
