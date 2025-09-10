using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Events;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public Stats enemyStats;
    public Drop enemyDrop;
    public Sprite enemySprite;
    public bool isDead = false;

    public void DropItem()
    {
        OnEnemyKilled.Invoke(enemyDrop);
        OnExperienceGained.Invoke(enemyDrop.experience);
        Debug.Log("Dropped Item");
    }

    public Enemy Clone()
    {
        Enemy clone = CreateInstance<Enemy>();
        clone.enemyName = this.enemyName;
        clone.enemySprite = this.enemySprite;
        clone.enemyStats = this.enemyStats.Clone();
        clone.enemyDrop = this.enemyDrop;
        clone.isDead = this.isDead;
        return clone;
    }
}
