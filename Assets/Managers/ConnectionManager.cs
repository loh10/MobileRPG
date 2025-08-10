using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using static Utils;

public class ConnectionManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

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
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), (dataResult) =>
        {
            if (dataResult.Data != null && dataResult.Data.ContainsKey("password"))
            {
                string storedPassword = dataResult.Data["password"].Value;
                if (storedPassword == Encryption(passwordInputField.text))
                {
                    Debug.Log("Connexion réussie !");
                }
                else
                {
                    Debug.LogError("Mot de passe incorrect.");
                }
            }
            else
            {
                Debug.LogError("Aucun mot de passe trouvé pour cet utilisateur.");
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
            PlayFabClientAPI.UpdateUserData(updateRequest, (updateResult) =>
            {
                var displayNameRequest = new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = usernameInputField.text
                };
                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (displayNameResult) =>
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