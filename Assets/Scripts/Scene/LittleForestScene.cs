using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LittleForestScene : BaseScene
{
	private GameObject characterInstance;

	private void OnEnable()
	{
		if(Manager.Fire.IsLeft)
		{
			Manager.Fire.DB
			.GetReference("UserData")
			.Child(Manager.Fire.UserID)
			.Child("Left")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot leftSnapshot = task.Result;

					string nickName = leftSnapshot.Child("nickName").Value.ToString();
					string type = leftSnapshot.Child("type").Value.ToString();
					float posX = float.Parse(leftSnapshot.Child("posX").Value.ToString());
					float posY = float.Parse(leftSnapshot.Child("posY").Value.ToString());
					float posZ = float.Parse(leftSnapshot.Child("posZ").Value.ToString());

					if(type == "0")
					{
						characterInstance = Instantiate(Manager.Fire.ManPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
					}
					else
					{
						characterInstance = Instantiate(Manager.Fire.WoManPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
					}
					TMP_Text nicknameUI = characterInstance.GetComponentInChildren<TMP_Text>();
					if (nicknameUI != null)
					{
						nicknameUI.text = nickName;
					}
				}
			});
		}
		else
		{
			Manager.Fire.DB
			.GetReference("UserData")
			.Child(Manager.Fire.UserID)
			.Child("Right")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result != null)
				{
					DataSnapshot rightSnapshot = task.Result;

					string nickName = rightSnapshot.Child("nickName").Value.ToString();
					string type = rightSnapshot.Child("type").Value.ToString();
					float posX = float.Parse(rightSnapshot.Child("posX").Value.ToString());
					float posY = float.Parse(rightSnapshot.Child("posY").Value.ToString());
					float posZ = float.Parse(rightSnapshot.Child("posZ").Value.ToString());

					if (type == "0")
					{
						characterInstance = Instantiate(Manager.Fire.ManPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
					}
					else
					{
						characterInstance = Instantiate(Manager.Fire.WoManPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
					}

					TMP_Text nicknameUI = characterInstance.GetComponentInChildren<TMP_Text>();
					if (nicknameUI != null)
					{
						nicknameUI.text = nickName;
					}
				}
			});
		}
	}

	public override IEnumerator LoadingRoutine()
	{
		yield return null;
	}
}
