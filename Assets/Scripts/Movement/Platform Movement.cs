using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public bool isVertical = false;
    public Transform trackDownLeft;
    public Transform trackUpRight;
    public float speed = 2f;
    bool isMoving = false;
    LineRenderer lineRenderer;
    public bool crane;

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = 5;
        lineRenderer.positionCount = 6;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void Update()
    {
        UpdateRope(crane);
        if (gameObject.activeSelf == false || GameProgress.instance.viewMode || GameProgress.instance.activeSubject != gameObject)
        {
            return;
        }

        isMoving = false;
        if (isVertical)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackUpRight.position,Time.deltaTime * speed);
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackDownLeft.position,Time.deltaTime * speed);
                isMoving = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackDownLeft.position,Time.deltaTime * speed);
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = Vector3.MoveTowards(transform.position,trackUpRight.position,Time.deltaTime * speed);
                isMoving = true;
            }            
        }

        if (isMoving && !PlaySFX.instance.checkPlatform())
        {
            PlaySFX.instance.playPlatform();
        }
        if (!isMoving)
        {
            PlaySFX.instance.stopPlatform();
        }

    }

    void UpdateRope(bool crane)
    {
        if (crane){
            RaycastHit2D hit = Physics2D.Raycast(transform.position+2.5f*Vector3.up, Vector3.up, 20, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                lineRenderer.SetPosition(0,transform.position + new Vector3(3,.5f,0));
                lineRenderer.SetPosition(1,transform.position + new Vector3(0,2.5f,0));
                lineRenderer.SetPosition(2,transform.position + new Vector3(-3,.5f,0));
                lineRenderer.SetPosition(3,transform.position + new Vector3(0,2.5f,0));
                lineRenderer.SetPosition(4,hit.point);
                lineRenderer.SetPosition(5,hit.point);
                lineRenderer.enabled = true;
            }
        }
        else
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position+3*Vector3.right, Vector3.right, 20, LayerMask.GetMask("Ground"));
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position+3f*Vector3.left, Vector3.left, 20, LayerMask.GetMask("Ground"));

            if (hit1.collider != null)
            {
                lineRenderer.SetPosition(0,hit2.point + new Vector2(0,.5f));
                lineRenderer.SetPosition(1,hit1.point + new Vector2(0,.5f));
                lineRenderer.SetPosition(2,transform.position + new Vector3(0,.5f,0));
                lineRenderer.SetPosition(3,transform.position + new Vector3(0,-.5f,0));
                lineRenderer.SetPosition(4,hit2.point + new Vector2(0,-.5f));
                lineRenderer.SetPosition(5,hit1.point + new Vector2(0,-.5f));
                lineRenderer.enabled = true;
            }
        }
    }
}