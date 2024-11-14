using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using static MonsterController;
using static UserData;

public class FirebaseManager : Singleton<FirebaseManager>
{
	[Header("Components")]
	[SerializeField] FirebaseApp app;
	public FirebaseApp App { get { return app; } }
	[SerializeField] FirebaseAuth auth;
	public FirebaseAuth Auth { get { return auth; } }
	[SerializeField] FirebaseDatabase db;
	public FirebaseDatabase DB { get { return db; } }
	[SerializeField] GameObject manPrefab;
	public GameObject ManPrefab { get { return manPrefab; } }
	[SerializeField] GameObject woManPrefab;
	public GameObject WoManPrefab { get { return woManPrefab; } }

	[Header("Specs")]
	[SerializeField] string userID;
	public string UserID { get { return userID; } set { userID = value; } }
	private bool isValid;
	public bool IsValid { get { return isValid; } }
	private bool isLeft;
	public bool IsLeft { get { return isLeft; } set { isLeft = value; } }

	protected override void Awake()
	{
		base.Awake();
		CheckDependency();
	}

	private async void CheckDependency()
	{
		DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
		if (dependencyStatus == DependencyStatus.Available)
		{
			app = FirebaseApp.DefaultInstance;
			auth = FirebaseAuth.DefaultInstance;
			db = FirebaseDatabase.DefaultInstance;
			isValid = true;
		}
		else
		{
			isValid = false;
			app = null;
			auth = null;
			db = null;
		}
	}

	private DatabaseReference GetUserReference(string position) =>
		db.GetReference("UserData").Child(userID).Child(position);

	public void CreateCharacter(
		string nickName, CharacterType type, string position, float x, float y, float z,
		string scene, int health, int mana, int gold, string weaponSlot, string shieldSlot,
		string cloakSlot, Dictionary<int, InventorySlotData> inventory,
		Dictionary<string, QuestData> quests, InventorySlotData[] quick, int qualityLevel)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		UserData userData = new UserData(nickName, type, position, x, y, z, scene, health, mana, gold, weaponSlot, shieldSlot, cloakSlot, inventory, quests, quick, qualityLevel);
		string json = JsonUtility.ToJson(userData);

		GetUserReference(position).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => { });
	}

	public void UpdateEquipmentSlot(string position, string slotType, string item)
	{
		GetUserReference(position).Child(slotType).SetValueAsync(item).ContinueWithOnMainThread(task => { });
	}

	public void AddQuest(string position, string questID, string questName)
	{
		QuestData newQuest = new QuestData(questID, questName, false);
		string json = JsonUtility.ToJson(newQuest);
		GetUserReference(position).Child("quests").Child(questID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => { });
	}

	public void LoadQuestData(string position, string questID, System.Action<QuestData> callback)
	{
		GetUserReference(position).Child("quests").Child(questID).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted)
			{
				callback(null);
				return;
			}

			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;
				callback(snapshot.Exists ? JsonUtility.FromJson<QuestData>(snapshot.GetRawJsonValue()) : null);
			}
		});
	}

	public void SaveItemToDatabase(int slotIndex, string itemName)
	{
		string position = isLeft ? "Left" : "Right";
		GetUserReference(position).Child("inventory").Child(slotIndex.ToString())
			.SetValueAsync(new Dictionary<string, object> { { "itemName", itemName } })
			.ContinueWithOnMainThread(task => { });
	}

	public void OnMonsterDie(MonsterType type)
	{
		string questID = "secondQuest";
		string position = isLeft ? "Left" : "Right";
		GetUserReference(position).Child("quests").Child(questID).GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot questSnapshot = task.Result;
					int mushroomCount = int.TryParse(questSnapshot.Child("mushroomCount").Value?.ToString(), out mushroomCount) ? mushroomCount : 0;
					int cactusCount = int.TryParse(questSnapshot.Child("cactusCount").Value?.ToString(), out cactusCount) ? cactusCount : 0;

					if (type == MonsterType.Mushroom)
					{
						questSnapshot.Child("mushroomCount").Reference.SetValueAsync(++mushroomCount);
					}
					else if (type == MonsterType.Cactus)
					{
						questSnapshot.Child("cactusCount").Reference.SetValueAsync(++cactusCount);
					}

					if (mushroomCount >= 3 && cactusCount >= 3)
					{
						questSnapshot.Child("isCompleted").Reference.SetValueAsync(true);
					}
				}
			});
	}

	public void UpdateGoldInDatabase(int newGold)
	{
		string position = isLeft ? "Left" : "Right";
		GetUserReference(position).Child("gold").GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && !task.IsFaulted)
				{
					int currentGold = int.Parse(task.Result.Value.ToString());
					GetUserReference(position).Child("gold").SetValueAsync(currentGold + newGold).ContinueWithOnMainThread(updateTask => { });
				}
			});
	}

	public void SavePotionData(int slotIndex, InventorySlotData inventorySlotData, bool isQuickSlot)
	{
		string position = isLeft ? "Left" : "Right";
		string slotType = isQuickSlot ? "quickSlots" : "inventory";

		GetUserReference(position).Child(slotType).Child(slotIndex.ToString())
			.SetValueAsync(new Dictionary<string, object>
			{
			{ "itemName", inventorySlotData.itemName },
			{ "quantity", inventorySlotData.mumber }
			})
			.ContinueWithOnMainThread(task => { });
	}

	public void SetQualitySetting(int level)
	{
		string position = isLeft ? "Left" : "Right";
		GetUserReference(position).Child("qualityLevel").SetValueAsync(level).ContinueWithOnMainThread(task => { });
	}
}
