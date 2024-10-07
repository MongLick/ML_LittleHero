using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

	public void EquipWeapon(string weapon)
	{
		equippedWeapon = weapon;
		UpdateWeapon();
	}

	public void EquipShield(string shield)
	{
		equippedShield = shield;
		UpdateShield();
	}

	public void EquipCloak(string cloak)
	{
		equippedCloak = cloak;
		UpdateCloak();
	}

	private void UpdateWeapon()
	{
		if (equippedWeapon == "sword1")
		{
			swordObject1.SetActive(true);
			swordObject2.SetActive(false);
		}
		else if(equippedWeapon == "sword2")
		{
			swordObject1.SetActive(false);
			swordObject2.SetActive(true);
		}
		else
		{
			swordObject1.SetActive(false);
			swordObject2.SetActive(false);
		}
	}

	private void UpdateShield()
	{
		shieldObject1.SetActive(equippedShield == "shield1");
		shieldObject2.SetActive(equippedShield == "shield2");
	}

	private void UpdateCloak()
	{
		cloakObject1.SetActive(equippedCloak == "cloak1");
		cloakObject2.SetActive(equippedCloak == "cloak2");
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

			UpdateWeapon();
			UpdateShield();
			UpdateCloak();
		}
	}
}
