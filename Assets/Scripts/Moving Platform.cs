using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public Transform[] tracks;
    public float speed = 2f;
    int currentIndex = 0;

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, tracks[currentIndex].position, speed*Time.deltaTime);
    }

    public void SwitchState()
    {
        currentIndex = currentIndex == 0 ? 1 : 0;
    }
}