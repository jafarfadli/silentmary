using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public Transform blockerTransform;
    public float speed = 5f;
    public Vector3 camPositionLeft;
    public Vector3 camPositionRight;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (transform.position.y == 0)
        {
            transform.position += (camPositionLeft - transform.position) * speed * Time.deltaTime;
            blockerTransform.position = new Vector3(transform.position.x-15, 0, 0);
        }
        else
        {
            transform.position += (camPositionRight - transform.position) * speed * Time.deltaTime;
            blockerTransform.position = new Vector3(transform.position.x+15, 0, 0);            
        }
    }

    public void MoveCamera(Vector3 newPositionLeft)
    {
        camPositionLeft = newPositionLeft;
        camPositionRight = new Vector3(-newPositionLeft.x, camPositionRight.y, newPositionLeft.z);
    }
}