using UnityEngine;

public static class Manager
{
	public static FirebaseManager Fire { get { return FirebaseManager.Instance; } }
	public static GameManager Game { get { return GameManager.Instance; } }
	public static PoolManager Pool { get { return PoolManager.Instance; } }
	public static ResourceManager Resource { get { return ResourceManager.Instance; } }
	public static SceneManager Scene { get { return SceneManager.Instance; } }
	public static SoundManager Sound { get { return SoundManager.Instance; } }
	public static UIManager UI { get { return UIManager.Instance; } }
	public static DataManager Data { get { return DataManager.Instance; } }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		FirebaseManager.ReleaseInstance();
		GameManager.ReleaseInstance();
		PoolManager.ReleaseInstance();
		ResourceManager.ReleaseInstance();
		SceneManager.ReleaseInstance();
		SoundManager.ReleaseInstance();
		UIManager.ReleaseInstance();
		DataManager.ReleaseInstance();

		FirebaseManager.CreateInstance();
		GameManager.CreateInstance();
		PoolManager.CreateInstance();
		ResourceManager.CreateInstance();
		SceneManager.CreateInstance();
		SoundManager.CreateInstance();
		UIManager.CreateInstance();
		DataManager.CreateInstance();
	}
}
