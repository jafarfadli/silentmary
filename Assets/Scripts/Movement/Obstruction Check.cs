using UnityEngine;

public class ObstructionCheck : MonoBehaviour
{
    Vector3 debugOrigin;
    Vector3 debugDirection;
    public bool checkObsOut(Vector3 origin, Vector2 direction, Vector3 offset)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(origin + offset, direction, 20f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(origin - offset, direction, 20f, LayerMask.GetMask("Ground"));

        return (hit1.collider == null) && (hit2.collider == null); // true = gakena
    }
    public bool checkObsIn(Vector3 origin, Vector2 direction, Vector3 offset)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(origin + offset, direction, 20f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(origin - offset, direction, 20f, LayerMask.GetMask("Ground"));

        return (hit1.collider != null) && (hit2.collider != null); // true = kena kena
    }
}