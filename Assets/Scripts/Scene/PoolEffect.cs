using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEffect : MonoBehaviour
{
    [SerializeField] PooledObject firePrefab;
    [SerializeField] PooledObject icePrefab;

	private void OnEnable()
	{
		Manager.Pool.CreatePool(firePrefab, 1, 1);
		Manager.Pool.CreatePool(icePrefab, 1, 1);
	}

	public void SpawnEffects(string prefabName, Transform potion)
	{
		if (prefabName == firePrefab.name)
		{
			Manager.Pool.GetPool(firePrefab, potion.position, Quaternion.identity);
		}
		else
		{
			Manager.Pool.GetPool(icePrefab, potion.position, Quaternion.identity);
		}
	}
}
