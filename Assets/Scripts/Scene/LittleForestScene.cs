using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LittleForestScene : BaseScene
{
	private GameObject characterInstance;
	[SerializeField] TMP_Text talkText;
	public TMP_Text TalkText { get { return talkText; } set { talkText = value; } }
	[SerializeField] Image talkBackImage;
	public Image TalkBackImage { get { return talkBackImage; } set { talkBackImage = value; } }
	[SerializeField] LayerMask playerLayer;
	public LayerMask PlayerLayer { get { return playerLayer; } set { playerLayer = value; } }
	[SerializeField] Button talkButton;
	public Button TalkButton { get { return talkButton; } set { talkButton = value; } }
	[SerializeField] Button closeButton;
	public Button CloseButton { get { return closeButton; } set { closeButton = value; } }
	[SerializeField] Shop shopBack;
	public Shop ShopBack { get { return shopBack; } set { shopBack = value; } }

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.JoinOrCreateRoom("RPG_MainRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
		Manager.Game.poolEffect = FindAnyObjectByType<PoolEffect>();
		Manager.Sound.PlayBGM(Manager.Sound.LittleForestSoundClip);
	}

	public override void OnJoinedRoom()
	{
		LoadCharacterData();
	}

	private void LoadCharacterData()
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
					DataSnapshot leftSnapshot = task.Result;

					string nickName = leftSnapshot.Child("nickName").Value.ToString();
					string type = leftSnapshot.Child("type").Value.ToString();
					float posX = float.Parse(leftSnapshot.Child("posX").Value.ToString());
					float posY = float.Parse(leftSnapshot.Child("posY").Value.ToString());
					float posZ = float.Parse(leftSnapshot.Child("posZ").Value.ToString());
					int health = int.Parse(leftSnapshot.Child("health").Value.ToString());
					int mana = int.Parse(leftSnapshot.Child("mana").Value.ToString());
					int gold = int.Parse(leftSnapshot.Child("gold").Value.ToString());
					string weapon = leftSnapshot.Child("weaponSlot").Value.ToString();
					string shield = leftSnapshot.Child("shieldSlot").Value.ToString();
					string cloak = leftSnapshot.Child("cloakSlot").Value.ToString();
					int qualityLevel = int.Parse(leftSnapshot.Child("qualityLevel").Value.ToString());

					if (type == "0")
					{
						characterInstance = PhotonNetwork.Instantiate("ManPlayer", new Vector3(posX, posY, posZ), Quaternion.identity);
					}
					else
					{
						characterInstance = PhotonNetwork.Instantiate("WoManPlayer", new Vector3(posX, posY, posZ), Quaternion.identity);
					}

					Manager.Game.player = FindAnyObjectByType<PlayerController>();
					TMP_Text nicknameUI = characterInstance.GetComponentInChildren<TMP_Text>();
					if (nicknameUI != null)
					{
						nicknameUI.text = nickName;
					}
					UserData.CharacterType characterType = (type == "0") ? UserData.CharacterType.Man : UserData.CharacterType.WoMan;
					Manager.Data.UserData = new UserData(nickName, characterType, "Left", posX, posY, posZ, "LittleForestScene", health, mana, gold, weapon, shield, cloak, new Dictionary<int, InventorySlotData>(), new Dictionary<string, QuestData>(), new InventorySlotData[4], qualityLevel);
					PlayerController character = GetComponent<PlayerController>();
					GraphicsUI graphicsUI = character.graphicsUI;
					InventoryUI inventoryUI = character.inventoryUI;
					EquipmentUI equipmentUI = character.equipmentUI;
					TMP_Dropdown qualityDropdown = character.dropdown;
					QualitySettings.SetQualityLevel(qualityLevel);
					EquipItems(weapon, shield, cloak, equipmentUI, inventoryUI);
					qualityDropdown.value = qualityLevel;
					graphicsUI.gameObject.SetActive(false);
					var inventoryData = leftSnapshot.Child("inventory");

					foreach (var item in inventoryData.Children)
					{
						int slotIndex = int.Parse(item.Key);
						string itemName = item.Child("itemName").Value.ToString();

						if (slotIndex < inventoryUI.InventorySlots.Length)
						{
							InventorySlot slot = inventoryUI.InventorySlots[slotIndex];

							if (!string.IsNullOrEmpty(itemName))
							{
								if (item.Child("quantity").Exists)
								{
									int itemQuantity = int.Parse(item.Child("quantity").Value.ToString());
									shopBack.LoadPotion(itemName, slotIndex, itemQuantity);
								}
								else
								{
									InventoryIcon newIcon = Instantiate(FindItemPrefab(itemName)).GetComponent<InventoryIcon>();
									slot.AddItem(newIcon);
								}
							}
						}
					}

					var quickSlotData = leftSnapshot.Child("quickSlots");

					if (quickSlotData.Exists)
					{
						foreach (var slot in quickSlotData.Children)
						{
							int slotIndex = int.Parse(slot.Key);
							string itemName = slot.Child("itemName").Value.ToString();

							if (!string.IsNullOrEmpty(itemName))
							{
								int itemQuantity = int.Parse(slot.Child("quantity").Value.ToString());
								LoadQuickSlotItem(itemName, slotIndex, itemQuantity);
							}
						}
					}

					Manager.Data.UserData.Gold = gold;
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

					string nickName = rightSnapshot.Child("nickName").Value.ToString();
					string type = rightSnapshot.Child("type").Value.ToString();
					float posX = float.Parse(rightSnapshot.Child("posX").Value.ToString());
					float posY = float.Parse(rightSnapshot.Child("posY").Value.ToString());
					float posZ = float.Parse(rightSnapshot.Child("posZ").Value.ToString());
					int health = int.Parse(rightSnapshot.Child("health").Value.ToString());
					int mana = int.Parse(rightSnapshot.Child("mana").Value.ToString());
					int gold = int.Parse(rightSnapshot.Child("gold").Value.ToString());
					string weapon = rightSnapshot.Child("weaponSlot").Value.ToString();
					string shield = rightSnapshot.Child("shieldSlot").Value.ToString();
					string cloak = rightSnapshot.Child("cloakSlot").Value.ToString();
					int qualityLevel = int.Parse(rightSnapshot.Child("qualityLevel").Value.ToString());

					if (type == "0")
					{
						characterInstance = PhotonNetwork.Instantiate("ManPlayer", new Vector3(posX, posY, posZ), Quaternion.identity);
					}
					else
					{
						characterInstance = PhotonNetwork.Instantiate("WoManPlayer", new Vector3(posX, posY, posZ), Quaternion.identity);
					}

					Manager.Game.player = FindAnyObjectByType<PlayerController>();
					TMP_Text nicknameUI = characterInstance.GetComponentInChildren<TMP_Text>();
					if (nicknameUI != null)
					{
						nicknameUI.text = nickName;
					}
					UserData.CharacterType characterType = (type == "0") ? UserData.CharacterType.Man : UserData.CharacterType.WoMan;
					Manager.Data.UserData = new UserData(nickName, characterType, "Left", posX, posY, posZ, "LittleForestScene", health, mana, gold, weapon, shield, cloak, new Dictionary<int, InventorySlotData>(), new Dictionary<string, QuestData>(), new InventorySlotData[4], qualityLevel);
					PlayerController character = characterInstance.GetComponent<PlayerController>();
					GraphicsUI graphicsUI = character.graphicsUI;
					InventoryUI inventoryUI = character.inventoryUI;
					EquipmentUI equipmentUI = character.equipmentUI;
					TMP_Dropdown qualityDropdown = character.dropdown;
					graphicsUI.gameObject.SetActive(true);
					inventoryUI.gameObject.SetActive(true);
					equipmentUI.gameObject.SetActive(true);
					QualitySettings.SetQualityLevel(qualityLevel);
					EquipItems(weapon, shield, cloak, equipmentUI, inventoryUI);
					qualityDropdown.value = qualityLevel;
					graphicsUI.gameObject.SetActive(false);
					var inventoryData = rightSnapshot.Child("inventory");

					foreach (var item in inventoryData.Children)
					{
						int slotIndex = int.Parse(item.Key);
						string itemName = item.Child("itemName").Value.ToString();

						if (slotIndex < inventoryUI.InventorySlots.Length)
						{
							InventorySlot slot = inventoryUI.InventorySlots[slotIndex];

							if (!string.IsNullOrEmpty(itemName))
							{
								if (!string.IsNullOrEmpty(itemName))
								{
									if (item.Child("quantity").Exists)
									{
										int itemQuantity = int.Parse(item.Child("quantity").Value.ToString());
										shopBack.LoadPotion(itemName, slotIndex, itemQuantity);
									}
									else
									{
										InventoryIcon newIcon = Instantiate(FindItemPrefab(itemName)).GetComponent<InventoryIcon>();
										slot.AddItem(newIcon);
									}
								}
							}
						}
					}

					var quickSlotData = rightSnapshot.Child("quickSlots");

					if (quickSlotData.Exists)
					{
						foreach (var slot in quickSlotData.Children)
						{
							int slotIndex = int.Parse(slot.Key);
							string itemName = slot.Child("itemName").Value.ToString();

							if (!string.IsNullOrEmpty(itemName))
							{
								Debug.Log(itemName);
								int itemQuantity = int.Parse(slot.Child("quantity").Value.ToString());
								LoadQuickSlotItem(itemName, slotIndex, itemQuantity);
							}
						}
					}

					Manager.Data.UserData.Gold = gold;
				}
			});
		}
	}

	private void EquipItems(string weapon, string shield, string cloak, EquipmentUI equipmentUI, InventoryUI inventoryUI)
	{
		PlayerEquipment playerEquipment = characterInstance.GetComponent<PlayerEquipment>();
		if (equipmentUI != null)
		{
			equipmentUI.gameObject.SetActive(true);
			inventoryUI.gameObject.SetActive(true);
		}

		GameObject weaponPrefab = FindItemPrefab(weapon);
		if (weaponPrefab != null)
		{
			EquipmentSlot weaponSlotComponent = equipmentUI.equipmentSlots[0];

			if (weaponSlotComponent != null && weapon != null)
			{
				GameObject weaponInstance = Instantiate(weaponPrefab, weaponSlotComponent.transform.position, Quaternion.identity, weaponSlotComponent.transform);
				playerEquipment.EquipWeapon(weapon);

				InventoryIcon weaponIcon = weaponInstance.GetComponent<InventoryIcon>();
				weaponSlotComponent.currentItem = weaponIcon;
				weaponSlotComponent.currentItem.itemName = weapon;
				weaponSlotComponent.currentItem.isEquipment = true;
				weaponSlotComponent.currentItem.equipmentSlot = weaponSlotComponent;
				weaponSlotComponent.currentItem.InventoryUI = inventoryUI;
			}
		}

		GameObject shieldPrefab = FindItemPrefab(shield);
		if (shieldPrefab != null)
		{
			EquipmentSlot shieldSlotComponent = equipmentUI.equipmentSlots[1];

			if (shieldSlotComponent != null && shield != null)
			{
				GameObject shieldInstance = Instantiate(shieldPrefab, shieldSlotComponent.transform.position, Quaternion.identity, shieldSlotComponent.transform);
				playerEquipment.EquipShield(shield);

				InventoryIcon shieldIcon = shieldInstance.GetComponent<InventoryIcon>();
				shieldSlotComponent.currentItem = shieldIcon;
				shieldSlotComponent.currentItem.itemName = shield;
				shieldSlotComponent.currentItem.isEquipment = true;
				shieldSlotComponent.currentItem.equipmentSlot = shieldSlotComponent;
				shieldSlotComponent.currentItem.InventoryUI = inventoryUI;
			}
		}

		GameObject cloakPrefab = FindItemPrefab(cloak);
		if (cloakPrefab != null)
		{
			EquipmentSlot cloakSlotComponent = equipmentUI.equipmentSlots[2];

			if (cloakSlotComponent != null && cloak != null)
			{
				GameObject cloakInstance = Instantiate(cloakPrefab, cloakSlotComponent.transform.position, Quaternion.identity, cloakSlotComponent.transform);
				playerEquipment.EquipCloak(cloak);

				InventoryIcon cloakIcon = cloakInstance.GetComponent<InventoryIcon>();
				cloakSlotComponent.currentItem = cloakIcon;
				cloakSlotComponent.currentItem.itemName = cloak;
				cloakIcon.isEquipment = true;
				cloakIcon.equipmentSlot = cloakSlotComponent;
				cloakIcon.InventoryUI = inventoryUI;
			}
		}

		if (equipmentUI != null)
		{
			equipmentUI.gameObject.SetActive(false);
			inventoryUI.gameObject.SetActive(false);
		}
	}

	private void LoadQuickSlotItem(string itemName, int slotIndex, int itemQuantity)
	{
		GameObject prefab;

		if (itemName == "hpPotion")
		{
			prefab = Resources.Load<GameObject>("Prefabs/hpPotion");
		}
		else if (itemName == "mpPotion")
		{
			prefab = Resources.Load<GameObject>("Prefabs/mpPotion");
		}
		else if (itemName == "Fire")
		{
			prefab = Resources.Load<GameObject>("Prefabs/Fire");
		}
		else if (itemName == "Ice")
		{
			prefab = Resources.Load<GameObject>("Prefabs/Ice");
		}
		else
		{
			return;
		}

		QuickSlot[] quickSlots = FindObjectsOfType<QuickSlot>();

		foreach (QuickSlot slot in quickSlots)
		{
			if (slot.slotIndex == slotIndex)
			{
				GameObject prefabObject = Instantiate(prefab, slot.transform);
				InventoryIcon potionIcon = prefabObject.GetComponent<InventoryIcon>();
				SkillIcon skillIcon = prefabObject.GetComponent<SkillIcon>();

				if (potionIcon != null)
				{
					potionIcon.quickSlot = slot;
					potionIcon.quantity = itemQuantity;
					potionIcon.UpdateQuantityText();
					slot.currentItem = potionIcon;
					break;
				}
				if (skillIcon != null)
				{
					skillIcon.name = itemName;
					skillIcon.quickSlot = slot;
					slot.currentSkill = skillIcon;
					break;
				}
			}
		}
	}

	private GameObject FindItemPrefab(string itemName)
	{
		return Resources.Load<GameObject>($"Prefabs/{itemName}");
	}

	private void Close()
	{
		talkBackImage.gameObject.SetActive(false);
	}

	public override IEnumerator LoadingRoutine()
	{
		yield return null;
	}
}
