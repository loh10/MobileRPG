using System;
using System.Xml;
using UnityEngine;

public static class Utils
{
    private static XmlDocument _currentXmlDoc;
    public static int totalLines;
    private static Languages _language = Languages.English;

    #region XmlManagement
    public static string XmlLineDisplayer( int index)
    {
        string line = "";
        try
        {
            // Navigate to the correct language and story node
            XmlNode languageNode = _currentXmlDoc.SelectSingleNode($"/Game/{_language}/Story");
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
                Debug.LogError($"Language node '{_language}' not found in the XML.");
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
            XmlNode languageNode = _currentXmlDoc.SelectSingleNode($"/Game/{_language}/Menu");
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
                Debug.LogError($"Language node '{_language}' not found in the XML.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing XML file: {ex.Message}");
        }
        return line;
    }

    public static void XmlLoader(XmlDocument xml_document)
    {
        XmlNode languageNode = xml_document.SelectSingleNode("/Game/English/Story");
        if (languageNode != null)
        {
            XmlNodeList lines = languageNode.SelectNodes("line");
            totalLines = lines != null ? lines.Count : 0;
            Debug.Log($"Total lines in the default language: {totalLines}");
        }
        else
        {
            Debug.LogError("Default language node 'English' not found in the XML.");
        }
        _currentXmlDoc = xml_document;
    }

    #endregion

    public static void SetCurrentLanguage(Languages new_language)
    {
        _language = new_language;
    }
}