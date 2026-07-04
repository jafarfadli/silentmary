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
    public GameObject[] switchingUI;
    public RectTransform switchingBgUI;
    public RectTransform switchingCloudUI;
    public GameObject bgLeftSilhouetteUI;
    public GameObject bgRightSilhouetteUI;
    public GameObject groundSilhouetteUI;
    public RectTransform groundParent;
    public int currentLevel = 1;
    public bool isPaused = false;
    bool isSwitching = false;
    bool isSwitchingFromLeft = true;
    float switchingTimer = Mathf.Infinity;
    bool isSwitchingForViewMode = false;
    public bool viewMode = false;
    public bool canEnterViewMode = true;
    public bool isTransitioning = false;
    public bool isTransitioningOpen = false;
    bool isTransitioningClose = false;
    public GameObject activeSubject = null;
    GameObject player;
    float switchingDuration = 1.8f;
    RectTransform swUI0Transform;
    RectTransform swUI3Transform;
    bool groundSilhouetteSet = false;
    public bool win = false;
    float startingTimer = 0f;
    bool spawned = false;

    void Awake()
    {
        isTransitioning = true;
        isTransitioningOpen = true;

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
        player.SetActive(false);
        bool isCheckpoint = PlayerPrefs.GetInt("CheckpointIsCheckpoint", 0) == 1;
        if (isCheckpoint)
        {
            GoToCheckpoint();
        }
        else
        {
            PlayerPrefs.SetFloat("CheckpointPlayerX", player.transform.position.x);
            PlayerPrefs.SetFloat("CheckpointPlayerY", player.transform.position.y);
            PlayerPrefs.SetFloat("CheckpointCameraX", Camera.main.transform.position.x);
            PlayerPrefs.SetFloat("CheckpointCameraY", Camera.main.transform.position.y);
            PlayerPrefs.SetInt("CheckpointFrameLeft", PlayerMovement.instance.currentFrame == leftFrame ? 1 : 0);            
        }   

        PawMovement.instance.OpenPaw();  
    }

    void Update()
    {
        startingTimer += Time.deltaTime;
        if (startingTimer > 1f && !spawned)
        {
            player.SetActive(true);
            activeSubject = player;
            PlaySFX.instance.playRespawn();
            startingTimer = Mathf.Infinity;
            spawned = true;
        }
        else if (!spawned)
        {
            return;
        }
        if (win)
        {
            return;
        }
        if (isTransitioning)
        {
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
                viewMode = false;
                CameraMovement.instance.SwitchFrameView(PlayerMovement.instance.currentFrame == rightFrame); 
                ViewProjection.instance.RemoveIndicator();
            }
            else
            {
                viewMode = true;
                CameraMovement.instance.SwitchFrameView(PlayerMovement.instance.currentFrame == leftFrame);
                bool isNotObstructed = PlayerMovement.instance.checkObstructionForSwitching();

                PlayerMovement.instance.otherFrame.setIndicator(PlayerMovement.instance.transform, isNotObstructed);
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
                if (!groundSilhouetteSet)
                {
                    groundParent.localScale = new Vector3(1,1,1);
                    SetGroundSilhouetteUI(true);
                    groundSilhouetteSet = true;
                }

                switchingBgUI.gameObject.SetActive(true);
                switchingCloudUI.gameObject.SetActive(true);
                groundParent.gameObject.SetActive(true);
                switchingUI[0].SetActive(true);
                
                swUI0Transform.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                switchingBgUI.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                switchingCloudUI.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                groundParent.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));

                switchingCloudUI.position += new Vector3(Time.deltaTime * -32,0,0);
            } else if (switchingTimer < dur1 + dur2)
            {
                if (groundSilhouetteSet)
                {
                    RemoveGroundSilhouetteUI();
                    groundSilhouetteSet = false;
                }
                
                bgLeftSilhouetteUI.SetActive(true);
                switchingUI[1].SetActive(true);
                switchingUI[0].SetActive(false);
            } else if (switchingTimer < dur1 + dur2 + dur3)
            {
                bgLeftSilhouetteUI.SetActive(false);
                bgRightSilhouetteUI.SetActive(true);
                switchingUI[2].SetActive(true);
                switchingUI[1].SetActive(false);
            } else if (switchingTimer < switchingDuration)
            {
                if (!groundSilhouetteSet)
                {
                    groundParent.localScale = new Vector3(1,1,1);
                    SetGroundSilhouetteUI(false);
                    groundSilhouetteSet = true;
                }

                bgRightSilhouetteUI.SetActive(false);
                switchingUI[3].SetActive(true);
                switchingUI[2].SetActive(false);

                swUI3Transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                switchingBgUI.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                switchingCloudUI.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                groundParent.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));

                switchingCloudUI.position -= new Vector3(Time.deltaTime * -32,0,0);
            } else
            {
                if (groundSilhouetteSet)
                {
                    RemoveGroundSilhouetteUI();
                    groundSilhouetteSet = false;
                }
                switchingUI[3].SetActive(false);
                switchingBgUI.gameObject.SetActive(false);
                switchingCloudUI.gameObject.SetActive(false);
                groundParent.gameObject.SetActive(false);
            }
        }
        else
        {
            if (switchingTimer < dur1)
            {
                if (!groundSilhouetteSet)
                {
                   groundParent.localScale = new Vector3(1,1,1);
                    SetGroundSilhouetteUI(false);
                    groundSilhouetteSet = true;
                }

                switchingBgUI.gameObject.SetActive(true);
                switchingCloudUI.gameObject.SetActive(true);
                groundParent.gameObject.SetActive(true);
                switchingUI[3].SetActive(true);

                swUI3Transform.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1),1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                switchingBgUI.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                switchingCloudUI.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));
                groundParent.localScale = Vector3.Lerp(new Vector3(2f,2f,1), new Vector3(1,1,1), 1 - Mathf.Pow(1- (switchingTimer/dur1),4));

                switchingCloudUI.position -= new Vector3(Time.deltaTime * -32,0,0);
            } else if (switchingTimer < dur1 + dur2)
            {
                if (groundSilhouetteSet)
                {
                    RemoveGroundSilhouetteUI();
                    groundSilhouetteSet = false;
                }
                
                bgRightSilhouetteUI.SetActive(true);
                switchingUI[2].SetActive(true);
                switchingUI[3].SetActive(false);
            } else if (switchingTimer < dur1 + dur2 + dur3)
            {
                bgRightSilhouetteUI.SetActive(false);
                bgLeftSilhouetteUI.SetActive(true);
                switchingUI[1].SetActive(true);
                switchingUI[2].SetActive(false);
            } else if (switchingTimer < switchingDuration)
            {
                if (!groundSilhouetteSet)
                {
                    groundParent.localScale = new Vector3(1,1,1);
                    SetGroundSilhouetteUI(true);
                    groundSilhouetteSet = true;
                }

                bgLeftSilhouetteUI.SetActive(false);
                switchingUI[0].SetActive(true);
                switchingUI[1].SetActive(false);

                swUI0Transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2,2,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                switchingBgUI.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                switchingCloudUI.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));
                groundParent.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(2f,2f,1), Mathf.Pow((switchingTimer-(dur1 + dur2 + dur3))/dur4,4));

                switchingCloudUI.position += new Vector3(Time.deltaTime * -32,0,0);
            } else
            {
                if (groundSilhouetteSet)
                {
                    RemoveGroundSilhouetteUI();
                    groundSilhouetteSet = false;
                }
                switchingUI[0].SetActive(false);
                switchingBgUI.gameObject.SetActive(false);
                switchingCloudUI.gameObject.SetActive(false);
                groundParent.gameObject.SetActive(false);
            }                
        }        
    }

    void SetGroundSilhouetteUI(bool isLeft)
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (leftFrame.obstructionCheck.gridCheck(i,j, isLeft))
                {
                    GameObject Silhouette = Instantiate(groundSilhouetteUI);
                    Silhouette.transform.SetParent(groundParent.transform, false);
                    RectTransform SilhouetteTransform = Silhouette.GetComponent<RectTransform>();
                    SilhouetteTransform.anchoredPosition = new Vector2(i- 14.5f,j-9.5f) *32;
                } 
            }
        }
    }
    void RemoveGroundSilhouetteUI()
    {
        foreach (Transform child in groundParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SwitchingFrame(bool switchingFromLeft)
    {
        switchingTimer = 0;
        isSwitchingFromLeft = switchingFromLeft;
        isSwitching = true;
        PlaySFX.instance.playSwitch();
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

    public void Dead()
    {
        isTransitioning = true;
        isTransitioningClose = true;
        PawMovement.instance.ClosePaw();
    }

    public void Win()
    {
        win = true;
        activeSubject = null;      
        isTransitioning = true;
        isTransitioningClose = true;
        PawMovement.instance.ClosePaw();  
        ProgressManager.instance.SaveProgress(currentLevel);
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("CheckpointIsCheckpoint", 0);
        int nextLevel = currentLevel + 1;
        string nextSceneName = "L" + nextLevel;
        uiManager.goToScene(nextSceneName);
    }
}