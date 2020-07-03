using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] public int ID = 0;
    [SerializeField] public string Category = "";

    public Button Button = null;

    private void Start()
    {
        Button = GetComponent<Button>();
    }

    private void Update()
    {
        
    }
}
