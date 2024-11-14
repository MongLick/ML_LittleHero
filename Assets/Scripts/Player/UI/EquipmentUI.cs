using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] EquipmentSlot[] equipmentSlots;
	public EquipmentSlot[] EquipmentSlots { get { return equipmentSlots; } set { equipmentSlots = value; } }
	[SerializeField] Button closeButton;
	[SerializeField] InventoryUI inventoryUI;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(PlayButtonSFX);
	}

	private void Close()
	{
		gameObject.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
	}

	private void PlayButtonSFX()
	{
		Manager.Sound.ButtonSFX();
	}
}
