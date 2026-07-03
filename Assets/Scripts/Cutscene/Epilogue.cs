using UnityEngine;
using TMPro;

public class Epilogue: MonoBehaviour
{
    public GameObject[] frames;
    public UnityEngine.UI.Image hytam;
    public float cutsceneDuration;
    RectTransform frame0Rect;
    Typing typing;
    float cutsceneTimer = 0;
    float[] durations = new float[]{0f,0f,0f,0f};
    float[] bounds = new float[]{0f,0f,0f,0f};
    string[] texts = new string[]{"","","","",""};

    public TMP_Text[] textComponents;
    bool[] typeds = new bool[]{false,false,false,false,false};

    bool brakePlayed = false;
    void Awake()
    {
        frame0Rect = frames[0].GetComponent<RectTransform>();
        typing = GetComponent<Typing>();
        durations[0] = cutsceneDuration * 1/4;
        durations[1] = cutsceneDuration * 1/4;
        durations[2] = cutsceneDuration * 1/4;
        durations[3] = cutsceneDuration * 1/4;

        for (int i=0; i < durations.Length; i++)
        {
            bounds[i] = 0;
            for (int j = 0; j <= i; j++)
            {
                bounds[i] += durations[j];
            }
        }
        for (int i = 0; i < textComponents.Length; i++)
        {
            texts[i]=textComponents[i].text;
            textComponents[i].text = "";
        }
    }
    void Update()
    {
        cutsceneTimer += Time.deltaTime;
        if (cutsceneTimer < bounds[0])
        {
            if (!brakePlayed){
                CutsceneSFX.instance.playBrake();
                brakePlayed = true;
            }
            frames[0].SetActive(true);          
        } else if (cutsceneTimer < bounds[1])
        {
            frames[1].SetActive(true);
            frames[0].SetActive(false);           
        } else if (cutsceneTimer < bounds[2])
        {
            frames[2].SetActive(true);
            frames[1].SetActive(false);             
        } else if (cutsceneTimer < bounds[3])
        {
            frames[3].SetActive(true);
            frames[2].SetActive(false);            
        } else
        {
            hytam.gameObject.SetActive(true);
            float alpha = cutsceneTimer - bounds[3];
            if (alpha < 1)
            {
                hytam.color = new Color(0,0,0,alpha);
            }
            else
            {
                UIManager.instance.goToScene("Select Level");
            }
        }
    }
}