using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] Manager manager;
    [SerializeField] RoomManager roomManager; 
    [SerializeField] GameObject title;
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

    // Start is called before the first frame update
    void Awake()
    {
        IDisposable subscription_title = roomManager.activetitle.Subscribe(x => {
           title.SetActive(x);
        });

        IDisposable subscription_retry = manager.canRetry.Subscribe(x => {
            retryButton.SetActive(x);
        });

        IDisposable subscription_start = roomManager.canStart.Subscribe(x => {
            startButton.SetActive(x);
        });

        IDisposable subscription_leftTeam = roomManager.canjoinTeam.Subscribe(x => {
            leftTeamButton.SetActive(x);
        });

        IDisposable subscription_rightTeam = roomManager.canjoinTeam.Subscribe(x => {
            rightTeamButton.SetActive(x);
        });
        Debug.Log("登録");

        //各種ボタンのボタンコンポーネントにlistenerを設定
        retryButton_button = retryButton.GetComponent<Button>();
        retryButton_button.onClick.AddListener(OnClickRetryButton);

        startButton_button = startButton.GetComponent<Button>();
        startButton_button.onClick.AddListener(OnClickStartButton);

        leftTeamButton_button = leftTeamButton.GetComponent<Button>();
        leftTeamButton_button.onClick.AddListener(OnClickLeftTeamButton);

        rightTeamButton_button = rightTeamButton.GetComponent<Button>();
        rightTeamButton_button.onClick.AddListener(OnClickRightTeamButton);
    }
    private void OnClickRetryButton()
    {
        Debug.Log("call");
        manager.canRetry.Value = false;
    }
    private void OnClickStartButton()
    {
        Debug.Log("call");
        manager.canRetry.Value = false;
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
}
