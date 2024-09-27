using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform player;
	public Vector3 offset;

	private void LateUpdate()
	{
		transform.position = player.position + offset;

		transform.rotation = Quaternion.Euler(45, 0, 0);
	}
}
