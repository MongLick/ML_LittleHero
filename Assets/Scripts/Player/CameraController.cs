using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Transform player;

	[Header("Vector")]
	[SerializeField] Vector3 offset;

	private void LateUpdate()
	{
		transform.position = player.position + offset;

		transform.rotation = Quaternion.Euler(45, 0, 0);
	}
}
