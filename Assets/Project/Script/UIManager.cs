using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] AudioClip choice;
    [SerializeField] AudioClip undo;
    //[SerializeField] Manager manager;
    [SerializeField] RoomManager_main roomManager;
    [SerializeField] PlayerList playerList;
    [SerializeField] GameObject title;
    //[SerializeField] GameObject textobj;
    [SerializeField] Text text;
    [SerializeField] Text result;
    [SerializeField] Text leftTeamList;
    [SerializeField] Text rightTeamList;
    //リトライ
    [SerializeField] GameObject retryButton;
    private Button retryButton_button;
    //ゲームを出る
    [SerializeField] GameObject leftGameButton;
    private Button leftGameButton_button;
    //スタート
    [SerializeField] GameObject startButton;
    private Button startButton_button;
    //左チーム参加
    [SerializeField] GameObject leftTeamButton;
    private Button leftTeamButton_button;
    //右チーム参加
    [SerializeField] GameObject rightTeamButton;
    private Button rightTeamButton_button;
    //チーム選択にもどる
    [SerializeField] GameObject backChoiceTeamButton;
    private Button backChoiceTeamButton_button;

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

        IDisposable subscription_leftplayerlist = playerList.leftPlayerList.Subscribe(x => {
            leftTeamList.text = x;
        });

        IDisposable subscription_rightplayerlist = playerList.rightPlayerList.Subscribe(x => {
            rightTeamList.text = x;
        });

        IDisposable subscription = roomManager.canRetry.Subscribe(x => {
        retryButton.SetActive(x);
        });

        IDisposable subscription_leftgame = roomManager.canLeftGame.Subscribe(x => {
            leftGameButton.SetActive(x);
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

        IDisposable subscription_back = roomManager.canBack.Subscribe(x => {
            backChoiceTeamButton.SetActive(x);
        });


        retryButton_button = retryButton.GetComponent<Button>();
        retryButton_button.onClick.AddListener(OnClickRetryButton);

        leftGameButton_button = leftGameButton.GetComponent<Button>();
        leftGameButton_button.onClick.AddListener(OnClickLeftGameButton);

        leftTeamButton_button = leftTeamButton.GetComponent<Button>();
        leftTeamButton_button.onClick.AddListener(OnClickLeftTeamButton);

        rightTeamButton_button = rightTeamButton.GetComponent<Button>();
        rightTeamButton_button.onClick.AddListener(OnClickRightTeamButton);

        startButton_button = startButton.GetComponent<Button>();
        startButton_button.onClick.AddListener(OnClickStartButton);

        backChoiceTeamButton_button = backChoiceTeamButton.GetComponent<Button>();
        backChoiceTeamButton_button.onClick.AddListener(OnClickBackButton);
    }
    private void OnClickRetryButton()
    {
        Debug.Log("call");
        AudioManager.SE_Play(choice);
        roomManager.Retry();
    }
    private void OnClickLeftTeamButton()
    {
        Debug.Log("call");
        AudioManager.SE_Play(choice);
        roomManager.joinLeftTeam();
    }
    private void OnClickRightTeamButton()
    {
        Debug.Log("call");
        AudioManager.SE_Play(choice);
        roomManager.joinRightTeam();
    }
    private void OnClickStartButton()
    {
        Debug.Log("callgamestart");
        AudioManager.SE_Play(choice);
        roomManager.GameStart();
    }
    private void OnClickBackButton()
    {
        Debug.Log("callgamestart");
        AudioManager.SE_Play(undo);
        roomManager.BackChoiceTeam();
    }
    private void OnClickLeftGameButton()
    {
        Debug.Log("callgamestart");
        AudioManager.SE_Play(undo);
        roomManager.leftGame();
    }
    
}
