using System.Collections.Generic;
using UnityEngine;

public class GridNeighborsSetup : MonoBehaviour
{
    [SerializeField] private float tileSize = 1.0f;

    private void Start()
    {
        SetupAllNeighbors();
    }

    public void SetupAllNeighbors()
    {
        GridTile[] allTiles = FindObjectsByType<GridTile>(FindObjectsSortMode.None);

        foreach (GridTile tile in allTiles)
        {
            SetupNeighborsForTile(tile, allTiles);
        }
    }

    private void SetupNeighborsForTile(GridTile tile, GridTile[] allTiles)
    {
        GridTile[] neighbors = new GridTile[4];

        foreach (GridTile other in allTiles)
        {
            if (other == tile) continue;

            Vector3 diff = other.transform.position - tile.transform.position;

            if (Mathf.Abs(diff.x) < 0.3f && Mathf.Abs(diff.z - tileSize) < 0.3f)
            {
                neighbors[0] = other;
            }
            else if (Mathf.Abs(diff.x) < 0.3f && Mathf.Abs(diff.z + tileSize) < 0.3f)
            {
                neighbors[1] = other;
            }
            else if (Mathf.Abs(diff.z) < 0.3f && Mathf.Abs(diff.x - tileSize) < 0.3f)
            {
                neighbors[2] = other;
            }
            else if (Mathf.Abs(diff.z) < 0.3f && Mathf.Abs(diff.x + tileSize) < 0.3f)
            {
                neighbors[3] = other;
            }
        }

        tile.neighbors = neighbors;
    }
}