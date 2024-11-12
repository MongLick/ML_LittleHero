using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
	[SerializeField] RectTransform rectTransform;
	[SerializeField] Canvas canvas;

	public void OnPointerDown(PointerEventData eventData)
	{
		rectTransform.transform.SetAsLastSibling();
	}

	public void OnDrag(PointerEventData eventData)
	{
		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
	}
}
