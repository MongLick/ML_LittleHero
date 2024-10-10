using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

public class LittleForestScene : BaseScene
{
	private GameObject characterInstance;

	private void OnEnable()
	{
		if (Manager.Fire.IsLeft)
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
					int health = int.Parse(leftSnapshot.Child("health").Value.ToString());
					int mana = int.Parse(leftSnapshot.Child("mana").Value.ToString());
					int gold = int.Parse(leftSnapshot.Child("gold").Value.ToString());
					string weapon = leftSnapshot.Child("weaponSlot").Value.ToString();
					string shield = leftSnapshot.Child("shieldSlot").Value.ToString();
					string cloak = leftSnapshot.Child("cloakSlot").Value.ToString();

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
					EquipItems(weapon, shield, cloak);
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
					int health = int.Parse(rightSnapshot.Child("health").Value.ToString());
					int mana = int.Parse(rightSnapshot.Child("mana").Value.ToString());
					int gold = int.Parse(rightSnapshot.Child("gold").Value.ToString());
					string weapon = rightSnapshot.Child("weaponSlot").Value.ToString();
					string shield = rightSnapshot.Child("shieldSlot").Value.ToString();
					string cloak = rightSnapshot.Child("cloakSlot").Value.ToString();

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

					EquipItems(weapon, shield, cloak);
				}
			});
		}
	}

	public override IEnumerator LoadingRoutine()
	{
		yield return null;
	}

	private void EquipItems(string weapon, string shield, string cloak)
	{
		GameObject equipmentUI = GameObject.Find("PlayerCanvas/EquipmentUI");
		PlayerEquipment playerEquipment = characterInstance.GetComponent<PlayerEquipment>();

		if (equipmentUI != null)
		{
			equipmentUI.SetActive(true);
		}

		GameObject weaponPrefab = FindItemPrefab(weapon);
		if (weaponPrefab != null)
		{
			Transform weaponSlot = GameObject.FindGameObjectWithTag("WeaponSlot").transform;
			EquipmentSlot weaponSlotComponent = weaponSlot.GetComponent<EquipmentSlot>();

			if (weaponSlot != null && weaponSlotComponent != null)
			{
				Instantiate(weaponPrefab, weaponSlot.position, Quaternion.identity, weaponSlot);
				playerEquipment.EquipWeapon(weapon);

				InventoryIcon weaponIcon = weaponPrefab.GetComponent<InventoryIcon>();
				weaponSlotComponent.currentItem = weaponIcon;
				weaponSlotComponent.currentItem.itemName = weapon;
			}
		}

		GameObject shieldPrefab = FindItemPrefab(shield);
		if (shieldPrefab != null)
		{
			Transform shieldSlot = GameObject.FindGameObjectWithTag("ShieldSlot").transform;
			EquipmentSlot shieldSlotComponent = shieldSlot.GetComponent<EquipmentSlot>();

			if (shieldSlot != null && shieldSlotComponent != null)
			{
				Instantiate(shieldPrefab, shieldSlot.position, Quaternion.identity, shieldSlot);
				playerEquipment.EquipShield(shield);

				InventoryIcon shieldIcon = shieldPrefab.GetComponent<InventoryIcon>();
				shieldSlotComponent.currentItem = shieldIcon;
				shieldSlotComponent.currentItem.itemName = shield;
			}
		}

		GameObject cloakPrefab = FindItemPrefab(cloak);
		if (cloakPrefab != null)
		{
			Transform cloakSlot = GameObject.FindGameObjectWithTag("CloakSlot").transform;
			EquipmentSlot cloakSlotComponent = cloakSlot.GetComponent<EquipmentSlot>();

			if (cloakSlot != null && cloakSlotComponent != null)
			{
				Instantiate(cloakPrefab, cloakSlot.position, Quaternion.identity, cloakSlot);
				playerEquipment.EquipCloak(cloak);

				InventoryIcon cloakIcon = cloakPrefab.GetComponent<InventoryIcon>();
				cloakSlotComponent.currentItem = cloakIcon;
				cloakSlotComponent.currentItem.itemName = cloak;
			}
		}

		if (equipmentUI != null)
		{
			equipmentUI.SetActive(false);
		}
	}

	private GameObject FindItemPrefab(string itemName)
	{
		return Resources.Load<GameObject>($"Prefabs/{itemName}");
	}
}
