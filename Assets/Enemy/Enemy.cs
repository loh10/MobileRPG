using System;
using UnityEngine;
using UnityEngine.UI;
using static Events;
public class Enemy : MonoBehaviour
{
    public Button dropButton;
    public Drop drop;

    public void Start()
    {
        dropButton.onClick.AddListener(DropItem);
    }

    private void DropItem()
    {
        OnEnemyKilled.Invoke(drop);
        Debug.Log("Dropped Item");
    }
}
