using System.Collections;
using UnityEngine;

namespace Player
{
    public class Shake : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats stats;

        // Components
        Rigidbody2D rb;

        // 
        bool isTorqueBeingAdded = false;
        // bool canShake = true;
        float timer = 0;

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
            eventController.shakeInAir += ShakeInAir;
            eventController.shakeOnRamp += ShakeOnRamp;
            eventController.shakeOnDune += ShakeOnDune;
            eventController.shakeOnWheel += ShakeOnWheel;
        }

        void FixedUpdate()
        {
            if (isTorqueBeingAdded)
            {
                timer += Time.deltaTime;
            }
        }

        //===============================================================
        //                          Shake Methods
        //===============================================================

        void ShakeInAir()
        {
            if (isTorqueBeingAdded)
            {
                return;
            }

            //
            if (rb.angularVelocity < 0)
            {
                isTorqueBeingAdded = true;
                StartCoroutine(ShakeForTime(false));
            }
            else if (rb.angularVelocity > 0)
            {
                isTorqueBeingAdded = true;
                StartCoroutine(ShakeForTime(true));


            }

        }

        void ShakeOnRamp()
        {
            // Slow down the car
            rb.AddForce(-rb.linearVelocity * stats.SlowDownForceOnRamp);
            ShakeInAir();
        }

        void ShakeOnDune()
        {
            // Slow down the car
            rb.AddForce(rb.linearVelocity * stats.SlowDownForceOnDune);
            ShakeInAir();
        }

        void ShakeOnWheel(string wheel)
        {
            // if (wheel == "front")
            // {
            //     Debug.Log("Shake on Front");
            //     rb.AddTorque(stats.UncontrolledBalanceForce);
            // }
            // else
            // {
            //     Debug.Log("Shake on Back");
            //     rb.AddTorque(-stats.UncontrolledBalanceForce);
            // }

            // rb.AddForce(-rb.velocity * stats.SlowDownForceOnRamp/2);
        }

        //===============================================================
        //                          Helper Methods
        //===============================================================

        IEnumerator ShakeForTime(bool isPositive)
        {
            timer = 0;
            while (timer < stats.TimeToStopShaking)
            {
                if (isPositive)
                {
                    rb.AddTorque(stats.UncontrolledBalanceForce);
                }
                else
                {
                    rb.AddTorque(-stats.UncontrolledBalanceForce);
                }
                yield return null;
            }
            isTorqueBeingAdded = false;
            // ChangeCenterOfMass(isPositive);

        }

        void ChangeCenterOfMass(bool isPositive)
        {
            if (isPositive)
            {
                rb.centerOfMass = new Vector2(stats.XCenterOfMass, stats.YCenterOfMass);
            }
            else
            {
                rb.centerOfMass = new Vector2(-stats.XCenterOfMass * 2, stats.YCenterOfMass);
            }
            Invoke("ResetCenterOfMass", stats.TimeToResetCenterOfMass);
        }

        void ResetCenterOfMass()
        {
            rb.centerOfMass = new Vector2(0, 0);
        }

        bool CheckIfGroundedMethod()
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(stats.FrontWheel.transform.position, Vector2.down, stats.ModelSize / 3 + 0.1f, stats.GroundLayer);

            if (hit.collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}