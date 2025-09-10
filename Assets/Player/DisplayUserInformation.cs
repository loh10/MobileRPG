using System;
using PlayFab;
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
    private float _timeForMaxHeal;

    [Header("Gold")]
    public TextMeshProUGUI userGoldText;

    [Header("User Visual")]
    public TextMeshProUGUI userDescriptionText;
    public Image userAvatarImage;
    public Image userBackgroundImage;

    public Button disconnectButton;

    private void Start()
    {
        DisplayUserInfo(currentUser);
    }

    private void OnEnable()
    {
        OnUserUpdate.AddListener(UpdatePlayerInformation);
        disconnectButton.onClick.AddListener(DisconnectUser);
    }

    private void UpdatePlayerInformation(User updated_user)
    {
        currentUser = updated_user;
        DisplayUserInfo(currentUser);
    }

    private void DisplayUserInfo(User user)
    {
        Debug.Log("experience =  " + user.experienceManager.CurrentXp + " / " + user.experienceManager.MaxXp);
        userNameText.text = user.userName;
        userLevelText.text = $"{XmlLineDisplayer("level")} {user.experienceManager.Level}";
        userXpText.text = $"{user.experienceManager.CurrentXp} / {user.experienceManager.MaxXp}";
        userXpBarImage.fillAmount = (float)user.experienceManager.CurrentXp / user.experienceManager.MaxXp;
        userHpText.text = $"{user.userCurrentHp} / {user.userMaxHp}";
        userHpBarImage.fillAmount = (float)user.userCurrentHp / user.userMaxHp;
        userGoldText.text = user.userCurrentGold + " g";
        userDescriptionText.text = user.userDescription;
        userAvatarImage.sprite = user.userAvatar;
        userBackgroundImage.sprite = user.userBanner;
    }

    private void DisconnectUser()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        currentUser = null;
        OnUserDisconnected.Invoke();
    }
}