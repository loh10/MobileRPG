using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "new Item")]
public class Item :ScriptableObject
{
    public int itemId;
    public string itemName;
    public EItemTypes itemType;
    public int itemPower;
    public int itemLevelRequirement;
    public Sprite itemSprite;
}
