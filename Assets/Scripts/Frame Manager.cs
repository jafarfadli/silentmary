using UnityEngine;
using TMPro;

public class FrameManager : MonoBehaviour
{
    public GameObject player;
    public ObstructionCheck obstructionCheck;
    public TextMeshProUGUI obstructedText;
    Color obstructedTextColor;

    void Awake()
    {
        obstructedTextColor = obstructedText.color;
        obstructedText.gameObject.SetActive(false);
    }

    public bool checkObstructionIn(Transform playerTransform)
    {
        bool obstructed = obstructionCheck.obstructed(new Vector3(-playerTransform.position.x, 10, 0), Vector2.down);
        obstructedText.color = obstructedTextColor;
        obstructedText.gameObject.SetActive(obstructed);

        return obstructed;
    }

    public bool checkObstructionOut(Transform playerTransform)
    {
        bool obstructed = obstructionCheck.obstructed(new Vector3(-playerTransform.position.x, 10, 0), Vector2.up);
        obstructedText.color = obstructedTextColor;
        obstructedText.gameObject.SetActive(obstructed);

        return obstructed;
    }

    public void switchOutFrame()
    {
        player.SetActive(false);
    }

    public void switchInFrame(Transform playerTransform, Transform cameraTransform)
    {
        player.transform.position = new Vector3(-playerTransform.position.x, transform.position.y + 10, 0);
        player.transform.localScale = new Vector3(-playerTransform.localScale.x, playerTransform.localScale.y, playerTransform.localScale.z);
        player.GetComponent<PlayerMovement>().switchingIn = true;
        player.SetActive(true);

        cameraTransform.position = new Vector3(-cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
    }

    void Update()
    {
        if (obstructedText.gameObject.activeSelf)
        {
            Color current_color = obstructedText.color;
            current_color = new Color(current_color.r, current_color.g, current_color.b,current_color.a - Time.deltaTime);
            obstructedText.color = current_color;
            if (current_color.a <= 0)
            {
                obstructedText.gameObject.SetActive(false);
            }            
        }
    }
}