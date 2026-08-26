using UnityEngine;

public class GridHoverHighlighter : MonoBehaviour
{
    [SerializeField] private Camera gridCamera;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private TrapPlacementController placementController; 

    [Header("Colors")]
    [SerializeField] private Color validColor = new Color(0.4f, 0.9f, 0.4f, 0.5f);
    [SerializeField] private Color invalidColor = new Color(0.9f, 0.25f, 0.25f, 0.5f);

    private Transform overlay;
    private Renderer overlayRenderer;
    private Material overlayMaterial;

    private void Awake()
    {
        gridCamera = FindAnyObjectByType<Camera>();
        placementController = FindAnyObjectByType<TrapPlacementController>();
        BuildOverlay();
    }

    private void BuildOverlay()
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "HoverHighlightOverlay (Auto-Generated)";
        quad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        Destroy(quad.GetComponent<Collider>());

        overlayMaterial = new Material(Shader.Find("Sprites/Default"));
        quad.GetComponent<Renderer>().sharedMaterial = overlayMaterial;

        overlay = quad.transform;
        overlayRenderer = quad.GetComponent<Renderer>();
        overlay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!CameraPhaseController.IsInSetupPhase)
        {
            overlay.gameObject.SetActive(false);
            return;
        }

        // On mobile, only highlight when the screen is being touched
        if (Application.isMobilePlatform && Input.touchCount == 0)
        {
            overlay.gameObject.SetActive(false);
            return;
        }

        // Hide overlay if pointer is over UI
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            if (Input.touchCount > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                overlay.gameObject.SetActive(false);
                return;
            }
            else if (Input.touchCount == 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                overlay.gameObject.SetActive(false);
                return;
            }
        }

        Vector3 pointerPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        Ray ray = gridCamera.ScreenPointToRay(pointerPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, gridLayerMask))
        {
            Transform tile = hit.collider.transform;
            Renderer tileRenderer = hit.collider.GetComponent<Renderer>();

            Vector3 position = tileRenderer != null
                ? new Vector3(tileRenderer.bounds.center.x, tileRenderer.bounds.max.y + 0.02f, tileRenderer.bounds.center.z)
                : hit.point + Vector3.up * 0.02f;

            bool isOccupied = placementController != null && placementController.IsTileOccupied(tile);

            overlay.gameObject.SetActive(true);
            overlay.position = position;
            overlayMaterial.color = isOccupied ? invalidColor : validColor;
        }
        else
        {
            overlay.gameObject.SetActive(false);
        }
    }
}