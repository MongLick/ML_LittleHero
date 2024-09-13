using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] CharacterController controller;
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;
	[SerializeField] bool isGround;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] Vector3 groundCheckBoxSize;

	private Vector3 moveDir;
	private Vector3 direction;
	private float ySpeed;

	private void Update()
	{
		GroundCheck();
		Move();
		Jump();
	}

	private void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir.x = input.x;
		moveDir.z = input.y;
	}

	private void OnJump(InputValue value)
	{
		if (isGround)
		{
			ySpeed = jumpSpeed;
		}
	}

	private void Move()
	{
		direction = new Vector3(moveDir.x, 0, moveDir.z).normalized;
		controller.Move(direction * moveSpeed * Time.deltaTime);
	}

	private void Jump()
	{
		if (isGround)
		{
			if (ySpeed < 0)
			{
				ySpeed = 0;
			}
		}
		else
		{
			ySpeed += Physics.gravity.y * Time.deltaTime;
		}

		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	private void GroundCheck()
	{
		isGround = Physics.CheckBox(transform.position, groundCheckBoxSize, Quaternion.identity, groundLayer);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 boxCenter = transform.position;

		Gizmos.DrawWireCube(boxCenter, groundCheckBoxSize);
	}
}
