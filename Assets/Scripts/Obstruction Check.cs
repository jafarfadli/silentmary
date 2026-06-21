using UnityEngine;

public class ObstructionCheck : MonoBehaviour
{
    Vector3 origin1;
    Vector3 dir1;
    public bool obstructed(Vector3 origin, Vector2 direction)
    {
        transform.position = origin;
        origin1 = origin;
        dir1 = direction;
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(1f,.5f), 0 ,direction, 20f, LayerMask.GetMask("Ground", "Moving Ground"));
        return hit.collider == null; // true = gakena
    }
    void Awake() {
        origin1 = Vector3.zero;
        dir1 = Vector3.zero;        
    }

    void Update() {
        Debug.DrawRay(origin1, dir1 * 20, Color.green);
    }
}