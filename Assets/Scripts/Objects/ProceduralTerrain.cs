using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{
    public float amplitude = 2f;
    public float frequency = 0.5f;
    public int points = 50;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Reference to the Edge Collider
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();

        // Reference to the Line Renderer
        lineRenderer = GetComponent<LineRenderer>();

        // Array of points for the terrain shape
        Vector2[] colliderPoints = new Vector2[points];
        lineRenderer.positionCount = points;

        for (int i = 0; i < points; i++)
        {
            float x = i * frequency;
            float y = Mathf.Sin(x) * amplitude;
            colliderPoints[i] = new Vector2(x, y);

            // Set point positions for Line Renderer (convert Vector2 to Vector3)
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        // Set the points to the Edge Collider
        edgeCollider.points = colliderPoints;
    }
}
