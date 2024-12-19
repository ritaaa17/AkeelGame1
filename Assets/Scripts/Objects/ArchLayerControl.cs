using UnityEngine;

public class ArchLayerControl : MonoBehaviour
{
    public GameObject player;  // Reference to the player

    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component

    void Start()
    {
        // Get the SpriteRenderer of the arch
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Calculate the position of the arch's center (this assumes the arch is centered at its pivot point)
        float archCenterX = transform.position.x;

        // Get the player's position
        float playerPositionX = player.transform.position.x;

        // Check if the player is to the left or right of the arch's center
        if (playerPositionX < archCenterX)
        {
            // Player is to the left of the arch, set the sorting order to maingroundOrder
            spriteRenderer.sortingOrder = 0;
        }
        else if (playerPositionX >= archCenterX)
        {
            // Player has crossed the middle of the arch, set the sorting order to foregroundOrder
            spriteRenderer.sortingOrder = 10;
        }

    }
}