using static Utils;
using UnityEngine;
using PlayFab;
using static Events;
using System;

public class User : MonoBehaviour
{
    public static User instance;
    public string PlayFabId
    {
        get;
        private set;
    }
    public string userName;
    [SerializeField] public ExperienceManager experienceManager;
    [Header("HP")] public int userCurrentHp;
    public int userMaxHp;
    [Header("Gold")] public int userCurrentGold;
    [Header("User Stats")] public UserStats userStats;
    public Sprite userAvatar;
    public Sprite userBanner;
    private int _userAvatarId;
    private int _userBannerId;
    public string userDescription;
    private const string LastResetKey = "LastHpResetDate";

    private void Start()
    {
        if (!instance)
            instance = this;
        OnUserConnected.AddListener(SetUsername);
        OnUserConnected.AddListener(_=> LoadUserData());
        OnUserConnected.AddListener(_=> CheckAndResetHp());
        OnUserUpdate.AddListener(UpdateUserInfo);
        OnItemEquipped.AddListener(EquipItem);
        OnItemUnequipped.AddListener(UnequipItem);
        OnItemBuy.AddListener((item, price) => RemoveGold(price));
        OnLifeGained.AddListener(AddHp);
        OnLifeLost.AddListener(RemoveHp);
    }
    private void CheckAndResetHp()
    {
        string lastReset = PlayerPrefs.GetString(LastResetKey, "");
        string today = DateTime.UtcNow.ToString("yyyyMMdd");
        if (lastReset != today)
        {
            userCurrentHp = userMaxHp;
            PlayerPrefs.SetString(LastResetKey, today);
            OnUserUpdate.Invoke(this);
        }
    }
    private void EquipItem(Item new_item)
    {
        switch (new_item.itemType)
        {
            case EItemTypes.ChestPlate:
            case EItemTypes.Legs:
                userStats.defence += new_item.itemPower;
                break;
            case EItemTypes.Weapons:
                userStats.strength += new_item.itemPower;
                break;
            case EItemTypes.Pets:
                userStats.luck += new_item.itemPower;
                break;
        }
    }
    private void UnequipItem(Item item_removed)
    {
        switch (item_removed.itemType)
        {
            case EItemTypes.ChestPlate:
            case EItemTypes.Legs:
                userStats.defence -= item_removed.itemPower;
                break;
            case EItemTypes.Weapons:
                userStats.strength -= item_removed.itemPower;
                break;
            case EItemTypes.Pets:
                userStats.luck -= item_removed.itemPower;
                break;
        }
    }
    private void SetUsername(string user_name)
    {
        userName = user_name;
        PlayFabId = PlayFabSettings.staticPlayer.PlayFabId;
        OnUserUpdate.Invoke(this);
        OnUserConnected.RemoveListener(SetUsername);
    }

    private static void UpdateUserInfo(User user)
    {
        PlayerState state = new PlayerState
        {
            name = user.name,
            level = user.experienceManager.Level,
            gold = user.userCurrentGold,
            description = user.userDescription,
            currentXp = user.experienceManager.CurrentXp,
            maxXp = user.experienceManager.MaxXp,
            currentHp = user.userCurrentHp,
            maxHp = user.userMaxHp,
            avatarID = user._userAvatarId,
            bannerID = user._userBannerId
        };
        string json = JsonUtility.ToJson(state);
        Debug.Log(json);
        SaveJson(json, "PlayerData",true);
    }
    public void AddGold(int amount)
    {
        userCurrentGold += amount;
        OnUserUpdate.Invoke(this);
        Debug.Log("Gold Added: " + amount);
    }
    private void RemoveGold(int amount)
    {
        if (userCurrentGold >= amount)
        {
            userCurrentGold -= amount;
            OnUserUpdate.Invoke(this);
        }
    }
    private void AddHp(int amount)
    {
        userCurrentHp += amount;
        if (userCurrentHp > userMaxHp)
            userCurrentHp = userMaxHp;
        OnUserUpdate.Invoke(this);
    }
    private void RemoveHp(int amount)
    {
        userCurrentHp -= amount;
        if (userCurrentHp < 0)
        {
            userCurrentHp = 0;
            Debug.Log("User Defeated!");
        }
        OnUserUpdate.Invoke(this);
    }
    private void LoadUserData()
    {
        LoadJsonFromPlayFab("PlayerData", json =>
        {
            PlayerState state = JsonUtility.FromJson<PlayerState>(json);
            Debug.Log(json);
            userDescription = state.description;
            userCurrentGold = state.gold;
            userMaxHp = state.maxHp;
            userCurrentHp = state.currentHp;
            experienceManager = new ExperienceManager(state.currentXp, state.maxXp, state.level);
            OnExperienceGained.RemoveAllListeners();
            OnExperienceGained.AddListener(experienceManager.GainExperience);
            userAvatar = Resources.Load<Sprite>($"Avatars/avatar_{state.avatarID}");
            userBanner = Resources.Load<Sprite>($"Banners/banner_{state.bannerID}");
            OnUserUpdate.Invoke(this);
        });
        OnExperienceGained.AddListener(experienceManager.GainExperience);

    }
}