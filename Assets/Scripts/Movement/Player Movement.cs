using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public float moveSpeed = 6f;
    public float jumpPower = 18f;
    public FrameManager currentFrame;
    public FrameManager otherFrame;
    Rigidbody2D body;
    BoxCollider2D boxCollider2D;
    Animator animator;
    public bool isSwitchingFrame = false;
    public bool switchingOutFrame = false;
    public bool switchingInFrame = false;
    public bool switchingFrameCarry = false;
    public GameObject carry;
    bool groundedBool = false;
    float ungroundedTimer = Mathf.Infinity;
    float childedTimer = Mathf.Infinity;
    bool leftWalk = false;
    bool rightWalk = true;
    void Awake()
    {
        instance = this;
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameProgress.instance.isPaused)
        {
            return;
        }

        childedTimer += Time.deltaTime;
        ungroundedTimer += Time.deltaTime;

        if (GameProgress.instance.isTransitioning || GameProgress.instance.viewMode || GameProgress.instance.win )
        {
            animator.SetBool("walk", false);
            animator.SetBool("grounded", true);
            animator.SetBool("telekinesis", false);
            return;
        }

        if (topped())
        {
            boxCollider2D.offset = new Vector2(0,-0.4f);
            boxCollider2D.size = new Vector2(1.8f,1f);
            childedTimer = 0;
        }
        else if (childedTimer > 0.5f)
        {
            boxCollider2D.offset = new Vector2(0,0f);
            boxCollider2D.size = new Vector2(1.8f,1.8f);
        }

        if ( gameObject.activeSelf == false || GameProgress.instance.viewMode || GameProgress.instance.activeSubject != gameObject)
        {
            animator.SetBool("walk", false);
            animator.SetBool("grounded", grounded());
            animator.SetBool("telekinesis", true);
            return;
        }
        if (!groundedBool && grounded() && body.linearVelocity.y > 5f)
        {
            PlaySFX.instance.playFall();
        }
        if (grounded())
        {
            groundedBool = true;
            ungroundedTimer = 0;
        }
        else if (ungroundedTimer > 0.1f)
        {
            groundedBool = false;   
        }

        if (!isSwitchingFrame)
        {
            float parentSignX = transform.parent == null? 1: transform.parent.GetComponent<Parenting>() == null? 1: transform.parent.GetComponent<Parenting>().getSignX();
            if (Input.GetKey(KeyCode.A))
            {
                RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, boxCollider2D.bounds.extents.x+0.1f, LayerMask.GetMask("Free Object", "Wild"));
                if (leftHit.collider != null)
                {
                    leftHit.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * parentSignX, transform.localScale.y, transform.localScale.z);
                leftWalk = true;
            } else
            {
                leftWalk = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, boxCollider2D.bounds.extents.x+0.1f, LayerMask.GetMask("Free Object", "Wild"));
                if (rightHit.collider != null)
                {
                    rightHit.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * parentSignX, transform.localScale.y, transform.localScale.z);
                rightWalk = true;
            }
            else
            {
                rightWalk = false;
            }
            if (Input.GetKeyDown(KeyCode.W) && groundedBool)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded() && !isSwitchingFrame)
        {
            if (transform.parent != null && (LayerMask.LayerToName(transform.parent.gameObject.layer) == "Free Object" || LayerMask.LayerToName(transform.parent.gameObject.layer) == "Wild"))
            {
                BoxCollider2D parentCollider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
                if (!otherFrame.checkObstructionIn(transform.parent, parentCollider.bounds.extents.x) 
                && !currentFrame.checkObstructionOut(transform.parent, parentCollider.bounds.extents.x)
                && !currentFrame.checkObstructionOut(transform, boxCollider2D.bounds.extents.x) 
                && !currentFrame.checkObstructionCam())
                {
                    isSwitchingFrame = true;
                    switchingOutFrame = true;
                    switchingFrameCarry = true;
                    carry = transform.parent.gameObject;                    
                }
            } else if (!otherFrame.checkObstructionIn(transform, boxCollider2D.bounds.extents.x) 
            && !currentFrame.checkObstructionOut(transform, boxCollider2D.bounds.extents.x) 
            && !currentFrame.checkObstructionCam())
            {   
                isSwitchingFrame = true;
                switchingOutFrame = true;
            }
        }
        if (switchingOutFrame)
        {
            if (switchingFrameCarry)
            {
                transform.parent = carry.transform;
                carry.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 50);
            } else{
                body.linearVelocity = new Vector2(0, 50);
            }

            if (transform.position.y > currentFrame.transform.position.y + 10)
            {
                switchingOutFrame = false;
                GameProgress.instance.SwitchingFrame(currentFrame == GameProgress.instance.leftFrame);
            }
        }
        if (switchingInFrame)
        {
            if (switchingFrameCarry){
                if ((carry.GetComponent<FreeObject>() != null && carry.GetComponent<FreeObject>().grounded()) ||
                (carry.GetComponent<WildMovement>() != null && carry.GetComponent<WildMovement>().grounded()))
                {
                    switchingInFrame = false;
                    switchingFrameCarry = false;
                    carry = null;
                    GameProgress.instance.canEnterViewMode = true;
                    isSwitchingFrame = false;
                }
            }
            else
            {
                if (grounded())
                {
                    switchingInFrame = false;
                    GameProgress.instance.canEnterViewMode = true;
                    isSwitchingFrame = false;
                }
            }
        }

        if ((leftWalk || rightWalk) && groundedBool && !PlaySFX.instance.checkWalk())
        {
            PlaySFX.instance.playWalk();
        }
        if ((!leftWalk && !rightWalk) || !groundedBool)
        {
            PlaySFX.instance.stopWalk();
        }

        animator.SetBool("walk", leftWalk || rightWalk);
        animator.SetBool("grounded", grounded());
        animator.SetBool("telekinesis", false);
    }

    bool grounded()
    {
        RaycastHit2D downHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size * 0.95f, 0, Vector2.down, 0.1f, LayerMask.GetMask("Ground", "Free Object", "Platform", "Wild"));
        return downHit.collider != null;
    }

    bool topped()
    {
        RaycastHit2D[] topHits = Physics2D.BoxCastAll(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.up, 0.5f, LayerMask.GetMask("Player","Free Object", "Wild"));
        return topHits.Length > 1;
    }

    public void SwitchInFrame()
    {
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.position = new Vector3(-cameraTransform.position.x, otherFrame.transform.position.y, cameraTransform.position.z);

        if (switchingFrameCarry)
        {
            carry.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            carry.transform.position = new Vector3(-carry.transform.position.x,otherFrame.transform.position.y + 10,0);
            body.linearVelocity = Vector2.zero;
            transform.position = carry.transform.position + new Vector3(0,2,0);
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
            transform.parent = carry.transform;
        }
        else
        {
            body.linearVelocity = Vector2.zero;
            transform.position = new Vector3(-transform.position.x,otherFrame.transform.position.y + 10,0);
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
        switchingInFrame = true;

        FrameManager dummyFrame = currentFrame;
        currentFrame = otherFrame;
        otherFrame = dummyFrame;
    }

    public bool checkObstructionForSwitching()
    {
        if (transform.parent != null && (LayerMask.LayerToName(transform.parent.gameObject.layer) == "Free Object" || LayerMask.LayerToName(transform.parent.gameObject.layer) == "Wild"))
        {
            BoxCollider2D parentCollider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
            if (!otherFrame.checkObstructionIn(transform.parent, parentCollider.bounds.extents.x) 
            && !currentFrame.checkObstructionOut(transform.parent, parentCollider.bounds.extents.x)
            && !currentFrame.checkObstructionOut(transform, boxCollider2D.bounds.extents.x) 
            && !currentFrame.checkObstructionCam())
            {
                return true;                  
            }
        } else if (!otherFrame.checkObstructionIn(transform, boxCollider2D.bounds.extents.x) 
        && !currentFrame.checkObstructionOut(transform, boxCollider2D.bounds.extents.x) 
        && !currentFrame.checkObstructionCam())
        {   
            return true;
        }
        return false;
    }
}
