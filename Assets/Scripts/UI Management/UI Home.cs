using UnityEngine;

public class UIHome : MonoBehaviour
{
    public GameObject mainUI;
    void Awake()
    {
        GetComponent<UIManager>().setUI(mainUI);
        Time.timeScale = 1;
    }
}