using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //===============================================================
    //                          Properties
    //===============================================================

    float length;
    Vector3 spawnPosition = Vector3.zero;

    [SerializeField] GameObject[] objectPrefabs;
    [SerializeField] float spawnTimeMin;
    [SerializeField] float spawnTimeMax;

    // State
    bool canSpawn = true;
    // [SerializeField] float[] spawnRates;
    // [SerializeField] float[] spawnTimeCheck;

    //===============================================================
    //                          Mono Methods
    //===============================================================

    void Start()
    {
        ShecduleNextSpawn();
    }

    void Update()
    {
        // spawnPosition = transform.position;

    }

    //===============================================================
    //                          Methods
    //===============================================================

    void ShecduleNextSpawn()
    {
        float spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
        Invoke("SpawnObject", spawnTime);
    }

    void SpawnObject()
    {

        spawnPosition = transform.position;

        // Instantiate the ground prefab at the spawn position
        int randomIndex = Random.Range(0, objectPrefabs.Length);

        Instantiate(objectPrefabs[randomIndex], spawnPosition, Quaternion.identity, transform);
        ShecduleNextSpawn();


    }
}
