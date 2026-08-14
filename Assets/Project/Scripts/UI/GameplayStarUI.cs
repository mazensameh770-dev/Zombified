using UnityEngine;
using UnityEngine.UI;

public class GameplayStarUI : MonoBehaviour
{
    [SerializeField] private TimeUI timeUI;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Image[] starImages;

    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar;

    private void Start()
    {
        timeUI.OnTimeChanged += UpdateStars;

        UpdateStars(timeUI.GetTime());
    }

    private void OnDestroy()
    {
        if (timeUI != null)
            timeUI.OnTimeChanged -= UpdateStars;
    }

    private void UpdateStars(float time)
    {
        if (gameManager.CurrentLevel == null)
            return;

        int starCount =
            gameManager.CurrentLevel.CalculateStars(time);

        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite =
                i < starCount ? filledStar : emptyStar;
        }
    }
}