using UnityEngine;

public class User : MonoBehaviour
{
    public string userName;
    public int userLevel;
    [Header("XP")] public int userCurrentXP;
    public int userMaxXP;
    [Header("HP")] public int userCurrentHP;
    public int userMaxHP;
    [Header("Gold")] public int userCurrentGold;
    [Header("User Stats")] public Sprite userAvatar;
    public Sprite userBackground;
    public string userDescription;
}