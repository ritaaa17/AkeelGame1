using UnityEditor;
using UnityEngine;

namespace Player
{
    public class CheckIfGrounded : MonoBehaviour
    {
        //===============================================================
        //                          Properties
        //===============================================================

        // Script Components
        EventController eventController;
        Stats stats;

        //===============================================================
        //                          Mono Methods
        //===============================================================

        void Awake()
        {
            eventController = GetComponent<EventController>();
            stats = GetComponent<Stats>();
        }

        void Start()
        {
            eventController.checkIfGrounded += CheckIfGroundedMethod;
            eventController.checkIfOnRamp += CheckIsOnRamp;
            eventController.checkIfOnDune += CheckIsOnDune;
            eventController.checkWheelOnGround += CheckWheelOnGround;
            eventController.checkIsSandDuneAhead += CheckIfSandDuneAhead;
        }

        //===============================================================
        //                          Checker Methods
        //===============================================================

        bool CheckIfGroundedMethod(float rayLength)
        {
            RaycastHit2D hit;
            // Debug.DrawRay(stats.Model.transform.position, Vector2.down * rayLength, Color.red);

            hit = Physics2D.Raycast(
                stats.FrontWheel.transform.position,
                Vector2.down,
                rayLength,
                stats.GroundLayer
            );

            Debug.DrawRay(stats.FrontWheel.transform.position, Vector2.down * rayLength, Color.yellow);

            if (hit.collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool CheckIsOnRamp(float rayLength)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(
                stats.FrontWheel.transform.position,
                Vector2.down,
                rayLength,
                stats.RampLayer
            );

            if (hit.collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        bool CheckIsOnDune(float rayLength)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(
                stats.FrontWheel.transform.position,
                Vector2.down,
                rayLength,
                stats.DuneLayer
            );

            if (hit.collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        bool CheckWheelOnGround(GameObject wheel)
        {
            RaycastHit2D hitGround, hitRamp, hitDune;
            hitGround = Physics2D.Raycast(
                wheel.transform.position,
                Vector2.down,
                stats.GetWheelSize(),
                stats.GroundLayer
            );

            hitRamp = Physics2D.Raycast(
                wheel.transform.position,
                Vector2.down,
                stats.GetWheelSize(),
                stats.RampLayer
            );

            hitDune = Physics2D.Raycast(
                wheel.transform.position,
                Vector2.down,
                stats.GetWheelSize(),
                stats.DuneLayer
            );

            // Debug.DrawRay(wheel.transform.position, Vector2.down * stats.GetWheelSize(), Color.red);


            if (hitGround.collider != null || hitRamp.collider != null || hitDune.collider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool CheckIfSandDuneAhead(float value)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(
                stats.Model.transform.position,
                Vector2.right,
                value,
                stats.DuneLayer
            );

            // Debug.DrawRay(stats.Model.transform.position, Vector2.right * value, Color.red);

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
