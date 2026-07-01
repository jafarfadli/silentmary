using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadlyObject : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SetActive(false);
            GameProgress.instance.RestartFromCheckpoint();
        }
        else if (other.gameObject.tag == "Free Object")
        {
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Wild")
        {
            other.gameObject.SetActive(false);
        }
    }
}