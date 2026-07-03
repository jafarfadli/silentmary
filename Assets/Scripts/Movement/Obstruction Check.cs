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

    public void setIndicatorPosition(Vector3 origin, bool isYes)
    {
        debugOrigin = origin;
        debugDirection = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 25f, LayerMask.GetMask("Ground","Deadly","Platform","Free Object","Wild"));
        ViewProjection.instance.SetIndicator(isYes, (Vector3)hit.point + new Vector3(0, 1.2f, 0));
    }

    public bool gridCheck(int gridX, int gridY, bool isLeft)
    {
        float checkpointCameraX = PlayerPrefs.GetFloat("CheckpointCameraX", 0);
        int checkpointFrameLeft = PlayerPrefs.GetInt("CheckpointFrameLeft", 1);

        Vector3 leftCamPos = new Vector3(checkpointCameraX * (2*checkpointFrameLeft -1),0,0);
        Vector3 rightCamPos = new Vector3(-checkpointCameraX * (2*checkpointFrameLeft -1),-60,0);

        if (isLeft)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(leftCamPos.x + gridX - 14.5f,leftCamPos.y+ gridY - 9.5f, 0), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));   
            return hit.collider != null;
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(rightCamPos.x + gridX - 14.5f,rightCamPos.y+ gridY - 9.5f, 0), Vector2.down, 0.1f, LayerMask.GetMask("Ground"));   
            return hit.collider != null;            
        }
    }

    void Update()
    {
        Debug.DrawRay(debugOrigin, debugDirection * 20f, Color.red);
    }
}