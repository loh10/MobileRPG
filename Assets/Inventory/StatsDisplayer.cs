using System;
using static Events;
using UnityEngine;
using TMPro;

public class StatsDisplayer : MonoBehaviour
{
    public TextMeshProUGUI statText;
    public EStatTypes statType;


    private void Start()
    {
        OnItemEquipped.AddListener(arg0 => DisplayStat(User.instance.userState));
        OnItemUnequipped.AddListener(arg0 => DisplayStat(User.instance.userState));
    }

    private void OnEnable()
    {
        DisplayStat(User.instance.userState);
    }

    private void DisplayStat(UserState all_stats)
    {
        switch (statType)
        {
            case EStatTypes.Strength:
                statText.text = all_stats.strength.ToString();
                break;
            case EStatTypes.Defence:
                statText.text = all_stats.defence.ToString();
                break;
            case EStatTypes.Luck:
                statText.text = all_stats.luck.ToString();
                break;
        }
    }
}
