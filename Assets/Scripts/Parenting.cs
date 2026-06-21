using UnityEngine;
using UnityEngine.UIElements;

public class Parenting : MonoBehaviour
{
    public Transform originalParent;
    BoxCollider2D boxCollider2D;
    Vector3 originalScale;
    Rigidbody2D body;
    Transform currentParent;
    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (boxCollider2D.enabled && !boxCollider2D.isTrigger)
        {
            Transform groundTransform = ground();
            if (checkAngle(transform.rotation.eulerAngles.z) && groundTransform != null)
            {
                adjustAngle();
                transform.SetParent(groundTransform, true);
                transform.localScale = new Vector3(1/groundTransform.localScale.x * Mathf.Sign(groundTransform.localScale.x)  * Mathf.Sign(transform.localScale.x),1/groundTransform.localScale.y,1/groundTransform.localScale.z);
                if (currentParent != groundTransform)
                {
                    body.linearVelocity = Vector2.zero;
                }
            }
            else
            {
                transform.SetParent(originalParent, true);
                transform.localScale = new Vector3(originalScale.x * Mathf.Sign(transform.localScale.x), originalScale.y, originalScale.z);
            }
            currentParent = groundTransform;
        }
    }

    Transform ground()
    {
        RaycastHit2D[] downHit = Physics2D.RaycastAll(transform.position, Vector2.down, Mathf.Max(boxCollider2D.bounds.extents.y,boxCollider2D.bounds.extents.x)+0.2f, LayerMask.GetMask("Ground","Player"));
        if (downHit.Length > 1)
        {
            return downHit[1].collider.transform;
        }
        return null;
    }

    bool checkAngle(float angle)
    {
        return Mathf.Min(angle % 90, 90 - (angle % 90)) < 5;
    }
    
    void adjustAngle()
    {
        float rotationZ = transform.eulerAngles.z;
        int targetRotationZ = Mathf.FloorToInt((rotationZ + 45) % 360 / 90) * 90;
        transform.rotation = Quaternion.Euler(0, 0, targetRotationZ);
    }

    public void releaseParent()
    {
        gameObject.transform.SetParent(originalParent, true);
        gameObject.transform.localScale = new Vector3(originalScale.x * Mathf.Sign(transform.localScale.x), originalScale.y, originalScale.z);   
    }
}