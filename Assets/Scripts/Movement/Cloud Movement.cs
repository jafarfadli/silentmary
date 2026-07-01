using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public bool movingLeft;
    public Transform[] clouds;
    public float speed = .5f;
    int directionX;

    void Awake()
    {
        directionX = movingLeft? -1:1;
    }
    void Update()
    {
        foreach (Transform cloud in clouds)
        {
            cloud.position += new Vector3(Time.deltaTime * directionX * speed,0,0);
            if ((cloud.position.x < -60 && movingLeft) || (cloud.position.x > 60 && !movingLeft))
            {
                cloud.position += new Vector3(180 * -directionX,0,0);
            }
        }
    }
}