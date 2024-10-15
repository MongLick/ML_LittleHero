using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
	public string nickName;
	public CharacterType type;
	public string position;
	public float posX;
	public float posY;
	public float posZ;
	public string scene;
	public int health;
	public int maxHealth;
	public int mana;
	public int maxMana;
	public int gold;
	public string weaponSlot;
	public string shieldSlot;
	public string cloakSlot;
	public List<string> inventory;

	public UserData(string nickName, CharacterType type, string position, float x, float y, float z, string scene,
					int health, int mana, int gold, string weapon, string shield, string cloak, List<string> inventory)
	{
		{
			this.nickName = nickName;
			this.type = type;
			this.position = position;
			this.posX = x;
			this.posY = y;
			this.posZ = z;
			this.scene = scene;
			this.health = health;
			this.maxHealth = health;
			this.mana = mana;
			this.maxMana = mana;
			this.gold = gold;
			this.weaponSlot = weapon;
			this.shieldSlot = shield;
			this.cloakSlot = cloak;
			this.inventory = inventory ?? new List<string>(new string[16]);
		}
	}

	public enum CharacterType { Man, WoMan }

	public event Action<int> OnHealthChanged;
	public int Health { get { return health; } set { { health = value; OnHealthChanged?.Invoke(health); } } }
	public event Action<int> OnManaChanged;
	public int Mana { get { return health; } set { { Mana = value; OnManaChanged?.Invoke(Mana); } } }
}
