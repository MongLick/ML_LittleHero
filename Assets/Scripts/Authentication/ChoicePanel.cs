using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
	[SerializeField] Button cancelButton;
	[SerializeField] Button confirmButton;
	[SerializeField] Button creationButton;
	[SerializeField] MainPanel mainPanel;

	private bool isLeftChoice;

	private void Awake()
	{
		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
	}

	public void ShowChoice(bool isLeft)
	{
		isLeftChoice = isLeft;
		gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
	}

	private void Cancel()
	{
		gameObject.SetActive(false);
		mainPanel.gameObject.SetActive(true);
	}

	private void Confirm()
	{
		if (isLeftChoice)
		{
			Manager.Fire.DB.GetReference("UserData")
				.Child(Manager.Fire.UserID)
				.Child("Left")
				.RemoveValueAsync()
				.ContinueWithOnMainThread(task =>
				{
					if (task.IsCompleted)
					{
						gameObject.SetActive(false);
						mainPanel.gameObject.SetActive(true);
						mainPanel.UIOffChange1();
					}
				});
		}
		else
		{
			Manager.Fire.DB.GetReference("UserData")
				.Child(Manager.Fire.UserID)
				.Child("Right")
				.RemoveValueAsync().ContinueWithOnMainThread(task =>
				{
					if (task.IsCompleted)
					{
						gameObject.SetActive(false);
						mainPanel.gameObject.SetActive(true);
						mainPanel.UIOffChange2();
					}
				});
		}
	}
}
