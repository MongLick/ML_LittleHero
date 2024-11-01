using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
	public InventoryIcon currentItem;
	public int slotIndex;
	[SerializeField] Button button;
	public SkillIcon currentSkill;

	private void Awake()
	{
		button.onClick.AddListener(Use);
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		SkillIcon draggedSkill = eventData.pointerDrag.GetComponent<SkillIcon>();		

		if (draggedItem != null && (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion))
		{
			QuickSlot originalSlot = draggedItem.quickSlot;
			InventorySlot previousSlot = draggedItem.parentSlot;
			InventoryIcon tempItem = currentItem;

			if (currentItem == null)
			{
				currentItem = draggedItem;
				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				if (previousSlot != null)
				{
					draggedItem.parentSlot = null;
					previousSlot.currentItem = null;
					Manager.Fire.SaveItemToDatabase(Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, previousSlot), null);
				}
			}
			else
			{
				if (originalSlot != null)
				{
					originalSlot.currentItem = tempItem;
					if (tempItem != null)
					{
						tempItem.transform.SetParent(originalSlot.transform);
						tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempItem.itemName, tempItem.quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				}
				else if (previousSlot != null)
				{
					previousSlot.currentItem = tempItem;
					if (tempItem != null)
					{
						tempItem.transform.SetParent(previousSlot.transform);
						tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						int previousSlotIndex = Array.IndexOf(Manager.Inven.InventoryUI.InventorySlots, previousSlot);
						Manager.Fire.SavePotionToDatabase(previousSlotIndex, new InventorySlotData(tempItem.itemName, tempItem.quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				}
			}

			draggedItem.quickSlot = this;
			if (tempItem != null)
			{
				tempItem.quickSlot = originalSlot;
				tempItem.parentSlot = previousSlot;
			}
		}
		else if (draggedSkill != null)
		{
			if (!draggedSkill.GetComponentInParent<SkillUI>())
			{
				QuickSlot originalSlot = draggedSkill.quickSlot;
				SkillIcon tempSkill = currentSkill;

				if (currentSkill == null)
				{
					currentSkill = draggedSkill;
					draggedSkill.transform.SetParent(transform);
					draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				}
				else
				{
					if (originalSlot != null)
					{
						originalSlot.currentSkill = tempSkill;
						if (tempSkill != null)
						{
							tempSkill.transform.SetParent(originalSlot.transform);
							tempSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						}

						currentSkill = draggedSkill;
						draggedSkill.transform.SetParent(transform);
						draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					}
				}

				draggedSkill.quickSlot = this;
				if (tempSkill != null)
				{
					tempSkill.quickSlot = originalSlot;
				}
			}
		}
	}

	public void Use()
	{
		if (currentItem != null)
		{
			if (currentItem.slotType == InventoryIcon.SlotType.hpPotion)
			{
				if (Manager.Data.UserData.Health >= Manager.Data.UserData.maxHealth)
				{
					return;
				}
				currentItem.UpdateQuantity(-1);
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				Manager.Data.UserData.Health += 20;
				if (Manager.Data.UserData.Health > Manager.Data.UserData.maxHealth)
				{
					Manager.Data.UserData.Health = Manager.Data.UserData.maxHealth;
				}
				if (currentItem.quantity <= 0)
				{
					Destroy(currentItem.gameObject);
					currentItem = null;
					Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				}
			}
			else if (currentItem.slotType == InventoryIcon.SlotType.mpPotion)
			{
				if (Manager.Data.UserData.Mana >= Manager.Data.UserData.maxMana)
				{
					return;
				}
				currentItem.UpdateQuantity(-1);
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));
				Manager.Data.UserData.Mana += 20;
				if (Manager.Data.UserData.Mana > Manager.Data.UserData.maxMana)
				{
					Manager.Data.UserData.Mana = Manager.Data.UserData.maxMana;
				}
				if (currentItem.quantity <= 0)
				{
					Destroy(currentItem.gameObject);
					currentItem = null;
					Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				}
			}
		}
	}

	public void SetSkill(string skillName)
	{
		QuickSlot[] quickSlots = FindObjectsOfType<QuickSlot>();
		foreach (QuickSlot slot in quickSlots)
		{
			if (slot.currentSkill != null && slot.currentSkill.name == skillName)
			{
				Destroy(slot.currentSkill);
				slot.currentSkill = null;
			}
		}

		if (currentSkill != null)
		{
			Destroy(currentSkill);
			currentSkill = null;
		}

		GameObject skillPrefab = null;

		if (skillName == "Fire")
		{
			skillPrefab = Resources.Load<GameObject>("Prefabs/Fire");
		}
		else if (skillName == "Ice")
		{
			skillPrefab = Resources.Load<GameObject>("Prefabs/Ice");
		}

		if (skillPrefab != null)
		{
			currentSkill = Instantiate(skillPrefab, transform).GetComponent<SkillIcon>();
			currentSkill.name = skillName;
			currentSkill.quickSlot = this;
			RectTransform skillRect = currentSkill.GetComponent<RectTransform>();
			skillRect.anchoredPosition = Vector2.zero;
		}
	}
}
