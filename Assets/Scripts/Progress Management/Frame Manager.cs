using UnityEngine;
using TMPro;

public class FrameManager : MonoBehaviour
{
    GameObject player;
    public ObstructionCheck obstructionCheck;
    public TextMeshProUGUI obstructedText;
    Color obstructedTextColor;

    void Awake()
    {
        obstructedTextColor = obstructedText.color;
        obstructedText.gameObject.SetActive(false);
    }

    void Start()
    {
        player = PlayerMovement.instance.gameObject;
    }

    public bool checkObstructionIn(Transform objectToSwitch, float offsetX)
    {
        bool okay = obstructionCheck.checkObsIn(new Vector3(-objectToSwitch.position.x,transform.position.y + 10, 0), Vector2.down, new Vector3 (offsetX,0,0));
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "Obstructed";
        obstructedText.gameObject.SetActive(!okay);

        return !okay;
    }

    public bool checkObstructionOut(Transform objectToSwitch, float offsetX)
    {
        bool okay = obstructionCheck.checkObsOut(new Vector3(objectToSwitch.position.x, objectToSwitch.position.y, 0), Vector2.up, new Vector3(offsetX,0,0));
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "Obstructed";
        obstructedText.gameObject.SetActive(!okay);

        return !okay;
    }

    public bool checkObstructionCam()
    {
        bool inCooldown = CameraMovement.instance.isMoving;
        obstructedText.color = obstructedTextColor;
        obstructedText.text = "In Cooldown";
        obstructedText.gameObject.SetActive(inCooldown);

        return inCooldown;
    }

    public void setIndicator(Transform objectToSwitch, bool isYes)
    {
        obstructionCheck.setIndicatorPosition(new Vector3(-objectToSwitch.position.x, transform.position.y + 10, 0), isYes);
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