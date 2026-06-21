using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public Transform blockerTransform;
    public float speed = 5f;
    public Vector3 camPositionLeft;
    public Vector3 camPositionRight;
    public bool isMoving = false;

    void Awake()
    {
        instance = this;

        camPositionLeft = new Vector3(camPositionLeft.x, camPositionLeft.y, transform.position.z);
        camPositionRight = new Vector3(camPositionRight.x, -60, transform.position.z);
    }

    void Update()
    {
        if (isMoving){
            if (transform.position.y == 0)
            {
                transform.position += (camPositionLeft - transform.position) * speed * Time.deltaTime;
                blockerTransform.position = new Vector3(transform.position.x-15.5f, 0, 0);
                if (Vector3.Distance(transform.position, camPositionLeft) < 10e-3)
                {
                    isMoving = false;
                }
            }
            else
            {
                transform.position += (camPositionRight - transform.position) * speed * Time.deltaTime;
                blockerTransform.position = new Vector3(transform.position.x+15.5f, 0, 0);  
                if (Vector3.Distance(transform.position, camPositionRight) < 10e-3)
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
}