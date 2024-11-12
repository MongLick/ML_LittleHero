using UnityEngine;

public class PlayerSkillAttack : MonoBehaviour
{
	[Header("Specs")]
	[SerializeField] LayerMask monsterLayer;
	[SerializeField] int damage;

	private void OnTriggerEnter(Collider other)
	{
		if (monsterLayer.Contain(other.gameObject.layer))
		{
			IDamageable damageable = other.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(damage, true);
			}
		}
	}
}
