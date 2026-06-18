using UnityEngine;

public class ObstructionCheck : MonoBehaviour
{
    public bool obstructed(Vector3 origin, Vector2 direction)
    {
        RaycastHit2D downHit = Physics2D.Raycast(origin, direction, 20f, LayerMask.GetMask("Ground"));
        return downHit.collider == null;
    }
}