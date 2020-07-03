using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{

    public delegate void OnLoadingCompleteHandler();

    public event OnLoadingCompleteHandler OnLoadingComplete;

    private int current = 0;
    private int minimum = 0;
    private int maximum = 360;
    private int step = 4;

    private Image loader;

    private bool load = false;


    public void StartLoading()
    {
        load = true;
        loader.enabled = true;
    }

    public void StopLoading()
    {
        load = false;
        current = 0;
        loader.enabled = false;
    }
 
    private void Start()
    {
        loader = GetComponent<Image>();
        StopLoading();
    }

    private void Update()
    {
        if(!load)
        {
            return;
        }

        float currentOffser = (float)current - (float)minimum;
        float maximumOffser = (float)maximum - (float)minimum;
        float fillAmount = currentOffser / maximumOffser;
        loader.fillAmount = fillAmount;

        if(current >= 360)
        {
            OnLoadingComplete?.Invoke();
            load = false;
        }

        current+=step;
    }
}
