using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;
using static Events;

public class DisplayUserInformation : MonoBehaviour
{
    public User currentUser;
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI userLevelText;

    [Header("XP")]
    public TextMeshProUGUI userXpText;
    public Image userXpBarImage;

    [Header("HP")]
    public TextMeshProUGUI userHpText;
    public Image userHpBarImage;

    [Header("Gold")]
    public TextMeshProUGUI userGoldText;

    [Header("User Visual")]
    public TextMeshProUGUI userDescriptionText;
    public Image userAvatarImage;
    public Image userBackgroundImage;

    private void Start()
    {
        DisplayUserInfo(currentUser);
    }

    private void OnEnable()
    {
        OnUserUpdate.AddListener(UpdatePlayerInformation);
    }

    private void OnDisable()
    {
        OnUserUpdate.RemoveListener(UpdatePlayerInformation);
    }

    private void UpdatePlayerInformation(User updated_user)
    {
        currentUser = updated_user;
        DisplayUserInfo(currentUser);
    }

    private void DisplayUserInfo(User user)
    {
        userNameText.text = user.userName;
        userLevelText.text = $"{XmlLineDisplayer("level")} {user.userLevel}";
        userXpText.text = $"{user.userCurrentXp} / {user.userMaxXp}";
        userXpBarImage.fillAmount = (float)user.userCurrentXp / user.userMaxXp;
        userHpText.text = $"{user.userCurrentHp} / {user.userMaxHp}";
        userHpBarImage.fillAmount = (float)user.userCurrentHp / user.userMaxHp;
        userGoldText.text = user.userCurrentGold.ToString();
        userDescriptionText.text = user.userDescription;
        userAvatarImage.sprite = user.userAvatar;
        userBackgroundImage.sprite = user.userBanner;
    }
}