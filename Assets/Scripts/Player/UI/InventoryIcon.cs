using UnityEngine;
using UnityEngine.EventSystems;
using static EquipmentSlot;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public SlotType slotType;
	private Transform canvas;
	private Transform previousParent;
	private RectTransform rect;
	private CanvasGroup canvasGroup;
	public string itemName;
	public InventorySlot parentSlot;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>().transform;
		rect = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		parentSlot = GetComponentInParent<InventorySlot>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.SetParent(canvas);
		transform.SetAsLastSibling();
		canvasGroup.alpha = 0.5f;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		rect.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (transform.parent == canvas)
		{
			ReturnToInventory();
		}

		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}

	public void ReturnToInventory()
	{
		transform.SetParent(parentSlot.transform);
		rect.position = parentSlot.GetComponent<RectTransform>().position;
	}
}
