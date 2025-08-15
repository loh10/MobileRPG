using System;
using System.Collections.Generic;
using System.Xml;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class Utils
{
    private static XmlDocument _currentXmlDoc;
    public static int totalLines;
    private static ELanguages _language = ELanguages.English;
    private static float _lastSaveTime = -10f;

    #region XmlManagement
    public static string XmlLineDisplayer(int index)
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

    #region DatabaseManagement
    [System.Serializable]
    public class PlayerState
    {
        public int level;
        public int gold;
        public string description;
        public int currentXp;
        public int maxXp;
        public int currentHp;
        public int maxHp;
        public int avatarID;
        public int bannerID;
    }

    [System.Serializable]
    public class PlayerInventory
    {
        public List<int> itemsId;
        public int equippedWeaponId;
        public int equippedChestId;
        public int equippedLegsId;
        public int equippedPetId;
    }

    public static void SaveJson(string json, string key)
    {
        if (Time.time - _lastSaveTime < 5f)
            return;
        _lastSaveTime = Time.time;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { key, json }
            }
        };
        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("Data saved successfully!"),
            error => Debug.LogError($"Error saving data: {error.GenerateErrorReport()}"));
    }

    public static void LoadInventoryJsonFromPlayFab(string key, Action<string> on_loaded)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                if (result.Data != null && result.Data.ContainsKey(key))
                {
                    string json = result.Data[key].Value;
                    on_loaded.Invoke(json);
                }
                else
                {
                    Debug.LogError($"Clé '{key}' non trouvée dans les UserData PlayFab.");
                }
            },
            error =>
            {
                Debug.LogError("Erreur lors de la récupération des UserData: " + error.GenerateErrorReport());
            });
    }


    #endregion

    #region PasswordManagement
    public static string Encryption(string password_to_hash)
    {
        using (var sha1 = System.Security.Cryptography.SHA1.Create())
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password_to_hash);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
    #endregion

    public static void SetCurrentLanguage(ELanguages new_language)
    {
        _language = new_language;
    }
}