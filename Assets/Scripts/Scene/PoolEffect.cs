using Photon.Pun;
using UnityEngine;

public class PoolEffect : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PooledObject firePrefab;
	[SerializeField] PooledObject icePrefab;

	private void OnEnable()
	{
		Manager.Pool.CreatePool(firePrefab, 1, 1);
		Manager.Pool.CreatePool(icePrefab, 1, 1);
	}

	[PunRPC]
	public void SpawnEffectRPC(string prefabName, Vector3 position)
	{
		PooledObject pooledEffect = null;

		if (prefabName == firePrefab.name)
		{
			pooledEffect = Manager.Pool.GetPool(firePrefab, position, Quaternion.identity);
		}
		else if (prefabName == icePrefab.name)
		{
			pooledEffect = Manager.Pool.GetPool(icePrefab, position, Quaternion.identity);
		}
	}

	public void SpawnEffects(string prefabName, Vector3 position)
	{
		PhotonView photonView = PhotonView.Get(this);
		photonView.RPC("SpawnEffectRPC", RpcTarget.All, prefabName, position);
	}
}
