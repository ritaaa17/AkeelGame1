using UnityEditor;
using UnityEngine;

public class WebGLCommunicator : MonoBehaviour
{
    LeaderboardManager leaderboardManager;
    MainMenu mainMenu;
    //================================================================
    //                         Called from WebGL Build
    //================================================================


    void Start()
    {
        // Get the leaderboard manager object from scene
        leaderboardManager = FindFirstObjectByType<LeaderboardManager>();
        mainMenu = FindFirstObjectByType<MainMenu>();

    }

    public void ReceiveData(string data)
    {
        if (!leaderboardManager)
            leaderboardManager = FindFirstObjectByType<LeaderboardManager>();

        // send data to leaderboard manager
        leaderboardManager.RecieveNewUserData(data);
    }

    public void ReceiveUrl(string url)
    {
        if (!leaderboardManager)
            leaderboardManager = FindFirstObjectByType<LeaderboardManager>();
        if (!mainMenu)
            leaderboardManager = FindFirstObjectByType<LeaderboardManager>();

        Debug.Log("Received url from JavaScript: " + url);
        // send url to leaderboard manager
        if (leaderboardManager)
            leaderboardManager.RecieveLiveAppUrl(url);
        if (mainMenu)
            mainMenu.RecieveLiveAppUrl(url);
    }

    public void RecieveUserId(string userId)
    {
        leaderboardManager.RecieveUserId(userId);
    }

    public class ResolutionData
    {
        public int width;
        public int height;
        public bool isFullScreen;
    }

    public void ResolutionChanged(string jsonData)
    {
        ResolutionData data = JsonUtility.FromJson<ResolutionData>(jsonData);
        Debug.Log($"Resolution changed to: {data.width}x{data.height}, FullScreen: {data.isFullScreen}");
        // Handle the resolution change here
        Screen.SetResolution(data.width, data.height, data.isFullScreen);
    }
}
