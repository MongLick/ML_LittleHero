using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button closeButton;
	[SerializeField] InventorySlot[] inventorySlots;
	public InventorySlot[] InventorySlots { get { return inventorySlots; } }

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
