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
		canvas = FindObjectOfType<Canvas>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.transform.SetParent(canvas.transform);
		skillImage.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		skillImage.raycastTarget = true;
		GameObject droppedObject = eventData.pointerCurrentRaycast.gameObject;

		if (droppedObject == null || droppedObject.GetComponent<QuickSlot>() == null)
		{
			transform.SetParent(quickSlot.transform);
			GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
	}
}