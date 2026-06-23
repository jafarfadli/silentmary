using System.Threading;
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
    public bool switchingCarry = false;
    public GameObject carry;
    Transform currentParent;
    Vector3 originalScale;
    bool groundedBool = false;
    float ungroundedTimer = Mathf.Infinity;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentParent = currentFrame.transform;
        originalScale = transform.localScale;
    }

    void Update()
    {
        ungroundedTimer += Time.deltaTime;
        if ( gameObject.activeSelf == false || GameProgress.instance.viewMode)
        {
            return;
        }
        if (grounded())
        {
            groundedBool = true;
            ungroundedTimer = 0;
        }
        else if (ungroundedTimer > 0.15f)
        {
            groundedBool = false;   
        }

        if (!switchingOut && !switchingIn)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * Mathf.Sign(transform.parent.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(transform.parent.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            if (Input.GetKeyDown(KeyCode.W) && grounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded())
        {
            if (!otherFrame.checkObstructionIn(transform) 
            && !currentFrame.checkObstructionOut(transform) 
            && !currentFrame.checkObstructionCam())
            {   
                switchingOut = true;
                GameProgress.instance.canEnterViewMode = false;
                if (LayerMask.LayerToName(transform.parent.gameObject.layer) == "Free Object")
                {
                    switchingCarry = true;
                    carry = transform.parent.gameObject;
                }
            }
        }
        if (switchingIn)
        {
            if (switchingCarry){
                if (carry.GetComponent<FreeObject>().grounded())
                {
                    switchingIn = false;
                    switchingCarry = false;
                    carry = null;
                    GameProgress.instance.canEnterViewMode = true;
                }
            }
            else
            {
                if (grounded())
                {
                    switchingIn = false;
                    GameProgress.instance.canEnterViewMode = true;
                }
            }
        }
        if (switchingOut){
            if (switchingCarry){
                if (carry.GetComponent<FreeObject>().grounded())
                {
                    carry.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(body.linearVelocity.x, 50);
                }
            }
            else
            {
                if (grounded())
                {
                    body.linearVelocity = new Vector2(body.linearVelocity.x, 50);
                }
            }

            if (transform.position.y > Camera.main.transform.position.y + 10)
            {
                currentFrame.switchOutFrame();
                otherFrame.switchInFrame(transform, Camera.main.transform, carry);
                switchingOut = false;
                switchingCarry = false;
                carry = null;
            }
        }

        animator.SetBool("walk", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
        animator.SetBool("grounded", grounded());
    }

    bool grounded()
    {
        RaycastHit2D downHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Free Object"));
        return downHit.collider != null;
    }
}
