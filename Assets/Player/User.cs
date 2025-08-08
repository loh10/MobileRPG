using UnityEngine;
using UnityEngine.Serialization;

public class User : MonoBehaviour
{
    public string userName;
    public int userLevel;
    [FormerlySerializedAs("userCurrentXP")] [Header("XP")] public int userCurrentXp;
    [FormerlySerializedAs("userMaxXP")] public int userMaxXp;
    [FormerlySerializedAs("userCurrentHP")] [Header("HP")] public int userCurrentHp;
    [FormerlySerializedAs("userMaxHP")] public int userMaxHp;
    [Header("Gold")] public int userCurrentGold;
    [Header("User Stats")] public Sprite userAvatar;
    public Sprite userBackground;
    public string userDescription;
}