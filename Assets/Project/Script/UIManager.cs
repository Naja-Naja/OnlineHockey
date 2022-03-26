using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject retryButton;
    private Button retryButton_button;
    [SerializeField] Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        IDisposable subscription = manager.canRetry.Subscribe(x => {
        retryButton.SetActive(x);
        });
        retryButton_button = retryButton.GetComponent<Button>();
        retryButton_button.onClick.AddListener(OnClickButton);
    }
    private void OnClickButton()
    {
        Debug.Log("call");
        manager.canRetry.Value = false;
    }
}
