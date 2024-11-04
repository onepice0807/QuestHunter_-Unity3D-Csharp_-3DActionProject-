using System;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public Action _callFunc;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnClickOkButton()
    {
        _callFunc();
    }

    public void OnClickClose()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
