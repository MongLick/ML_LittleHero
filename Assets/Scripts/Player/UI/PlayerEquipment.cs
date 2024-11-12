using Photon.Pun;
using UnityEngine;

public class PlayerEquipment : MonoBehaviourPun, IPunObservable
{
	[Header("Components")]
	[SerializeField] GameObject swordObject1;
	[SerializeField] GameObject swordObject2;
	[SerializeField] GameObject shieldObject1;
	[SerializeField] GameObject shieldObject2;
	[SerializeField] GameObject cloakObject1;
	[SerializeField] GameObject cloakObject2;

	[Header("Specs")]
	[SerializeField] string equippedWeapon = "";
	[SerializeField] string equippedShield = "";
	[SerializeField] string equippedCloak = "";

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
		else if (equippedWeapon == "sword2")
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
		if (equippedShield == "shield1")
		{
			shieldObject1.SetActive(true);
			shieldObject2.SetActive(false);
		}
		else if (equippedShield == "shield2")
		{
			shieldObject1.SetActive(false);
			shieldObject2.SetActive(true);
		}
		else
		{
			shieldObject1.SetActive(false);
			shieldObject2.SetActive(false);
		}
	}

	private void UpdateCloak()
	{
		if (equippedCloak == "cloak1")
		{
			cloakObject1.SetActive(true);
			cloakObject2.SetActive(false);
		}
		else if (equippedCloak == "cloak2")
		{
			cloakObject1.SetActive(false);
			cloakObject2.SetActive(true);
		}
		else
		{
			cloakObject1.SetActive(false);
			cloakObject2.SetActive(false);
		}
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
