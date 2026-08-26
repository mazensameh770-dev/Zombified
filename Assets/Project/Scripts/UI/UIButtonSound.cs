using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.isMobilePlatform) return;
        if (button != null && !button.interactable) return;
        SoundManager.Instance.PlayButtonHover();
    }

    private void PlayClick()
    {
        SoundManager.Instance.PlayButtonClick();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayClick);
    }
}