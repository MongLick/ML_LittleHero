using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] Button closeButton;
	[SerializeField] InventorySlot[] inventorySlots;
	public InventorySlot[] InventorySlots { get { return inventorySlots; } }

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
