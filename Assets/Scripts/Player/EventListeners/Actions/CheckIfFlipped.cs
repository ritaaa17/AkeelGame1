using System;
using UnityEngine;

namespace Player
{
    public class CheckIfFlipped : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats stats;

        // Components
        Rigidbody2D rb;

        //===============================================================
        //                          Mono Methods
        //===============================================================

        void Awake()
        {
            eventController = GetComponent<EventController>();
            stats = GetComponent<Stats>();

            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            // eventController.balance += CheckIfFlippedMethod;
        }

        void FixedUpdate()
        {
            CheckCarCollision();
        }

        //===============================================================
        //                          Checker Methods
        //===============================================================

        void CheckCarCollision()
        {
            if (rb.rotation >= stats.MaxRotationZPositive || rb.rotation <= stats.MaxRotationZNegative)
            {
                float rayLength = 0.1f;
                if (rb.rotation >= stats.MaxRotationZPositive)
                    rayLength = (stats.ModelSize * 2) + stats.GetWheelSize() + 0.1f;
                else if (rb.rotation <= stats.MaxRotationZNegative)
                    rayLength = stats.GetWheelSize() + 0.1f;

                bool isGrounded = eventController.CheckIfGrounded(rayLength);
                bool isOnRamp = eventController.CheckIfOnRamp(rayLength);
                bool isOnDune = eventController.CheckIfOnDune(rayLength);

                if (isGrounded || isOnRamp || isOnDune)
                {
                    eventController.Lose();
                }
                else
                {
                }
            }
        }


    }
}