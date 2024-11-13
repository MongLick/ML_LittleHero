using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] HeroPanel heroPanel;
	[SerializeField] Button logoutButton, editButton, startButton;
	[SerializeField] Button creation1, creation2, delete1, delete2, pressed1, pressed2;
	[SerializeField] Animator manAnimator1, woManAnimator1, manAnimator2, woManAnimator2;
	[SerializeField] TMP_Text leftNickName, rightNickName;
	[SerializeField] GameObject[] man1Items, man2Items, woMan1Items, woMan2Items;

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

		BindButtonSounds();
	}

	private void BindButtonSounds()
	{
		Button[] buttons = { logoutButton, editButton, startButton, creation1, creation2, delete1, delete2, pressed1, pressed2 };
		foreach (Button btn in buttons)
		{
			btn.onClick.AddListener(Manager.Sound.ButtonSFX);
		}
	}

	private void OnEnable()
	{
		if (Manager.Fire.Auth == null) return;

		Manager.Fire.UserID = Manager.Fire.Auth.CurrentUser.UserId;
		LoadCharacterData(Manager.Fire.UserID);
		ResetButtonStates();
		startButton.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		startButton.gameObject.SetActive(false);
	}

	public void LoadCharacterData(string userId)
	{
		LoadCharacter("Left", userId, leftNickName, manAnimator1, woManAnimator1, man1Items, woMan1Items, creation1, delete1, pressed1);
		LoadCharacter("Right", userId, rightNickName, manAnimator2, woManAnimator2, man2Items, woMan2Items, creation2, delete2, pressed2);
	}

	private void LoadCharacter(string side, string userId, TMP_Text nickNameText, Animator manAnimator, Animator womanAnimator, GameObject[] manItems, GameObject[] womanItems, Button creationButton, Button deleteButton, Button pressedButton)
	{
		Manager.Fire.DB
		.GetReference("UserData")
		.Child(userId)
		.Child(side)
		.GetValueAsync()
		.ContinueWithOnMainThread(task =>
		{
			if (task.IsCompleted && task.Result != null)
			{
				DataSnapshot snapshot = task.Result;
				string nickName = snapshot.Child("nickName").Value.ToString();
				string type = snapshot.Child("type").Value.ToString();
				string weapon = snapshot.Child("weaponSlot").Value.ToString();
				string shield = snapshot.Child("shieldSlot").Value.ToString();
				string cloak = snapshot.Child("cloakSlot").Value.ToString();

				nickNameText.text = nickName;
				SetCharacterAppearance(type, weapon, shield, cloak, manAnimator, womanAnimator, manItems, womanItems);

				creationButton.gameObject.SetActive(false);
				deleteButton.gameObject.SetActive(true);
				pressedButton.gameObject.SetActive(true);
			}
			else
			{
				if (side == "Left") UpdateUIForChoice("Left");
				else UpdateUIForChoice("Right");
			}
		});
	}

	private void SetCharacterAppearance(string type, string weapon, string shield, string cloak, Animator manAnimator, Animator womanAnimator, GameObject[] manItems, GameObject[] womanItems)
	{
		bool isMan = type == "0";
		manAnimator.gameObject.SetActive(isMan);
		womanAnimator.gameObject.SetActive(!isMan);

		SetItemsAppearance(weapon, shield, cloak, isMan ? manItems : womanItems);
	}

	private void SetItemsAppearance(string weapon, string shield, string cloak, GameObject[] items)
	{
		SetItemActive(items[0], weapon == "sword1");
		SetItemActive(items[1], weapon == "sword2");
		SetItemActive(items[2], shield == "shield1");
		SetItemActive(items[3], shield == "shield2");
		SetItemActive(items[4], cloak == "cloak1");
		SetItemActive(items[5], cloak == "cloak2");
	}

	private void SetItemActive(GameObject item, bool isActive)
	{
		item.SetActive(isActive);
	}

	public void OnLeftButtonPressed()
	{
		SetButtonStates(pressed1, pressed2, true);
		heroPanel.SetCharacterPosition("Left");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	public void OnRightButtonPressed()
	{
		SetButtonStates(pressed2, pressed1, true);
		heroPanel.SetCharacterPosition("Right");
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	private void SetButtonStates(Button activeButton, Button inactiveButton, bool isActive)
	{
		activeButton.image.color = isActive ? Color.green : Color.white;
		inactiveButton.image.color = Color.white;
	}

	private void ResetButtonStates()
	{
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.white;
	}

	public void UpdateUIForChoice(string side)
	{
		if (side == "Left")
		{
			leftNickName.text = "";
			ResetCharacterAppearance(manAnimator1, woManAnimator1, man1Items, woMan1Items);
			creation1.gameObject.SetActive(true);
			delete1.gameObject.SetActive(false);
			pressed1.gameObject.SetActive(false);
		}
		else if (side == "Right")
		{
			rightNickName.text = "";
			ResetCharacterAppearance(manAnimator2, woManAnimator2, man2Items, woMan2Items);
			creation2.gameObject.SetActive(true);
			delete2.gameObject.SetActive(false);
			pressed2.gameObject.SetActive(false);
		}
	}

	private void ResetCharacterAppearance(Animator manAnimator, Animator womanAnimator, GameObject[] manItems, GameObject[] womanItems)
	{
		manAnimator.gameObject.SetActive(false);
		womanAnimator.gameObject.SetActive(false);
		foreach (GameObject item in manItems) item.SetActive(false);
		foreach (GameObject item in womanItems) item.SetActive(false);
	}

	private void Logout()
	{
		ResetButtonStates();
		Manager.Fire.Auth.SignOut();
		panelController.SetActivePanel(PanelController.Panel.Login);
		PhotonNetwork.Disconnect();
	}

	private void Edit()
	{
		ResetButtonStates();
		panelController.SetActivePanel(PanelController.Panel.Edit);
	}

	private void GameStart()
	{
		ResetButtonStates();
		string side = Manager.Fire.IsLeft ? "Left" : "Right";
		LoadSceneForSide(side);
	}

	private void LoadSceneForSide(string side)
	{
		Manager.Fire.DB
		.GetReference("UserData")
		.Child(Manager.Fire.UserID)
		.Child(side)
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

	private void Creation()
	{
		ResetButtonStates();
		panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	private void Delete1() { panelController.ShowChoice(true); }
	private void Delete2() { panelController.ShowChoice(false); }

	private void Pressed1()
	{
		SetVictoryState(true, false);
		SetButtonStates(pressed1, pressed2, true);
		startButton.gameObject.SetActive(true);
		Manager.Fire.IsLeft = true;
	}

	private void Pressed2()
	{
		SetVictoryState(false, true);
		SetButtonStates(pressed2, pressed1, true);
		startButton.gameObject.SetActive(true);
		Manager.Fire.IsLeft = false;
	}

	private void SetVictoryState(bool leftVictory, bool rightVictory)
	{
		SetAnimatorVictoryState(manAnimator1, woManAnimator1, leftVictory);
		SetAnimatorVictoryState(manAnimator2, woManAnimator2, rightVictory);
	}

	private void SetAnimatorVictoryState(Animator manAnimator, Animator womanAnimator, bool victory)
	{
		if (manAnimator.gameObject.activeInHierarchy) manAnimator.SetBool("Victory", victory);
		if (womanAnimator.gameObject.activeInHierarchy) womanAnimator.SetBool("Victory", victory);
	}
}
