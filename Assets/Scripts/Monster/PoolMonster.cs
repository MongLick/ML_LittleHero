using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMonster : MonoBehaviour
{
	[SerializeField] PooledObject mushroomPrefab;
	[SerializeField] PooledObject cactusPrefab;
	[SerializeField] Transform[] mushroomSpawnPoints;
	[SerializeField] Transform[] cactusSpawnPoints;

	private void OnEnable()
	{
		Manager.Pool.CreatePool(mushroomPrefab, 3, 3);
		Manager.Pool.CreatePool(cactusPrefab, 3, 3);
		SpawnMonsters();
	}

	private void SpawnMonsters()
	{
		for (int i = 0; i < mushroomSpawnPoints.Length; i++)
		{
			PooledObject mushroomInstance = Manager.Pool.GetPool(mushroomPrefab, mushroomSpawnPoints[i].position, Quaternion.identity);
			MonsterController mushroomController = mushroomInstance.GetComponent<MonsterController>();

			mushroomController.SpawnPos = mushroomSpawnPoints[i];
		}

		for (int i = 0; i < cactusSpawnPoints.Length; i++)
		{
			PooledObject cactusInstance = Manager.Pool.GetPool(cactusPrefab, cactusSpawnPoints[i].position, Quaternion.identity);
			MonsterController cactusController = cactusInstance.GetComponent<MonsterController>();

			cactusController.SpawnPos = cactusSpawnPoints[i];
		}
	}
}
