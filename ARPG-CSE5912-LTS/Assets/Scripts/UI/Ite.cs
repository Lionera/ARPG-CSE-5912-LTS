using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Ite : ScriptableObject
{

	new public string name = "New Item";    // Name of the item
	public Sprite icon = null;              // Item icon
	public bool showInInventory = true;
	public bool stackable;
	public ItemType type;
	[SerializeField] public GameObject prefab;

	public int attackDamage;
	public int defendRate;
	public string utilityUsage;
	//public int amount = 1;
	public virtual void Use()
	{
		// Use the item
		// Something may happen
		Debug.Log("using " + name);
	}
	
	public enum ItemType
	{
		weapon,
		armor,
		utility

	}

}
