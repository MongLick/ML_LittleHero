using Photon.Pun;
using UnityEngine;

public class PlayerEquipment : MonoBehaviourPun
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

	[PunRPC]
	public void EquipWeapon(string weapon)
	{
		equippedWeapon = weapon;
		UpdateEquipment(swordObject1, swordObject2, equippedWeapon == "sword1", equippedWeapon == "sword2");
	}

	[PunRPC]
	public void EquipShield(string shield)
	{
		equippedShield = shield;
		UpdateEquipment(shieldObject1, shieldObject2, equippedShield == "shield1", equippedShield == "shield2");
	}

	[PunRPC]
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

	public void TryEquipWeapon(string weapon)
	{
		photonView.RPC("EquipWeapon", RpcTarget.All, weapon);
	}

	public void TryEquipShield(string shield)
	{
		photonView.RPC("EquipShield", RpcTarget.All, shield);
	}

	public void TryEquipCloak(string cloak)
	{
		photonView.RPC("EquipCloak", RpcTarget.All, cloak);
	}
}
