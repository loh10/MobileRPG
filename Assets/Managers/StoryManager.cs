using System;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;
using Random = UnityEngine.Random;
using static Events;

public class StoryManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public Button walkButton;
    private int _lastIndex = -1;
    private int _currentIndex = -1;
    private int _totalLines = 0;
    private int _enemyAppearChance = 15;
    public  EnemyManager enemyManager;

    private void OnEnable()
    {
        _totalLines = totalLines;
        if(_currentIndex == -1)
            Walk();
        walkButton.onClick.AddListener(Walk);
        OnLanguageChanged.AddListener(DisplayStoryLine);
        OnEnemyKilled.AddListener(_=> walkButton.interactable = true);
    }

    private void Walk()
    {
        if(Random.Range(0, 100) < _enemyAppearChance)
        {
            int enemyIndex = Random.Range(0, enemyManager.enemiesList.Count);
            enemyManager.SpawnEnemy(enemyIndex);
            storyText.text = $"<color=red>{XmlLineDisplayer("newEnemy")} </color>\n" + XmlMonsterName(enemyManager.enemiesList[enemyIndex].enemyName);
            walkButton.interactable = false;
            return;
        }
        _currentIndex = SetNextLineIndex(_lastIndex, _totalLines);
        DisplayStoryLine();
    }

    private int SetNextLineIndex(int current_index, int max_index)
    {
        int nextIndex = 0;
        int attempts = 0;
        while (nextIndex == current_index && attempts < 10)
        {
            attempts++;
            nextIndex = Random.Range(0, max_index);
        }

        return nextIndex;
    }

    private void DisplayStoryLine()
    {
        string lineToDisplay = XmlLineDisplayer( _currentIndex);
        storyText.text = lineToDisplay;
        _lastIndex = _currentIndex;
    }
}
