using UnityEngine;

namespace Player
{
    public class ModelCollision : MonoBehaviour
    {
        //========================================================
        //                       Properties
        //========================================================

        // Components
        [SerializeField] EventController eventController;
        Rigidbody2D rb;

        //========================================================
        //                     Mono Methods
        //========================================================

        void Awake()
        {
            // Components
            rb = GetComponent<Rigidbody2D>();
        }

        //========================================================
        //                       Methods
        //========================================================

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Wheel"))
            {
                eventController.ModelCollided();
            }
        }

    }
}