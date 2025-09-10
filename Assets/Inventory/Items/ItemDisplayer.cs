using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Events;
using static Utils;

public class ItemDisplayer : MonoBehaviour
{
    public Item item;
    public Image itemSprite;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPower;
    public Button equipButton;

    // Sell
    public Button sellButton;
    public GameObject sellInfoPanel;
    public Button fastSellButton;
    public Button confirmSellButton;
    public Button cancelSellButton;
    public TMP_InputField priceField;

    private void OnEnable()
    {
        if (!item) return;
        itemName.text = XmlItemName(item.itemName, item.itemType);
        equipButton.interactable = item.itemLevelRequirement <= User.instance.experienceManager.Level;
    }

    public void SetItem(Item new_item)
    {
        itemSprite.sprite = new_item.itemSprite;
        itemPower.text = ItemPowerString(new_item.itemPower);
        itemName.text = XmlItemName(new_item.itemName, new_item.itemType);
        item = new_item;
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => EquipItem(new_item));
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(SellItem);
    }

    private void EquipItem(Item new_item)
    {
        OnItemEquipped.Invoke(new_item);
    }

    private string ItemPowerString(int power)
    {
        return $"+ {power}";
    }

    private int GetPrice()
    {
        if (string.IsNullOrWhiteSpace(priceField.text)) return 0;
        var match = System.Text.RegularExpressions.Regex.Match(priceField.text, @"^\d+");
        if (match.Success && int.TryParse(match.Value, out int currentPrice))
            return currentPrice;
        return 0;
    }

    private void ConfirmSellingInfo()
    {
        MarketManager.AddListing(item.itemId,GetPrice());
        sellInfoPanel.SetActive(false);
        MarketManager.SetAsVendor();
        OnItemSold.Invoke(item,GetPrice());
    }

    private void SellItem()
    {
        sellInfoPanel.SetActive(true);
        confirmSellButton.onClick.RemoveAllListeners();
        confirmSellButton.onClick.AddListener(ConfirmSellingInfo);
        fastSellButton.onClick.RemoveAllListeners();
        fastSellButton.onClick.AddListener(FastSellItem);
        cancelSellButton.onClick.AddListener(() => sellInfoPanel.SetActive(false));
    }

    private void FastSellItem()
    {
        sellInfoPanel.SetActive(false);
        OnItemSold.Invoke(item, item.itemBasePrice);
    }

}