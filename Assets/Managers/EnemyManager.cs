using static Utils;
using static Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemiesList = new List<Enemy>();
    public Image enemyImage;
    public Button enemyButton;
    public TextMeshProUGUI enemyNameText;
    private Enemy _currentEnemy;
    private Stats _enemyStats;

    private void Awake()
    {
        OnEnemyKilled.AddListener(_=>UnsetEnemy());
        UnsetEnemy();
    }

    private void UnsetEnemy()
    {
        enemyImage.enabled = false;
        enemyButton.onClick.RemoveAllListeners();
        enemyNameText.text = "";
    }

    public void SpawnEnemy(int enemy_index)
    {
        if (enemy_index < 0 || enemy_index >= enemiesList.Count)
            return;
        Enemy enemyToSpawn = enemiesList[enemy_index];
        SpawnEnemy(enemyToSpawn);
    }

    private void SpawnEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;
        SetEnemy(enemy);
    }

    private void SetEnemy(Enemy enemy)
    {
        _currentEnemy = enemy.Clone();
        _enemyStats = _currentEnemy.enemyStats.Clone();
        enemyImage.enabled = true;
        enemyImage.sprite = _currentEnemy.enemySprite;
        enemyNameText.text = XmlMonsterName(_currentEnemy.enemyName);
        enemyButton.interactable = true;
        enemyButton.onClick.AddListener(NewTurn);
        Debug.Log($"Setting up Enemy: {_currentEnemy.name}");
    }

    private void NewTurn()
    {
        PlayerAttack();
        if (_currentEnemy.isDead)
            return;
        EnemyAttack();
    }

    private void PlayerAttack()
    {
        _enemyStats.life -= User.instance.userStats.strength;
        Debug.Log("Attacking Enemy for " + User.instance.userStats.strength + " damage.");
        if (_enemyStats.life > 0) return;
        _currentEnemy.DropItem();
        _currentEnemy.isDead = true;
        OnUserUpdate.Invoke(User.instance);
    }

    private void EnemyAttack()
    {
        int damage = _enemyStats.strength - User.instance.userStats.defence;
        if (damage < 0) damage = 0;
        OnLifeLost.Invoke(damage);
    }
}