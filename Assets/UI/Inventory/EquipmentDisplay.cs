using UnityEngine;
using static Events;
using UnityEngine.UI;

public class EquipmentDisplay : MonoBehaviour
{
    public EItemTypes itemType;
    public Item item;
    public Image itemSprite;

    private void Awake()
    {
        OnItemEquipped.AddListener(DisplayItem);
    }

    public void DisplayItem(Item new_item)
    {
        if (new_item.itemType != itemType) return;
        item = new_item;
        itemSprite.sprite = item.itemSprite;
    }
}