using System;
using System.Xml;
using TMPro;
using UnityEngine;
using static Utils;

public class UITextDisplayer : MonoBehaviour
{
    public string lineName;
    private void Start()
    {
        SetText(lineName);
    }

    private void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = XmlLineDisplayer(text);
    }

    private void OnEnable()
    {
        Events.OnLanguageChanged.AddListener(ChangeLanguage);
    }

    private void OnDisable()
    {
        Events.OnLanguageChanged.RemoveListener(ChangeLanguage);
    }

    private void ChangeLanguage ()
    {
        SetText(lineName);
    }


}