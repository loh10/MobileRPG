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
        Events.OnLanguageChanged.AddListener(ChangeLanguage);
        SetText(lineName);
    }

    private void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = XmlLineDisplayer(text);
        Debug.Log(XmlLineDisplayer(text));
    }

    private void ChangeLanguage ()
    {
        SetText(lineName);
    }
}