using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController gameController = FindFirstObjectByType<GameController>();
            // gameController.ShowTutorial2();
        }
    }
}
