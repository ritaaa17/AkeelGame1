using UnityEngine;

public class SpawnClouds : MonoBehaviour
{
    public GameObject[] dayClouds;
    public GameObject[] middayClouds;
    public GameObject[] nightClouds;

    public float daySpawnRate = 2f;
    public float middaySpawnRate = 1.5f;
    public float nightSpawnRate = 1f;

    private float nextSpawnTime = 0f;
    private int currentStage = 0; // 0 = day, 1 = midday, 2 = night

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCloud();
            SetNextSpawnTime();
        }
    }

    void SpawnCloud()
{
    GameObject[] cloudsToSpawn = GetCloudsForCurrentStage();
    if (cloudsToSpawn.Length > 0)
    {
        int randomIndex = Random.Range(0, cloudsToSpawn.Length);
        GameObject cloud = Instantiate(cloudsToSpawn[randomIndex], transform.position, Quaternion.identity);
        
        // Set the sorting order based on the current stage or position.
        SpriteRenderer spriteRenderer = cloud.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.y); // You can adjust this based on your needs
        }
    }
}


    GameObject[] GetCloudsForCurrentStage()
    {
        switch (currentStage)
        {
            case 0:
                return dayClouds;
            case 1:
                return middayClouds;
            case 2:
                return nightClouds;
            default:
                return new GameObject[0];
        }
    }

    void SetNextSpawnTime()
    {
        float spawnRate = GetSpawnRateForCurrentStage();
        nextSpawnTime = Time.time + spawnRate;
    }

    float GetSpawnRateForCurrentStage()
    {
        switch (currentStage)
        {
            case 0:
                return daySpawnRate;
            case 1:
                return middaySpawnRate;
            case 2:
                return nightSpawnRate;
            default:
                return 5f;
        }
    }

    public void SetStage(int stage)
    {
        currentStage = stage;
    }
}