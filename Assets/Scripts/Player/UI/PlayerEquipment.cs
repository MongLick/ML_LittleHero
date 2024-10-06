using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviourPun, IPunObservable
{
	[SerializeField] GameObject swordObject1;
	[SerializeField] GameObject swordObject2;
	[SerializeField] GameObject shieldObject1;
	[SerializeField] GameObject shieldObject2;
	[SerializeField] GameObject cloakObject1;
	[SerializeField] GameObject cloakObject2;

	public string equippedWeapon = "";
	public string equippedShield = "";
	public string equippedCloak = "";

	public void EquipItem(string weapon, string shield, string cloak)
	{
		if (equippedWeapon != weapon)
		{
			equippedWeapon = weapon;
		}

		if (equippedShield != shield)
		{
			equippedShield = shield;
		}

		if (equippedCloak != cloak)
		{
			equippedCloak = cloak;
		}

		UpdateEquipmentVisuals();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(equippedWeapon);
			stream.SendNext(equippedShield);
			stream.SendNext(equippedCloak);
		}
		else
		{
			equippedWeapon = (string)stream.ReceiveNext();
			equippedShield = (string)stream.ReceiveNext();
			equippedCloak = (string)stream.ReceiveNext();

			UpdateEquipmentVisuals();
		}
	}

	private void UpdateEquipmentVisuals()
	{
		if(equippedWeapon == "sword1")
		{
			swordObject1.SetActive(true);
			swordObject2.SetActive(false);
		}
		if(equippedWeapon == "sword2")
		{
			swordObject1.SetActive(false);
			swordObject2.SetActive(true);
		}
		if (equippedShield == "shield1")
		{
			shieldObject1.SetActive(true);
			shieldObject2.SetActive(false);
		}
		if (equippedShield == "shield2")
		{
			shieldObject1.SetActive(false);
			shieldObject2.SetActive(true);
		}
		if (equippedCloak == "cloak1")
		{
			cloakObject1.SetActive(true);
			cloakObject2.SetActive(false);
		}
		if (equippedCloak == "cloak2")
		{
			cloakObject1.SetActive(false);
			cloakObject2.SetActive(true);
		}
	}
}
