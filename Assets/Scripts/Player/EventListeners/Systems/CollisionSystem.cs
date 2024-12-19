using UnityEngine;

namespace Player
{
    public class CheckpointCollision : MonoBehaviour
    {
        //================================================================
        //                          Properties
        //================================================================
        private EventController eventController;
        private GameController gameController;

        //================================================================
        //                          Mono Methods
        //================================================================

        void Awake()
        {
            eventController = GetComponent<EventController>();
            if (eventController == null)
            {
                Debug.LogError("EventController component not found on this GameObject.");
            }

            gameController = FindAnyObjectByType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("GameControl object not found in the scene.");
            }
        }

        //================================================================
        //                         Methods
        //================================================================

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Checkpoint"))
            {
                // Handle checkpoint collision
                int currentCheckpointIndex = other.GetComponent<Checkpoint>().Index;
                gameController.SetCurrentCheckpointIndex(currentCheckpointIndex);
            }
            else if (other.CompareTag("Can"))
            {
                eventController.CollidedWithCan();
                // Destroy(other.gameObject);
            }
            else if(other.CompareTag("Finish"))
            {
                eventController.Finish();
            }
            
        }

        void OnTriggerStay2D(Collider2D other)
        {
            // if (other.CompareTag("Rotation Helper"))
            // {
            //     eventController.RotationHelper(false);
            // }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // if (other.CompareTag("Rotation Helper"))
            // {
            //     eventController.RotationHelper(true);
            // }
        }
    }
}