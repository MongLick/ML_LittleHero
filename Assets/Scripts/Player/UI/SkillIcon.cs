using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Header("Components")]
	[SerializeField] Canvas canvas;
	[SerializeField] Image skillImage;
	[SerializeField] QuickSlot quickSlot;
	public QuickSlot QuickSlot { get { return quickSlot; } set { quickSlot = value; } }

	[Header("Specs")]
	[SerializeField] string skillName;
	public string SkillName { get { return skillName; } }

	private void Awake()
	{
		InitializeCanvas();
	}

	private void InitializeCanvas()
	{
		canvas = FindObjectOfType<Canvas>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		StartDrag(eventData);
	}

	private void StartDrag(PointerEventData eventData)
	{
		transform.SetParent(canvas.transform);
		skillImage.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Dragging(eventData);
	}

	private void Dragging(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		EndDrag(eventData);
	}

	private void EndDrag(PointerEventData eventData)
	{
		skillImage.raycastTarget = true;

		GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;
		if (IsDroppedOutsideQuickSlot(droppedObject))
		{
			ReturnToOriginalPosition();
		}
	}

	private bool IsDroppedOutsideQuickSlot(GameObject droppedObject)
	{
		return droppedObject == null || droppedObject.GetComponent<QuickSlot>() == null;
	}

	private void ReturnToOriginalPosition()
	{
		transform.SetParent(quickSlot.transform);
		GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
	}
}