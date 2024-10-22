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
	public Dictionary<int, InventorySlotData> inventory;
	public Dictionary<string, QuestData> quests;

	public UserData(string nickName, CharacterType type, string position, float x, float y, float z, string scene,
					int health, int mana, int gold, string weapon, string shield, string cloak, Dictionary<int, InventorySlotData> inventory, Dictionary<string, QuestData> quests)
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
			this.inventory = inventory ?? new Dictionary<int, InventorySlotData>();
			for (int i = 0; i < 16; i++)
			{
				this.inventory[i] = new InventorySlotData();
			}

			this.quests = quests ?? new Dictionary<string, QuestData>();
		}
	}

	public enum CharacterType { Man, WoMan }

	public event Action<int> OnHealthChanged;
	public int Health { get { return health; } set { { health = value; OnHealthChanged?.Invoke(health); } } }
	public event Action<int> OnManaChanged;
	public int Mana { get { return mana; } set { { mana = value; OnManaChanged?.Invoke(Mana); } } }
	public event Action OnGoldChanged;
	public int Gold { get { return gold; } set { { gold = value; OnGoldChanged?.Invoke(); } } }
}

[Serializable]
public class QuestData
{
	public string questID;
	public string questName;
	public bool isCompleted;
	public int mushroomCount;
	public int cactusCount;

	public QuestData(string questID, string questName, bool isCompleted, int mushroomCount = 0, int cactusCount = 0)
	{
		this.questID = questID;
		this.questName = questName;
		this.isCompleted = isCompleted;
		this.mushroomCount = mushroomCount;
		this.cactusCount = cactusCount;
	}
}

[Serializable]
public class InventorySlotData
{
	public string itemName;
	public int mumber;

	public InventorySlotData(string itemName = "", int mumber = 0)
	{
		this.itemName = itemName;
		this.mumber = mumber;
	}
}
