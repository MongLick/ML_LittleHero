using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public string skillName;
	public Image skillImage;
	private Image skillCopy; 
	private Canvas canvas;
	public QuickSlot quickSlot;
	public SkillIcon targetSkillIcon;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		skillCopy = new GameObject("SkillCopy").AddComponent<Image>();
		skillCopy.sprite = skillImage.sprite;
		skillCopy.transform.SetParent(canvas.transform);
		skillCopy.raycastTarget = false;

		RectTransform copyRect = skillCopy.GetComponent<RectTransform>();
		copyRect.sizeDelta = skillImage.rectTransform.sizeDelta;
		copyRect.position = skillImage.rectTransform.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (skillCopy != null)
		{
			skillCopy.transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		quickSlot = eventData.pointerEnter?.GetComponent<QuickSlot>();
		targetSkillIcon = eventData.pointerEnter?.GetComponent<SkillIcon>();

		if (quickSlot != null)
		{
			quickSlot.SetSkill(skillName);
		}
		else if (targetSkillIcon != null)
		{
			QuickSlot parentQuickSlot = targetSkillIcon.GetComponentInParent<QuickSlot>();
			if (parentQuickSlot != null)
			{
				parentQuickSlot.SetSkill(skillName);
			}
		}

		Destroy(skillCopy);
		skillCopy = null;
	}
}