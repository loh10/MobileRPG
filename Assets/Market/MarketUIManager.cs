using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIManager : MonoBehaviour
{

    public GameObject itemUi;

    private void OnEnable()
    {
        MarketManager.GetVendors(vendor_ids =>
        {
            if (vendor_ids.Count == 0)
                return;

            LoadAllMarkets(vendor_ids, DisplayMarket);
        });
    }

    private void OnDisable()
    {
        DestroyAllMarkets();
    }

    private void LoadAllMarkets(List<string> vendor_ids, System.Action<List<ListingWithOwner>> on_complete)
    {
        List<ListingWithOwner> allListings = new List<ListingWithOwner>();
        int pending = vendor_ids.Count;

        foreach (var id in vendor_ids)
        {
            MarketManager.GetPlayerMarket(id, market =>
            {
                foreach (var listing in market.listings)
                {
                    allListings.Add(new ListingWithOwner
                    {
                        vendorId = id,
                        listing = listing
                    });
                }

                pending--;
                if (pending <= 0)
                {
                    on_complete?.Invoke(allListings);
                }
            });
        }
    }

    private void DestroyAllMarkets()
    {
        foreach (Transform child in GetComponentInChildren<VerticalLayoutGroup>().transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DisplayMarket(List<ListingWithOwner> all_listings)
    {
        DestroyAllMarkets();
        foreach (var listingWithOwner in all_listings)
        {
            var listing = listingWithOwner.listing;
            if(listingWithOwner.vendorId == User.instance.PlayFabId)
                continue;
            Debug.Log($"Vendeur: {listingWithOwner.vendorId} | Item {listing.itemId} - Prix: {listing.price} - ID annonce: {listing.listingId}");
            GameObject newItem = Instantiate(itemUi, transform);
            newItem.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform);
            newItem.GetComponent<MarketItemDisplayer>().SetItem(InventoryManager.instance.GetItemById(listing.itemId), listing.price, listingWithOwner);
        }
    }
}