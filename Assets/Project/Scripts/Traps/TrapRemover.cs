using UnityEngine;
using UnityEngine.UI;

public class TrapRemover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera placementCamera;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private RectTransform removeButtonRect;
    [SerializeField] private Button removeButton;

    private GridTile hoveredTileWithTrap;

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
            GridTile tileState = hit.collider.GetComponent<GridTile>();

            if (tileState != null && tileState.GetCurrentObject() != null)
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
        SoundManager.Instance.PlayTrapRemove();
        HideButton();

        if (CardSelectionManager.Instance != null)
        {
            CardSelectionManager.Instance.UpdateAllCardsForCurrentLevel();
        }
    }
}