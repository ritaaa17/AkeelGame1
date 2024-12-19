using UnityEngine;

public class TextLanguage : MonoBehaviour
{
    [SerializeField]
    private string language;

    public string CurrentLanguage
    {
        get { return language; }
        set { language = value; }
    }
}