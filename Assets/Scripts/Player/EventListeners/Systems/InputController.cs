using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputController : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats playerStats;

        // Inputs
        [SerializeField] InputActionReference move;
        [SerializeField] InputActionReference goTo;
        [SerializeField] InputActionReference moveMobile;


        //===============================================================
        //                          Mono Methods
        //===============================================================

        void OnEnable()
        {
            move.action.Enable();
            goTo.action.Enable();
            moveMobile.action.Enable();
        }

        void OnDisable()
        {
            move.action.Disable();
            goTo.action.Disable();
            moveMobile.action.Disable();
        }

        void Awake()
        {
            eventController = GetComponent<EventController>();
            playerStats = GetComponent<Stats>();
        }

        void Start()
        {
            goTo.action.performed += ctx => GoToNearestCheckPoint();

            // moveMobile.action.performed += ctx =>
            //     {
            //         Debug.Log(ctx);
            //         Vector2 moveDirection = ctx.ReadValue<Vector2>();
            //         Debug.Log($"Movement direction: {moveDirection}");
            //     };
        }

        void FixedUpdate()
        {
            MovePressed(); // Optional if continuous movement is desired
            MoveTouchPressed(); // Optional if continuous movement is desired
        }


        //===============================================================
        //                              Methods
        //===============================================================

        void MovePressed()
        {
            if (move.action.ReadValue<float>() > 0)
            {
                eventController.MovePressed();
            }
            else if (move.action.ReadValue<float>() < 0)
            {
                eventController.BrakePressed();
            }
            else if (move.action.ReadValue<float>() == 0)
            {
                eventController.MoveNotPressed();
            }
        }

        void MoveTouchPressed()
        {
            UnityEngine.InputSystem.LowLevel.TouchState data = moveMobile.action.ReadValue<UnityEngine.InputSystem.LowLevel.TouchState>();
            Vector2 position = data.position;
            Vector2 touchPosition = position;
                if(touchPosition == Vector2.zero)
                {
                    eventController.MoveNotPressed();
                }
                else if (touchPosition.x < Screen.width / 2)
                {
                    // Touch is on the left side of the screen
                    eventController.BrakePressed();
                }
                else
                {
                    // Touch is on the right side of the screen
                    eventController.MovePressed();
                }
        }

        void GoToNearestCheckPoint()
        {
            playerStats.GameController.MovePlayerToLastCheckpoint();
        }

    }
}