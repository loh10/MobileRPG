using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class DisplayUserInformation : MonoBehaviour
{
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
        userLevelText.text = $"{XmlLineDisplayer("level")} {user.userLevel}";
        // XP
        userXpText.text = $"{user.userCurrentXp} / {user.userMaxXp}";
        userXpBarImage.fillAmount = (float)user.userCurrentXp / user.userMaxXp;
        //HP
        userHpText.text = $"{user.userCurrentHp} / {user.userMaxHp}";
        userHpBarImage.fillAmount = (float)user.userCurrentHp / user.userMaxHp;
        //Gold
        userGoldText.text = user.userCurrentGold.ToString();
        //Other
        userDescriptionText.text = user.userDescription;
        userAvatarImage.sprite = user.userAvatar;
        userBackgroundImage.sprite = user.userBackground;
    }
}