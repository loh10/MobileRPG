using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Events;
using static Utils;

public class MarketItemDisplayer : MonoBehaviour
{
    public Item item;
    public Image itemSprite;
    private int _itemPrice;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPower;
    private ListingWithOwner _currentListing;
    public Button buyButton;

    private void OnDestroy()
    {
        buyButton.onClick.RemoveAllListeners();
    }

    public void SetItem(Item new_item,int price, ListingWithOwner listing)
    {
        _currentListing = listing;
        itemSprite.sprite = new_item.itemSprite;
        itemPower.text = ItemPowerString(new_item.itemPower);
        itemName.text = XmlItemName(new_item.itemName, new_item.itemType);
        _itemPrice = price;
        itemPrice.text = _itemPrice.ToString();
        item = new_item;
        buyButton.interactable = User.instance.userCurrentGold >= price;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        OnItemBuy.Invoke(item,_itemPrice);
        Debug.Log("Buy Item " + item.itemName);
        MarketManager.RemoveItemFromListing(_currentListing.vendorId, _currentListing.listing.listingId);
        Destroy(gameObject);
    }

    private string ItemPowerString(int power)
    {
        return $"+ {power}";
    }

 }
