using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
	public InventoryIcon currentItem;
	public int slotIndex;
	[SerializeField] Button button;
	public SkillIcon currentSkill;
	private Vector3 spawnPosition;
	[SerializeField] TMP_Text timeText;
	[SerializeField] Image hideImage;
	private float coolTime;
	private float time;
	private bool isCooldown = false;

	private void Awake()
	{
		button.onClick.AddListener(Use);
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		SkillIcon draggedSkill = eventData.pointerDrag.GetComponent<SkillIcon>();

		if (draggedItem != null && isCooldown)
		{
			if(draggedItem.quickSlot != null)
			{
				draggedItem.transform.SetParent(draggedItem.quickSlot.transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
			else
			{
				draggedItem.ReturnToInventory();
			}
			return;
		}
		else if(draggedSkill != null && isCooldown)
		{
			if (draggedSkill.quickSlot != null)
			{
				draggedSkill.transform.SetParent(draggedSkill.quickSlot.transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
			return;
		}

		if (draggedItem != null && (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion))
		{
			QuickSlot originalSlot = draggedItem.quickSlot;
			InventorySlot previousSlot = draggedItem.parentSlot;
			InventoryIcon tempItem = currentItem;

			if (currentSkill != null && draggedItem.parentSlot != null)
			{
				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData("", 0));
				Destroy(currentSkill.gameObject);
				currentSkill = null;
			}
			else if (currentSkill != null && draggedItem.parentSlot == null)
			{
				SkillIcon tempSkill = currentSkill;

				originalSlot.currentItem = null;
				originalSlot.currentSkill = tempSkill;
				tempSkill.transform.SetParent(originalSlot.transform);
				tempSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				tempSkill.quickSlot = originalSlot;

				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempSkill.name, 0));

				currentItem = draggedItem;
				currentSkill = null;
				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				draggedItem.quickSlot = this;

				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.itemName, draggedItem.quantity));
				return;
			}

			if (currentItem == null)
			{
				if (originalSlot != null)
				{
					originalSlot.currentItem = null;
				}
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
			QuickSlot originalSlot = draggedSkill.quickSlot;
			SkillIcon tempSkill = currentSkill;

			if (currentItem != null)
			{
				originalSlot.currentItem = currentItem;
				originalSlot.currentSkill = null;
				currentItem.transform.SetParent(originalSlot.transform);
				currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				currentItem.quickSlot = originalSlot;

				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(currentItem.itemName, currentItem.quantity));

				currentSkill = draggedSkill;
				currentItem = null;
				draggedSkill.transform.SetParent(transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				draggedSkill.quickSlot = this;

				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(draggedSkill.name, 0));
				return;
			}

			if (currentSkill == null)
			{
				currentSkill = draggedSkill;
				draggedSkill.transform.SetParent(transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				originalSlot.currentSkill = null;
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(draggedSkill.name, 0));
				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData("", 0));
			}
			else
			{
				originalSlot.currentSkill = tempSkill;
				tempSkill.transform.SetParent(originalSlot.transform);
				tempSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				tempSkill.quickSlot = originalSlot;
				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempSkill.name, 0));

				currentSkill = draggedSkill;
				draggedSkill.transform.SetParent(transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(draggedSkill.name, 0));
			}
			draggedSkill.quickSlot = this;
		}
	}

	public void Use()
	{
		if (isCooldown)
		{
			return;
		}

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
			coolTime = 3f;
			StartCoroutine(SkillTimeCheck());
		}
		else if (currentSkill != null && !Manager.Game.player.IsAttackCooltime && !Manager.Game.player.IsAttack && !(Manager.Data.UserData.mana <= 0))
		{
			Manager.Data.UserData.Mana -= 20;
			Manager.Game.player.SkillAttack(currentSkill.skillName);
			coolTime = 5f;
			StartCoroutine(SkillTimeCheck());
		}
	}

	public void SetSkill(string skillName)
	{
		QuickSlot[] quickSlots = FindObjectsOfType<QuickSlot>();
		foreach (QuickSlot slot in quickSlots)
		{
			if (slot.currentSkill != null && slot.currentSkill.name == skillName)
			{
				if(slot.isCooldown)
				{
					return;
				}
				Manager.Fire.SavePotionQuickSlot(slot.slotIndex, new InventorySlotData("", 0));
				Destroy(slot.currentSkill.gameObject);
				slot.currentSkill = null;
			}
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
			if (currentItem != null)
			{
				currentItem.ReturnToInventory();
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				currentItem.quickSlot = null;
				currentItem = null;
			}

			if (currentSkill != null)
			{
				Destroy(currentSkill.gameObject);
			}

			currentSkill = Instantiate(skillPrefab, transform).GetComponent<SkillIcon>();
			RectTransform skillRect = currentSkill.GetComponent<RectTransform>();
			skillRect.anchoredPosition = Vector2.zero;
			currentSkill.name = skillName;
			currentSkill.quickSlot = this;
			Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentSkill.name, 0));
		}
	}

	private IEnumerator SkillTimeCheck()
	{
		isCooldown = true;
		button.enabled = false;
		hideImage.gameObject.SetActive(true);
		hideImage.transform.SetAsLastSibling();
		timeText.transform.SetAsLastSibling();
		time = coolTime;
		timeText.gameObject.SetActive(true);
		while (time > 0)
		{
			timeText.text = time.ToString("00");
			hideImage.fillAmount = time / coolTime;
			time -= Time.deltaTime;
			yield return null;
		}
		timeText.gameObject.SetActive(false);
		hideImage.gameObject.SetActive(false);
		button.enabled = true;
		if (currentItem != null)
		{
			currentItem.transform.SetAsLastSibling();
		}
		else if(currentSkill != null)
		{
			currentSkill.transform.SetAsLastSibling();
		}
		isCooldown = false;
		yield return null;
	}
}
