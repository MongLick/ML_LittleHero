using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] PlayerController playerController;

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
				damageable.TakeDamage(damage, playerController, false);
			}
		}
	}
}
