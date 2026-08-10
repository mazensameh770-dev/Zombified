using System.Collections.Generic;
using UnityEngine;

public class GridNeighborsSetup : MonoBehaviour
{
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private LayerMask tileLayer;

    private void Start()
    {
        SetupAllNeighbors();
    }

    public void SetupAllNeighbors()
    {
        GridTileState[] allTiles = FindObjectsByType<GridTileState>(FindObjectsSortMode.None);

        foreach (GridTileState tile in allTiles)
        {
            SetupNeighborsForTile(tile);
        }
    }

    private void SetupNeighborsForTile(GridTileState tile)
    {
        List<GridTileState> neighborList = new List<GridTileState>();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(tile.transform.position, dir, out RaycastHit hit, tileSize, tileLayer))
            {
                GridTileState neighborTile = hit.collider.GetComponent<GridTileState>();
                if (neighborTile != null)
                {
                    neighborList.Add(neighborTile);
                }
            }
        }

        tile.neighbors = neighborList.ToArray();
    }
}