using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    private int _currentLanguageIndex = 0;


    public void ChangeLanguage(int index)
    {
        switch (index)
        {
            case 0:
                _currentLanguageIndex = 0; // English
                break;
            case 1:
                _currentLanguageIndex = 1; // French
                break;
            default:
                _currentLanguageIndex = 0; // Default to English
                break;
        }
        Debug.Log($"Language changed to: {(Languages)_currentLanguageIndex}");
        Utils.SetCurrentLanguage((Languages)_currentLanguageIndex);
        Events.OnLanguageChanged.Invoke();
    }
}
