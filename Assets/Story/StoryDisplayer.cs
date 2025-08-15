using System;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;
using Random = UnityEngine.Random;

public class StoryDisplayer : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public static event Action OnWalkButtonClicked;
    public Button walkButton;
    private int _lastIndex = -1;
    private int _currentIndex = -1;
    private int _totalLines = 0;

    private void OnEnable()
    {
        _totalLines = totalLines;
        if(_currentIndex == -1)
            Walk();
        walkButton.onClick.AddListener(Walk);
        Events.OnLanguageChanged.AddListener(DisplayStoryLine);
        
    }

    private void OnDisable()
    {
        walkButton.onClick.RemoveAllListeners();
        Events.OnLanguageChanged.RemoveListener(DisplayStoryLine);
    }

    private void Walk()
    {
        OnWalkButtonClicked?.Invoke();
        _currentIndex = SetNextLineIndex(_lastIndex, _totalLines);
        DisplayStoryLine();
    }

    private void DisplayStoryLine()
    {

        string lineToDisplay = XmlLineDisplayer( _currentIndex);
        storyText.text = lineToDisplay;
        _lastIndex = _currentIndex;
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
}
