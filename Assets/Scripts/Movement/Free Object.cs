using UnityEngine;

public class FreeObject : MonoBehaviour {
    BoxCollider2D boxCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public bool grounded()
    {
        RaycastHit2D[] downHits = Physics2D.BoxCastAll(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Player", "Free Object","Platform", "Wild"));
        if (downHits.Length > 1)
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            gameObject.SetActive(false);
        }
    }
}