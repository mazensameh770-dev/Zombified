using UnityEngine;
using UnityEngine.UI;

public class LevelButtonStars : MonoBehaviour
{
    [SerializeField] private int levelIndex;

    [Header("Star Sprites")]
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar;

    private Image leftStar;
    private Image middleStar;
    private Image rightStar;

    private Button button;  

    private void Awake()
    {
        button = GetComponent<Button>();

        leftStar = FindStar("L_Star");
        middleStar = FindStar("M_Star");
        rightStar = FindStar("R_Star");
    }

    private void Start()
    {
        UpdateLevel();
    }

    public void UpdateLevel()
    {
        button.interactable =
            LevelProgress.IsLevelUnlocked(levelIndex);

        UpdateStars();
    }

    private void UpdateStars()
    {
        int savedStars = LevelProgress.GetStars(levelIndex);

        UpdateStar(leftStar, savedStars >= 1);
        UpdateStar(middleStar, savedStars >= 2);
        UpdateStar(rightStar, savedStars >= 3);
    }

    private void UpdateStar(Image star, bool filled)
    {
        if (star == null)
            return;

        star.sprite = filled ? filledStar : emptyStar;
    }

    private Image FindStar(string starName)
    {
        Transform star = FindChildRecursive(transform, starName);

        if (star == null)
        {
            Debug.LogWarning(
                $"Could not find {starName} under {gameObject.name}"
            );

            return null;
        }

        Image image = star.GetComponent<Image>();

        if (image == null)
        {
            Debug.LogWarning(
                $"{starName} was found, but it doesn't have an Image component."
            );
        }

        return image;
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform result = FindChildRecursive(child, childName);

            if (result != null)
                return result;
        }

        return null;
    }
}