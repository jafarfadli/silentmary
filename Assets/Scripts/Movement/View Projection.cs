using UnityEngine;

public class ViewProjection: MonoBehaviour
{
    public static ViewProjection instance;
    public Transform indicatorYes;
    public Transform indicatorNo;

    void Awake()
    {
        instance = this;
    }

    public void SetIndicator(bool isYes, Vector3 position)
    {
        if (isYes)
        {
            indicatorYes.gameObject.SetActive(true);
            indicatorYes.position = position;
        }
        else
        {
            indicatorNo.gameObject.SetActive(true);
            indicatorNo.position = position;
        }
    }

    public void RemoveIndicator()
    {
        indicatorYes.gameObject.SetActive(false);
        indicatorNo.gameObject.SetActive(false);
    }
}