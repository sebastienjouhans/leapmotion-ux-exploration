using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{ 

    public delegate void OnCollisionEnterHandler(Collider2D collision);
    public delegate void OnCollisionExitHandler(Collider2D collision);


    public event OnCollisionEnterHandler OnCollisionEnter;
    public event OnCollisionExitHandler OnCollisionExit;


    private void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollisionEnter?.Invoke(collision);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        OnCollisionExit?.Invoke(collision);
    }

}
