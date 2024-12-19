using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab; // The prefab of the ground section
    float groundLength = 20f; // Length of each ground section
    public Transform carTransform; // Reference to the car's transform

    private float spawnPosition = 0f; // The position to spawn the next ground section

    void Start()
    {
        // Spawn initial ground sections
        for (int i = 0; i < 3; i++)
        {
            SpawnGround();
        }

        groundLength = groundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Check if we need to spawn a new ground section
        if (carTransform.position.x > spawnPosition - (groundLength * 2))
        {
            SpawnGround();
        }
    }

    void SpawnGround()
    {
        // Instantiate the ground prefab at the spawn position
        Instantiate(groundPrefab, new Vector3(spawnPosition, carTransform.position.y-5, 0), Quaternion.identity);
        spawnPosition += groundLength;
    }
}
