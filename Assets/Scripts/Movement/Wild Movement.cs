using UnityEngine;

public class WildMovement: MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpPower = 18f;
    Rigidbody2D body;
    BoxCollider2D boxCollider2D;
    Animator animator;
    bool groundedBool = false;
    float ungroundedTimer = Mathf.Infinity;
    float childedTimer = Mathf.Infinity;
    bool leftWalk = false;
    bool rightWalk = true;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        childedTimer += Time.deltaTime;
        ungroundedTimer += Time.deltaTime;
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
            animator.SetBool("controlled", false);
            return;
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

        float parentSignX = transform.parent == null? 1: Mathf.Sign(transform.parent.localScale.x);
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

        animator.SetBool("walk", rightWalk || leftWalk);
        animator.SetBool("grounded", grounded());
        animator.SetBool("controlled", true);
    }

    public bool grounded()
    {
        RaycastHit2D[] downHits = Physics2D.BoxCastAll(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, LayerMask.GetMask("Ground", "Free Object", "Player", "Platform", "Wild"));
        return downHits.Length > 1;
    }

    bool topped()
    {
        RaycastHit2D[] topHits = Physics2D.BoxCastAll(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.up, 0.5f, LayerMask.GetMask("Player","Free Object", "Wild"));
        return topHits.Length > 1;
    }    
}