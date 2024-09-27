using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

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

	public void CreateCharacter(string nickname, CharacterType type, string position, float x, float y, float z, string scene)
	{
		FirebaseUser user = auth.CurrentUser;
		string userID = user.UserId;
		UserData userData = new UserData(nickname, type, position, x, y, z, scene);
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
}
