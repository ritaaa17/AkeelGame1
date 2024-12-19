using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tilePrefab; // The tile asset you created
    public int tileOffset = 1; // How many tiles ahead of the player you generate
    private Transform player;

    void Start()
    {
        // Find the player GameObject tagged as "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Start generating tiles when the game starts
        GenerateInitialTiles();
    }

    void Update()
    {
        GenerateTilesUnderPlayer();
    }

    private void GenerateInitialTiles()
    {
        // Generate an initial flat platform under the player's starting position
        Vector3Int startPos = tilemap.WorldToCell(player.position);
        for (int x = startPos.x - 5; x <= startPos.x + 5; x++)
        {
            Vector3Int tilePos = new Vector3Int(x, startPos.y - 1, 0);
            tilemap.SetTile(tilePos, tilePrefab);
        }
    }

    private void GenerateTilesUnderPlayer()
    {
        // Calculate the player's current tile position
        Vector3Int playerTilePos = tilemap.WorldToCell(player.position);
        Vector3Int newTilePos = new Vector3Int(playerTilePos.x + tileOffset, playerTilePos.y - 1, 0);

        // Generate only if there's no tile already
        if (!tilemap.HasTile(newTilePos))
        {
            tilemap.SetTile(newTilePos, tilePrefab);
        }
    }
}
