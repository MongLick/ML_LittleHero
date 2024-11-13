using UnityEngine;

public class DataManager : Singleton<DataManager>
{
	[Header("Components")]
	[SerializeField] UserData userData;
	public UserData UserData { get { return userData; } set { userData = value; } }
}
