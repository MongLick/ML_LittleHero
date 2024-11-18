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
	[Header("Components")]
	[SerializeField] LayerMask playerLayer;
	public LayerMask PlayerLayer { get { return playerLayer; } set { playerLayer = value; } }
	[SerializeField] TMP_Text talkText;
	public TMP_Text TalkText { get { return talkText; } set { talkText = value; } }
	[SerializeField] Image talkBackImage;
	public Image TalkBackImage { get { return talkBackImage; } set { talkBackImage = value; } }
	[SerializeField] Button talkButton;
	public Button TalkButton { get { return talkButton; } set { talkButton = value; } }
	[SerializeField] Button closeButton;
	public Button CloseButton { get { return closeButton; } set { closeButton = value; } }
	[SerializeField] Shop shopBack;
	public Shop ShopBack { get { return shopBack; } set { shopBack = value; } }
	[SerializeField] GameObject characterInstance;
	[SerializeField] PlayerController character;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (PhotonNetwork.IsConnectedAndReady)
		{
			JoinMainRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
		Manager.Game.PoolEffect = FindAnyObjectByType<PoolEffect>();
		Manager.Sound.PlayBGM(Manager.Sound.LittleForestSoundClip);
	}

	public override void OnJoinedRoom()
	{
		LoadCharacterData();
	}

	public override void OnConnectedToMaster()
	{
		JoinMainRoom();
	}

	private void JoinMainRoom()
	{
		PhotonNetwork.JoinOrCreateRoom("RPG_MainRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
	}

	private void LoadCharacterData()
	{
		string side = Manager.Fire.IsLeft ? "Left" : "Right";
		Manager.Fire.DB
			.GetReference("UserData")
			.Child(Manager.Fire.UserID)
			.Child(side)
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					InitializeCharacter(task.Result, side);
				}
			});
	}

	private void InitializeCharacter(DataSnapshot snapshot, string side)
	{
		string nickName = snapshot.Child("nickName").Value.ToString();
		string type = snapshot.Child("type").Value.ToString();
		float posX = float.Parse(snapshot.Child("posX").Value.ToString());
		float posY = float.Parse(snapshot.Child("posY").Value.ToString());
		float posZ = float.Parse(snapshot.Child("posZ").Value.ToString());
		int health = int.Parse(snapshot.Child("health").Value.ToString());
		int mana = int.Parse(snapshot.Child("mana").Value.ToString());
		int gold = int.Parse(snapshot.Child("gold").Value.ToString());
		string weapon = snapshot.Child("weaponSlot").Value.ToString();
		string shield = snapshot.Child("shieldSlot").Value.ToString();
		string cloak = snapshot.Child("cloakSlot").Value.ToString();
		int qualityLevel = int.Parse(snapshot.Child("qualityLevel").Value.ToString());

		characterInstance = PhotonNetwork.Instantiate(
			type == "0" ? "ManPlayer" : "WoManPlayer",
			new Vector3(posX, posY, posZ),
			Quaternion.identity
		);

		Manager.Game.Player = FindAnyObjectByType<PlayerController>();
		TMP_Text nicknameUI = characterInstance.GetComponentInChildren<TMP_Text>();
		if (nicknameUI != null) nicknameUI.text = nickName;

		UserData.CharacterType characterType = (type == "0") ? UserData.CharacterType.Man : UserData.CharacterType.WoMan;
		Manager.Data.UserData = new UserData(nickName, characterType, side, posX, posY, posZ, "LittleForestScene", health, mana, gold, weapon, shield, cloak, new Dictionary<int, InventorySlotData>(), new Dictionary<string, QuestData>(), new InventorySlotData[4], qualityLevel);

		character = characterInstance.GetComponent<PlayerController>();
		InitializeUI(qualityLevel, weapon, shield, cloak, snapshot);

		Manager.Data.UserData.Gold = gold;
	}

	private void InitializeUI(int qualityLevel, string weapon, string shield, string cloak, DataSnapshot snapshot)
	{
		GraphicsUI graphicsUI = character.GraphicsUI;
		InventoryUI inventoryUI = character.InventoryUI;
		EquipmentUI equipmentUI = character.EquipmentUI;
		TMP_Dropdown qualityDropdown = character.Dropdown;

		QualitySettings.SetQualityLevel(qualityLevel);
		qualityDropdown.value = qualityLevel;

		EquipItems(weapon, shield, cloak, equipmentUI, inventoryUI);
		graphicsUI.gameObject.SetActive(false);

		var inventoryData = snapshot.Child("inventory");
		foreach (var item in inventoryData.Children)
		{
			int slotIndex = int.Parse(item.Key);
			string itemName = item.Child("itemName").Value.ToString();

			if (slotIndex < inventoryUI.InventorySlots.Length && !string.IsNullOrEmpty(itemName))
			{
				InventorySlot slot = inventoryUI.InventorySlots[slotIndex];
				if (item.Child("quantity").Exists)
				{
					int itemQuantity = int.Parse(item.Child("quantity").Value.ToString());
					shopBack.LoadPotion(itemName, slotIndex, itemQuantity, inventoryUI);
				}
				else
				{
					InventoryIcon newIcon = Instantiate(FindItemPrefab(itemName)).GetComponent<InventoryIcon>();
					slot.AddItem(newIcon);
				}
			}
		}

		var quickSlotData = snapshot.Child("quickSlots");
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
			EquipmentSlot weaponSlotComponent = equipmentUI.EquipmentSlots[0];

			if (weaponSlotComponent != null && weapon != null)
			{
				GameObject weaponInstance = Instantiate(weaponPrefab, weaponSlotComponent.transform.position, Quaternion.identity, weaponSlotComponent.transform);
				playerEquipment.EquipWeapon(weapon);

				InventoryIcon weaponIcon = weaponInstance.GetComponent<InventoryIcon>();
				weaponSlotComponent.CurrentItem = weaponIcon;
				weaponSlotComponent.CurrentItem.ItemName = weapon;
				weaponSlotComponent.CurrentItem.IsEquipment = true;
				weaponSlotComponent.CurrentItem.EquipmentSlot = weaponSlotComponent;
				weaponSlotComponent.CurrentItem.InventoryUI = inventoryUI;
			}
		}

		GameObject shieldPrefab = FindItemPrefab(shield);
		if (shieldPrefab != null)
		{
			EquipmentSlot shieldSlotComponent = equipmentUI.EquipmentSlots[1];

			if (shieldSlotComponent != null && shield != null)
			{
				GameObject shieldInstance = Instantiate(shieldPrefab, shieldSlotComponent.transform.position, Quaternion.identity, shieldSlotComponent.transform);
				playerEquipment.EquipShield(shield);

				InventoryIcon shieldIcon = shieldInstance.GetComponent<InventoryIcon>();
				shieldSlotComponent.CurrentItem = shieldIcon;
				shieldSlotComponent.CurrentItem.ItemName = shield;
				shieldSlotComponent.CurrentItem.IsEquipment = true;
				shieldSlotComponent.CurrentItem.EquipmentSlot = shieldSlotComponent;
				shieldSlotComponent.CurrentItem.InventoryUI = inventoryUI;
			}
		}

		GameObject cloakPrefab = FindItemPrefab(cloak);
		if (cloakPrefab != null)
		{
			EquipmentSlot cloakSlotComponent = equipmentUI.EquipmentSlots[2];

			if (cloakSlotComponent != null && cloak != null)
			{
				GameObject cloakInstance = Instantiate(cloakPrefab, cloakSlotComponent.transform.position, Quaternion.identity, cloakSlotComponent.transform);
				playerEquipment.EquipCloak(cloak);

				InventoryIcon cloakIcon = cloakInstance.GetComponent<InventoryIcon>();
				cloakSlotComponent.CurrentItem = cloakIcon;
				cloakSlotComponent.CurrentItem.ItemName = cloak;
				cloakIcon.IsEquipment = true;
				cloakIcon.EquipmentSlot = cloakSlotComponent;
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
			if (slot.SlotIndex == slotIndex)
			{
				GameObject prefabObject = Instantiate(prefab, slot.transform);
				InventoryIcon potionIcon = prefabObject.GetComponent<InventoryIcon>();
				SkillIcon skillIcon = prefabObject.GetComponent<SkillIcon>();

				if (potionIcon != null)
				{
					potionIcon.InventoryUI = character.InventoryUI;
					potionIcon.QuickSlot = slot;
					potionIcon.Quantity = itemQuantity;
					potionIcon.UpdateQuantityText();
					slot.CurrentItem = potionIcon;
					break;
				}
				if (skillIcon != null)
				{
					skillIcon.name = itemName;
					skillIcon.QuickSlot = slot;
					slot.CurrentSkill = skillIcon;
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
