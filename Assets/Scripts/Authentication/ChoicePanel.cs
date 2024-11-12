using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Button cancelButton;
	[SerializeField] Button confirmButton;
	[SerializeField] Button creationButton;
	[SerializeField] MainPanel mainPanel;

	[Header("Specs")]
	[SerializeField] string choice;
	private bool isLeftChoice;

	private void Awake()
	{
		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
		AddButtonSFXListener(cancelButton);
		AddButtonSFXListener(confirmButton);
		AddButtonSFXListener(creationButton);
	}

	private void AddButtonSFXListener(Button button)
	{
		button.onClick.AddListener(Manager.Sound.ButtonSFX);
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
        choice = isLeftChoice ? "Left" : "Right";

        Manager.Fire.DB.GetReference("UserData")
            .Child(Manager.Fire.UserID)
            .Child(choice)
            .RemoveValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    gameObject.SetActive(false);
                    mainPanel.gameObject.SetActive(true);

                    if (isLeftChoice)
                    {
                        mainPanel.UpdateUIForLeftChoice();
                    }
                    else
                    {
                        mainPanel.UpdateUIForRightChoice();
                    }
                }
            });
    }
}
