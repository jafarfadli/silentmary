using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject[] allUI;

    void Awake()
    {
        instance = this;
    }
    public void clearUI()
    {
        foreach (GameObject ui in allUI)
        {
            ui.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void goToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void setUI(GameObject newUI)
    {
        clearUI();
        newUI.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}