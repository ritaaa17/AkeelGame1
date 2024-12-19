using UnityEngine;

public class DestroyOnLeftOfCamera : MonoBehaviour
{
    private Camera mainCamera;
    private float objectWidth;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Calculate the width of the object for accurate positioning
        objectWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void Update()
    {
        // Get the left boundary of the camera's view in world coordinates
        float cameraLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        // Check if the object's right side is fully outside of the camera's left boundary
        if (transform.position.x + objectWidth < cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}
