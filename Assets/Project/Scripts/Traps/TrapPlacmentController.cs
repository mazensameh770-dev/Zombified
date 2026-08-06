using System.Collections.Generic;
using UnityEngine;

public class TrapPlacementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera placementCamera;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private float ghostScale = 0.6f; 

    private TrapCardData selectedTrapData;
    private GameObject ghostInstance;
    private Transform currentHoveredTile;

    private readonly List<IPlacementRule> placementRules = new List<IPlacementRule>();

    private void Awake()
    {
        placementRules.Add(new TileNotBlockedRule());
        placementRules.Add(new TileNotOccupiedRule());
    }

    private void Start()
    {
        CardSelectionManager.Instance.OnCardSelected += HandleCardSelected;
    }

    private void OnDisable()
    {
        if (CardSelectionManager.Instance != null)
            CardSelectionManager.Instance.OnCardSelected -= HandleCardSelected;
    }

    private void HandleCardSelected(TrapCardData trapData)
    {
        selectedTrapData = trapData;

        if (ghostInstance != null) Destroy(ghostInstance);
        currentHoveredTile = null;

        if (selectedTrapData != null)
        {
            ghostInstance = Instantiate(selectedTrapData.trapPrefab);
            ghostInstance.transform.localScale = selectedTrapData.trapPrefab.transform.localScale * ghostScale;
            ApplyGhostLook(ghostInstance);
        }
    }

    private void Update()
    {
        if (selectedTrapData == null || ghostInstance == null) return;

        Ray ray = placementCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, gridLayerMask))
        {
            currentHoveredTile = hit.collider.transform;
            ghostInstance.SetActive(true);
            ghostInstance.transform.position = GetTileCenter(currentHoveredTile);
            SnapToTileSurface(ghostInstance, GetTileCenter(currentHoveredTile));

            if (Input.GetMouseButtonDown(0))
            {
                GridTileState tileState = currentHoveredTile.GetComponent<GridTileState>();
                if (tileState == null || IsPlacementValid(tileState))
                {
                    PlaceTrapOnCurrentTile();
                }
            }
        }
        else
        {
            currentHoveredTile = null;
            ghostInstance.SetActive(false);
        }
    }

    private bool IsPlacementValid(GridTileState tile)
    {
        foreach (IPlacementRule rule in placementRules)
        {
            if (!rule.IsValid(tile)) return false;
        }
        return true;
    }

    public bool IsTileOccupied(Transform tile)
    {
        GridTileState tileState = tile.GetComponent<GridTileState>();
        return tileState != null && !IsPlacementValid(tileState);
    }

    private void PlaceTrapOnCurrentTile()
    {
        Vector3 spawnPosition = GetTileCenter(currentHoveredTile);
        GameObject placedTrap = Instantiate(selectedTrapData.trapPrefab, spawnPosition, Quaternion.identity);
        SnapToTileSurface(placedTrap, spawnPosition);

        GridTileState tileState = currentHoveredTile.GetComponent<GridTileState>();
        if (tileState != null) tileState.SetPlacedTrap(placedTrap, selectedTrapData);

        CardSelectionManager.Instance.NotifyTrapPlaced();
    }

    private Vector3 GetTileCenter(Transform tile)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        Vector3 center = tileRenderer != null ? tileRenderer.bounds.center : tile.position;
        float topY = tileRenderer != null ? tileRenderer.bounds.max.y : tile.position.y;
        return new Vector3(center.x, topY, center.z);
    }

    private void SnapToTileSurface(GameObject obj, Vector3 tileTopPosition)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            obj.transform.position = tileTopPosition;
            return;
        }

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        float verticalOffset = tileTopPosition.y - combinedBounds.min.y;
        obj.transform.position = new Vector3(tileTopPosition.x, obj.transform.position.y + verticalOffset, tileTopPosition.z);
    }

    private void ApplyGhostLook(GameObject ghost)
    {
        foreach (Collider col in ghost.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        if (ghostMaterial == null) return;

        foreach (Renderer renderer in ghost.GetComponentsInChildren<Renderer>())
        {
            Material[] ghostMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < ghostMaterials.Length; i++)
            {
                ghostMaterials[i] = ghostMaterial;
            }
            renderer.materials = ghostMaterials;
        }
    }
}