using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Listing
{
    public string listingId;
    public int itemId;
    public int price;
}

public class ListingWithOwner
{
    public string vendorId;
    public Listing listing;
}


[Serializable]
public class MarketData
{
    public List<Listing> listings = new List<Listing>();
}

public static class MarketManager
{
    private static MarketData _selfMarket = new MarketData();

    private static MarketData _localMarket = new MarketData();

    private static List<string> _vendorIds = new List<string>();

    private static void SaveMarketData()
    {
        string json = JsonUtility.ToJson(_selfMarket);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "PlayerMarket", json }
            },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("Market saved."),
            error => Debug.LogError(error.GenerateErrorReport()));

        SaveLocalMarketData();
    }

    private static void SaveLocalMarketData()
    {
        string json = JsonUtility.ToJson(_selfMarket);
        PlayerPrefs.SetString("PlayerMarket", json);
        PlayerPrefs.Save();
    }

    private static void LoadLocalMarketData()
    {
        if (PlayerPrefs.HasKey("PlayerMarket"))
        {
            string json = PlayerPrefs.GetString("PlayerMarket");
            _localMarket = JsonUtility.FromJson<MarketData>(json);
        }
        else
        {
            _localMarket = new MarketData();
        }

        CompareLocalMarketData();
    }

    private static void CompareLocalMarketData()
    {
        foreach (var listing in _localMarket.listings)
        {
            if (!_selfMarket.listings.Exists(l => l.listingId == listing.listingId))
            {
                User.instance.AddGold(listing.price);
            }
        }
    }

    public static void LoadMarketData(Action<MarketData> on_loaded)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = PlayFabSettings.staticPlayer.PlayFabId
        };

        PlayFabClientAPI.GetUserData(request, result =>
        {
            if (result.Data != null && result.Data.TryGetValue("PlayerMarket", out var value))
            {
                Debug.Log("market loaded");
                _selfMarket = JsonUtility.FromJson<MarketData>(value.Value);
            }
            else
            {
                Debug.Log("new listing created");
                _selfMarket = new MarketData();
                SaveMarketData();
            }

            on_loaded?.Invoke(_selfMarket);
        }, error => Debug.LogError(error.GenerateErrorReport()));

        LoadLocalMarketData();
    }

    public static void GetPlayerMarket(string play_fab_id, Action<MarketData> on_loaded)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = play_fab_id
        };

        PlayFabClientAPI.GetUserData(request, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("PlayerMarket"))
            {
                var market = JsonUtility.FromJson<MarketData>(result.Data["PlayerMarket"].Value);
                on_loaded?.Invoke(market);
            }
            else
            {
                on_loaded?.Invoke(new MarketData());
            }
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public static void AddListing(int item_id, int price)
    {
        var listing = new Listing
        {
            listingId = _selfMarket.listings.Count.ToString(),
            itemId = item_id,
            price = price
        };

        _selfMarket.listings.Add(listing);
        SaveMarketData();
    }

    public static void RemoveListing(string listing_id)
    {
        var listing = _selfMarket.listings.Find(l => l.listingId == listing_id);
        if (listing != null)
        {
            _selfMarket.listings.Remove(listing);
        }
        SaveMarketData();
    }

    public static void RegisterAsVendor()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "HasMarket", Value = 1 }
                }
            }, result => Debug.Log("Registered as vendor"),
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    public static void GetVendors(Action<List<string>> on_result)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest {
            StatisticName = "HasMarket",
            StartPosition = 0,
            MaxResultsCount = 100 // nb max de vendeurs
        }, result =>
        {
            _vendorIds = new List<string>();
            foreach (var entry in result.Leaderboard)
            {
                _vendorIds.Add(entry.PlayFabId);
            }
            on_result?.Invoke(_vendorIds);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public static void SetAsVendor()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "HasMarket", Value = 1 }
                }
            },
            result => {
                Debug.Log("Joueur enregistré comme vendeur");
            },
            error => {
                Debug.LogError("Impossible d'enregistrer comme vendeur: " + error.GenerateErrorReport());
            });
    }

    public static void UnsetAsVendor()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "HasMarket", Value = 0 }
                }
            },
            result => {
                Debug.Log("Joueur retiré des vendeurs");
            },
            error => {
                Debug.LogError("Impossible de retirer le joueur: " + error.GenerateErrorReport());
            });
    }

    public static void RemoveItemFromListing(string player_id, string listing_id, Action<bool> on_complete = null)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "RemovePlayerListing",
            FunctionParameter = new
            {
                playerId = player_id,
                listingId = listing_id
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result =>
            {
                if (result.Error != null)
                {
                    Debug.LogError($"Cloud Script Error: {result.Error.Error}");
                    on_complete?.Invoke(false);
                    return;
                }

                bool success = result.FunctionResult != null && (bool)result.FunctionResult;
                if (success)
                {
                    Debug.Log($"Successfully removed listing {listing_id} from player {player_id}'s market");
                }
                else
                {
                    Debug.LogError($"Failed to remove listing {listing_id} from player {player_id}'s market");
                }

                on_complete?.Invoke(success);
            },
            error =>
            {
                Debug.LogError($"Failed to execute Cloud Script: {error.GenerateErrorReport()}");
                on_complete?.Invoke(false);
            });
    }
}


