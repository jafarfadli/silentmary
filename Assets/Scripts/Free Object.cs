using UnityEngine;

public class FreeObject : MonoBehaviour {
    BoxCollider2D boxCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public bool grounded()
    {
        RaycastHit2D downHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Free Object"));
        return downHit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            gameObject.SetActive(false);
        }
    }
}