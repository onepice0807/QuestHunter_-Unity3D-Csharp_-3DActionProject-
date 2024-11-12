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
        Time.timeScale = 1; // 게임 재개
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
