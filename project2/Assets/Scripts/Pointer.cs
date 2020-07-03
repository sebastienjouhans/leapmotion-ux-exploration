using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] public GameObject mainCanvas = null;
    [SerializeField] public GameObject loaderGO = null;


    private Image pointerImg = null;
    private Loader loader = null;
    private int buttonID = int.MaxValue;
    private string category = "";
    private string interactiveTag = "interactive";


    public delegate void OnSelectionCompleteHandler(int buttonID, string category);

    public event OnSelectionCompleteHandler OnSelectionComplete;

    private void Start()
    {
        pointerImg = GetComponent<Image>();
        loader = loaderGO.GetComponent<Loader>();
        loader.OnLoadingComplete += LoaderOnLoadingComplete;
    }

    private void LoaderOnLoadingComplete()
    {
        OnSelectionComplete?.Invoke(buttonID, category);
    }

    private void Update()
    {
        //pointerImg.transform.position = Input.mousePosition;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<UIButton>() == null)
        {
            return;
        }

        if (collision.CompareTag(interactiveTag))
        {
            var renderer = collision.GetComponent<Button>();
            if (renderer != null)
            {
                loader.StartLoading();
                renderer.OnPointerEnter(null);
                buttonID = collision.GetComponent<UIButton>().ID;
                category = collision.GetComponent<UIButton>().Category;                
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<UIButton>() == null)
        {
            return;
        }

        if (collision.CompareTag(interactiveTag))
        {
            var renderer = collision.GetComponent<Button>();
            if (renderer != null)
            {
                loader.StopLoading();
                renderer.OnPointerExit(null);
                buttonID = int.MaxValue;
                category = "";
            }
        }
    }

}
