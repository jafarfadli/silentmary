using UnityEngine;

public class Debugging : MonoBehaviour {
    public Transform player;
    public Transform camTransform;
    public Transform newCheckpointTransform;
    public Transform newCameraTransform;

    void Start()
    {
        player= newCheckpointTransform;
        camTransform = newCameraTransform;
    }
}