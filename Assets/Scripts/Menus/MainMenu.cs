using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Collections;
using Player;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{

    //===============================================================================================
    //                                           Properties
    //===============================================================================================

    [Header("Leaderboard")]
    public GameObject leaderboardEntryPrefab;     // Prefab for each leaderboard entry (player's box)
    public Transform leaderboardContainer;        // Parent container for the leaderboard entries (background image)
    public Image backgroundImage;                 // Background image for the leaderboard container
    public GameObject leaderboardInScene;         // Leaderboard object in the scene
    public TMP_Text playerRankText;
    public TMP_Text playerScoreText;
    public TMP_Text playerNameText;
    public GameObject[] toDisableOnLeaderboard;

    // Declare the external JavaScript function
    [DllImport("__Internal")]
    private static extern void onUnityResolutionChange(int width, int height, bool fullscreen);

    // Properties to hold the leaderboard data and user data
    public List<LeaderboardManager.LeaderboardEntry> LeaderboardEntries { get; private set; } = new List<LeaderboardManager.LeaderboardEntry>();
    private string apiUrl = "https://localhost:3000";

    //===============================================================================================
    //                                           Mono Methods
    //===============================================================================================

    void Start()
    {
        // Screen.SetResolution(390, 844, true); // Width, Height, Fullscreen
        // GetCurrentResolution();
        // if (Display.displays.Length > 1)
        // {
        //     Display.displays[1].Activate();
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
        UpdateLeaderboard(); // Update the leaderboard data from the server
        leaderboardInScene.SetActive(false);
        // Invoke("GetCurrentResolution", 1f);

    }
    public void StartGame()
    {
        // Screen.SetResolution(390, 844, true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    //===============================================================================================
    //                                           Main Methods
    //===============================================================================================

    public void Share()
    {
        Application.OpenURL("https://google.com");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeLanguage(string s)
    {
        Debug.Log(PlayerPrefs.GetString("language"));
        PlayerPrefs.SetString("language", "ar");
    }

    // Call this function to notify the HTML page of resolution changes
    public void GetCurrentResolution()
    {
        // int width = 390;
        // int height = 844;
        // Screen.SetResolution(width, height, false);

        // // Send resolution information to the HTML page

        // if (Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     onUnityResolutionChange(width, height);
        // }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // GL.Clear(true, true, Color.black);
            // Camera.main.Render();
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
            onUnityResolutionChange(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }
    }

    //===============================================================================================
    //                                       Leaderboard Methods
    //===============================================================================================

    public void UpdateLeaderboard()
    {
        StartCoroutine(UpdateLeaderboardCoroutine());
    }

    // Method to fetch leaderboard data
    public IEnumerator UpdateLeaderboardCoroutine()
    {
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/leaderboard", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Add a custom certificate handler to bypass certificate validation
            request.certificateHandler = new BypassCertificateHandler();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Connection Error: " + request.error);
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("HTTP Error: " + request.error);
            }
            else
            {
                List<LeaderboardManager.LeaderboardEntry> NewLeaderboardEntries = new List<LeaderboardManager.LeaderboardEntry>();
                // Deserialize the response
                var response = JsonUtility.FromJson<LeaderboardManager.LeaderboardResponse>(request.downloadHandler.text);


                // Save the leaderboard data
                // LeaderboardEntries.Clear();
                int i = 1;
                foreach (var entry in response.leaderboard)
                {
                    entry.rank = i++;
                    NewLeaderboardEntries.Add(entry);
                }
                LeaderboardEntries = NewLeaderboardEntries;


                // Save the specific user's leaderboard entry
                // UserLeaderboardEntry = GetUserLeaderboardEntry(response);

                // Debug.Log("Leaderboard fetched successfully");
                // Debug.Log("Your rank: " + response.userRank);

                // Optional: log all leaderboard entries
                foreach (var entry in NewLeaderboardEntries)
                {
                    Debug.Log("Name: " + entry.first_name + ", Score: " + entry.score + ", Rank: " + entry.rank + ", User ID: " + entry.userId);
                }

            }
        }
    }

    // Custom Certificate Handler to bypass SSL certificate validation
    public class BypassCertificateHandler : CertificateHandler
    {
        // Always returns true, bypassing SSL validation
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    //===============================================================================================
    //                                        Public Methods
    //===============================================================================================

    public void PresentLeaderboard()
    {
        UpdateLeaderboard(); // Update the leaderboard data from the server
        StartCoroutine(PresentLeaderboardCoroutine());

    }

    IEnumerator PresentLeaderboardCoroutine()
    {
        Debug.Log("Presenting leaderboard");
        yield return new WaitForSeconds(1.5f);
        Screen.SetResolution(500, 1080, false);

        // Send resolution information to the HTML page

        // if (Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     onUnityResolutionChange(500, 1080);
        // }
        // Update data for leaderboard and player
        LeaderboardManager.LeaderboardEntry playerEntry = GetSelfRank(PlayerPrefs.GetString("userId"));
        DisableObjectsOnLeaderboard();
        // Set UI elements
        leaderboardInScene.SetActive(true);
        Debug.Log("Leaderboard in scene: " + leaderboardInScene.activeSelf);
        // finishPanelEnglish.SetActive(false);
        // finishPanelArabic.SetActive(false);

        // Clear any previous leaderboard entries in the UI
        foreach (Transform child in leaderboardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Check if leaderboard entries are available
        Debug.Log("Leaderboard entries count: " + LeaderboardEntries.Count);
        if (LeaderboardEntries.Count > 0)
        {
            // Loop through the leaderboard entries and instantiate UI elements
            foreach (var entry in LeaderboardEntries)
            {
                // Instantiate the leaderboard entry prefab
                GameObject entryObject = Instantiate(leaderboardEntryPrefab);

                // Set the parent to the leaderboard container
                entryObject.transform.SetParent(leaderboardContainer.transform, false); // false keeps local position

                // Get the TextMeshProUGUI components for Rank, Name, and Score
                TextMeshProUGUI rankText = entryObject.transform.Find("Rank").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI nameText = entryObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scoreText = entryObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();

                if (rankText != null && nameText != null && scoreText != null)
                {
                    // Set the Rank, Name, and Score text
                    rankText.text = "" + entry.rank.ToString(); // Assuming userRank is in the LeaderboardEntry
                    nameText.text = entry.first_name + " " + entry.last_name;
                    scoreText.text = "" + entry.score;


                    // Debug.Log("Set Rank: " + entry.userRank + ", Name: " + entry.name + ", Score: " + entry.score);
                }
                else
                {
                    Debug.LogError("Missing TextMeshProUGUI components in prefab (Rank, Name, or Score).");
                }
            }

            // Ensure the leaderboard container is updated properly after adding all entries
            LayoutRebuilder.ForceRebuildLayoutImmediate(leaderboardContainer.GetComponent<RectTransform>());

            // Set the player's rank and score
            if (playerEntry != null)
            {
                playerRankText.text = playerEntry.rank.ToString();
                playerScoreText.text = playerEntry.score.ToString();
                playerNameText.text = playerEntry.first_name + " " + playerEntry.last_name;
            }
            else if (playerEntry == null)
            {
                playerRankText.text = "N/A";
                playerScoreText.text = "N/A";
                playerNameText.text = "N/A";
            }
            Debug.Log("Leaderboard displayed successfully.");
        }
        else
        {
            Debug.LogWarning("No leaderboard entries to display.");
        }
    }


    public void RecieveLiveAppUrl(string url)
    {
        Debug.Log("Received live app URL: " + url);
        // Save the URL
        apiUrl = url;
    }

    public void RecieveUserId(string userId)
    {
        // Save the user ID
        PlayerPrefs.SetString("userId", userId);
        PlayerPrefs.Save();
    }

    // Get rank of player in leaderboard
    public LeaderboardManager.LeaderboardEntry GetSelfRank(string userId)
    {
        Debug.Log("Getting rank for user: " + userId);
        // Find LeaderboardEntry in leaderboard
        foreach (var entry in LeaderboardEntries)
        {
            if (entry.userId == userId)
            {
                Debug.Log("Found user: " + entry.first_name + " " + entry.last_name + ", Rank: " + entry.rank + ", Score: " + entry.score);
                return entry;
            }
        }
        Debug.Log("User not found in leaderboard");
        return null;
    }

    void DisableObjectsOnLeaderboard()
    {
        foreach (GameObject obj in toDisableOnLeaderboard)
        {
            obj.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main Menu");
    }




}