using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[Header("Components")]
	[SerializeField] PoolEffect poolEffect;
	public PoolEffect PoolEffect { get { return poolEffect; } set { poolEffect = value; } }
	[SerializeField] PlayerController player;
	public PlayerController Player { get { return player; } set { player = value; } }
}
