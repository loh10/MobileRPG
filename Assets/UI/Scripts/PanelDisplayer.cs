using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Events;

public class PanelDisplayer : MonoBehaviour
{
    //ConnectionHandler
    public GameObject connectionPanel;
    //MainMenu
    public GameObject mainMenuPanel;
    public Button mainMenuButton;
    //Story
    public GameObject storyPanel;
    public Button storyButton;
    //Inventory
    public GameObject inventoryPanel;
    public Button inventoryButton;
    //User
    public GameObject userPanel;
    public Button userButton;


    private GameObject _currentPanel;


    private void Start()
    {
        mainMenuButton.onClick.AddListener(ShowMainMenu);
        storyButton.onClick.AddListener(ShowStory);
        userButton.onClick.AddListener(ShowUser);
        inventoryButton.onClick.AddListener(ShowInventory);
        ShowConnectionPanel();
    }

    private void OnEnable()
    {
        OnUserConnected.AddListener(ShowMainMenu);
    }

    private void OnDisable()
    {
        OnUserConnected.RemoveListener(ShowMainMenu);
    }
    private void ShowMainMenu(string arg0)
    {
        ShowMainMenu();
    }
    private void ShowConnectionPanel()
    {
        ShowPanel(connectionPanel);
    }
    private void ShowMainMenu()
    {
        ShowPanel(mainMenuPanel);
    }
    private void ShowStory()
    {
        ShowPanel(storyPanel);
    }
    private void ShowInventory()
    {
        ShowPanel(inventoryPanel);

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
