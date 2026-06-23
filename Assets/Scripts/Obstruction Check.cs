using UnityEngine;

public class ObstructionCheck : MonoBehaviour
{
    public bool obstructed(Vector3 origin, Vector2 direction)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(origin + new Vector3(0.5f,0,0), direction, 20f, LayerMask.GetMask("Ground", "Moving Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(origin - new Vector3(0.5f,0,0), direction, 20f, LayerMask.GetMask("Ground", "Moving Ground"));

        return (hit1.collider == null) && (hit2.collider == null); // true = gakena
    }
}