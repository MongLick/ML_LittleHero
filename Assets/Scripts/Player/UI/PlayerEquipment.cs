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
		UpdateEquipment(swordObject1, swordObject2, equippedWeapon == "sword1", equippedWeapon == "sword2");
	}

	public void EquipShield(string shield)
	{
		equippedShield = shield;
		UpdateEquipment(shieldObject1, shieldObject2, equippedShield == "shield1", equippedShield == "shield2");
	}

	public void EquipCloak(string cloak)
	{
		equippedCloak = cloak;
		UpdateEquipment(cloakObject1, cloakObject2, equippedCloak == "cloak1", equippedCloak == "cloak2");
	}

	private void UpdateEquipment(GameObject object1, GameObject object2, bool isEquipped1, bool isEquipped2)
	{
		object1.SetActive(isEquipped1);
		object2.SetActive(isEquipped2);
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

	private void UpdateWeapon() => UpdateEquipment(swordObject1, swordObject2, equippedWeapon == "sword1", equippedWeapon == "sword2");
	private void UpdateShield() => UpdateEquipment(shieldObject1, shieldObject2, equippedShield == "shield1", equippedShield == "shield2");
	private void UpdateCloak() => UpdateEquipment(cloakObject1, cloakObject2, equippedCloak == "cloak1", equippedCloak == "cloak2");
}
