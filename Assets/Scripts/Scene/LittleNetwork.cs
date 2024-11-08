using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleNetwork : MonoBehaviourPunCallbacks
{
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		foreach (var player in PhotonNetwork.PlayerList)
		{
			if (player != newPlayer)
			{
				RecreateOrSyncPlayer(player);
			}
		}
	}

	private void RecreateOrSyncPlayer(Player player)
	{
		
	}
}
