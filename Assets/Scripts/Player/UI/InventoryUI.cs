using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	/*[SerializeField] Button closeButton;
	[SerializeField] InventoryIcon[] icons;
	private ItemData[] items;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	private void Start()
	{
		items = new ItemData[]
		{
			new ItemData("Sword", ItemType.Weapon, Resources.Load<Sprite>("Icons/sword")),
			new ItemData("Shield", ItemType.Shield, Resources.Load<Sprite>("Icons/shield")),
			new ItemData("Cloak", ItemType.Cloak, Resources.Load<Sprite>("Icons/cloak")),
			new ItemData("PotionHP", ItemType.PotionHP, Resources.Load<Sprite>("Icons/hp")),
			new ItemData("PotionMP", ItemType.PotionHP, Resources.Load<Sprite>("Icons/mp")),
		};

		UpdateInventoryUI(items);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	public void UpdateInventoryUI(ItemData[] items)
	{
		for (int i = 0; i < icons.Length; i++)
		{
			if (i < items.Length)
			{
				icons[i].AddItem(items[i]);
			}
			else
			{
				icons[i].ClearSlot();
			}
		}
	}*/
}
