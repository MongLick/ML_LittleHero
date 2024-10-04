using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Transform canvas;
	private Transform previousParent;
	private RectTransform rect;
	private CanvasGroup canvasGroup;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>().transform;
		rect = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	/*public void AddItem(ItemData newItem)
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
	}*/

	public void OnBeginDrag(PointerEventData eventData)
	{
		previousParent = transform.parent;
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
			transform.SetParent(previousParent);
			rect.position = previousParent.GetComponent<RectTransform>().position;
		}

		canvasGroup.alpha = 1.0f;
		canvasGroup.blocksRaycasts = true;
	}
}
