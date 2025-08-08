using UnityEngine;
using System.Xml;
using static Utils;

public class XmlManager : MonoBehaviour
{
    public string xmlFileName = "Game";
    private XmlDocument _xmlDoc;

    private void Awake()
    {
        InitializeXml();
    }

    private void InitializeXml()
    {
        var xmlTextAsset = Resources.Load<TextAsset>(xmlFileName);

        if (!xmlTextAsset)
            return;

        _xmlDoc = new XmlDocument();
        _xmlDoc.Load(new System.IO.StringReader(xmlTextAsset.text));
        XmlLoader(_xmlDoc);

    }
}