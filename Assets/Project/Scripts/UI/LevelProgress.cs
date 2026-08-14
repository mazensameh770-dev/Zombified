using UnityEngine;

public static class LevelProgress
{
    private const string StarsKeyPrefix = "LevelStars_";
    private const string UnlockedKeyPrefix = "LevelUnlocked_";

    public static void SaveStars(int levelIndex, int stars)
    {
        int currentStars = GetStars(levelIndex);

        if (stars > currentStars)
        {
            PlayerPrefs.SetInt(
                StarsKeyPrefix + levelIndex,
                stars
            );

            PlayerPrefs.Save();
        }
    }

    public static int GetStars(int levelIndex)
    {
        return PlayerPrefs.GetInt(
            StarsKeyPrefix + levelIndex,
            0
        );
    }

    public static bool IsCompleted(int levelIndex)
    {
        return GetStars(levelIndex) > 0;
    }

    public static void UnlockLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(
            UnlockedKeyPrefix + levelIndex,
            1
        );

        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        // Level 1 is always unlocked.
        if (levelIndex == 0)
            return true;

        return PlayerPrefs.GetInt(
            UnlockedKeyPrefix + levelIndex,
            0
        ) == 1;
    }
}