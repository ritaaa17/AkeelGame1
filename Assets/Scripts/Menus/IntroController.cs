using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class IntroController : MonoBehaviour
{
    // Declare the external JavaScript function
    [DllImport("__Internal")]
    private static extern void onUnityResolutionChange(int width, int height, bool fullscreen);
    // [SerializeField] TMP_Text text;

    void Awake()
    {
        // GetCurrentResolution();
    }

    void Start()
    {
        // if (Display.displays.Length > 1)
        // {
        //     // Display.displays[1].Activate();
        //     // Screen.SetResolution(Display.displays[1].systemWidth, Display.displays[1].systemHeight, true);
        //     Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        // }
        // Screen.SetResolution(390, 844, false); // Width, Height, Fullscreen
        // if (Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     GL.Clear(true, true, Color.black);
        //     Camera.main.Render();
        //     Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        //     onUnityResolutionChange(Screen.currentResolution.width, Screen.currentResolution.height, false);
        // }
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        // SceneManager.LoadScene("Main Menu");
        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GetCurrentResolution()
    {
        // int width = 390;
        // int height = 844;
        // Screen.SetResolution(width, height, FullScreenMode.Windowed);

        // // Send resolution information to the HTML page

        // if (Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     onUnityResolutionChange(width, height);
        // }
    }
}