using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class ResultMessage : MonoBehaviour
{

    [SerializeField] Text text;
    [SerializeField] Manager manager;
    void Start()
    {
        IDisposable subscription = manager.result.Subscribe(x => {
            text.text=x;
        });
    }
}
