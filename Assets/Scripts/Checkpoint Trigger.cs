using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform newCheckpointTransform;
    public Transform newCameraTransform;
    public bool leftFrame;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
        GameProgress.instance.SetCheckpoint(newCheckpointTransform.position, newCameraTransform.position, leftFrame);
        }
    }
}