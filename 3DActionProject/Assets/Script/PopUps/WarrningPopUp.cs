using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Application;

public class WarrningPopUp : MonoBehaviour
{
    [SerializeField] private Text _DescriptionText;

    public Action _oKCallFunc = null;
    public Action _cancelFunc = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetDescription(string text)
    {
        _DescriptionText.text = text;
    }

    public void SetOkButtonCallback(Action okCallback)
    {
        _oKCallFunc = okCallback;
    }

    public void SetCancelButtonCallback(Action cancelCallback)
    {
        _cancelFunc = cancelCallback;
    }

    public void OnClickOkButton()
    {
        this.gameObject.SetActive(false);
        _oKCallFunc?.Invoke();
    }

    public void OnClickCancelButton()
    {
        this.gameObject.SetActive(false);
        _cancelFunc?.Invoke();
    }
}
