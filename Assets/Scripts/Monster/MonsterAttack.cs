using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
	[SerializeField] LayerMask palyerLayer;
	[SerializeField] int damage;

	private void OnTriggerEnter(Collider other)
	{
		if (palyerLayer.Contain(other.gameObject.layer))
		{
			IDamageable damageable = other.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(damage, true);
			}
		}
	}
}
