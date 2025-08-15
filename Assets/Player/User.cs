using static Utils;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using static Events;

public class User : MonoBehaviour
{
    public static User instance;
    public string userName;
    public int userLevel;
    [Header("XP")] public int userCurrentXp;
    public int userMaxXp;
    [Header("HP")] public int userCurrentHp;
    public int userMaxHp;
    [Header("Gold")] public int userCurrentGold;
    [Header("User Stats")] public UserState userState;
    public Sprite userAvatar;
    public Sprite userBanner;
    private int _userAvatarId;
    private int _userBannerId;
    public string userDescription;


    private void Start()
    {
        if(!instance)
            instance = this;
        OnUserConnected.AddListener(SetUsername);
        OnUserUpdate.AddListener(UpdateUserInfo);
        OnItemEquipped.AddListener(EquipItem);
        OnItemUnequipped.AddListener(UnequipItem);
    }

    private void OnDisable()
    {
        OnUserUpdate.RemoveListener(UpdateUserInfo);
    }

    private void EquipItem(Item new_item)
    {
        switch (new_item.itemType)
        {
            case EItemTypes.Chests:
            case EItemTypes.Legs:
                userState.defence += new_item.itemPower;
                break;
            case EItemTypes.Weapons:
                userState.strength += new_item.itemPower;
                break;
            case EItemTypes.Pets:
                userState.luck += new_item.itemPower;
                break;
        }
    }

    private void UnequipItem(Item new_item)
    {
            switch (new_item.itemType)
            {
                case EItemTypes.Chests:
                case EItemTypes.Legs:
                    userState.defence -= new_item.itemPower;
                    break;
                case EItemTypes.Weapons:
                    userState.strength -= new_item.itemPower;
                    break;
                case EItemTypes.Pets:
                    userState.luck -= new_item.itemPower;
                    break;
            }
    }


    private void SetUsername(string user_name)
    {
        userName = user_name;
        OnUserUpdate.Invoke(this);
        OnUserConnected.RemoveListener(SetUsername);
    }

    private static void UpdateUserInfo(User user)
    {
        PlayerState state = new PlayerState
        {
            level = user.userLevel,
            gold = user.userCurrentGold,
            description = user.userDescription,
            currentXp = user.userCurrentXp,
            maxXp = user.userMaxXp,
            currentHp = user.userCurrentHp,
            maxHp = user.userMaxHp,
            avatarID = user._userAvatarId,
            bannerID = user._userBannerId
        };

        string json = JsonUtility.ToJson(state);

        SaveJson(json, "PlayerData");
    }
}
