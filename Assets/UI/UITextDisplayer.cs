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
        GetComponent<TextMeshProUGUI>().text =XmlLineDisplayer( lineName);
    }
}