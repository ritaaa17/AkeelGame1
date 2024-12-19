using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    //===============================================================
    //                          Properties
    //===============================================================

    // Stats
    public float speed = 1;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] Direction direction = Direction.Left;
    Vector2 directionVector;
    public float angle = 0;

    // Components
    Rigidbody2D rb;

    //================================================================
    //                          Mono Methods
    //================================================================

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Start()
    {
        DecideOnDirection();
    }

    void FixedUpdate()
    {
        MoveAction();
    }

    //================================================================
    //                          Methods
    //================================================================

    public void MoveAction()
    {
        rb.AddForce(directionVector * speed);
    }

    //================================================================
    //                          Helpers
    //================================================================

    void DecideOnDirection()
    {
        switch (direction)
        {
            case Direction.Left:
                directionVector = Vector2.left;
                break;
            case Direction.Right:
                directionVector = Vector2.right;
                break;
            case Direction.Up:
                directionVector = Vector2.up;
                break;
            case Direction.Down:
                directionVector = Vector2.down;
                break;
        }
    }
}