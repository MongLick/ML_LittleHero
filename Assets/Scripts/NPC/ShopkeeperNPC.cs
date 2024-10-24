using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperNPC : MonoBehaviour
{
    [SerializeField] InteractAdapter interactAdapter;
    [SerializeField] LittleForestScene scene;
	private bool isInteract;

	private void Awake()
	{
		interactAdapter.OnInteracted.AddListener(OnInteract);
		scene.TalkButton.onClick.AddListener(() => OnInteract(null));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(true);
			isInteract = true;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(true);
			isInteract = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (scene.PlayerLayer.Contain(other.gameObject.layer))
		{
			scene.TalkButton.gameObject.SetActive(false);
			scene.TalkBackImage.gameObject.SetActive(false);
			isInteract = false;
		}
	}

	private void OnInteract(PlayerController player)
	{
		if (!isInteract)
		{
			return;
		}

		scene.TalkButton.gameObject.SetActive(false);
		scene.TalkBackImage.gameObject.SetActive(false);
		scene.ShopBack.gameObject.SetActive(true);
	}
}
