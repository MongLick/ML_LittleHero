using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
	[Header("Components")]
	[SerializeField] InventoryIcon currentItem;
	public InventoryIcon CurrentItem { get { return currentItem; } set { currentItem = value; } }
	[SerializeField] SkillIcon currentSkill;
	public SkillIcon CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
	[SerializeField] Button button;
	[SerializeField] TMP_Text timeText;
	[SerializeField] Image hideImage;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] PlayerController player;

	[Header("Vector")]
	private Vector3 spawnPosition;

	[Header("Specs")]
	[SerializeField] float coolTime;
	[SerializeField] float time;
	[SerializeField] int slotIndex;
	public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
	private bool isCooldown;

	private void Awake()
	{
		button.onClick.AddListener(Use);
		button.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	public void OnDrop(PointerEventData eventData)
	{
		InventoryIcon draggedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
		SkillIcon draggedSkill = eventData.pointerDrag.GetComponent<SkillIcon>();

		if (draggedItem != null && isCooldown)
		{
			if (draggedItem.QuickSlot != null)
			{
				draggedItem.transform.SetParent(draggedItem.QuickSlot.transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
			else
			{
				draggedItem.ReturnToInventory();
			}
			return;
		}
		else if (draggedSkill != null && isCooldown)
		{
			if (draggedSkill.QuickSlot != null)
			{
				draggedSkill.transform.SetParent(draggedSkill.QuickSlot.transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
			return;
		}

		if (draggedItem != null && (draggedItem.slotType == InventoryIcon.SlotType.hpPotion || draggedItem.slotType == InventoryIcon.SlotType.mpPotion))
		{
			QuickSlot originalSlot = draggedItem.QuickSlot;
			InventorySlot previousSlot = draggedItem.ParentSlot;
			InventoryIcon tempItem = currentItem;

			if (currentSkill != null && draggedItem.ParentSlot != null)
			{
				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData("", 0));
				Destroy(currentSkill.gameObject);
				currentSkill = null;
			}
			else if (currentSkill != null && draggedItem.ParentSlot == null)
			{
				SkillIcon tempSkill = currentSkill;

				originalSlot.currentItem = null;
				originalSlot.currentSkill = tempSkill;
				tempSkill.transform.SetParent(originalSlot.transform);
				tempSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				tempSkill.QuickSlot = originalSlot;

				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempSkill.name, 0));

				currentItem = draggedItem;
				currentSkill = null;
				draggedItem.transform.SetParent(transform);
				draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				draggedItem.QuickSlot = this;

				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.ItemName, draggedItem.Quantity));
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
				Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.ItemName, draggedItem.Quantity));
				if (previousSlot != null)
				{
					draggedItem.ParentSlot = null;
					previousSlot.CurrentItem = null;
					Manager.Fire.SaveItemToDatabase(Array.IndexOf(inventoryUI.InventorySlots, previousSlot), null);
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
						Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempItem.ItemName, tempItem.Quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.ItemName, draggedItem.Quantity));
				}
				else if (previousSlot != null)
				{
					previousSlot.CurrentItem = tempItem;
					if (tempItem != null)
					{
						tempItem.transform.SetParent(previousSlot.transform);
						tempItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
						int previousSlotIndex = Array.IndexOf(inventoryUI.InventorySlots, previousSlot);
						Manager.Fire.SavePotionToDatabase(previousSlotIndex, new InventorySlotData(tempItem.ItemName, tempItem.Quantity));
					}

					currentItem = draggedItem;
					draggedItem.transform.SetParent(transform);
					draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					Manager.Fire.SavePotionQuickSlot(slotIndex, new InventorySlotData(draggedItem.ItemName, draggedItem.Quantity));
				}
			}

			draggedItem.QuickSlot = this;
			if (tempItem != null)
			{
				tempItem.QuickSlot = originalSlot;
				tempItem.ParentSlot = previousSlot;
			}
		}
		else if (draggedSkill != null)
		{
			QuickSlot originalSlot = draggedSkill.QuickSlot;
			SkillIcon tempSkill = currentSkill;

			if (currentItem != null)
			{
				originalSlot.currentItem = currentItem;
				originalSlot.currentSkill = null;
				currentItem.transform.SetParent(originalSlot.transform);
				currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				currentItem.QuickSlot = originalSlot;

				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity));

				currentSkill = draggedSkill;
				currentItem = null;
				draggedSkill.transform.SetParent(transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				draggedSkill.QuickSlot = this;

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
				tempSkill.QuickSlot = originalSlot;
				Manager.Fire.SavePotionQuickSlot(originalSlot.slotIndex, new InventorySlotData(tempSkill.name, 0));

				currentSkill = draggedSkill;
				draggedSkill.transform.SetParent(transform);
				draggedSkill.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(draggedSkill.name, 0));
			}
			draggedSkill.QuickSlot = this;
		}
	}

	public void Use()
	{
		if (isCooldown || player.IsStunned)
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
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity));
				Manager.Data.UserData.Health += 20;
				if (Manager.Data.UserData.Health > Manager.Data.UserData.maxHealth)
				{
					Manager.Data.UserData.Health = Manager.Data.UserData.maxHealth;
				}
				if (currentItem.Quantity <= 0)
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
				Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData(currentItem.ItemName, currentItem.Quantity));
				Manager.Data.UserData.Mana += 20;
				if (Manager.Data.UserData.Mana > Manager.Data.UserData.maxMana)
				{
					Manager.Data.UserData.Mana = Manager.Data.UserData.maxMana;
				}
				if (currentItem.Quantity <= 0)
				{
					Destroy(currentItem.gameObject);
					currentItem = null;
					Manager.Fire.SavePotionQuickSlot(this.slotIndex, new InventorySlotData("", 0));
				}
			}
			coolTime = 3f;
			StartCoroutine(SkillTimeCheck());
		}
		else if (currentSkill != null && !Manager.Game.Player.IsAttackCooltime && !Manager.Game.Player.IsAttack && !(Manager.Data.UserData.mana <= 0))
		{
			Manager.Data.UserData.Mana -= 20;
			Manager.Game.Player.SkillAttack(currentSkill.SkillName);
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
				if (slot.isCooldown)
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
				currentItem.QuickSlot = null;
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
			currentSkill.QuickSlot = this;
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
		else if (currentSkill != null)
		{
			currentSkill.transform.SetAsLastSibling();
		}
		isCooldown = false;
		yield return null;
	}
}
