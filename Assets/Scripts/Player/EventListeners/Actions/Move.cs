using UnityEngine;
using System.Collections;

namespace Player
{
    public class Move : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        private EventController eventController;
        private Stats playerStats;

        // Components
        private Rigidbody2D rb;

        // Wheel Joints for movement control
        WheelJoint2D backWheel;
        WheelJoint2D frontWheel;

        // Helpers
        [SerializeField] float currentSpeed = 0f;
        [SerializeField] bool canIncreaseAcceleration = true;
        [SerializeField] float timerForDeceleration = 0;
        bool isBraking = false;


        //===============================================================
        //                          Mono Methods
        //===============================================================
        void Awake()
        {
            eventController = GetComponent<EventController>();
            playerStats = GetComponent<Stats>();

            //
            rb = GetComponent<Rigidbody2D>();

            // Get Wheel Joints
            WheelJoint2D[] wheels = GetComponents<WheelJoint2D>();
            backWheel = wheels[0];
            frontWheel = wheels[1];
        }

        void Start()
        {
            eventController.move += MoveAction;
            eventController.brake += BrakeAction;
            eventController.movementNotOccuring += MovementNotOccuring;
        }

        void FixedUpdate()
        {
            // Debug.DrawRay(playerStats.FrontWheel.transform.position, Vector2.down * (playerStats.GetWheelSize() + 500f), Color.blue);

            // MoveAction(); // Optional if continuous movement is desired
        }

        //===============================================================
        //                              Move Methods
        //===============================================================

        void MoveAction(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            float powerUpModifier = isPowerUp ? playerStats.PowerUpModifier : 1;
            if (isOnDune)
            {
                MoveActionOnDune(isGrounded, isOnRamp, isOnDune, isPowerUp, canRotate);
                return;
            }
            // Reset the timer for deceleration
            timerForDeceleration = 0;
            // StopAllCoroutines();

            // Increase speed based on acceleration
            if (canIncreaseAcceleration)
            {
                currentSpeed += playerStats.Acceleration;
                canIncreaseAcceleration = false;
                Invoke("ResetAcceleration", playerStats.TimeToResetAcceleraction);
            }

            // Clamp the speed to the maximum speed
            currentSpeed = Mathf.Clamp(currentSpeed, 0, playerStats.MaxSpeed * powerUpModifier);

            // Configure motor for the front wheel
            if (rb.linearVelocity.x < 0)
            {
                Debug.Log("rb.linearVelocity.x < 0");

                // Gradually reduce the velocity for smoother transition
                rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y);

                JointMotor2D frontMotor = new JointMotor2D
                {
                    motorSpeed = -(currentSpeed / (playerStats.BrakeToMoveForceModifier * 0.5f)) * powerUpModifier, // Adjust speed for smoother transition
                    maxMotorTorque = (playerStats.MaxMotorTorque * powerUpModifier) * 1.5f // Adjust torque for smoother transition
                };

                // Configure motor for the back wheel
                JointMotor2D backMotor = new JointMotor2D
                {
                    motorSpeed = -(currentSpeed / (playerStats.BrakeToMoveForceModifier * 0.5f)) * powerUpModifier, // Keep consistent with the front
                    maxMotorTorque = playerStats.MaxMotorTorque * 1.5f // Adjust torque for smoother transition
                };

                backWheel.motor = backMotor;
                backWheel.useMotor = true;
                frontWheel.motor = frontMotor;
                frontWheel.useMotor = true;
            }
            else
            {

                JointMotor2D frontMotor = new JointMotor2D
                {
                    motorSpeed = -currentSpeed * powerUpModifier, // Negative for forward movement
                    maxMotorTorque = playerStats.MaxMotorTorque * powerUpModifier // Adjust based on desired wheel strength
                };

                // Configure motor for the back wheel
                JointMotor2D backMotor = new JointMotor2D
                {
                    motorSpeed = -currentSpeed * powerUpModifier, // Keep consistent with the front
                    maxMotorTorque = playerStats.MaxMotorTorque // Adjust as needed
                };
                backWheel.motor = backMotor;
                backWheel.useMotor = true;
                frontWheel.motor = frontMotor;
                frontWheel.useMotor = true;
            }




            if (!isOnDune && !isOnRamp && !isGrounded && canRotate) // in air
                Rotate(true, true);
            // else if (isOnDune) // on dune
            //     Rotate(false, false);
        }

        void MoveActionOnDune(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            float powerUpModifier = isPowerUp ? playerStats.PowerUpModifier : 1;
            // Reset the timer for deceleration
            timerForDeceleration = 0;
            // StopAllCoroutines();

            if (canIncreaseAcceleration)
            {
                currentSpeed += playerStats.DuneAcceleration * powerUpModifier;
                canIncreaseAcceleration = false;
                Invoke("ResetAcceleration", playerStats.DuneTimeToResetAcceleraction);
            }

            // Clamp the speed to the maximum speed
            currentSpeed = Mathf.Clamp(currentSpeed, 0, playerStats.DuneMaxSpeed * powerUpModifier);

            // Configure motor for the front wheel
            JointMotor2D frontMotor = new JointMotor2D
            {
                motorSpeed = -currentSpeed * powerUpModifier, // Negative for forward movement
                maxMotorTorque = playerStats.DuneMaxMotorTorque * powerUpModifier // Adjust based on desired wheel strength
            };

            // Configure motor for the back wheel
            JointMotor2D backMotor = new JointMotor2D
            {
                motorSpeed = -currentSpeed * powerUpModifier, // Keep consistent with the front
                maxMotorTorque = playerStats.DuneMaxMotorTorque * powerUpModifier // Adjust as needed
            };

            backWheel.motor = backMotor;
            backWheel.useMotor = true;
            frontWheel.motor = frontMotor;
            frontWheel.useMotor = true;

            // if (!isOnDune && !isOnRamp && isGrounded) // in air
            //     Rotate(true, true);
            // else if (isOnDune) // on dune
            //     Rotate(false, false);

        }

        //===============================================================
        //                              Brake Methods
        //===============================================================

        void BrakeAction(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            // Reset the timer for deceleration
            timerForDeceleration = 0;
            // StopAllCoroutines();

            if (isOnDune)
            {
                BrakeActionOnDune(isGrounded, isOnRamp, isOnDune, isPowerUp, canRotate);
                return;
            }

            if (canIncreaseAcceleration)
            {
                currentSpeed += playerStats.Acceleration;
                canIncreaseAcceleration = false;
                Invoke("ResetAcceleration", playerStats.TimeToResetAcceleraction);
            }
            // Configure motor for the front wheel
            JointMotor2D frontMotor = new JointMotor2D
            {
                motorSpeed = (currentSpeed / playerStats.BrakeModifier) * playerStats.PowerUpModifier, // Negative for forward movement
                maxMotorTorque = (playerStats.MaxMotorTorque / playerStats.BrakeModifier) * playerStats.PowerUpModifier // Adjust based on desired wheel strength
            };

            // Configure motor for the back wheel
            JointMotor2D backMotor = new JointMotor2D
            {
                motorSpeed = (currentSpeed / playerStats.BrakeModifier) * playerStats.PowerUpModifier, // Keep consistent with the front
                maxMotorTorque = (playerStats.MaxMotorTorque / playerStats.BrakeModifier) * playerStats.PowerUpModifier // Adjust as needed
            };

            backWheel.motor = backMotor;
            backWheel.useMotor = true;
            frontWheel.motor = frontMotor;
            frontWheel.useMotor = true;

            if (!isOnDune && !isOnRamp && !isGrounded && canRotate) // in air
                Rotate(false, true);
            // else if (isOnDune) // on dune
            //     Rotate(true, false);

        }

        void BrakeActionOnDune(bool isGrounded, bool isOnRamp, bool isOnDune, bool isPowerUp, bool canRotate)
        {
            // Reset the timer for deceleration
            timerForDeceleration = 0;
            // StopAllCoroutines();

            if (canIncreaseAcceleration)
            {
                currentSpeed += playerStats.DuneAcceleration;
                canIncreaseAcceleration = false;
                Invoke("ResetAcceleration", playerStats.DuneTimeToResetAcceleraction);
            }
            // Configure motor for the front wheel
            JointMotor2D frontMotor = new JointMotor2D
            {
                motorSpeed = (currentSpeed / playerStats.DuneBrakeModifier) * playerStats.PowerUpModifier, // Negative for forward movement
                maxMotorTorque = (playerStats.DuneMaxMotorTorque / playerStats.DuneBrakeModifier) * playerStats.PowerUpModifier // Adjust based on desired wheel strength
            };

            // Configure motor for the back wheel
            JointMotor2D backMotor = new JointMotor2D
            {
                motorSpeed = (currentSpeed / playerStats.DuneBrakeModifier) * playerStats.PowerUpModifier, // Keep consistent with the front
                maxMotorTorque = (playerStats.DuneMaxMotorTorque / playerStats.DuneBrakeModifier) * playerStats.PowerUpModifier // Adjust as needed
            };

            backWheel.motor = backMotor;
            backWheel.useMotor = true;
            frontWheel.motor = frontMotor;
            frontWheel.useMotor = true;

            // if (!isOnDune && !isOnRamp && isGrounded) // in air
            //     Rotate(false, true);
            // else if (isOnDune) // on dune
            //     Rotate(true, false);

        }

        //===============================================================
        //                              No Input Methods
        //===============================================================

        void MovementNotOccuring()
        {
            timerForDeceleration += Time.deltaTime;
            if (timerForDeceleration > playerStats.TimeToStartDeceleration)
                StartCoroutine(SlowDownMotors());
        }

        IEnumerator SlowDownMotors()
        {
            float initialSpeed = currentSpeed; // Capture the current speed to start slowing down
            float decelerationRate = playerStats.Deceleration; // Adjust this value to control how fast it decelerates

            while (currentSpeed > 0)
            {
                currentSpeed -= decelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0); // Ensure it doesn't go below 0

                JointMotor2D frontMotor = new JointMotor2D
                {
                    motorSpeed = -currentSpeed, // Negative for forward movement
                    maxMotorTorque = playerStats.MaxMotorTorque // Adjust based on desired wheel strength
                };

                JointMotor2D backMotor = new JointMotor2D
                {
                    motorSpeed = -currentSpeed, // Keep consistent with the front
                    maxMotorTorque = playerStats.MaxMotorTorque // Adjust as needed
                };

                backWheel.motor = backMotor;
                backWheel.useMotor = true;
                frontWheel.motor = frontMotor;
                frontWheel.useMotor = true;

                yield return null; // Wait for the next frame
            }

            // Once stopped, set final motor speeds to 0
            JointMotor2D finalMotor = new JointMotor2D
            {
                motorSpeed = 0,
                maxMotorTorque = playerStats.MaxMotorTorque
            };
            backWheel.motor = finalMotor;
            frontWheel.motor = finalMotor;
        }

        //===============================================================
        //                          Rotation
        //===============================================================

        void Rotate(bool isMove, bool isInAir)
        {


            float balanceForceModifier = CalculateBalanceForceModifier();

            if (isMove && isInAir)
                rb.AddTorque(-playerStats.BalanceForce);
            else if (!isMove && isInAir)
                rb.AddTorque(playerStats.BalanceForce);
        }

        float CalculateBalanceForceModifier()
        {
            // Perform the raycast to get the distance
            RaycastHit2D hit = Physics2D.Raycast(
                playerStats.FrontWheel.transform.position,
                Vector2.down,
                playerStats.GetWheelSize() + 500f,
                playerStats.DuneLayer
            );

            float distance = hit.distance;

            // Example calculation based on the car's current force
            float currentForce = rb.linearVelocity.magnitude; // or any other relevant property
            float forceModifier = Mathf.Clamp(currentForce / 10f, 1f, 3f); // Adjust the divisor and range as needed

            // Adjust the modifier based on the distance using a combination of linear and inverse functions
            // float distanceModifier = Mathf.Clamp01(1f / (distance + 1f) + distance / 20f); // Adjust the divisors as needed

            // Combine the force modifier and distance modifier
            // float modifier = forceModifier * distanceModifier;
            float modifier = forceModifier;

            return modifier;
        }

        void ResetAcceleration()
        {
            canIncreaseAcceleration = true;
        }

    }
}
