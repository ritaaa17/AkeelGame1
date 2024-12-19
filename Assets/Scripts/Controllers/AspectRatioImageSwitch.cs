using System;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioImageSwitch : MonoBehaviour
{
    public float checkTime = 0.2f;
    public Image targetImage;

    public Sprite portraitSprite;
    public Sprite landscapeSprite;

    private float timer = 0;

    private void Start()
    {
        CheckAspectRatio();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > checkTime)
        {
            timer = 0;
            CheckAspectRatio();
        }
    }

    private void CheckAspectRatio()
    {
        if (Screen.width > Screen.height)
        {
            targetImage.sprite = landscapeSprite;
        }
        else
        {
            targetImage.sprite = portraitSprite;
        }
    }
}
