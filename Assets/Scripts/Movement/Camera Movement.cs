using UnityEngine;
using TMPro;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public TextMeshPro contextText;
    public float speed = 3f;
    public Vector3 camPositionLeft;
    public Vector3 camPositionRight;
    public bool isMoving = false;
    public bool viewMode = false;
    void Awake()
    {
        instance = this;
        camPositionLeft = new Vector3(camPositionLeft.x, 0, transform.position.z);
        camPositionRight = new Vector3(camPositionRight.x, -60, transform.position.z);   
    }

    void Update()
    {
        if(transform.position.y == 0)
        {
            contextText.text = "LEFT";
        }
        else
        {
            contextText.text = "RIGHT";
        }

        if (isMoving){
            if (transform.position.y == 0)
            {
                transform.position += (camPositionLeft - transform.position) * speed * Time.deltaTime;
                if (Vector3.Distance(transform.position, camPositionLeft) < 0.1f)
                {
                    isMoving = false;
                }
            }
            else
            {
                transform.position += (camPositionRight - transform.position) * speed * Time.deltaTime;  
                if (Vector3.Distance(transform.position, camPositionRight) < 0.1f)
                {
                    isMoving = false;
                }          
            }
        }
    }

    public void MoveCamera(Vector3 newPosition, bool frameLeft)
    {
        if (frameLeft){
            camPositionLeft = new Vector3(newPosition.x, camPositionLeft.y, transform.position.z);
            camPositionRight = new Vector3(-newPosition.x, camPositionRight.y, transform.position.z);
            isMoving = true;
        }
        else
        {
            camPositionLeft = new Vector3(-newPosition.x, camPositionLeft.y, transform.position.z);
            camPositionRight = new Vector3(newPosition.x, camPositionRight.y, transform.position.z);
            isMoving = true;
        }
    }

    public void SwitchFrameView(bool frameLeft)
    {
        if (frameLeft){
            transform.position = camPositionRight;
        }
        else
        {
            transform.position = camPositionLeft;
        }        
    }

    public void SetCamPos(Vector3 newPosition, bool frameLeft)
    {
       if (frameLeft){
            camPositionLeft = new Vector3(newPosition.x, camPositionLeft.y, transform.position.z);
            camPositionRight = new Vector3(-newPosition.x, camPositionRight.y, transform.position.z);
        }
        else
        {
            camPositionLeft = new Vector3(-newPosition.x, camPositionLeft.y, transform.position.z);
            camPositionRight = new Vector3(newPosition.x, camPositionRight.y, transform.position.z);
        }
    }
}