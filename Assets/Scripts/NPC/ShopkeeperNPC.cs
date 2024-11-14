using Photon.Pun;
using UnityEngine;

public class ShopkeeperNPC : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] InteractAdapter interactAdapter;
	[SerializeField] LittleForestScene scene;
	[SerializeField] PlayerController player;
	[SerializeField] InventorySlot[] inventorySlots;
	public InventorySlot[] InventorySlots { get { return inventorySlots; } }
	[SerializeField] QuickSlot[] quickSlot;
	public QuickSlot[] QuickSlot { get { return quickSlot; } }

	[Header("Specs")]
	private bool isInteract;

	private void Awake()
	{
		interactAdapter.OnInteracted.AddListener(OnInteract);
		scene.TalkButton.onClick.AddListener(() => OnInteract(null));
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerInteraction(other, true);
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerInteraction(other, true);
	}

	private void OnTriggerExit(Collider other)
	{
		PlayerInteraction(other, false);
	}

	private void PlayerInteraction(Collider other, bool isEntering)
	{
		if (!scene.PlayerLayer.Contain(other.gameObject.layer)) return;

		PhotonView photonView = other.GetComponent<PhotonView>();
		if (photonView.IsMine)
		{
			if (isEntering)
			{
				scene.TalkButton.gameObject.SetActive(true);
				isInteract = true;
				player = other.GetComponent<PlayerController>();
				inventorySlots = player.InventoryUI.InventorySlots;
				quickSlot = player.QuickSlots;
			}
			else
			{
				scene.TalkButton.gameObject.SetActive(false);
				scene.TalkBackImage.gameObject.SetActive(false);
				isInteract = false;
				player = null;
				inventorySlots = null;
				quickSlot = null;
			}
		}
	}

	private void OnInteract(PlayerController player)
	{
		if (!isInteract) return;

		scene.TalkButton.gameObject.SetActive(false);
		scene.TalkBackImage.gameObject.SetActive(false);
		scene.ShopBack.gameObject.SetActive(true);
	}
}
