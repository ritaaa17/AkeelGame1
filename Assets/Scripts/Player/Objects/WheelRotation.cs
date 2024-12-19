using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public GameObject wheel1;
    public GameObject wheel2;

    public float wheelSpeed = 20f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotateWheels();
    }

    void RotateWheels()
    {
        float rotationSpeed = -(rb2d.linearVelocity.x * wheelSpeed); // Adjust the multiplier as needed
        wheel1.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        wheel2.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}