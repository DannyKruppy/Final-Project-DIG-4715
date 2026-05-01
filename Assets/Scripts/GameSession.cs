using System.Collections.Generic;

public static class GameSession
{
    // Current playthrough level times
    public static Dictionary<string, float> levelTimes =
        new Dictionary<string, float>();

    // Save runtime level time
    public static void SaveLevelTime(string levelName, float time)
    {
        if (levelTimes.ContainsKey(levelName))
        {
            levelTimes[levelName] = time;
        }
        else
        {
            levelTimes.Add(levelName, time);
        }
    }

    // Check if level completed this run
    public static bool HasLevel(string levelName)
    {
        return levelTimes.ContainsKey(levelName);
    }

    // Get runtime level time
    public static float GetLevelTime(string levelName)
    {
        if (levelTimes.ContainsKey(levelName))
        {
            return levelTimes[levelName];
        }

        return -1f;
    }

    // Reset current run
    public static void ResetRun()
    {
        levelTimes.Clear();
    }
}