using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public string skillName;
	private Canvas canvas;
	public QuickSlot quickSlot;
	public Image skillImage;

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