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
        GridTile[] allTiles = FindObjectsByType<GridTile>(FindObjectsSortMode.None);

        foreach (GridTile tile in allTiles)
        {
            SetupNeighborsForTile(tile);
        }
    }

    private void SetupNeighborsForTile(GridTile tile)
    {
        List<GridTile> neighborList = new List<GridTile>();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(tile.transform.position, dir, out RaycastHit hit, tileSize, tileLayer))
            {
                GridTile neighborTile = hit.collider.GetComponent<GridTile>();
                if (neighborTile != null)
                {
                    neighborList.Add(neighborTile);
                }
            }
        }

        tile.neighbors = neighborList.ToArray();
    }
}