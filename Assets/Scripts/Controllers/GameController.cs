using System.Collections;
using UnityEngine;
using Player;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameController : MonoBehaviour
{
    //================================================================
    //                          Properties
    //================================================================
    public static GameController Instance { get; private set; }
    // Script Components
    LeaderboardManager leaderboardManager;
    UIController uiController;

    // Timer
    bool canCount = false;
    float elapsedTime = 0f;
    public float ElapsedTime { get { return elapsedTime; } }

    // Checkpoints
    [Header("Checkpoints")]
    GameObject[] checkPoints;
    [SerializeField] int checkpointStartIndex = 0;

    // Player
    [Header("Player Related")]
    public GameObject player;
    private int currentCheckpointIndex;
    [SerializeField] int sky2ChecpointIndex = 3;
    [SerializeField] int sky3ChecpointIndex = 5;
    GameObject currentSky;
    GameObject skyToSpawn;

    // Sky
    [Header("Sky")]
    [SerializeField] GameObject morningSky;
    [SerializeField] GameObject daySky;
    [SerializeField] GameObject nightSky;
    [SerializeField] float skyToSpawnThreshold = 20;

    // Particles
    [Header("Particles")]
    [SerializeField] ParticleSystem dustParticles;
    [SerializeField] float zoomedOutStartSize = 0.5f;
    [SerializeField] float zoomedInStartSize = 0.2f;

    // Helper
    bool playerHasData = false;
    bool isWin = false;

    // Declare the external JavaScript function
    [DllImport("__Internal")]
    private static extern void onUnityResolutionChange(int width, int height, bool isFullScreen);

    [DllImport("__Internal")]
    private static extern void onOpenJotForm();

    //================================================================
    //                          Mono Methods
    //================================================================

    void Awake()
    {
        // Set Resolution
        // Screen.SetResolution(1920, 1080, true);

        // Script Components
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        

        GetCurrentResolution();
    }

    void Start()
    {
        leaderboardManager = LeaderboardManager.Instance;
        uiController = UIController.Instance;
        SetupGameBeforeCount();
        CheckPlayerHasData();
    }

    void Update()
    {
        GameTimer();
    }

    //================================================================
    //                       Setup Methods
    //================================================================



    public void GetCurrentResolution()
    {
        // int width = 1920;
        // int height = 1080;
        // Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        // Screen.currentResolution
        // go full screen with current resolution
        // Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        // Send resolution information to the HTML page

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // GL.Clear(true, true, Color.black);
            // Camera.main.Render();
            Screen.SetResolution(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                true
            );
            onUnityResolutionChange(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                true
            );
        }
    }

    void SetupGameBeforeCount()
    {
        canCount = false;
        player.GetComponent<State>().Pause();
    }

    public void SetupGameAfterCount()
    {
        canCount = true;
        player.GetComponent<State>().Play();
    }

    void CheckPlayerHasData()
    {
        if (PlayerPrefs.HasKey("userId"))
        {
            // Player has data
            Debug.Log("Player has data");
            playerHasData = true;

        }
        else
        {
            // Player has no data
            Debug.Log("Player has no data");
            playerHasData = false;
        }
    }

    //================================================================
    //                    Methods For Checkpoint
    //================================================================

    // Go to nearest checkpoint
    public void MovePlayerToLastCheckpoint()
    {
        // Search for checkpoint via currentCheckpointIndex
        foreach (GameObject checkpoint in checkPoints)
        {
            if (checkpoint.GetComponent<Checkpoint>().Index == currentCheckpointIndex)
            {
                player.transform.position = checkpoint.transform.position;
                break;
            }
        }
        player.transform.rotation = new Quaternion(0, 0, 0, 0);


    }

    // Go to  checkpoint specified
    public void MoveToStartingCheckPoint()
    {
        // Search for checkpoint via checkpointStartIndex
        foreach (GameObject checkpoint in checkPoints)
        {
            if (checkpoint.GetComponent<Checkpoint>().Index == checkpointStartIndex)
            {
                player.transform.position = checkpoint.transform.position;
                break;
            }
        }
    }

    //================================================================
    //                         Methods For Sky
    //================================================================

    public void SetCurrentCheckpointIndex(int index)
    {
        Debug.Log("Current checkpoint index: " + index);
        currentCheckpointIndex = index;
        if (currentCheckpointIndex >= sky2ChecpointIndex && currentCheckpointIndex < sky3ChecpointIndex)
        {
            SpawnClouds[] cloudSpawners = FindObjectsByType<SpawnClouds>(FindObjectsSortMode.None);
            foreach (SpawnClouds cloudSpawner in cloudSpawners)
            {
                cloudSpawner.SetStage(1);
            }

        }
        else if (currentCheckpointIndex >= sky3ChecpointIndex)
        {
            SpawnClouds[] cloudSpawners = FindObjectsByType<SpawnClouds>(FindObjectsSortMode.None);
            foreach (SpawnClouds cloudSpawner in cloudSpawners)
            {
                cloudSpawner.SetStage(2);
            }
        }

    }

    void CheckAndSpawnSky()
    {
        //
        if (currentCheckpointIndex == sky2ChecpointIndex)
        {
            skyToSpawn = daySky;
        }
        else if (currentCheckpointIndex == sky3ChecpointIndex)
        {
            skyToSpawn = nightSky;
        }

        // Check if the current sky is leaving the camera view
        Vector3 skyViewportPos = Camera.main.WorldToViewportPoint(currentSky.transform.position);
        if (skyViewportPos.x <= skyToSpawnThreshold) // Adjust the threshold as needed
        {
            // Spawn a new sky at the player's current position
            Vector3 newSkyPosition = currentSky.transform.position + new Vector3(currentSky.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
            currentSky = Instantiate(skyToSpawn, newSkyPosition, Quaternion.identity);
        }
    }

    //================================================================
    //                         Methods For Particles
    //================================================================

    public void BiggifyParticles()
    {
        var main = dustParticles.main;
        main.startSize = zoomedOutStartSize;
        // Place particle system object on the right side of the camera edge
        dustParticles.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
    }

    public void SmallifyParticles()
    {
        var main = dustParticles.main;
        main.startSize = zoomedInStartSize;
        // Place particle system object on the right side of the camera edge
        dustParticles.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
    }

    //================================================================
    //                         Button Methods
    //================================================================

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //================================================================
    //                         Update Methods
    //================================================================

    void GameTimer()
    {
        if (canCount)
            elapsedTime += Time.deltaTime;
    }

    //================================================================
    //                         Finish Methods
    //================================================================

    // Called by player state on loss
    public void Lose()
    {
        canCount = false;
        isWin = false;
        StartCoroutine(FinishGame());
    }

    // Called by player state on win
    public void Finish()
    {
        canCount = false;
        isWin = true;
        StartCoroutine(FinishGame());
    }

    // Finish the game
    IEnumerator FinishGame()
    {
        // Player and timer pause
        FindFirstObjectByType<State>().Pause();

        // Show finish panel
        uiController.Finish(isWin);

        leaderboardManager.UpdateLeaderboard();

        // Send data to leaderboard
        if (playerHasData && isWin)
        {
            leaderboardManager.SendLeaderboardData();

        }
        else if (!playerHasData && isWin)
        {
            yield return new WaitForSeconds(1.5f);
            // Player has no data
            Debug.Log("Player has no data");
            onOpenJotForm();

        }
        leaderboardManager.UpdateLeaderboard();


        yield return null;
    }

    // used by leaderboard manager
    public void ForceJotFormOpen()
    {
        onOpenJotForm();
    }

    public void PlayerDataSaved()
    {
        playerHasData = true;
        if (isWin)
            Finish();
        else if (!isWin)
            Lose();
    }

    public void BackToMenu()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



}
