using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentNPC : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] InteractAdapter interactAdapter;
	[SerializeField] LittleForestScene scene;
	[SerializeField] PlayerController playerController;

	[Header("Specs")]
	private bool isInteract;

	private void Awake()
	{
		interactAdapter.OnInteracted.AddListener(OnInteract);
		scene.TalkButton.onClick.AddListener(() => OnInteract(null));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			PhotonView photonView = other.GetComponent<PhotonView>();
			if (photonView.IsMine)
			{
				scene.TalkButton.gameObject.SetActive(true);
				isInteract = true;
				playerController = other.GetComponent<PlayerController>();
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			if (playerController == null)
			{
				playerController = other.GetComponent<PlayerController>();
			}
			PhotonView photonView = other.GetComponent<PhotonView>();
			if (photonView.IsMine)
			{
				scene.TalkButton.gameObject.SetActive(true);
				isInteract = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			PhotonView photonView = other.GetComponent<PhotonView>();
			if (photonView.IsMine)
			{
				scene.TalkButton.gameObject.SetActive(false);
				scene.TalkBackImage.gameObject.SetActive(false);
				isInteract = false;
				playerController = null;
			}
		}
	}

	private void OnInteract(PlayerController player)
	{
		if (!isInteract)
		{
			return;
		}
		scene.TalkButton.gameObject.SetActive(false);
		scene.TalkBackImage.gameObject.SetActive(true);
		scene.ShopBack.gameObject.SetActive(false);

		string questID = "firstQuest";
		string questName = "ù ��° ����Ʈ";
		string position = Manager.Fire.IsLeft ? "Left" : "Right";
		InventoryUI inventoryUI = playerController.InventoryUI;
		Manager.Fire.LoadQuestData(position, questID, (questData) =>
		{
			if (questData == null)
			{
				scene.TalkText.text = "���ο� �����̱� ��� ���������� ��� �����غ�";
				Manager.Fire.AddQuest(position, questID, questName);
				List<string> itemNames = new List<string> { "cloak1", "cloak2", "shield1", "shield2", "sword1", "sword2" };
				int addedItems = 0;
				foreach (string itemName in itemNames)
				{
					InventoryIcon newItemPrefab = Resources.Load<InventoryIcon>($"Prefabs/{itemName}");
					Debug.Log(newItemPrefab);
					if (newItemPrefab != null)
					{
						for (int i = 0; i < inventoryUI.InventorySlots.Length; i++)
						{
							if (inventoryUI.InventorySlots[i].CurrentItem == null)
							{
								InventoryIcon newItem = Instantiate(newItemPrefab, inventoryUI.InventorySlots[i].transform);
								newItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
								newItem.InventoryUI = inventoryUI;
								inventoryUI.InventorySlots[i].CurrentItem = newItem;
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
				Manager.Fire.DB
				.GetReference("UserData")
				.Child(Manager.Fire.UserID)
				.Child(position)
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
								.Child(position)
								.Child("quests")
								.Child(questID)
								.Child("isCompleted")
								.SetValueAsync(true)
								.ContinueWithOnMainThread(task =>
								{
									string secondQuestID = "secondQuest";
									string secondQuestName = "�� ��° ����Ʈ";
									Manager.Fire.AddQuest(position, secondQuestID, secondQuestName);
									scene.TalkText.text = "���� ���� 3����, ������ ���� 3������ ��� �Ͷ�.";
								});
						}
					}
				});
			}
			else
			{
				string secondQuestID = "secondQuest";
				Manager.Fire.DB
				.GetReference("UserData")
				.Child(Manager.Fire.UserID)
				.Child(position)
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
							scene.TalkText.text = "���� ���� 3����, ������ ���� 3������ ��� �ٽ� ���� �ɾ��";
						}
					}
				});
			}
		});
	}
}
