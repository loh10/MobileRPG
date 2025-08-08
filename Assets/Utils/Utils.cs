using System;
using System.Xml;
using UnityEngine;

public static class Utils
{
    private static XmlDocument _currentXmlDoc;
    public static int totalLines;
    public static Languages language = Languages.English;

    #region XmlManagement
    public static string XmlLineDisplayer( int index)
    {
        string line = "";
        try
        {
            // Navigate to the correct language and story node
            XmlNode languageNode = _currentXmlDoc.SelectSingleNode($"/Game/{language}/Story");
            if (languageNode != null)
            {
                XmlNodeList lines = languageNode.SelectNodes("line");
                if (index >= 0 && lines != null && index < lines.Count)
                {
                    line = lines[index].InnerText;
                }
                else
                {
                    Debug.LogError("Index out of range for the selected language.");
                }
            }
            else
            {
                Debug.LogError($"Language node '{language}' not found in the XML.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing XML file: {ex.Message}");
        }
        return line;
    }
    public static string XmlLineDisplayer(string line_name)
    {
        string line = "";
        try
        {
            XmlNode languageNode = _currentXmlDoc.SelectSingleNode($"/Game/{language}/Menu");
            if (languageNode != null)
            {
                XmlNode lineNode = languageNode.SelectSingleNode(line_name);
                if (lineNode != null)
                {
                    line = lineNode.InnerText;
                }
                else
                {
                    Debug.LogError($"Line with name '{line_name}' not found in the selected language.");
                }
            }
            else
            {
                Debug.LogError($"Language node '{language}' not found in the XML.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing XML file: {ex.Message}");
        }
        return line;
    }
    public static void XmlLoader(string path)
    {

        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(path);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading XML file: {ex.Message}");
            return;
        }

        XmlNode languageNode = xmlDoc.SelectSingleNode("/Game/English/Story");
        if (languageNode != null)
        {
            XmlNodeList lines = languageNode.SelectNodes("line");
            if (lines != null)
            {
                totalLines = lines.Count;
            }
        }
        else
        {
            Debug.LogError("Default language node 'English' not found in the XML.");
        }
        _currentXmlDoc = xmlDoc;
    }
    public static void SetCurrentXmlDoc(XmlDocument xml_doc)
    {
        _currentXmlDoc = xml_doc;
    }
    #endregion

    public static void SetCurrentLanguage(Languages new_language)
    {
        language = new_language;
    }
}