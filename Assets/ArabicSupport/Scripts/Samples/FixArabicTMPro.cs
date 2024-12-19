using UnityEngine;
using System.Collections;
using ArabicSupport;
using TMPro;

public class FixArabicTMPro : MonoBehaviour {

    public bool showTashkeel = true;
    public bool useHinduNumbers = true;

    // Use this for initialization
    void Start () {
        TMP_Text textMesh = gameObject.GetComponent<TMP_Text>();
        Debug.Log(textMesh.text);

        string fixedText = ArabicFixer.Fix(textMesh.text, showTashkeel, useHinduNumbers);

        gameObject.GetComponent<TMP_Text>().text = fixedText;

		Debug.Log(fixedText);
    }

}
