using UnityEngine;
using UnityEngine.UI;

public class TrapRemover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera placementCamera;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private RectTransform removeButtonRect;
    [SerializeField] private Button removeButton;

    private GridTileState hoveredTileWithTrap;

    private void Awake()
    {
        removeButton.onClick.AddListener(HandleRemoveClicked);
        removeButtonRect.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool canRemoveAnything = CameraPhaseController.IsInSetupPhase && !CardSelectionManager.Instance.HasCardSelected;

        if (!canRemoveAnything)
        {
            HideButton();
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return; 

        Ray ray = placementCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, gridLayerMask))
        {
            GridTileState tileState = hit.collider.GetComponent<GridTileState>();

            if (tileState != null && tileState.currentObject != null)
            {
                hoveredTileWithTrap = tileState;
                removeButtonRect.gameObject.SetActive(true);
            }
        }
    }

    private void HideButton()
    {
        hoveredTileWithTrap = null;
        removeButtonRect.gameObject.SetActive(false);
    }

    private void HandleRemoveClicked()
    {
        if (hoveredTileWithTrap == null) return;
        hoveredTileWithTrap.RemoveObject();
        HideButton();
    }
}