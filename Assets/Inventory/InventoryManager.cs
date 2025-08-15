using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Events;
using static Utils;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public List<Item> currentsItems = new ();
    public Item[] allItems;
    public GameObject itemDisplayPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;

    //Display
    public EquipmentDisplay weaponSprite;
    public EquipmentDisplay chestSprite;
    public EquipmentDisplay legSprite;
    public EquipmentDisplay petSprite;

    private void Start()
    {
        OnEnemyKilled.AddListener(AddItem);
        OnItemEquipped.AddListener(EquipItem);
        if (_instance == null)
        {
            _instance = this;
        }

        OnUserConnected.AddListener(arg0 => LoadItems());
    }

    public void SellItem(Item item)
    {
        // Logic to sell the item
        RemoveItem(item);
    }

    private void RemoveItem(Item item)
    {
        foreach (ItemDisplayer itemDisplayer in verticalLayoutGroup.transform.GetComponentsInChildren<ItemDisplayer>())
        {
            if (itemDisplayer.item == item)
            {
                Destroy(itemDisplayer.gameObject);
                OnItemRemoved.Invoke(item);
                break;
            }
        }

        SaveInventory();
    }

    private void AddItem(Drop drop)
    {
        Item newItem = drop.DropRandomItem();
        AddItem(newItem);
    }

    private void AddItem(Item item)
    {
        CreateItemDisplay(item);
        currentsItems.Add(item);
        OnItemAdded.Invoke(item);
        SaveInventory();
    }

    private void CreateItemDisplay(Item item)
    {
        ItemDisplayer newItem = Instantiate(itemDisplayPrefab, verticalLayoutGroup.transform).GetComponent<ItemDisplayer>();
        newItem.SetItem(item);
    }

    private void EquipItem(Item item)
    {
        if (ItemTypeAlreadyEquipped(item))
            UnequipItem(item);
        RemoveItem(item);
    }

    private void UnequipItem(Item item)
    {
        AddItem(item);
        OnItemUnequipped.Invoke(item);
    }

    private bool ItemTypeAlreadyEquipped(Item item)
    {
        switch (item.itemType)
        {
            case EItemTypes.Weapons:
                if (weaponSprite.item != null) return true;
                break;
            case EItemTypes.Chests:
                if (chestSprite.item != null) return true;
                break;
            case EItemTypes.Legs:
                if (legSprite.item != null) return true;
                break;
            case EItemTypes.Pets:
                if (petSprite.item != null) return true;
                break;
        }

        return false;
    }

    private void SaveInventory()
    {
        PlayerInventory playerInventory = new PlayerInventory
        {
            itemsId = GetAllItemsIds(),
            equippedWeaponId = GetItemId(weaponSprite.item),
            equippedChestId = GetItemId(chestSprite.item),
            equippedLegsId = GetItemId(legSprite.item),
            equippedPetId = GetItemId(petSprite.item)
        };
        string json = JsonUtility.ToJson(playerInventory);
        SaveJson(json, "PlayerInventory");
    }

    private List<int> GetAllItemsIds()
    {
        List<int> itemsIds = new List<int>();
        foreach (Item item in currentsItems)
        {
            itemsIds.Add(item.itemId);
        }

        return itemsIds;
    }

    private int GetItemId(Item item)
    {
        if (item == null)
            return -1;
        return item.itemId;
    }

    private void LoadItems()
    {
        LoadInventoryJsonFromPlayFab("PlayerInventory", json =>
        {
            PlayerInventory playerInventory = JsonUtility.FromJson<PlayerInventory>(json);
            currentsItems.Clear();
            foreach (int itemId in playerInventory.itemsId)
            {
                Item item = Array.Find(allItems, i => i.itemId == itemId);
                if (!item)
                    continue;
                AddItem(item);
            }

            EquipLoadedItems(playerInventory.equippedWeaponId, weaponSprite);
            EquipLoadedItems(playerInventory.equippedChestId, chestSprite);
            EquipLoadedItems(playerInventory.equippedLegsId, legSprite);
            EquipLoadedItems(playerInventory.equippedPetId, petSprite);
        });
    }

    private void EquipLoadedItems(int item_id,EquipmentDisplay display)
    {
        Item itemToEquip = Array.Find(allItems, i => i.itemId == item_id);
        if (itemToEquip != null)
        {
            OnItemEquipped.Invoke(itemToEquip);
            display.DisplayItem(itemToEquip);
        }

    }
}