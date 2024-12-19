using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LanguageSetOnStart : MonoBehaviour
{
   public UnityEvent OnArabicStart;
   public UnityEvent OnEngStart;
   private void Start()
   {
      if (PlayerPrefs.GetString("language") == "en")
      {
         OnEngStart?.Invoke();
      }
      else
      {
         OnArabicStart?.Invoke();
      }
   }
}
