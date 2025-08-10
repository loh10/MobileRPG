using UnityEngine;
using UnityEngine.Serialization;

public class User : MonoBehaviour
{
    public string userName;
    private string _password;
    public int userLevel;
    [Header("XP")]
     public int userCurrentXp;
    public int userMaxXp;
    [Header("HP")]
    public int userCurrentHp;
    public int userMaxHp;
    [Header("Gold")]
    public int userCurrentGold;
    [Header("User Stats")]
    public Sprite userAvatar;
    public Sprite userBackground;
    public string userDescription;
}