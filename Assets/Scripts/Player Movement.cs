using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public FrameManager currentFrame;
    public FrameManager otherFrame;
    LayerMask solidObjectLayer;
    Rigidbody2D body;
    BoxCollider2D boxCollider2D;
    Animator animator;
    public bool switchingOut = false;
    public bool switchingIn = false;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        if (!switchingOut && !switchingIn)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetKeyDown(KeyCode.W) && grounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded())
        {
            if (!otherFrame.checkObstructionIn(transform))
            {   
                switchingOut = true;
            }
        }
        if (switchingIn)
        {
            if (grounded())
            {
                switchingIn = false;
            }
        }
        if (switchingOut){
            if (grounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, 50);
            }
            if (transform.position.y >= Camera.main.transform.position.y + 10)
            {
                currentFrame.switchOutFrame();
                otherFrame.switchInFrame(transform, Camera.main.transform);
                switchingOut = false;
            }
        }

        Debug.Log(grounded());

        animator.SetBool("walk", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
        animator.SetBool("grounded", grounded());
    }

    bool grounded()
    {
        RaycastHit2D downHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        return downHit.collider != null;
    }
}
