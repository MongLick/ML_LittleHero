using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
	[SerializeField] PanelController panelController;
	[SerializeField] HeroPanel heroPanel;
	[SerializeField] Button logoutButton;
	[SerializeField] Button editButton;
	[SerializeField] Button startButton;
	[SerializeField] Button creation1;
	[SerializeField] Button creation2;
	[SerializeField] Button delete1;
	[SerializeField] Button delete2;
	[SerializeField] Button pressed1;
	[SerializeField] Button pressed2;
	[SerializeField] Animator manAnimator1;
	[SerializeField] Animator woManAnimator1;
	[SerializeField] Animator manAnimator2;
	[SerializeField] Animator woManAnimator2;
	[SerializeField] TMP_Text leftNickName;
	[SerializeField] TMP_Text rightNickName;
	[SerializeField] GameObject man1Weapon1;
	[SerializeField] GameObject man1Weapon2;
	[SerializeField] GameObject man1Shield1;
	[SerializeField] GameObject man1Shield2;
	[SerializeField] GameObject man1Cloak1;
	[SerializeField] GameObject man1Cloak2;
	[SerializeField] GameObject man2Weapon1;
	[SerializeField] GameObject man2Weapon2;
	[SerializeField] GameObject man2Shield1;
	[SerializeField] GameObject man2Shield2;
	[SerializeField] GameObject man2Cloak1;
	[SerializeField] GameObject man2Cloak2;
	[SerializeField] GameObject woMan1Weapon1;
	[SerializeField] GameObject woMan1Weapon2;
	[SerializeField] GameObject woMan1Shield1;
	[SerializeField] GameObject woMan1Shield2;
	[SerializeField] GameObject woMan1Cloak1;
	[SerializeField] GameObject woMan1Cloak2;
	[SerializeField] GameObject woMan2Weapon1;
	[SerializeField] GameObject woMan2Weapon2;
	[SerializeField] GameObject woMan2Shield1;
	[SerializeField] GameObject woMan2Shield2;
	[SerializeField] GameObject woMan2Cloak1;
	[SerializeField] GameObject woMan2Cloak2;

	private void Awake()
	{
		logoutButton.onClick.AddListener(Logout);
		editButton.onClick.AddListener(Edit);
		startButton.onClick.AddListener(GameStart);

		creation1.onClick.AddListener(Creation);
		creation1.onClick.AddListener(OnLeftButtonPressed);
		delete1.onClick.AddListener(Delete1);
		pressed1.onClick.AddListener(Pressed1);

		creation2.onClick.AddListener(Creation);
		creation2.onClick.AddListener(OnRightButtonPressed);
		delete2.onClick.AddListener(Delete2);
		pressed2.onClick.AddListener(Pressed2);

		logoutButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		editButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		startButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		creation1.onClick.AddListener(Manager.Sound.ButtonSFX);
		creation2.onClick.AddListener(Manager.Sound.ButtonSFX);
		delete1.onClick.AddListener(Manager.Sound.ButtonSFX);
		delete2.onClick.AddListener(Manager.Sound.ButtonSFX);
		pressed1.onClick.AddListener(Manager.Sound.ButtonSFX);
		pressed2.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	private void OnEnable()
	{
		if (Manager.Fire.Auth == null)
		{
			return;
		}

		Manager.Fire.UserID = Manager.Fire.Auth.CurrentUser.UserId;
		LoadCharacterData(Manager.Fire.UserID);
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		startButton.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		startButton.gameObject.SetActive(false);
	}

	public void LoadCharacterData(string userId)
	{
		Manager.Fire.DB
		.GetReference("UserData")
		.Child(userId)
		.Child("Left")
		.GetValueAsync()
		.ContinueWithOnMainThread(task =>
		{
			if (task.IsCompleted && task.Result != null)
			{
				DataSnapshot leftSnapshot = task.Result;
				string nickName = leftSnapshot.Child("nickName").Value.ToString();
				string type = leftSnapshot.Child("type").Value.ToString();
				string Weapon = leftSnapshot.Child("weaponSlot").Value.ToString();
				string Shield = leftSnapshot.Child("shieldSlot").Value.ToString();
				string Cloak = leftSnapshot.Child("cloakSlot").Value.ToString();

				leftNickName.text = nickName;
				if (type == "0")
				{
					manAnimator1.gameObject.SetActive(true);
					woManAnimator1.gameObject.SetActive(false);
					if (Weapon == "sword1")
					{
						man1Weapon1.SetActive(true);
						man1Weapon2.SetActive(false);
					}
					else if (Weapon == "sword2")
					{
						man1Weapon1.SetActive(false);
						man1Weapon2.SetActive(true);
					}
					else
					{
						man1Weapon1.SetActive(false);
						man1Weapon2.SetActive(false);
					}
					if (Shield == "shield1")
					{
						man1Shield1.SetActive(true);
						man1Shield2.SetActive(false);
					}
					else if (Shield == "shield2")
					{
						man1Shield1.SetActive(false);
						man1Shield2.SetActive(true);
					}
					else
					{
						man1Shield1.SetActive(false);
						man1Shield2.SetActive(false);
					}
					if (Cloak == "cloak1")
					{
						man1Cloak1.SetActive(true);
						man1Cloak2.SetActive(false);
					}
					else if (Cloak == "cloak2")
					{
						man1Cloak1.SetActive(false);
						man1Cloak2.SetActive(true);
					}
					else
					{
						man1Cloak1.SetActive(false);
						man1Cloak2.SetActive(false);
					}
				}
				else if (type == "1")
				{
					woManAnimator1.gameObject.SetActive(true);
					manAnimator1.gameObject.SetActive(false);
					if (Weapon == "sword1")
					{
						woMan1Weapon1.SetActive(true);
						woMan1Weapon2.SetActive(false);
					}
					else if (Weapon == "sword2")
					{
						woMan1Weapon1.SetActive(false);
						woMan1Weapon2.SetActive(true);
					}
					else
					{
						woMan1Weapon1.SetActive(false);
						woMan1Weapon2.SetActive(false);
					}
					if (Shield == "shield1")
					{
						woMan1Shield1.SetActive(true);
						woMan1Shield2.SetActive(false);
					}
					else if (Shield == "shield2")
					{
						woMan1Shield1.SetActive(false);
						woMan1Shield2.SetActive(true);
					}
					else
					{
						woMan1Shield1.SetActive(false);
						woMan1Shield2.SetActive(false);
					}
					if (Cloak == "cloak1")
					{
						woMan1Cloak1.SetActive(true);
						woMan1Cloak2.SetActive(false);
					}
					else if (Cloak == "cloak2")
					{
						woMan1Cloak1.SetActive(false);
						woMan1Cloak2.SetActive(true);
					}
					else
					{
						woMan1Cloak1.SetActive(false);
						woMan1Cloak2.SetActive(false);
					}
				}

				creation1.gameObject.SetActive(false);
				delete1.gameObject.SetActive(true);
				pressed1.gameObject.SetActive(true);
			}
			else
			{
				UIOffChange1();
			}
		});

		Manager.Fire.DB
			.GetReference("UserData")
			.Child(userId)
			.Child("Right")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot rightSnapshot = task.Result;
					string nickName = rightSnapshot.Child("nickName").Value.ToString();
					string type = rightSnapshot.Child("type").Value.ToString();
					string Weapon = rightSnapshot.Child("weaponSlot").Value.ToString();
					string Shield = rightSnapshot.Child("shieldSlot").Value.ToString();
					string Cloak = rightSnapshot.Child("cloakSlot").Value.ToString();

					rightNickName.text = nickName;
					if (type == "0")
					{
						manAnimator2.gameObject.SetActive(true);
						woManAnimator2.gameObject.SetActive(false);
						if (Weapon == "sword1")
						{
							man2Weapon1.SetActive(true);
							man2Weapon2.SetActive(false);
						}
						else if (Weapon == "sword2")
						{
							man2Weapon1.SetActive(false);
							man2Weapon2.SetActive(true);
						}
						else
						{
							man2Weapon1.SetActive(false);
							man2Weapon2.SetActive(false);
						}
						if (Shield == "shield1")
						{
							man2Shield1.SetActive(true);
							man2Shield2.SetActive(false);
						}
						else if (Shield == "shield2")
						{
							man2Shield1.SetActive(false);
							man2Shield2.SetActive(true);
						}
						else
						{
							man2Shield1.SetActive(false);
							man2Shield2.SetActive(false);
						}
						if (Cloak == "cloak1")
						{
							man2Cloak1.SetActive(true);
							man2Cloak2.SetActive(false);
						}
						else if (Cloak == "cloak2")
						{
							man2Cloak1.SetActive(false);
							man2Cloak2.SetActive(true);
						}
						else
						{
							man2Cloak1.SetActive(false);
							man2Cloak2.SetActive(false);
						}
					}
					else if (type == "1")
					{
						woManAnimator2.gameObject.SetActive(true);
						manAnimator2.gameObject.SetActive(false);
						if (Weapon == "sword1")
						{
							woMan2Weapon1.SetActive(true);
							woMan2Weapon2.SetActive(false);
						}
						else if (Weapon == "sword2")
						{
							woMan2Weapon1.SetActive(false);
							woMan2Weapon2.SetActive(true);
						}
						else
						{
							woMan2Weapon1.SetActive(false);
							woMan2Weapon2.SetActive(false);
						}
						if (Shield == "shield1")
						{
							woMan2Shield1.SetActive(true);
							woMan2Shield2.SetActive(false);
						}
						else if (Shield == "shield2")
						{
							woMan2Shield1.SetActive(false);
							woMan2Shield2.SetActive(true);
						}
						else
						{
							woMan2Shield1.SetActive(false);
							woMan2Shield2.SetActive(false);
						}
						if (Cloak == "cloak1")
						{
							woMan2Cloak1.SetActive(true);
							woMan2Cloak2.SetActive(false);
						}
						else if (Cloak == "cloak2")
						{
							woMan2Cloak1.SetActive(false);
							woMan2Cloak2.SetActive(true);
						}
						else
						{
							woMan2Cloak1.SetActive(false);
							woMan2Cloak2.SetActive(false);
						}
					}
					creation2.gameObject.SetActive(false);
					delete2.gameObject.SetActive(true);
					pressed2.gameObject.SetActive(true);
				}
				else
				{
					UIOffChange2();
				}
			});
	}

	public void OnLeftButtonPressed()
	{
		pressed1.image.color = Color.green;
		pressed2.image.color = Color.white;
		heroPanel.SetCharacterPosition("Left");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	public void OnRightButtonPressed()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.green;
		heroPanel.SetCharacterPosition("Right");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	public void UIOffChange1()
	{
		leftNickName.text = "";
		manAnimator1.gameObject.SetActive(false);
		woManAnimator1.gameObject.SetActive(false);
		creation1.gameObject.SetActive(true);
		delete1.gameObject.SetActive(false);
		pressed1.gameObject.SetActive(false);
	}

	public void UIOffChange2()
	{
		rightNickName.text = "";
		manAnimator2.gameObject.SetActive(false);
		woManAnimator2.gameObject.SetActive(false);
		creation2.gameObject.SetActive(true);
		delete2.gameObject.SetActive(false);
		pressed2.gameObject.SetActive(false);
	}

	private void Logout()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		Manager.Fire.Auth.SignOut();
		panelController.SetActivePanel(PanelController.Panel.Login);
		PhotonNetwork.Disconnect();
	}

	private void Edit()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		panelController.SetActivePanel(PanelController.Panel.Edit);
	}

	private void GameStart()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;

		if (Manager.Fire.IsLeft)
		{
			Manager.Fire.DB
			.GetReference("UserData")
			.Child(Manager.Fire.UserID)
			.Child("Left")
			.Child("scene")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot snapshot = task.Result;
					string sceneName = snapshot.Value.ToString();

					Manager.Scene.LoadScene(sceneName);
				}
			});
		}
		else
		{
			Manager.Fire.DB
			.GetReference("UserData")
			.Child(Manager.Fire.UserID)
			.Child("Right")
			.Child("scene")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot snapshot = task.Result;
					string sceneName = snapshot.Value.ToString();

					Manager.Scene.LoadScene(sceneName);
				}
			});
		}
	}

	private void Creation()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	private void Delete1()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		panelController.ShowChoice(true);
	}

	private void Delete2()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		panelController.ShowChoice(false);
	}

	private void Pressed1()
	{
		if (manAnimator1.gameObject.activeInHierarchy)
		{
			manAnimator1.SetBool("Victory", true);
		}
		if (woManAnimator1.gameObject.activeInHierarchy)
		{
			woManAnimator1.SetBool("Victory", true);
		}
		if (manAnimator2.gameObject.activeInHierarchy)
		{
			manAnimator2.SetBool("Victory", false);
		}
		if (woManAnimator2.gameObject.activeInHierarchy)
		{
			woManAnimator2.SetBool("Victory", false);
		}
		pressed1.image.color = Color.green;
		pressed2.image.color = Color.white;
		startButton.gameObject.SetActive(true);
		Manager.Fire.IsLeft = true;
	}

	private void Pressed2()
	{
		if (manAnimator1.gameObject.activeInHierarchy)
		{
			manAnimator1.SetBool("Victory", false);
		}
		if (woManAnimator1.gameObject.activeInHierarchy)
		{
			woManAnimator1.SetBool("Victory", false);
		}
		if (manAnimator2.gameObject.activeInHierarchy)
		{
			manAnimator2.SetBool("Victory", true);
		}
		if (woManAnimator2.gameObject.activeInHierarchy)
		{
			woManAnimator2.SetBool("Victory", true);
		}
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.green;
		startButton.gameObject.SetActive(true);
		Manager.Fire.IsLeft = false;
	}
}
