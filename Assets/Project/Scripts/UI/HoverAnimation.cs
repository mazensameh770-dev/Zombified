using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotateValue = 15f;
    [SerializeField] private bool scale = true;
    [SerializeField] private float scaleValue = 1.1f;
    [SerializeField] private float animationTime = 1f;
    private Vector3 rotationAmount;
    private Button button;
    private void Awake() {
        button = GetComponent<Button>();
        rotationAmount = new Vector3(0, 0, rotateValue);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (button != null && !button.interactable) return;
        if (rotate) {
            transform.DORotate(rotationAmount, animationTime).SetEase(Ease.OutBack);
        }
        if (scale) {
            transform.DOScale(transform.localScale * scaleValue, animationTime).SetEase(Ease.InOutQuad);
        }
    }
    private void OnDisable() {
        transform.DOKill();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (button != null && !button.interactable) return;
        if (rotate) {
            transform.DORotate(Vector3.zero, animationTime).SetEase(Ease.OutBack);
        }
        if (scale) {
            transform.DOScale(Vector3.one, animationTime).SetEase(Ease.InOutQuad);
        }
    }
}
