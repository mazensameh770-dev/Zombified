using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Zombie zombie;
    [SerializeField] private Image fillImage;

    [Header("Settings")]
    [SerializeField] private float lerpSpeed = 5f;

    private float targetFillAmount = 1f;

    private void Awake()
    {
        if (zombie == null)
        {
            zombie = GetComponentInParent<Zombie>();
        }

        if (fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = 1f;
        }

        if (zombie != null)
        {
            zombie.OnHealthChanged += HandleHealthChanged;

            if (zombie.MaxHealth > 0)
            {
                targetFillAmount = Mathf.Clamp01((float)zombie.CurrentHealth / zombie.MaxHealth);
            }
        }
    }

    private void OnDisable()
    {
        if (zombie != null)
        {
            zombie.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        if (maxHealth > 0)
        {
            targetFillAmount = Mathf.Clamp01((float)currentHealth / maxHealth);
        }
    }

    private void Update()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        }
    }
}
