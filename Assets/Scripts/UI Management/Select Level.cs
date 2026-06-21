using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour {
    public void GoToLevel(int currentLevel)
    {
        PlayerPrefs.SetInt("CheckpointIsCheckpoint", 0);
        GetComponent<UIManager>().goToScene("L"+currentLevel.ToString());
    }
}