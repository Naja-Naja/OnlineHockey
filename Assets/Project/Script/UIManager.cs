using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] TitleUIManager titleUIManager;
    [SerializeField] Text titleText;
    [SerializeField] GameObject textBox;
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
    [SerializeField] GameObject titleStartButton;
    private Button titleStartButton_button;
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
        //IDisposable subscription_title = titleUIManager.Title.Subscribe(x => {
        //    TextFade(x, titleText);
        //});

        //IDisposable subscription_titleStart = titleUIManager.startbutton.Subscribe(x => {
        //    ButtonFadeScale(titleStartButton, x);
        //    Debug.Log("callstartbutton" + x);
        //});

        //IDisposable subscription_textbox = titleUIManager.textBox.Subscribe(x => {
        //    ButtonFadeScale(textBox, x);
        //});

        IDisposable subscription_text = roomManager.text.Subscribe(x => {
            TextFade(x, text);
        });

        IDisposable subscription_result = roomManager.result.Subscribe(x => {
            TextFade(x,result);
        });

        IDisposable subscription_leftplayerlist = playerList.leftPlayerList.Subscribe(x => {
            TextFade(x, leftTeamList);
        });

        IDisposable subscription_rightplayerlist = playerList.rightPlayerList.Subscribe(x => {
            TextFade(x, rightTeamList);
        });

        IDisposable subscription = roomManager.canRetry.Subscribe(x => {
            ButtonFadeScale(retryButton, x);
        });

        IDisposable subscription_leftgame = roomManager.canLeftGame.Subscribe(x => {
            ButtonFadeScale(leftGameButton, x);
        });

        IDisposable subscription_leftTeam = roomManager.canjoinTeam.Subscribe(x => {
            ButtonFadeScale(leftTeamButton,x);
        });

        IDisposable subscription_rightTeam = roomManager.canjoinTeam.Subscribe(x => {
            ButtonFadeScale(rightTeamButton, x);
        });

        IDisposable subscription_start = roomManager.canStart.Subscribe(x => {
            ButtonFadeScale(startButton, x);
        });

        IDisposable subscription_back = roomManager.canBack.Subscribe(x => {
            ButtonFadeScale(backChoiceTeamButton, x);
        });

        titleStartButton_button = titleStartButton.GetComponent<Button>();
        titleStartButton_button.onClick.AddListener(OnClickTitleStartButton);

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

        //titleUIManager.OpenTitlePage();
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
    private void OnClickTitleStartButton()
    {
        AudioManager.SE_Play(choice);
        roomManager.InputPlayerName(textBox.GetComponent<InputField>().text);
        Debug.Log("yorname is " + textBox.GetComponent<InputField>().text);
        //titleUIManager.CloseTitlePage();
        TextFade("", titleText);
        ButtonFadeScale(textBox, false);
        ButtonFadeScale(titleStartButton, false);

    }
    private void ButtonFadeScale(GameObject button, bool Isactive)
    {
        if (Isactive)
        {
            button.transform.DOScale(Vector3.one, 0.1f).OnStart(() => button.SetActive(true));

        }
        else
        {
            button.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => button.SetActive(false));

        }
    }
    private void TextFade(string sentence,Text text)
    {
        text.text = "";
        text.DOText(sentence, 0.3f);
    }
    //private void ButtonFadeInScale(GameObject button)
    //{
    //}
}
