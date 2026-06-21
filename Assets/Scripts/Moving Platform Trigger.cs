using UnityEngine;

public class MovingPlatformTrigger : MonoBehaviour {
    BoxCollider2D boxCollider2D;
    public MovingPlatform movingPlatform;
    float cooldownTimer = 0;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public bool isTriggered()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(0, 1), 0.1f, LayerMask.GetMask("Player"));
        return raycastHit.collider != null;
    }

    void Update()
    {
        if (isTriggered() && cooldownTimer > 0.5f)
        {
            if (Input.GetKey(KeyCode.P))
            {
                cooldownTimer = 0;
                movingPlatform.SwitchState();
            }
        }
        cooldownTimer += Time.deltaTime;
    }
}