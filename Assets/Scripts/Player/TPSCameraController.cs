using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField] Transform cameraRoot;
	[SerializeField] float mouseSensitivity;
	private Vector2 inputDir;
	private float xRotation;

	/*private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}*/

	private void LateUpdate()
	{
		xRotation -= inputDir.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -5f, 30f);

		transform.Rotate(Vector3.up, inputDir.x * mouseSensitivity * Time.deltaTime);
		cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
	}

	private void OnLook(InputValue value)
	{
		inputDir = value.Get<Vector2>();
	}
}
