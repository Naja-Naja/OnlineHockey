using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    //[SerializeField] Manager manager;
    [SerializeField] RoomManager_main roomManager;
    [SerializeField] GameObject title;
    //[SerializeField] GameObject textobj;
    [SerializeField] Text text;
    [SerializeField] Text result;
    //リトライ
    [SerializeField] GameObject retryButton;
    private Button retryButton_button;
    //スタート
    [SerializeField] GameObject startButton;
    private Button startButton_button;
    //左チーム参加
    [SerializeField] GameObject leftTeamButton;
    private Button leftTeamButton_button;
    //右チーム参加
    [SerializeField] GameObject rightTeamButton;
    private Button rightTeamButton_button;

    void Awake()
    {
        //text = textobj.GetComponent<Text>();

        //IDisposable subscription_textobj = roomManager.activeText.Subscribe(x => {
        //    textobj.SetActive(x);
        //});

        IDisposable subscription_text = roomManager.text.Subscribe(x => {
            text.text=x;
        });

        IDisposable subscription_result = roomManager.result.Subscribe(x => {
            result.text = x;
        });

        IDisposable subscription = roomManager.canRetry.Subscribe(x => {
        retryButton.SetActive(x);
        });

        IDisposable subscription_leftTeam = roomManager.canjoinTeam.Subscribe(x => {
            leftTeamButton.SetActive(x);
        });

        IDisposable subscription_rightTeam = roomManager.canjoinTeam.Subscribe(x => {
            rightTeamButton.SetActive(x);
        });

        IDisposable subscription_start = roomManager.canStart.Subscribe(x => {
            startButton.SetActive(x);
        });


        retryButton_button = retryButton.GetComponent<Button>();
        retryButton_button.onClick.AddListener(OnClickRetryButton);

        leftTeamButton_button = leftTeamButton.GetComponent<Button>();
        leftTeamButton_button.onClick.AddListener(OnClickLeftTeamButton);

        rightTeamButton_button = rightTeamButton.GetComponent<Button>();
        rightTeamButton_button.onClick.AddListener(OnClickRightTeamButton);

        startButton_button = startButton.GetComponent<Button>();
        startButton_button.onClick.AddListener(OnClickStartButton);
    }
    private void OnClickRetryButton()
    {
        Debug.Log("call");
        roomManager.Retry();
    }
    private void OnClickLeftTeamButton()
    {
        Debug.Log("call");
        roomManager.joinLeftTeam();
    }
    private void OnClickRightTeamButton()
    {
        Debug.Log("call");
        roomManager.joinRightTeam();
    }
    private void OnClickStartButton()
    {
        Debug.Log("callgamestart");
        roomManager.GameStart();
    }
}
