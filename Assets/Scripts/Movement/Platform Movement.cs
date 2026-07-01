using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public bool isVertical = false;
    public Transform trackDownLeft;
    public Transform trackUpRight;
    public float speed = 2f;

    void Update()
    {
        if (gameObject.activeSelf == false || GameProgress.instance.viewMode || GameProgress.instance.activeSubject != gameObject)
        {
            return;
        }

        if (isVertical)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackUpRight.position,Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackDownLeft.position,Time.deltaTime * speed);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackDownLeft.position,Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackUpRight.position,Time.deltaTime * speed);
            }            
        }
    }
}