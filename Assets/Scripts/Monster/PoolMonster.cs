using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolMonster : MonoBehaviour
{
	[SerializeField] PooledObject mushroomPrefab;
	[SerializeField] PooledObject cactusPrefab;
	[SerializeField] Transform[] mushroomSpawnPoints;
	[SerializeField] Transform[] cactusSpawnPoints;

	private void OnEnable()
	{
		Manager.Pool.CreatePool(mushroomPrefab, 3, 5);
		Manager.Pool.CreatePool(cactusPrefab, 3, 5);
		SpawnMonsters();
	}

	private void SpawnMonsters()
	{
		for (int i = 0; i < mushroomSpawnPoints.Length; i++)
		{
			PooledObject mushroomInstance = Manager.Pool.GetPool(mushroomPrefab, mushroomSpawnPoints[i].position, Quaternion.identity);
			MonsterController mushroomController = mushroomInstance.GetComponent<MonsterController>();

			mushroomController.SpawnPos = mushroomSpawnPoints[i];
			mushroomController.OnDieEvent.AddListener(MushroomDie);
		}

		for (int i = 0; i < cactusSpawnPoints.Length; i++)
		{
			PooledObject cactusInstance = Manager.Pool.GetPool(cactusPrefab, cactusSpawnPoints[i].position, Quaternion.identity);
			MonsterController cactusController = cactusInstance.GetComponent<MonsterController>();

			cactusController.SpawnPos = cactusSpawnPoints[i];
			cactusController.OnDieEvent.AddListener(CactusDie);
		}
	}

	private void MushroomDie(MonsterController monster)
	{
		StartCoroutine(MonsterDieCoroutine(monster));
	}

	private IEnumerator MonsterDieCoroutine(MonsterController monster)
	{
		yield return new WaitForSeconds(3f);
		Manager.Pool.GetPool(mushroomPrefab, monster.SpawnPos.position, Quaternion.identity);
		monster.Initialize();
	}

	private void CactusDie(MonsterController monster)
	{
		StartCoroutine(CactusDieCoroutine(monster));
	}

	private IEnumerator CactusDieCoroutine(MonsterController monster)
	{
		yield return new WaitForSeconds(3f);
		Manager.Pool.GetPool(cactusPrefab, monster.SpawnPos.position, Quaternion.identity);
		monster.Initialize();
	}
}
