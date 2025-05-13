using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class DisplayUserInformation : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI userLevelText;

    [Header("XP")]
    public TextMeshProUGUI userXPText;
    public Image userXPBarImage;

    [Header("HP")]
    public TextMeshProUGUI userHPText;
    public Image userHPBarImage;

    [Header("Gold")]
    public TextMeshProUGUI userGoldText;

    [Header("User Visual")] public TextMeshProUGUI userDescriptionText;
    public Image userAvatarImage;
    public Image userBackgroundImage;

    public Languages language = Languages.English;


    private void Start()
    {
        User user = GetComponent<User>();
        if (user != null)
        {
            DisplayUserInfo(user);
        }
        else
        {
            Debug.LogError("User component not found on this GameObject.");
        }
    }

    private void DisplayUserInfo(User user)
    {
        userNameText.text = user.userName;
        userLevelText.text = $"{XmlLineDisplayer("level", language)} {user.userLevel}";
        // XP
        userXPText.text = $"{user.userCurrentXP} / {user.userMaxXP}";
        userXPBarImage.fillAmount = (float)user.userCurrentXP / user.userMaxXP;
        //HP
        userHPText.text = $"{user.userCurrentHP} / {user.userMaxHP}";
        userHPBarImage.fillAmount = (float)user.userCurrentHP / user.userMaxHP;
        //Gold
        userGoldText.text = user.userCurrentGold.ToString();
        //Other
        userDescriptionText.text = user.userDescription;
        userAvatarImage.sprite = user.userAvatar;
        userBackgroundImage.sprite = user.userBackground;
    }
}