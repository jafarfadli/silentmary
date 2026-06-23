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

    public bool checkObstructionIn(Transform playerTransform) // gakena = obstructed
    {
        bool obstructed = obstructionCheck.obstructed(new Vector3(-playerTransform.position.x,transform.position.y + 10, 0), Vector2.down);
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "Obstructed";
        obstructedText.gameObject.SetActive(obstructed);

        return obstructed;
    }

    public bool checkObstructionOut(Transform playerTransform) // kena = obstructed
    {
        bool NOTobstructed = obstructionCheck.obstructed(new Vector3(playerTransform.position.x, playerTransform.position.y, 0), Vector2.up);
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "Obstructed";
        obstructedText.gameObject.SetActive(!NOTobstructed);

        return !NOTobstructed;
    }

    public bool checkObstructionCam()
    {
        bool inCooldown = CameraMovement.instance.isMoving;
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "In Cooldown";
        obstructedText.gameObject.SetActive(inCooldown);

        return inCooldown;
    }

    public void switchOutFrame()
    {
        player.SetActive(false);
        player.transform.SetParent(transform, true);
    }

    public void switchInFrame(Transform playerTransform, Transform cameraTransform, GameObject carry = null)
    {
        player.transform.position = new Vector3(-playerTransform.position.x, transform.position.y + 10, 0);
        player.transform.localScale = new Vector3(-playerTransform.localScale.x, playerTransform.localScale.y, playerTransform.localScale.z);
        player.GetComponent<PlayerMovement>().switchingIn = true;

        if (carry != null)
        {
            player.GetComponent<PlayerMovement>().switchingCarry = true;
            player.GetComponent<PlayerMovement>().carry = carry;
            carry.transform.position = player.transform.position - new Vector3(0,1,0);
            carry.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
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