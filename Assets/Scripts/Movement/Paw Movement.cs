using UnityEngine;

public class PawMovement: MonoBehaviour
{
    public Transform[] pawHelpers;
    public Transform pawTransform;
    bool isClosing = false;
    bool isOpening = false;
    bool isTransitioning = false;
    Vector3 openPosition;
    Vector3 openScale;
    Vector3 closePosition;
    Vector3 closeScale;

    void Awake()
    {
        pawTransform = transform;
        openPosition = Camera.main.transform.position + new Vector3(0, 25, 0);
        openScale = new Vector3(10, 10, 1);
        closePosition = Camera.main.transform.position + new Vector3(0, -25, 0);
        closeScale = new Vector3(10, 10, 1);
    }

    void Update()
    {
        if (isOpening)
        {
            pawTransform.position = Vector3.Lerp(pawTransform.position, openPosition, 5 * Time.deltaTime);
            pawTransform.localScale = Vector3.Lerp(pawTransform.localScale, openScale, 5 * Time.deltaTime);  
            AdjustHelpers();  

            if (Vector3.Distance(pawTransform.position, openPosition) < 0.1f && Vector3.Distance(pawTransform.localScale, openScale) < 0.1f)
            {
                isOpening = false;
            }        
        }
        if (isClosing)
        {
            closePosition = PlayerMovement.instance.transform.position - 2.5f *pawTransform.localScale.y * Vector3.up;
            pawTransform.position = Vector3.Lerp(pawTransform.position, closePosition, 5 * Time.deltaTime);
            pawTransform.localScale = Vector3.Lerp(pawTransform.localScale, closeScale, 5 * Time.deltaTime);
            AdjustHelpers();
            
            if (Vector3.Distance(pawTransform.position, closePosition) < 0.1f && Vector3.Distance(pawTransform.localScale, closeScale) < 0.1f)
            {
                isClosing = false;
            }
        }
    }

    void AdjustHelpers()
    {
        float scaleHelperXr = (Camera.main.transform.position.x + 15)-(pawTransform.position.x + pawTransform.localScale.x*5);
        float scaleHelperXl = (pawTransform.position.x - pawTransform.localScale.x*5)-(Camera.main.transform.position.x - 15);
        float scaleHelperYt = (Camera.main.transform.position.y + 10)-(pawTransform.position.y + pawTransform.localScale.y*5);
        float scaleHelperYb = (pawTransform.position.y - pawTransform.localScale.y*5)-(Camera.main.transform.position.y - 10);

        pawHelpers[0].localScale = new Vector3(scaleHelperXr, 20, 1);
        pawHelpers[1].localScale = new Vector3(scaleHelperXl, 20, 1);
        pawHelpers[2].localScale = new Vector3(30, scaleHelperYt, 1);
        pawHelpers[3].localScale = new Vector3(30, scaleHelperYb, 1);

        pawHelpers[0].transform.position = Camera.main.transform.position + new Vector3(15 - scaleHelperXr/2, 0, 0);
        pawHelpers[1].transform.position = Camera.main.transform.position + new Vector3(-15 + scaleHelperXl/2, 0, 0);
        pawHelpers[2].transform.position = Camera.main.transform.position + new Vector3(0, 10 - scaleHelperYt/2, 0);
        pawHelpers[3].transform.position = Camera.main.transform.position + new Vector3(0, -10 + scaleHelperYb/2, 0);
    }

    void OpenPaw()
    {
        pawTransform.position = Camera.main.transform.position;
        pawTransform.localScale = new Vector3(0, 0, 1);
        openPosition = Camera.main.transform.position + new Vector3(0, 25, 0); 
        openScale = new Vector3(10, 10, 1);       
        isOpening = true;
    }

    void ClosePaw()
    {
        pawTransform.position = Camera.main.transform.position + new Vector3(0, -25, 0); 
        pawTransform.localScale = new Vector3(10, 10, 1);
        closePosition = PlayerMovement.instance.transform.position;
        closeScale = new Vector3(0, 0, 1);       
        isClosing = true;
    }
}