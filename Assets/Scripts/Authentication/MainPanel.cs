using Firebase.Database;
using Firebase.Extensions;
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

	private string userID;
	public string UserID { get { return userID; } }

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
	}

	private void OnEnable()
	{
		if (Manager.Fire.Auth == null)
		{
			return;
		}

		userID = Manager.Fire.Auth.CurrentUser.UserId;
		LoadCharacterData(userID);
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

				leftNickName.text = nickName;
				if (type == "0")
				{
					manAnimator1.gameObject.SetActive(true);
					woManAnimator1.gameObject.SetActive(false);
				}
				else if (type == "1")
				{
					woManAnimator1.gameObject.SetActive(true);
					manAnimator1.gameObject.SetActive(false);
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

					rightNickName.text = nickName;
					if (type == "0")
					{
						manAnimator2.gameObject.SetActive(true);
						woManAnimator2.gameObject.SetActive(false);
					}
					else if (type == "1")
					{
						woManAnimator2.gameObject.SetActive(true);
						manAnimator2.gameObject.SetActive(false);
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
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		heroPanel.SetCharacterPosition("Left");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	public void OnRightButtonPressed()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
		heroPanel.SetCharacterPosition("Right");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	public void UIOffChange1()
	{
		leftNickName .text = "";
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
		Manager.Scene.LoadScene("LittleForestScene");
	}

	private void Creation()
	{
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
	}
}
