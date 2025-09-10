using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Events;

public class PanelDisplayer : MonoBehaviour
{
    //ConnectionHandler
    public GameObject connectionPanel;
    //Fight
    public GameObject fightPanel;
    public Button fightButton;
    //Shop
    public GameObject shopPanel;
    public Button shopButton;
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
        fightButton.onClick.AddListener(ShowFight);
        shopButton.onClick.AddListener(ShowShop);
        storyButton.onClick.AddListener(ShowStory);
        userButton.onClick.AddListener(ShowUser);
        inventoryButton.onClick.AddListener(ShowInventory);
        ShowConnectionPanel();
        OnUserDisconnected.AddListener(ShowConnectionPanel);
    }
    private void OnEnable()
    {
        OnUserConnected.AddListener(OnConnected);
    }

    private void OnConnected(string arg0)
    {
        ShowStory();
    }
    private void ShowConnectionPanel()
    {
        ShowPanel(connectionPanel);
    }
    private void ShowFight()
    {
        ShowPanel(fightPanel);
    }
    private void ShowShop()
    {
        ShowPanel(shopPanel);

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
