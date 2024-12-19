using UnityEngine;

namespace Player
{
    public class State : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats stats;

        // Components
        Rigidbody2D rb;

        // State
        public bool canMove = false;
        [SerializeField] bool isGrounded = false;
        [SerializeField] bool isOnRamp = false;
        [SerializeField] bool isOnDune = false;
        [SerializeField] bool isFrontWheelOnGround = false;
        [SerializeField] bool isBackWheelOnGround = false;
        [SerializeField] bool isSandDuneAhead = false;
        [SerializeField] bool canRotate = true;
        bool isFlipped = false;

        // Can
        [SerializeField] bool isPowerupActive = false;
        float powerUpTimer;

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
            // if(canMove)
            // eventController.movePressed += MovePressed;
            // eventController.brakePressed += BrakePressed;
            // eventController.moveNotPressed += MoveNotPressed;
            // eventController.lose += Lose;
            // eventController.finish += Finish;
            // eventController.collidedWithCan += CollidedWithCan;
            // eventController.rotationHelper += (bool value) => canRotate = value;

            GoToFirstCheckPoint();

        }

        void FixedUpdate()
        {
            CheckCarLocation();
            CheckCameraNeeded();
            // Clamp Y Velocity
            // rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -stats.MaxXVelocity, stats.MaxXVelocity);
            // rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -stats.MaxYVelocity, stats.MaxYVelocity);

            // Clamp Z Rotation
            rb.rotation = Mathf.Clamp(
                rb.rotation,
                stats.MaxRotationZNegative,
                stats.MaxRotationZPositive
            );

            // Powerup timer
            if (isPowerupActive)
            {
                powerUpTimer += Time.deltaTime;
                if (powerUpTimer >= stats.CanPowerupTime)
                {
                    powerUpTimer = 0;
                    isPowerupActive = false;
                    Debug.Log("Powerup Deactivated");

                }
            }

            //
            // if (canRotate)
            //     rb.centerOfMass = new Vector2(-0.5f, 0);
            // else rb.centerOfMass = new Vector2(0, 0);
            // Clamp rb angular velocity
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, stats.AngularVelocityNegative, stats.AngularVelocityPositive);

            if(!canMove)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0;
            }
        }

        //===============================================================
        //                          Move Methods
        //===============================================================

        void MovePressed()
        {
            eventController.Move(isGrounded, isOnRamp, isOnDune, isPowerupActive, canRotate);
        }

        void BrakePressed()
        {
            eventController.Brake(isGrounded, isOnRamp, isOnDune, isPowerupActive, canRotate);
        }

        void MoveNotPressed()
        {
            eventController.MovementNotOccuring();
        }

        //===============================================================
        //                        Checker Methods
        //===============================================================

        void CheckCarLocation()
        {
            float wheelSize = stats.GetWheelSize();
            isGrounded = eventController.CheckIfGrounded(wheelSize);
            isOnRamp = eventController.CheckIfOnRamp(wheelSize);
            isOnDune = eventController.CheckIfOnDune(wheelSize);
            isFrontWheelOnGround = eventController.CheckWheelOnGround(stats.FrontWheel);
            isBackWheelOnGround = eventController.CheckWheelOnGround(stats.BackWheel);
            isSandDuneAhead = eventController.CheckIfSandDuneAhead(stats.DistanceToZoomOutForDune);

            // isFrontWheelOnGround;
            if (!isGrounded && !isOnRamp && !isOnDune)
            {
                // Debug.Log("In Air");
                eventController.ShakeInAir();
                // eventController.Zoom("out");

            }
            // else if ((isFrontWheelOnGround && !isBackWheelOnGround) || (!isFrontWheelOnGround && isBackWheelOnGround))
            // {
            //     // if (isFrontWheelOnGround)
            //     //     eventController.ShakeOnOneWheel("front");
            //     // else
            //     //     eventController.ShakeOnOneWheel("back");
            //     // eventController.Zoom("in");

            // }
            else if ((!isGrounded && isOnRamp) || (isGrounded && isOnRamp))
            {
                // Debug.Log("On Ramp");
                // eventController.ShakeOnRamp();
                // eventController.Zoom("in");
            }
            else if ((!isGrounded && isOnDune) || (isGrounded && isOnDune))
            {
                // Debug.Log("On Dune");
                // eventController.ShakeOnDune();
                // eventController.Zoom("out");
            }
            else if (isGrounded)
            {
                // Debug.Log("Grounded");
            }
        }

        void CheckCameraNeeded()
        {

            if (isSandDuneAhead)
            {
                eventController.Zoom("out", false);
                stats.GameController.BiggifyParticles();
            }
            else if (isGrounded)
            {
                eventController.Zoom("in", false);
                stats.GameController.SmallifyParticles();
            }
            else
            {
                eventController.Zoom("out", true);
                stats.GameController.BiggifyParticles();
            }
        }

        //===============================================================
        //                        Coliision Methods
        //===============================================================

        void CollidedWithCan()
        {
            isPowerupActive = true;
            powerUpTimer = 0;

        }

        //===============================================================
        //                Game Controller Related Methods
        //===============================================================

        void Lose()
        {
            stats.GameController.Lose();

        }

        void Finish()
        {
            stats.GameController.Finish();
        }

        void GoToFirstCheckPoint()
        {
            stats.GameController.MoveToStartingCheckPoint();
        }

        public void Pause()
        {
            canMove = false;
            eventController.movePressed -= MovePressed;
            eventController.brakePressed -= BrakePressed;
            eventController.moveNotPressed -= MoveNotPressed;
            eventController.lose -= Lose;
            eventController.finish -= Finish;
            eventController.collidedWithCan -= CollidedWithCan;
            eventController.rotationHelper -= (bool value) => canRotate = value;
        }

        public void Play()
        {
            canMove = true;
            eventController.movePressed += MovePressed;
            eventController.brakePressed += BrakePressed;
            eventController.moveNotPressed += MoveNotPressed;
            eventController.lose += Lose;
            eventController.finish += Finish;
            eventController.collidedWithCan += CollidedWithCan;
            eventController.rotationHelper += (bool value) => canRotate = value;
        }
    }
}
