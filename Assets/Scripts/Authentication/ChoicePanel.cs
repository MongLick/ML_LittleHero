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

	private void Awake()
	{
		cancelButton.onClick.AddListener(Cancel);
		confirmButton.onClick.AddListener(Confirm);
	}

	public void ShowChoice()
	{
		gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
	}

	public void Cancel()
	{
		gameObject.SetActive(false);
		mainPanel.gameObject.SetActive(true);
	}

	public void Confirm()
	{
		// 데이터베이스에서 캐릭터 삭제 진행
		gameObject.SetActive(false);
		creationButton.gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(true);
	}
}
