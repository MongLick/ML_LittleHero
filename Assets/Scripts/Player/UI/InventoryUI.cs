using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
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
