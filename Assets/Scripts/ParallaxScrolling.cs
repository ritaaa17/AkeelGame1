using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    float startPosX, startPosY;
    GameObject cam;
    [SerializeField] float parallaxEffectX = 0.5f; // Speed of the parallax effect on the X-axis
    [SerializeField] float initialPosY; // Initial Y position

    void Start()
    {
        startPosX = transform.localPosition.x;
        startPosY = initialPosY;
        cam = Camera.main.gameObject;
        transform.localPosition = new Vector3(startPosX, startPosY, transform.localPosition.z);
    }

    void FixedUpdate()
    {
        // Apply parallax effect only
        float distX = cam.transform.position.x * parallaxEffectX;
        transform.localPosition = new Vector3(startPosX + distX, startPosY, transform.localPosition.z);
    }
}
