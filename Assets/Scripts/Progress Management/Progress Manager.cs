using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;
    private const string progressKey = "progress";

    private void Awake()
    {
        instance = this;
    }

    public void SaveProgress(int currentLevel)
    {
        int currentProgress = GetProgress();
        if (currentLevel == currentProgress)
        {
            PlayerPrefs.SetInt(progressKey, currentProgress + 1);
            PlayerPrefs.Save();   
        }
    }

    public int GetProgress()
    {
        return PlayerPrefs.GetInt(progressKey, 0);
    }
}