using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUIIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Canvas canvas;
	public string skillName;
	private Image skillCopy;
	public Image skillImage;
	public QuickSlot quickSlot;
	public SkillIcon targetSkillIcon;
	private InventoryIcon targetInventoryIcon;

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
		targetInventoryIcon = eventData.pointerEnter?.GetComponent<InventoryIcon>();

		if (quickSlot != null)
		{
			if (quickSlot.currentItem != null)
			{
				targetInventoryIcon = quickSlot.GetComponentInChildren<InventoryIcon>();
			}
			else if (quickSlot.currentSkill != null)
			{
				targetSkillIcon = quickSlot.GetComponentInChildren<SkillIcon>();
			}
			quickSlot.SetSkill(skillName);
			quickSlot = null;
		}
		else if (targetSkillIcon != null)
		{
			QuickSlot parentQuickSlot = targetSkillIcon.GetComponentInParent<QuickSlot>();
			if (parentQuickSlot != null)
			{
				parentQuickSlot.SetSkill(skillName);
				targetSkillIcon = null;
			}
		}
		else if(targetInventoryIcon != null)
		{
			QuickSlot parentQuickSlot = targetInventoryIcon.GetComponentInParent<QuickSlot>();
			if (parentQuickSlot != null)
			{
				parentQuickSlot.SetSkill(skillName);
				targetInventoryIcon = null;
			}
		}

		Destroy(skillCopy.gameObject);
		skillCopy = null;
	}
}