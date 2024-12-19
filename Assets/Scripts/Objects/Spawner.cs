using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] float timeToSpawn = 5;
    [SerializeField] float rampSpeed = 0;
    [SerializeField] Vector2 lengthRange = new Vector2(1, 5); // Min and max length

    void Start()
    {
        InvokeRepeating("SpawnObject", 0, timeToSpawn);
    }

    void SpawnObject()
    {
        // Randomly select an object from the array
        int index = Random.Range(0, objectsToSpawn.Length);
        GameObject selectedObject = objectsToSpawn[index];

        // Set a random length for the selected object
        float randomLength = Random.Range(lengthRange.x, lengthRange.y);
        selectedObject.transform.localScale = new Vector3(randomLength, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);

        // Adjust speed and angle
        selectedObject.GetComponent<MoveInDirection>().speed += rampSpeed;
        float angle = Random.Range(10, 50);
        selectedObject.GetComponent<MoveInDirection>().angle = angle;

        // Instantiate the selected object
        Instantiate(selectedObject, transform.position, selectedObject.transform.rotation);
    }
}