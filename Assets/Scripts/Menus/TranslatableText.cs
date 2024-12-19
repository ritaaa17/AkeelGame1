using UnityEngine;
using TMPro;

public class TranslatableText : MonoBehaviour
{
    [SerializeField] private string arabicText;
    [SerializeField] private string englishText;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public string ArabicText => arabicText;
    public string EnglishText => englishText;

    private void Start()
    {
    }

    public void SetTextToArabic()
    {
        textMeshPro.text = arabicText;
    }

    public void SetTextToEnglish()
    {
        textMeshPro.text = englishText;
    }
}