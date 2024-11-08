using Photon.Pun;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
	public GameObject PlayerPhotonView(string name, float x, float y, float z)
	{
		GameObject characterInstance = PhotonNetwork.Instantiate(name, new Vector3(x, y, z), Quaternion.identity);
		return characterInstance;
	}
}
