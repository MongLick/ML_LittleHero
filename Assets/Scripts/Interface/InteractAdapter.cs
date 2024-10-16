using UnityEngine;
using UnityEngine.Events;

public class InteractAdapter : MonoBehaviour, IInteractable
{
	[Header("UnityEvent")]
	[SerializeField] UnityEvent<PlayerController> onInteracted;
	public UnityEvent<PlayerController> OnInteracted { get { return onInteracted; } set { onInteracted = value; } }

	public void Interact(PlayerController player)
	{
		onInteracted?.Invoke(player);
	}
}
