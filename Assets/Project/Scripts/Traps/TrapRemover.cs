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

        bool isInputTriggered = Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        if (!isInputTriggered) return;

        if (IsPointerOverUI()) return;

        Vector3 inputPos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        Ray ray = placementCamera.ScreenPointToRay(inputPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, gridLayerMask))
        {
            GridTile tileState = hit.collider.GetComponent<GridTile>();

            if (tileState != null && tileState.GetCurrentObject() != null && tileState.GetCurrentObject() is Trap)
            {
                hoveredTileWithTrap = tileState;
                removeButtonRect.gameObject.SetActive(true);
            }
        }
    }

    private bool IsPointerOverUI()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null) return false;

        if (Input.touchCount > 0)
        {
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
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