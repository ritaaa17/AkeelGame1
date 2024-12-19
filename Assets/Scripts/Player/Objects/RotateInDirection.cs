using UnityEngine;
using UnityEngine.UIElements;

public class RotateInDirection : MonoBehaviour
{
    //===============================================================
    //                          Properties
    //===============================================================

    // Components
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] bool canRotate;
    

    //===============================================================
    //                          Mono Methods
    //===============================================================

    void Awake()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canRotate)
        Rotate();
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    //===============================================================
    //                          Rotate Methods
    //===============================================================

    void Rotate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed);
    }
}
