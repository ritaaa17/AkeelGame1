using System;
using UnityEngine;

namespace Player
{
    public class Balance : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats playerStats;

        // Components
        Rigidbody2D rb;

        // Helpers
        bool isBalancing = false;

        //===============================================================
        //                          Mono Methods
        //===============================================================
        void Awake()
        {
            eventController = GetComponent<EventController>();
            playerStats = GetComponent<Stats>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            // eventController.balance += BalanceAction;
            rb.centerOfMass = new Vector2(playerStats.XCenterOfMass, playerStats.YCenterOfMass);
        }

        void FixedUpdate()
        {
        }

        //===============================================================
        //                              Methods
        //===============================================================

        void BalanceAction(bool value)
        {
            isBalancing = true;
            if (value)
            {
                // rb.MoveRotation(rb.rotation - playerStats.BalanceForce);
                rb.AddTorque(-playerStats.BalanceForce);

            }
            else
            {
                rb.AddTorque(playerStats.BalanceForce);
                // rb.MoveRotation(rb.rotation + playerStats.BalanceForce);

            }
            Invoke("BalancingFinished", 1f);
        }


        //===============================================================
        //                      Helper Methods
        //===============================================================

        void BalancingFinished()
        {
            isBalancing = false;
        }

    }
}