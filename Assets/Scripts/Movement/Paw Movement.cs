using UnityEngine;

public class PawMovement: MonoBehaviour
{
    public static PawMovement instance;
    public Transform[] pawHelpers;
    public Transform pawTransform;
    bool isClosing = false;
    bool isOpening = false;
    Vector3 openPosition;
    Vector3 openPositionStart;
    Vector3 openScale;
    Vector3 openScaleStart;
    Vector3 closePosition;
    Vector3 closePositionStart;
    Vector3 closeScale;
    Vector3 closeScaleStart;
    float transitioningDuration = 1f;
    float transitioningTimer = Mathf.Infinity;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isOpening)
        {
            transitioningTimer += Time.deltaTime;
            pawTransform.position = Vector3.Lerp(openPositionStart, openPosition, Mathf.Pow(transitioningTimer/transitioningDuration,10));
            pawTransform.localScale = Vector3.Lerp(openScaleStart, openScale, Mathf.Pow(transitioningTimer/transitioningDuration,4));  
            AdjustHelpers();  

            if (Vector3.Distance(pawTransform.position, openPosition) < 0.1f && Vector3.Distance(pawTransform.localScale, openScale) < 0.1f)
            {
                pawTransform.gameObject.SetActive(false);
                isOpening = false;
                GameProgress.instance.isTransitioning = false;
                GameProgress.instance.isTransitioningOpen = false;
            }        
        }
        if (isClosing)
        {
            transitioningTimer += Time.deltaTime;
            closePosition = PlayerMovement.instance.transform.position;
            pawTransform.position = Vector3.Lerp(closePositionStart, closePosition,1- Mathf.Pow(1-transitioningTimer/transitioningDuration,10));
            pawTransform.localScale = Vector3.Lerp(closeScaleStart, closeScale,1- Mathf.Pow(1-transitioningTimer/transitioningDuration,4));
            AdjustHelpers();
            
            if (Vector3.Distance(pawTransform.position, closePosition) < 0.1f && Vector3.Distance(pawTransform.localScale, closeScale) < 0.1f)
            {
                // pawTransform.gameObject.SetActive(false);
                // isClosing = false;
                if (GameProgress.instance.win)
                {
                    if (GameProgress.instance.currentLevel == 6)
                    {
                        UIManager.instance.goToScene("Cutscene Epilogue");
                    }
                    else
                    {
                        GameProgress.instance.NextLevel();   
                    }
                } else
                {
                    GameProgress.instance.RestartFromCheckpoint();
                }
            }
        }
    }

    void AdjustHelpers()
    {
        float scaleHelperXr = (Camera.main.transform.position.x + 15)-(pawTransform.position.x + pawTransform.localScale.x*5);
        float scaleHelperXl = (pawTransform.position.x - pawTransform.localScale.x*5)-(Camera.main.transform.position.x - 15);
        float scaleHelperYt = (Camera.main.transform.position.y + 10)-(pawTransform.position.y + pawTransform.localScale.y*5);
        float scaleHelperYb = (pawTransform.position.y - pawTransform.localScale.y*5)-(Camera.main.transform.position.y - 10);

        pawHelpers[0].localScale = new Vector3(scaleHelperXr, 20, 1); // kanan
        pawHelpers[1].localScale = new Vector3(scaleHelperXl, 20, 1); // kiri
        pawHelpers[2].localScale = new Vector3(30, scaleHelperYt, 1); // atas
        pawHelpers[3].localScale = new Vector3(30, scaleHelperYb, 1); // bawah

        pawHelpers[0].transform.position = Camera.main.transform.position + new Vector3(15 - scaleHelperXr/2, 0, 10);
        pawHelpers[1].transform.position = Camera.main.transform.position + new Vector3(-15 + scaleHelperXl/2, 0, 10);
        pawHelpers[2].transform.position = Camera.main.transform.position + new Vector3(0, 10 - scaleHelperYt/2, 10);
        pawHelpers[3].transform.position = Camera.main.transform.position + new Vector3(0, -10 + scaleHelperYb/2, 10);
    }

    public void OpenPaw()
    {
        openPositionStart = PlayerMovement.instance.transform.position;
        openScaleStart = new Vector3(0, 0, 1);
        openPosition = Camera.main.transform.position + new Vector3(0, 15, 10); 
        openScale = new Vector3(10, 10, 1);   
        transitioningTimer = 0;    
        isOpening = true;
        pawTransform.gameObject.SetActive(true);
    }

    public void ClosePaw()
    {
        closePositionStart = Camera.main.transform.position + new Vector3(0, 15, 10);
        closeScaleStart = new Vector3(10, 10, 1);
        closePosition = PlayerMovement.instance.transform.position;
        closeScale = new Vector3(0, 0, 1);
        transitioningTimer = 0;       
        isClosing = true;
        pawTransform.gameObject.SetActive(true);
    }
}