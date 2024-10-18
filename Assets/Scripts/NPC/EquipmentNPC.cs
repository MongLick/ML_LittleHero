using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentNPC : MonoBehaviour
{
	[SerializeField] InteractAdapter interactAdapter;
	[SerializeField] LittleForestScene scene;

	private void Awake()
	{
		interactAdapter.OnInteracted.AddListener(OnInteract);
		scene.TalkButton.onClick.AddListener(() => OnInteract(null));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(false);
			scene.TalkBackImage.gameObject.SetActive(false);
		}
	}

	private void OnInteract(PlayerController player)
	{
		scene.TalkButton.gameObject.SetActive(false);
		scene.TalkBackImage.gameObject.SetActive(true);

		string questID = "firstQuest";
		string questName = "ù ��° ����Ʈ";
		string position = Manager.Fire.IsLeft ? "Left" : "Right";

		Manager.Fire.LoadQuestData(position, questID, (questData) =>
		{
			if (questData == null)
			{
				scene.TalkText.text = "���ο� �����̱� ��� ���������� ��� �����غ�";

				Manager.Fire.AddQuest(position, questID, questName);

				List<string> itemNames = new List<string> { "cloak1", "cloak2", "shield1", "shield2", "sword1", "sword2" };

				InventorySlot[] inventorySlots = Manager.Inven.InventoryUI.InventorySlots;
				int addedItems = 0;

				foreach (string itemName in itemNames)
				{
					InventoryIcon newItemPrefab = Resources.Load<InventoryIcon>($"Prefabs/{itemName}");
					if (newItemPrefab != null)
					{
						for (int i = 0; i < inventorySlots.Length; i++)
						{
							if (inventorySlots[i].currentItem == null)
							{
								InventoryIcon newItem = Instantiate(newItemPrefab, inventorySlots[i].transform);
								newItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
								inventorySlots[i].currentItem = newItem;
								addedItems++;
								Manager.Fire.SaveItemToDatabase(i, itemName);
								break;
							}
						}
					}
				}
			}
			else if (!questData.isCompleted)
			{
				if (Manager.Fire.IsLeft)
				{
					Manager.Fire.DB
					.GetReference("UserData")
					.Child(Manager.Fire.UserID)
					.Child("Left")
					.GetValueAsync()
					.ContinueWithOnMainThread(task =>
					{
						if (task.IsCompleted && task.Result != null)
						{
							DataSnapshot LefttSnapshot = task.Result;
							string weapon = LefttSnapshot.Child("weaponSlot").Value?.ToString();
							string shield = LefttSnapshot.Child("shieldSlot").Value?.ToString();
							string cloak = LefttSnapshot.Child("cloakSlot").Value?.ToString();

							if (string.IsNullOrEmpty(weapon) || string.IsNullOrEmpty(shield) || string.IsNullOrEmpty(cloak))
							{
								scene.TalkText.text = "���� ��� �������� �ʾҾ�. ����, ����, ���� ��� �����غ�.";
							}
							else
							{
								scene.TalkText.text = "��� ��� �����߱�! ���� ���� ����Ʈ�� �����ص� ����.";
								Manager.Fire.DB
								.GetReference("UserData")
								.Child(Manager.Fire.UserID)
								.Child("Left")
								.Child("quests")
								.Child(questID)
								.Child("isCompleted")
								.SetValueAsync(true)
								.ContinueWithOnMainThread(task =>
								{

								});
							}
						}
					});
				}
				else
				{
					Manager.Fire.DB
					.GetReference("UserData")
					.Child(Manager.Fire.UserID)
					.Child("Right")
					.GetValueAsync()
					.ContinueWithOnMainThread(task =>
					{
						if (task.IsCompleted && task.Result != null)
						{
							DataSnapshot rightSnapshot = task.Result;
							string weapon = rightSnapshot.Child("weaponSlot").Value?.ToString();
							string shield = rightSnapshot.Child("shieldSlot").Value?.ToString();
							string cloak = rightSnapshot.Child("cloakSlot").Value?.ToString();
							if (string.IsNullOrEmpty(weapon) || string.IsNullOrEmpty(shield) || string.IsNullOrEmpty(cloak))
							{
								scene.TalkText.text = "���� ��� �������� �ʾҾ�. ��� �����غ�.";
							}
							else
							{
								scene.TalkText.text = "��� ��� �����߱�! ���� ���� ����Ʈ�� �����ص� ����.";
								Manager.Fire.DB
								.GetReference("UserData")
								.Child(Manager.Fire.UserID)
								.Child("Right")
								.Child("quests")
								.Child(questID)
								.Child("isCompleted")
								.SetValueAsync(true)
								.ContinueWithOnMainThread(task =>
								{

								});
							}
						}
					});
				}
			}
			else
			{
				string secondQuestID = "secondQuest";
				Manager.Fire.DB
				.GetReference("UserData")
				.Child(Manager.Fire.UserID)
				.Child(Manager.Fire.IsLeft ? "Left" : "Right")
				.Child("quests")
				.Child(secondQuestID)
				.GetValueAsync()
				.ContinueWithOnMainThread(task =>
				{
					if (task.IsCompleted && task.Result != null)
					{
						DataSnapshot secondQuestSnapshot = task.Result;
						bool isSecondQuestCompleted = secondQuestSnapshot.Child("isCompleted").Value != null && (bool)secondQuestSnapshot.Child("isCompleted").Value;

						if (isSecondQuestCompleted)
						{
							scene.TalkText.text = "�Ϻ��ϱ� ���� ������ο��� �������� ��";
						}
						else
						{
							if (secondQuestSnapshot.Child("questName").Value == null)
							{
								scene.TalkText.text = "���� ���� 3����, ������ ���� 3������ ��� �Ͷ�.";
								Manager.Fire.AddQuest(position, secondQuestID, "�� ��° ����Ʈ");
							}
							else
							{
								scene.TalkText.text = "���� ���� 3����, ������ ���� 3������ ��� �ٽ� ���� �ɾ��";
							}
						}
					}
				});
			}
		});
	}
}
