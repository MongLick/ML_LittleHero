using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UserData;

public class FirebaseManager : Singleton<FirebaseManager>
{
	private FirebaseApp app;
	public FirebaseApp App { get { return app; } }

	private FirebaseAuth auth;
	public FirebaseAuth Auth { get { return auth; } }

	private FirebaseDatabase db;
	public FirebaseDatabase DB { get { return db; } }

	private bool isValid;
	public bool IsValid { get { return isValid; } }

	private bool isLeft;
	public bool IsLeft { get { return isLeft; }  set { isLeft = value; } }

	private string userID;
	public string UserID { get { return userID; } set { userID = value; } }

	[SerializeField] GameObject manPrefab;
	public GameObject ManPrefab { get { return manPrefab; } }
	[SerializeField] GameObject woManPrefab;
	public GameObject WoManPrefab { get { return woManPrefab; } }

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

			Debug.Log("Firebase Check and FixDependencies success");
			isValid = true;
		}
		else
		{
			Debug.LogError("Firebase Check and FixDependencies fail");
			isValid = false;

			app = null;
			auth = null;
			db = null;
		}
	}

	public void CreateCharacter(string nickName, CharacterType type, string position, float x, float y, float z, string scene, int health, int mana, int gold, string weaponSlot, string shieldSlot, string cloakSlot, List<string> inventory, Dictionary<string, QuestData> quests)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		UserData userData = new UserData(nickName, type, position, x, y, z, scene, health, mana, gold, weaponSlot, shieldSlot, cloakSlot, inventory, quests);
		string json = JsonUtility.ToJson(userData);
		Manager.Fire.DB
			.GetReference("UserData")
			.Child(userID)
			.Child(position)
			.SetRawJsonValueAsync(json)
			.ContinueWithOnMainThread(task =>
			{
				
			});
	}

	public void UpdateWeaponSlot(string position, string weapon)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		DatabaseReference reference = Manager.Fire.DB
		.GetReference("UserData")
		.Child(userID)
		.Child(position);
		reference.Child("weaponSlot").SetValueAsync(weapon).ContinueWithOnMainThread(task =>
		{
			
		});
	}
	public void UpdateShieldSlot(string position, string shield)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		DatabaseReference reference = Manager.Fire.DB
		.GetReference("UserData")
		.Child(userID)
		.Child(position);
		reference.Child("shieldSlot").SetValueAsync(shield).ContinueWithOnMainThread(task =>
		{

		});
	}
	public void UpdateCloakSlot(string position, string cloak)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		DatabaseReference reference = Manager.Fire.DB
		.GetReference("UserData")
		.Child(userID)
		.Child(position);
		reference.Child("cloakSlot").SetValueAsync(cloak).ContinueWithOnMainThread(task =>
		{

		});
	}

	public void AddQuest(string position, string questID, string questName)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;

		QuestData newQuest = new QuestData(questID, questName, false);
		DatabaseReference reference = Manager.Fire.DB
			.GetReference("UserData")
			.Child(userID)
			.Child(position)
			.Child("quests")
			.Child(questID);

		string json = JsonUtility.ToJson(newQuest);
		reference.SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
		{

		});
	}

	public void LoadQuestData(string position, string questID, System.Action<QuestData> callback)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;

		DatabaseReference reference = Manager.Fire.DB
			.GetReference("UserData")
			.Child(userID)
			.Child(position)
			.Child("quests")
			.Child(questID);

		reference.GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted)
			{
				Debug.LogError("퀘스트 데이터 로드 오류: " + task.Exception);
				callback(null);
				return;
			}

			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;
				if (snapshot.Exists)
				{
					string json = snapshot.GetRawJsonValue();
					QuestData questData = JsonUtility.FromJson<QuestData>(json);
					callback(questData);
				}
				else
				{
					callback(null);
				}
			}
		});
	}
}
