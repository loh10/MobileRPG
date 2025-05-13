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
    private Languages _currentLanguage = Languages.English;
    private int _lastIndex = -1;
    private int _currentIndex = -1;
    private int _totalLines = 0;


    private void Awake()
    {
        _totalLines = totalLines;
    }

    private void OnEnable()
    {
        walkButton.onClick.AddListener(Walk);
    }

    private void OnDisable()
    {
        walkButton.onClick.RemoveAllListeners();
    }

    private void Walk()
    {
        OnWalkButtonClicked?.Invoke();
        NextStoryLine();
    }

    private void NextStoryLine()
    {
        _currentIndex = SetNextLineIndex(_lastIndex, _totalLines);
        string lineToDisplay = XmlLineDisplayer( _currentIndex, _currentLanguage);
        if (!string.IsNullOrEmpty(lineToDisplay))
        {
            storyText.text = lineToDisplay;
        }
        else
        {
            Debug.LogError("Failed to retrieve the story line.");
        }
        _lastIndex = _currentIndex;
    }

    private int SetNextLineIndex(int currentIndex, int maxIndex)
    {
        int nextIndex = 0;
        int attempts = 0;
        while (nextIndex == currentIndex && attempts < 10)
        {
            attempts++;
            nextIndex = Random.Range(0, maxIndex);
            if (nextIndex == currentIndex)
            {
                Debug.LogWarning("Next index is the same as the current index. Generating a new one.");
            }
        }

        return nextIndex;
    }
}
