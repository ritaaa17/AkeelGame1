using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class TutorialMenu : MonoBehaviour
{
    // Declare the external JavaScript function
    [DllImport("__Internal")]
    private static extern void onUnityResolutionChange(int width, int height, bool fullscreen);

    void Awake()
    {
        // Set Resolution
        // Screen.SetResolution(1920, 1080, true);
        GetCurrentResolution();
    }

    // Method to go to the next scene
    public void GoToNextScene()
    {
        // Assuming the next scene is the next in the build index
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Call this function to notify the HTML page of resolution changes
    public void GetCurrentResolution()
    {
        // if (Display.displays.Length > 1)
        // {
        //     Display.displays[1].Activate();
        //     // Screen.SetResolution(Display.displays[1].systemWidth, Display.displays[1].systemHeight, true);
        //     Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        // }
        // Screen.SetResolution(390, 844, false); // Width, Height, Fullscreen
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            GL.Clear(true, true, Color.black);
            Camera.main.Render();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            onUnityResolutionChange(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }
}