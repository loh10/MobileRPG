using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using static Utils;
using static Events;

public class ConnectionManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    private void OnEnable()
    {
        string password = PlayerPrefs.GetString("password", "");
        string username = PlayerPrefs.GetString("username", "");
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;
        usernameInputField.text = username;
        passwordInputField.text = password;
        Connect();
        OnUserDisconnected.AddListener(ResetInformation);
    }

    private void ResetInformation()
    {
        usernameInputField.text = "";
        passwordInputField.text = "";
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("password");
    }

    public void Connect()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = usernameInputField.text,
            CreateAccount = false
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), (data_result) =>
        {
            if (data_result.Data.TryGetValue("password", out var value))
            {
                string storedPassword = value.Value;
                if (storedPassword == Encryption(passwordInputField.text))
                {
                    PlayerPrefs.SetString("username", usernameInputField.text);
                    PlayerPrefs.SetString("password", passwordInputField.text);
                    OnUserConnected.Invoke(usernameInputField.text);

                    Debug.Log("Connexion réussie !");
                }
                else
                {
                    //TODO : DISPLAY ERROR
                    Debug.LogError("Mot de passe incorrect.");
                }
            }
        }, OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void CreateAccount()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = usernameInputField.text,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, (result) =>
        {
            var updateRequest = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { "password", Encryption(passwordInputField.text) }
                }
            };
            PlayFabClientAPI.UpdateUserData(updateRequest, (update_result) =>
            {
                var displayNameRequest = new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = usernameInputField.text
                };
                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (display_name_result) =>
                {
                    Debug.Log("Compte créé avec succès et display name défini !");
                }, OnRegisterFailure);
            }, OnRegisterFailure);
        }, OnRegisterFailure);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}