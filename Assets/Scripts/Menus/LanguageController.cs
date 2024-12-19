using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    private const string LanguageKey = "language";
    private const string DefaultLanguage = "en";

    // Fonts
    // public TMP_FontAsset ArabicFont;
    // public TMP_FontAsset EnglishFont;

    void Start()
    {
        CheckAndSetLanguage();
        FindTextLanguage();
        // GetAllTranslatableText();
    }

    private void CheckAndSetLanguage()
    {
        if (!PlayerPrefs.HasKey(LanguageKey))
        {
            PlayerPrefs.SetString(LanguageKey, DefaultLanguage);
            PlayerPrefs.Save();
        }

        string language = PlayerPrefs.GetString(LanguageKey);
        if (language != "ar" && language != "en")
        {
            PlayerPrefs.SetString(LanguageKey, DefaultLanguage);
            PlayerPrefs.Save();
        }

    }

    // void GetAllTranslatableText()
    // {
    //     // Find all TranslatableText components in the scene
    //     TranslatableText[] texts = FindObjectsOfType<TranslatableText>();

    //     // Check player prefs for the language
    //     string language = PlayerPrefs.GetString(LanguageKey);

    //     // Loop through all the TranslatableText components
    //     foreach (TranslatableText text in texts)
    //     {
    //         // Set the font

    //         // Set the text based on the language
    //         if (language == "ar")
    //         {
    //             text.gameObject.GetComponent<TextMeshProUGUI>().font = ArabicFont;
    //             text.gameObject.GetComponent<TextMeshProUGUI>().text = text.ArabicText;
    //         }
    //         else
    //         {
    //             text.gameObject.GetComponent<TextMeshProUGUI>().font = EnglishFont;
    //             text.gameObject.GetComponent<TextMeshProUGUI>().text = text.EnglishText;
    //         }
    //     }
    // }

    void FindTextLanguage()
    {
        // Find all TextLanguage components in the scene
        TextLanguage[] texts = Resources.FindObjectsOfTypeAll<TextLanguage>();

        // Check player prefs for the language
        string language = PlayerPrefs.GetString(LanguageKey);

        // Loop through all the TextLanguage components
        foreach (TextLanguage text in texts)
        {
            if (text.CurrentLanguage == language)
            {
                text.gameObject.SetActive(true);
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}
