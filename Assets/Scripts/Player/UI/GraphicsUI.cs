using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsUI : MonoBehaviour
{
	[SerializeField] Button closeButton;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}
}
