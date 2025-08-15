using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Events;

public class ItemDisplayer : MonoBehaviour
{
    public Item item;
    public Image itemSprite;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPower;
    public Button equipButton;

    public void SetItem(Item new_item)
    {
        itemSprite.sprite = new_item.itemSprite;
        itemName.text = new_item.itemName;
        itemPower.text = new_item.itemPower.ToString();
        item = new_item;
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipItem(new_item));
    }

    private void EquipItem(Item new_item)
    {
        OnItemEquipped.Invoke(new_item);
    }
}