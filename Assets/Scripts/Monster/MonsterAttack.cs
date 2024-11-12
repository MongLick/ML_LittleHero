using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
	[Header("Specs")]
	[SerializeField] LayerMask palyerLayer;
	[SerializeField] int damage;

	private void OnTriggerEnter(Collider other)
	{
		if (palyerLayer.Contain(other.gameObject.layer))
		{
			IDamageable damageable = other.GetComponent<IDamageable>();
			if (damageable != null)
			{
				bool isCritical = Random.Range(0f, 1f) < 0.33f;
				damageable.TakeDamage(damage, isCritical);
			}
		}
	}
}
