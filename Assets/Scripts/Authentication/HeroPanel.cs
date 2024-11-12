using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UserData;

public class HeroPanel : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PanelController panelController;
	[SerializeField] TMP_InputField nickName;
	[SerializeField] Button manPressed;
	[SerializeField] Button woManPressed;
	[SerializeField] Button cancelButton;
	[SerializeField] Button confirmButton;
	[SerializeField] Animator manAnimator;
	[SerializeField] Animator woManAnimator;

	[Header("Specs")]
	[SerializeField] string characterPosition;
	private bool manChoice;
	private bool woManChoice;

	private void Awake()
	{
		manPressed.onClick.AddListener(() => SelectCharacter(true));
		woManPressed.onClick.AddListener(() => SelectCharacter(false));
		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);

		AddSoundEffect(manPressed);
		AddSoundEffect(woManPressed);
		AddSoundEffect(cancelButton);
		AddSoundEffect(confirmButton);
	}

	private void AddSoundEffect(Button button)
	{
		button.onClick.AddListener(Manager.Sound.ButtonSFX);
	}

	public void SetCharacterPosition(string position)
	{
		characterPosition = position;
	}

	private void SelectCharacter(bool isMan)
	{
		manChoice = isMan;
		woManChoice = !isMan;

		manAnimator.SetBool("Victory", manChoice);
		woManAnimator.SetBool("Victory", woManChoice);

		manPressed.image.color = manChoice ? Color.green : Color.white;
		woManPressed.image.color = woManChoice ? Color.green : Color.white;
	}

	private void Cancel()
	{
		ResetSelection();
		panelController.SetActivePanel(PanelController.Panel.Main);
	}

	private void Confirm()
	{
		string name = nickName.text.Trim();

		if (!ValidateSelection(name)) return;

		CharacterType characterType = manChoice ? CharacterType.Man : CharacterType.WoMan;
		Dictionary<int, InventorySlotData> inventory = InitializeInventory();

		Manager.Fire.CreateCharacter(name, characterType, characterPosition, -25, 4, -7, "LittleForestScene", 100, 100, 0,
			null, null, null, inventory, new Dictionary<string, QuestData>(), new InventorySlotData[4], 2);

		ResetSelection();
		panelController.SetActivePanel(PanelController.Panel.Main);
	}

	private bool ValidateSelection(string name)
	{
		if (!manChoice && !woManChoice)
		{
			panelController.ShowInfo("캐릭터를 선택해주세요");
			return false;
		}

		if (string.IsNullOrEmpty(name))
		{
			panelController.ShowInfo("닉네임을 설정해주세요");
			return false;
		}

		return true;
	}

	private Dictionary<int, InventorySlotData> InitializeInventory()
	{
		Dictionary<int, InventorySlotData> inventory = new Dictionary<int, InventorySlotData>();
		for (int i = 0; i < 16; i++)
		{
			inventory[i] = new InventorySlotData();
		}
		return inventory;
	}

	private void ResetSelection()
	{
		manPressed.image.color = Color.white;
		woManPressed.image.color = Color.white;
		nickName.text = "";
		manChoice = false;
		woManChoice = false;
	}
}