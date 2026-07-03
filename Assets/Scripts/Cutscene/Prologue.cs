using TMPro;
using UnityEngine;

public class Prologue: MonoBehaviour
{
    public GameObject[] frames;
    public UnityEngine.UI.Image hytam;
    public float cutsceneDuration;
    public RectTransform BgUI;
    public RectTransform CloudUI;
    public RectTransform[] Grounds;
    RectTransform frame0Rect;
    Typing typing;
    float cutsceneTimer = 0;
    float[] durations = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
    float[] bounds = new float[]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
    string[] texts = new string[]{"","","","","","","",""};

    public TMP_Text[] textComponents;
    bool[] typeds = new bool[]{false,false,false,false,false,false,false,false};

    bool paintingPlayed = false;
    bool trunkPlayed = false;
    void Awake()
    {
        frame0Rect = frames[0].GetComponent<RectTransform>();
        typing = GetComponent<Typing>();
        durations[0] = cutsceneDuration * 1/8;
        durations[1] = cutsceneDuration * .5f/8;
        durations[2] = cutsceneDuration * 1/8;
        durations[3] = cutsceneDuration * .5f/8;
        durations[4] = cutsceneDuration * 1/8;
        durations[5] = cutsceneDuration * .5f/8;
        durations[6] = cutsceneDuration * 1/8;
        durations[7] = cutsceneDuration * .5f/8;
        durations[8] = cutsceneDuration * 1/8;
        durations[9] = cutsceneDuration * 1/8;

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
            if (!typeds[0] && cutsceneTimer < durations[0]/2)
            {
                typing.StartTyping(texts[0],textComponents[0]);
                typeds[0] = true;
            } else if (!typeds[1] && cutsceneTimer > durations[0]/2 && cutsceneTimer < durations[0]){
                typing.StartTyping(texts[1],textComponents[1]);
                typeds[1] = true;                
            }
            frames[0].SetActive(true);
            frame0Rect.anchoredPosition = new Vector3(692.32f,0,0);
        } else if (cutsceneTimer < bounds[1])
        {
            textComponents[0].text = "";
            textComponents[1].text = "";
            frame0Rect.anchoredPosition = Vector3.Lerp(new Vector3(692.32f,0,0), new Vector3(0,0,0), 1 - Mathf.Pow(1- ((cutsceneTimer-bounds[0])/durations[1]),2));
        } else if (cutsceneTimer < bounds[2])
        {
            if (!typeds[2] && cutsceneTimer-bounds[1] < durations[2]*2/3)
            {
                typing.StartTyping(texts[2],textComponents[2]);
                typeds[2] = true;
            } else if (!typeds[3] && cutsceneTimer-bounds[1] > durations[2] *2/3 && cutsceneTimer-bounds[1] < durations[2]){
                typing.StartTyping(texts[3],textComponents[3]);
                typeds[3] = true;                
            }            
        } else if (cutsceneTimer < bounds[3])
        {
            textComponents[2].text = "";
            textComponents[3].text = "";
            frame0Rect.anchoredPosition = Vector3.Lerp(new Vector3(0,0,0), new Vector3(-692.32f,0,0), 1 - Mathf.Pow(1- ((cutsceneTimer-bounds[2])/durations[3]),2));
        } else if (cutsceneTimer < bounds[4])
        {
            if (!typeds[4])
            {
                typing.StartTyping(texts[4],textComponents[4]);
                typeds[4] = true;
            }             
        } else if (cutsceneTimer < bounds[5])
        {
            if (!paintingPlayed){
            CutsceneSFX.instance.playPainting();
            paintingPlayed = true;
            }
            textComponents[4].text = "";
            frames[1].SetActive(true);
            frames[0].SetActive(false);           
        } else if (cutsceneTimer < bounds[6])
        {
            if (!typeds[5])
            {
                typing.StartTyping(texts[5],textComponents[5]);
                typeds[5] = true;
            }   
            frames[2].SetActive(true);
            frames[1].SetActive(false);             
        } else if (cutsceneTimer < bounds[7])
        {
            textComponents[5].text = "";
            frames[3].SetActive(true);
            frames[2].SetActive(false);            
        } else if (cutsceneTimer < bounds[8])
        {
            if (!trunkPlayed){
            CutsceneSFX.instance.playTrunk();
            trunkPlayed = true;
            }
            if (!typeds[6])
            {
                typing.StartTyping(texts[6],textComponents[6]);
                typeds[6] = true;
            }   
            frames[4].SetActive(true);
            frames[3].SetActive(false);            
        } else if (cutsceneTimer < bounds[9])
        {
            textComponents[6].text = "";
            if (!typeds[7])
            {
                typing.StartTyping(texts[7],textComponents[7]);
                typeds[7] = true;
            }   
            CloudUI.gameObject.SetActive(true);
            BgUI.gameObject.SetActive(true);
            Grounds[0].gameObject.SetActive(true);
            Grounds[1].gameObject.SetActive(true);

            frames[5].SetActive(true);
            frames[4].SetActive(false);  

            CloudUI.position += new Vector3(Time.deltaTime * -32,0,0);    
            Grounds[0].position += new Vector3(Time.deltaTime * -8,0,0);   
            Grounds[1].position += new Vector3(Time.deltaTime * -8,0,0);       
        }
        else
        {
            textComponents[7].text = "";
            hytam.gameObject.SetActive(true);
            float alpha = cutsceneTimer - bounds[9];
            if (alpha < 1)
            {
                hytam.color = new Color(0,0,0,alpha);
            }
            else
            {
                PlayerPrefs.SetInt("CheckpointIsCheckpoint", 0);
                UIManager.instance.goToScene("L1");
            }
        }
    }
}