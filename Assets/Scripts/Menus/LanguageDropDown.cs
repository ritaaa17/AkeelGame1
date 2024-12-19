using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class LanguageDropDown : MonoBehaviour
{
    [SerializeField] Button arabicFlag;
    [SerializeField] Button englishFlag;
    [SerializeField] Button listButton;

    Vector3 chosenFlagPosition = new Vector3(-15.2f, 0, 0);
    Vector3 unchosenFlagPosition = new Vector3(-15.2f, -30, 0);

    void Awake()
    {
        CheckLanguage();
    }

    void CheckLanguage()
    {
        if (PlayerPrefs.GetString("language") == "ar")
        {
            arabicFlag.transform.localPosition = chosenFlagPosition;
            englishFlag.transform.localPosition = unchosenFlagPosition;
            // Hide the other flag
            englishFlag.gameObject.SetActive(false);
        }
        else
        {
            arabicFlag.transform.localPosition = unchosenFlagPosition;
            englishFlag.transform.localPosition = chosenFlagPosition;
            arabicFlag.gameObject.SetActive(false);
        }
    }

    public void ListClicked()
    {
        englishFlag.gameObject.SetActive(true);
        arabicFlag.gameObject.SetActive(true);
    }

    public void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString("language", language);
        CheckLanguage();
        // Restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
