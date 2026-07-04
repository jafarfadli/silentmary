using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelAvailability: MonoBehaviour
{
    public Button[] levels;
    void Awake()
    {
        int currentProgress = ProgressManager.instance.GetProgress();

        for (int i = currentProgress+1; i < levels.Length; i++)
        {
            levels[i].enabled = false;
            levels[i].gameObject.GetComponent<Image>().color = new Color(.5f,.5f,.5f,1);
        }
    }
}