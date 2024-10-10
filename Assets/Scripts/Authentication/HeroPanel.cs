using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UserData;

public class HeroPanel : MonoBehaviour
{
	[SerializeField] PanelController panelController;
	[SerializeField] Button manPressed;
	[SerializeField] Button woManPressed;
	[SerializeField] TMP_InputField nickName;
	[SerializeField] Button cancelButton;
	[SerializeField] Button confirmButton;
	[SerializeField] Animator manAnimator;
	[SerializeField] Animator woManAnimator;

	private bool manChoice;
	private bool woManChoice;
	private string characterPosition;

	private void Awake()
	{
		manPressed.onClick.AddListener(ManPressed);
		woManPressed.onClick.AddListener(WoManPressed);
		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
	}

	public void SetCharacterPosition(string position)
	{
		characterPosition = position;
	}

	private void ManPressed()
	{
		manAnimator.SetBool("Victory", true);
		woManAnimator.SetBool("Victory", false);
		manPressed.image.color = Color.green;
		woManPressed.image.color = Color.white;
		manChoice = true;
		woManChoice = false;
	}

	private void WoManPressed()
	{
		woManAnimator.SetBool("Victory", true);
		manAnimator.SetBool("Victory", false);
		woManPressed.image.color = Color.green;
		manPressed.image.color = Color.white;
		manChoice = false;
		woManChoice = true;
	}

	private void Cancel()
	{
		manPressed.image.color = Color.white;
		woManPressed.image.color = Color.white;
		nickName.text = "";
		panelController.SetActivePanel(PanelController.Panel.Main);
	}

	private void Confirm()
	{
		string name = nickName.text.Trim();
		if (!manChoice && !woManChoice)
		{
			panelController.ShowInfo("캐릭터를 선택해주세요");
			return;
		}
		if (string.IsNullOrEmpty(name))
		{
			panelController.ShowInfo("닉네임을 설정해주세요");
			return;
		}

		manPressed.image.color = Color.white;
		woManPressed.image.color = Color.white;
		CharacterType characterType = manChoice ? CharacterType.Man : CharacterType.WoMan;
		Manager.Fire.CreateCharacter(name, characterType, characterPosition, -25, 4, -7, "LittleForestScene", 100, 100, 0, null, null, null, new List<string>(new string[16]));
		panelController.SetActivePanel(PanelController.Panel.Main);
		nickName.text = "";
	}
}