using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;
    public UIManager uiManager;
    public GameObject pauseUI;
    public GameObject winUI;
    public int currentLevel = 1;
    bool isPaused = false;
    public bool viewMode = false;
    public bool canEnterViewMode = true;
    public GameObject playerLeft;
    public GameObject playerRight;

    void Awake()
    {
        instance = this;
        uiManager.clearUI();
        Time.timeScale = 1;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Free Object"), LayerMask.NameToLayer("Blocker"), true);
    }

    void Start()
    {
        bool isCheckpoint = PlayerPrefs.GetInt("CheckpointIsCheckpoint", 0) == 1;
        if (isCheckpoint)
        {
            GoToCheckpoint();
        }        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartFromCheckpoint();
        }
        if (Input.GetKeyDown(KeyCode.V) && canEnterViewMode)
        {
            if (!viewMode)
            {
                EnterViewMode();
            }
            else
            {
                ExitViewMode();
            }
        }
    }

    void EnterViewMode()
    {
        CameraMovement.instance.SwitchFrameView(playerLeft.activeSelf);
        viewMode = true;
    }

    void ExitViewMode()
    {
        CameraMovement.instance.SwitchFrameView(!playerLeft.activeSelf);
        viewMode = false;
    }

    public void SetCheckpoint(Vector3 playerPosition, Vector3 cameraPosition, bool frameLeft)
    {
        PlayerPrefs.SetFloat("CheckpointPlayerX", playerPosition.x);
        PlayerPrefs.SetFloat("CheckpointPlayerY", playerPosition.y);
        PlayerPrefs.SetFloat("CheckpointCameraX", cameraPosition.x);
        PlayerPrefs.SetFloat("CheckpointCameraY", cameraPosition.y);
        PlayerPrefs.SetInt("CheckpointFrameLeft", frameLeft ? 1 : 0);
        PlayerPrefs.SetInt("CheckpointIsCheckpoint", 1);
    }

    public void GoToCheckpoint()
    {
        float checkpointPlayerX = PlayerPrefs.GetFloat("CheckpointPlayerX", 0);
        float checkpointPlayerY = PlayerPrefs.GetFloat("CheckpointPlayerY", 0);
        float checkpointCameraX = PlayerPrefs.GetFloat("CheckpointCameraX", 0);
        float checkpointCameraY = PlayerPrefs.GetFloat("CheckpointCameraY", 0);
        bool checkpointFrameLeft = PlayerPrefs.GetInt("CheckpointFrameLeft", 1) == 1;

        if (checkpointFrameLeft)
        {
            playerLeft.transform.position = new Vector3(checkpointPlayerX, checkpointPlayerY, 0);
            playerLeft.SetActive(true);
            playerRight.SetActive(false);
        }
        else
        {
            playerRight.transform.position = new Vector3(checkpointPlayerX, checkpointPlayerY, 0);
            playerRight.SetActive(true);
            playerLeft.SetActive(false);
        }

        Vector3 newCamPos = new Vector3(checkpointCameraX, checkpointCameraY, Camera.main.transform.position.z);

        if (checkpointFrameLeft){
            CameraMovement.instance.SetCamPos(newCamPos, true);
        }
        else
        {
            CameraMovement.instance.SetCamPos(newCamPos, false);
        }

        Camera.main.transform.position = newCamPos;
    }
    public void ResumeGame()
    {
        uiManager.clearUI();
        Time.timeScale = 1;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void RestartFromCheckpoint()
    {
        uiManager.goToScene(SceneManager.GetActiveScene().name);
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt("CheckpointIsCheckpoint", 0);

        uiManager.goToScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        winUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;        
    }
}