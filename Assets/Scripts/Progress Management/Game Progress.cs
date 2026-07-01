using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;

    public FrameManager leftFrame;
    public FrameManager rightFrame;
    public UIManager uiManager;
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject[] switchingUI;
    public int currentLevel = 1;
    bool isPaused = false;
    bool isSwitching = false;
    bool isSwitchingFromLeft = true;
    float switchingTimer = Mathf.Infinity;
    bool isSwitchingForViewMode = false;
    public bool viewMode = false;
    public bool canEnterViewMode = true;
    bool isEnteringViewMode = false;
    public GameObject activeSubject;
    GameObject player;
    float switchingDuration = 1.8f;
    RectTransform swUI0Transform;
    RectTransform swUI3Transform;

    void Awake()
    {
        instance = this;
        uiManager.clearUI();
        Time.timeScale = 1;

        swUI0Transform = switchingUI[0].GetComponent<RectTransform>();
        swUI3Transform = switchingUI[3].GetComponent<RectTransform>();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Free Object"), LayerMask.NameToLayer("Blocker"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wild"), LayerMask.NameToLayer("Blocker"), true);
    }

    void Start()
    {
        player = PlayerMovement.instance.gameObject;
        activeSubject = player;
        bool isCheckpoint = PlayerPrefs.GetInt("CheckpointIsCheckpoint", 0) == 1;
        if (isCheckpoint)
        {
            GoToCheckpoint();
        }        
    }

    void Update()
    {
        Debug.Log(activeSubject.name);
        if (isSwitchingForViewMode)
        {
            switchingTimer += Time.deltaTime;
            EnterSwitchingAnimation();
            if (switchingTimer > switchingDuration)
            {
                if (isEnteringViewMode)
                {
                    CameraMovement.instance.SwitchFrameView(PlayerMovement.instance.currentFrame == leftFrame);
                    isEnteringViewMode = false;
                }
                else
                {
                    CameraMovement.instance.SwitchFrameView(PlayerMovement.instance.currentFrame == rightFrame); 
                    viewMode = false; 
                }
                switchingTimer = Mathf.Infinity;
                isSwitchingForViewMode = false;
            }
    
            return;
        }
        if (isSwitching)
        {
            switchingTimer += Time.deltaTime;
            EnterSwitchingAnimation();
            if (switchingTimer > switchingDuration)
            {
                PlayerMovement.instance.SwitchInFrame();
                switchingTimer = Mathf.Infinity;
                isSwitching = false;
            }
    
            return;
        }
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
        if (Input.GetKeyDown(KeyCode.V) && canEnterViewMode && !isSwitchingForViewMode)
        {
            if (viewMode)
            {
                SwitchingFrameForViewMode(PlayerMovement.instance.currentFrame == rightFrame);
            }
            else
            {
                isEnteringViewMode = true;
                viewMode = true;
                SwitchingFrameForViewMode(PlayerMovement.instance.currentFrame == leftFrame);
            }
        }

        if (Input.GetMouseButtonDown(0) && !PlayerMovement.instance.isSwitchingFrame)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 100f, LayerMask.GetMask("Wild","Platform","Player"));

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == activeSubject)
                {
                    activeSubject = player;
                }
                else
                {
                    activeSubject = hit.collider.gameObject;   
                }
            }
        }

        if (activeSubject != null && !activeSubject.activeSelf)
        {
            activeSubject = player;
        }
    }

    void EnterSwitchingAnimation()
    {
        float dur1 = switchingDuration*5/16;
        float dur2 = switchingDuration*3/16;
        float dur3 = switchingDuration*3/16;
        float dur4 = switchingDuration*5/16;
        if (isSwitchingFromLeft){
            if (switchingTimer < dur1)
            {
                switchingUI[0].SetActive(true);
                swUI0Transform.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
            } else if (switchingTimer < dur1 + dur2)
            {
                switchingUI[1].SetActive(true);
                switchingUI[0].SetActive(false);
            } else if (switchingTimer < dur1 + dur2 + dur3)
            {
                switchingUI[2].SetActive(true);
                switchingUI[1].SetActive(false);
            } else if (switchingTimer < switchingDuration)
            {
                switchingUI[3].SetActive(true);
                switchingUI[2].SetActive(false);
                swUI3Transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
            } else
            {
                switchingUI[3].SetActive(false);
            }
        }
        else
        {
            if (switchingTimer < dur1)
            {
                switchingUI[3].SetActive(true);
                swUI3Transform.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1),1 - Mathf.Pow(1- (switchingTimer/dur1),4));
            } else if (switchingTimer < dur1 + dur2)
            {
                switchingUI[2].SetActive(true);
                switchingUI[3].SetActive(false);
            } else if (switchingTimer < dur1 + dur2 + dur3)
            {
                switchingUI[1].SetActive(true);
                switchingUI[2].SetActive(false);
            } else if (switchingTimer < switchingDuration)
            {
                switchingUI[0].SetActive(true);
                switchingUI[1].SetActive(false);
                swUI0Transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2,2,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
            } else
            {
                switchingUI[0].SetActive(false);
            }                
        }        
    }

    public void SwitchingFrame(bool switchingFromLeft)
    {
        switchingTimer = 0;
        isSwitchingFromLeft = switchingFromLeft;
        isSwitching = true;
    }

    public void SwitchingFrameForViewMode(bool switchingFromLeft)
    {
        switchingTimer = 0;
        isSwitchingFromLeft = switchingFromLeft;
        isSwitchingForViewMode = true;
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

        player.transform.position = new Vector3(checkpointPlayerX, checkpointPlayerY, 0);

        Vector3 newCamPos = new Vector3(checkpointCameraX, checkpointCameraY, Camera.main.transform.position.z);

        if (checkpointFrameLeft){
            CameraMovement.instance.SetCamPos(newCamPos, true);
        }
        else
        {
            CameraMovement.instance.SetCamPos(newCamPos, false);
            PlayerMovement.instance.currentFrame = rightFrame;
            PlayerMovement.instance.otherFrame = leftFrame;
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
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