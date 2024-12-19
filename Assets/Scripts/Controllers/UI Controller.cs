using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIController : MonoBehaviour
{
    //================================================================
    //                          Properties
    //================================================================
    public static UIController Instance { get; private set; }
    // Script Components
    LeaderboardManager leaderboardManager;
    GameController gameController;

    // UI
    [Header("Timer")]
    public TMP_Text timeDisplay; // Assuming you have a Text UI element to display the time

    [Header("Starting counter")]
    public GameObject startingCounterPanel;
    public TMP_Text startingCounterTimer;
    public AudioSource startingCounterAudio;

    [Header("Finishing")]
    public GameObject finishPanelArabic;
    public GameObject finishPanelEnglish;
    public GameObject finishWinTextImageEnglish;
    public GameObject finishWinTextImageArabic;
    public GameObject finishLoseTextImageEnglish;
    public GameObject finishLoseTextImageArabic;

    // public TMP_Text statusText;
    public TMP_Text finalScoreText;

    [Header("Leaderboard")]
    public GameObject leaderboardEntryPrefab;     // Prefab for each leaderboard entry (player's box)
    public Transform leaderboardContainer;        // Parent container for the leaderboard entries (background image)
    public Image backgroundImage;                 // Background image for the leaderboard container
    public GameObject leaderboardInScene;         // Leaderboard object in the scene
    public TMP_Text playerRankText;
    public TMP_Text playerScoreText;
    public TMP_Text playerNameText;

    //================================================================
    //                          Mono Methods
    //================================================================

    void Awake()
    {
        // Script Components
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        leaderboardManager =LeaderboardManager.Instance;
        gameController = GameController.Instance;
        SetupBeforeStart();
    }

    void Update()
    {
        UpdateTimer();
    }

    //================================================================
    //                          Start Methods
    //================================================================

    void SetupBeforeStart()
    {
        // Hide the leaderboard
        leaderboardInScene.SetActive(false);
        // Hide the finish panel
        finishPanelEnglish.SetActive(false);
        finishPanelArabic.SetActive(false);
        // Show the starting counter panel
        startingCounterPanel.SetActive(true);

        // Start the counter
        StartCoroutine(StartCounter());

    }

    IEnumerator StartCounter()
    {
        // Counter change
        startingCounterTimer.text = "3";
        startingCounterAudio.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        startingCounterTimer.text = "2";
        yield return new WaitForSeconds(1);
        startingCounterTimer.text = "1";
        yield return new WaitForSeconds(1.5f);
        startingCounterPanel.SetActive(false);

        // Inform game controller counter is done
        gameController.SetupGameAfterCount();
    }

    //================================================================
    //                      Update Methods
    //================================================================

    public void UpdateTimer()
    {
        float time = gameController.ElapsedTime;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000F);
        timeDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    //================================================================
    //                      Finish Methods
    //================================================================

    public void Finish(bool isWin)
    {
        // Check playerprefs for language
        if (PlayerPrefs.GetString("Language") == "ar")
        {
            finishPanelArabic.SetActive(true);
            finishPanelEnglish.SetActive(false);
            if (isWin)
            {
                finishWinTextImageArabic.SetActive(true);
                finishLoseTextImageArabic.SetActive(false);
            }
            else
            {
                finishWinTextImageArabic.SetActive(false);
                finishLoseTextImageArabic.SetActive(true);
            }

        }
        else
        {
            finishPanelArabic.SetActive(false);
            finishPanelEnglish.SetActive(true);
            if (isWin)
            {
                finishWinTextImageEnglish.SetActive(true);
                finishLoseTextImageEnglish.SetActive(false);
            }
            else
            {
                finishWinTextImageEnglish.SetActive(false);
                finishLoseTextImageEnglish.SetActive(true);
            }
        }

        // Set final status
        if (isWin)
        {
            // statusText.text = "Well Done!";
            finalScoreText.text = "Your score: " + timeDisplay.text;

        }
        else
        {
            // statusText.text = "So Close!";
            finalScoreText.text = "Stay Focused & Race Again";
        }
    }

    public void PresentLeaderboard()
    {
        leaderboardManager.UpdateLeaderboard(); // Update the leaderboard data from the server
        StartCoroutine(PresentLeaderboardCoroutine());

    }

    IEnumerator PresentLeaderboardCoroutine()
    {
        yield return new WaitForSeconds(1.5f);


        leaderboardInScene.SetActive(true);
        finishPanelEnglish.SetActive(false);
        finishPanelArabic.SetActive(false);


        foreach (Transform child in leaderboardContainer.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Destroyed old elements.");

        var leaderboardSnapshot = new List<LeaderboardManager.LeaderboardEntry>(leaderboardManager.LeaderboardEntries);
        LeaderboardManager.LeaderboardEntry playerEntry = leaderboardManager.GetSelfRank(PlayerPrefs.GetString("userId"), leaderboardSnapshot);


        Debug.Log("Leaderboard entries count: " + leaderboardSnapshot.Count);

        if (leaderboardSnapshot.Count > 0)
        {
            foreach (var entry in leaderboardSnapshot)
            {
                try
                {

                    GameObject entryObject = Instantiate(leaderboardEntryPrefab);
                    entryObject.transform.SetParent(leaderboardContainer.transform, false);

                    TextMeshProUGUI rankText = entryObject.transform.Find("Rank").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI nameText = entryObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI scoreText = entryObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();

                    if (rankText != null && nameText != null && scoreText != null)
                    {
                        // name variable
                        string fullName = $"{entry.first_name} {entry.last_name}";
                        // limit the name to 20 characters
                        if (fullName.Length > 20)
                        {
                            fullName = fullName.Substring(0, 17) + "...";
                        }
                        rankText.text = entry.rank.ToString();
                        nameText.text = fullName;
                        scoreText.text = entry.score.ToString();

                        if (playerEntry != null && entry.userId == playerEntry.userId)
                        {
                            entryObject.GetComponent<Image>().color = Color.red;
                            playerEntry = entry;
                        }
                    }
                    else
                    {
                        Debug.LogError("Missing TextMeshProUGUI components in prefab (Rank, Name, or Score).");
                        yield break;
                    }

                    // Debug.Log("Entry displayed successfully.");
                }
                catch (Exception e)
                {
                    Debug.LogError("Error: " + e.Message);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(leaderboardContainer.GetComponent<RectTransform>());

            Debug.Log("Putting in player entry in his list.");

            if (playerEntry != null)
            {
                playerRankText.text = playerEntry.rank.ToString();
                playerScoreText.text = playerEntry.score.ToString();
                playerNameText.text = $"{playerEntry.first_name} {playerEntry.last_name}";
            }

            // find playerentry in the list


            Debug.Log("Leaderboard displayed successfully.");
        }
        else
        {
            Debug.LogWarning("No leaderboard entries to display.");
        }
    }

    public void BackPressed()
    {
        GameController.Instance.BackToMenu();
    }
    
    public void PlayAgainPressed()
    {
        GameController.Instance.PlayAgain();
    }

}
