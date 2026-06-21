using UnityEngine;

public class Fish : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            GameProgress.instance.Win();
            gameObject.SetActive(false);
        }
    }
}