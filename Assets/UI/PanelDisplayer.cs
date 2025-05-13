using UnityEngine;
using UnityEngine.UI;

public class PanelDisplayer : MonoBehaviour
{
    //MainMenu
    public GameObject mainMenuPanel;
    public Button mainMenuButton;
    //Story
    public GameObject storyPanel;
    public Button storyButton;
    //User
    public GameObject userPanel;
    public Button userButton;


    private GameObject _currentPanel;


    private void Start()
    {
        mainMenuButton.onClick.AddListener(ShowMainMenu);
        storyButton.onClick.AddListener(ShowStory);
        userButton.onClick.AddListener(ShowUser);

        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        ShowPanel(mainMenuPanel);
    }
    private void ShowStory()
    {
        ShowPanel(storyPanel);
    }
    private void ShowUser()
    {
        ShowPanel(userPanel);
    }
    private void ShowPanel(GameObject panel)
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }

        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }
}
