using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Unity.Collections.LowLevel.Unsafe;

public class LeaderboardManager : MonoBehaviour
{
    //====================================================================================================
    //                                          Properties
    //====================================================================================================
    public static LeaderboardManager Instance { get; private set; }
    

    // Script Components
    GameController gameController;
    UIController uiController;

    //====================================================================================================
    //                                          Helper Classes
    //====================================================================================================
    [System.Serializable]
    public class LeaderboardData
    {
        public string first_name;
        public string last_name;
        public string email;
        public string score;
        public string phoneNumber;
        public string userId; // Ensure uniqueID is here
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string first_name;
        public string last_name;
        public string email;
        public string score;
        public string phoneNumber;
        public string userId;
        public string siloId;
        public int rank;
    }

    [System.Serializable]
    public class LeaderboardResponse
    {
        public string message;
        public string uniqueID; // Add uniqueID here to match response
        public LeaderboardEntry[] leaderboard;
        public int userRank;
    }

    [System.Serializable]
    public class PlayerData
    {
        public string userId;
        public string siloId;
        public string name;
        public string first_name;
        public string last_name;
        public string email;
        public string phoneNumber;
    }

    // Properties to hold the leaderboard data and user data
    public List<LeaderboardEntry> LeaderboardEntries { get; private set; } = new List<LeaderboardEntry>();

    //====================================================================================================
    //                                        Mono Methods
    //====================================================================================================

    private string apiUrl = "https://localhost:3000"; // Your API URL

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Script Components
        

        // You can test this method by uncommenting below
        // GetOrCreatePlayerID("Omar", "omar@hotmail.com", "9542321");
        // UpdateLeaderboard();
    }

    private void Start()
    {
        gameController =GameController.Instance;
        uiController = UIController.Instance;
    }
    //====================================================================================================
    //                                          Saving Methods
    //====================================================================================================

    // Called when player finishes the game
    public void SendLeaderboardData()
    {
        // 
        Debug.Log("Sending Leaderboard Data");
        // Get Score
        float time = gameController.ElapsedTime;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000F);
        String score = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        // Get the unique player ID from PlayerPrefs
        string userId = PlayerPrefs.GetString("userId", null);
        string first_name = PlayerPrefs.GetString("first_name", "");

        // Ensure the uniqueID is not null or empty
        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(first_name))
        {
            Debug.LogError("Player Unique ID not found, creating new one");

            // Create a new unique ID if not found (this should be a form)
            gameController.ForceJotFormOpen();
            return;
        }
        else
        {
            // Get the player's name, email, and phone number from PlayerPrefs
            string last_name = PlayerPrefs.GetString("last_name", "");
            string email = PlayerPrefs.GetString("email", "");
            string phoneNumber = PlayerPrefs.GetString("phoneNumber", "");

            // Get the player's score from PlayerPrefs


            LeaderboardData data = new LeaderboardData
            {
                first_name = first_name,
                last_name = last_name,
                email = email,
                score = score,
                phoneNumber = phoneNumber,
                userId = userId // Add the unique ID to the data
            };

            StartCoroutine(SendDataToServer(data));
        }


    }

    private IEnumerator SendDataToServer(LeaderboardData data)
    {
        string jsonData = JsonUtility.ToJson(data);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/save-leaderboard", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Add a custom certificate handler to bypass certificate validation
            request.certificateHandler = new BypassCertificateHandler();

            request.timeout = 10;

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
                Debug.Log("Success: " + request.downloadHandler.text);
            }

            Debug.Log("Response Code: " + request.responseCode);
        }
    }

    //====================================================================================================
    //                                          Leaderboard Methods
    //====================================================================================================

    // Method to update the leaderboard data called when opening leaderboard
    public void UpdateLeaderboard()
    {
        StartCoroutine(UpdateLeaderboardCoroutine());
    }

    // Method to fetch leaderboard data
    public IEnumerator UpdateLeaderboardCoroutine()
    {
        Debug.Log("Fetching Leaderboard Data in leaderboard manager");
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
                Debug.Log("Response recieved of latest leaderboard data: ");
                List<LeaderboardEntry> NewLeaderboardEntries = new List<LeaderboardEntry>();
                
                // Deserialize the response
                var response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
                
                Debug.Log("Response after fromJson to leaderboard response: " + response);


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
                // foreach (var entry in NewLeaderboardEntries)
                // {
                //     Debug.Log("Name: " + entry.first_name + ", Score: " + entry.score + ", Rank: " + entry.rank + ", User ID: " + entry.userId);
                // }

            }
        }
    }

    // Method to extract the specific user's leaderboard entry based on userRank
    private LeaderboardEntry GetUserLeaderboardEntry(LeaderboardResponse response)
    {
        // The userRank is used to find the specific user's leaderboard entry
        if (response.userRank > 0 && response.userRank <= response.leaderboard.Length)
        {
            return response.leaderboard[response.userRank - 1]; // Adjust for 0-based index
        }
        return null;
    }

    // Get rank of player in leaderboard
    public LeaderboardEntry GetSelfRank(string userId, List<LeaderboardEntry> LeaderboardEntriesSent)
    {
        Debug.Log("Getting rank for user: " + userId);
        // Find LeaderboardEntry in leaderboard
        foreach (var entry in LeaderboardEntriesSent)
        {
            if (entry.userId == userId)
            {
                // Debug.Log("Found user: " + entry.first_name + " " + entry.last_name + ", Rank: " + entry.rank + ", Score: " + entry.score);
                return entry;
            }
        }
        Debug.Log("User not found in leaderboard");
        return null;
    }

    //====================================================================================================
    //                                          Unique ID Methods
    //====================================================================================================

    // Should be called after data is brought from the webhook from the server
    public void GetOrCreatePlayerID(string name, string email, string phoneNumber)
    {
        StartCoroutine(GetOrCreatePlayerIDCoroutine(name, email, phoneNumber));
    }

    private IEnumerator GetOrCreatePlayerIDCoroutine(string name, string email, string phoneNumber)
    {
        // Prepare data to send
        PlayerData data = new PlayerData
        {
            name = name,
            email = email,
            phoneNumber = phoneNumber
        };

        // Convert the data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Log the JSON data to verify its content

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + "/generate-id", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Add a custom certificate handler to bypass certificate validation
            request.certificateHandler = new BypassCertificateHandler();

            request.timeout = 10;

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
                // Log the response to verify its content
                Debug.Log("Response: " + request.downloadHandler.text);

                // Parse the response to get the unique ID
                var response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);

                if (response != null && response.message == "Unique ID generated successfully")
                {
                    string uniqueID = response.uniqueID;

                    // Save or update the unique ID in PlayerPrefs
                    PlayerPrefs.SetString("PlayerUniqueID", uniqueID);
                    PlayerPrefs.SetString("PlayerName", name);
                    PlayerPrefs.SetString("PlayerEmail", email);
                    PlayerPrefs.SetString("PlayerPhoneNumber", phoneNumber);
                    PlayerPrefs.Save();
                    Debug.Log("Player ID saved/updated: " + uniqueID);
                }
                else
                {
                    Debug.LogError("Failed to generate or retrieve Unique ID.");
                }
            }
        }
    }

    //====================================================================================================
    //                                          Other Methods
    //====================================================================================================

    //
    public void RecieveNewUserData(string data)
    {
        // Parse the data
        var userData = JsonUtility.FromJson<PlayerData>(data);
        // Save the data
        PlayerPrefs.SetString("userId", userData.userId);
        PlayerPrefs.SetString("siloId", userData.siloId);
        PlayerPrefs.SetString("first_name", userData.first_name);
        PlayerPrefs.SetString("last_name", userData.last_name);
        PlayerPrefs.SetString("email", userData.email);
        PlayerPrefs.SetString("phoneNumber", userData.phoneNumber);
        PlayerPrefs.Save();
        // Game Controller data saved
        gameController.PlayerDataSaved();

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

    // Custom Certificate Handler to bypass SSL certificate validation
    public class BypassCertificateHandler : CertificateHandler
    {
        // Always returns true, bypassing SSL validation
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
