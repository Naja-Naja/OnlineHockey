using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;

public class RoomManager_main : MonoBehaviourPunCallbacks
{
    [SerializeField] RoomAPI roomAPI;
    public ReactiveProperty<bool> canjoinTeam;
    public ReactiveProperty<bool> canStart;
    public ReactiveProperty<bool> canRetry;
    public ReactiveProperty<string> text;
    public ReactiveProperty<string> result;
    bool avatarCreated = false;
    string myTeam;
    private void Awake()
    {
        //roomAPI = new RoomAPI();
        //エントリーポイントentrypoint
        //サーバーに接続
        roomAPI.ConnectServer();
    }
    //サーバー接続のコールバックでルームに接続
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    //入室のコールバックで自身をChoiceに、ホストなら部屋をMatchingに設定
    public override void OnJoinedRoom()
    {
        roomAPI.roomPropatiesInit();
        //UIの初期化
        canStart.Value = false;
        canRetry.Value = false;
        result.Value = "";
        myTeam = "beforejoin";
        //参加チーム選択ボタンを表示
        canjoinTeam.Value = true;
        text.Value = "Choice your Team";
    }
    //参加チーム選択ボタンによってチームに参加する
    public void joinLeftTeam()
    {
        canjoinTeam.Value = false;
        Debug.Log("joinleft");
        myTeam = "Left";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Ready…";

    }
    public void joinRightTeam()
    {
        canjoinTeam.Value = false;
        Debug.Log("joinright");
        myTeam = "Right";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Ready…";
    }
    //チーム登録状況アップデートのコールバックでゲーム参加可能なら開始ボタンを生成する
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("開始チェック");
        if (PhotonNetwork.IsMasterClient)
        {
            canStart.Value = roomAPI.CheckCanGameStart();
        }
    }
    //開始ボタンが押されたら部屋をGameに設定
    public void GameStart()
    {
        roomAPI.SetRoomProperties("Game");
        if (PhotonNetwork.IsMasterClient)
        {
            roomAPI.CreateBall();
        }
    }
    //部屋のプロパティ変更のコールバック
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        RoomPropertiesUpDate_CreateAvatar();
        RoomPropertiesUpDate_Checkmywin();
    }
    //ルームプロパティがGameに変更されたときにAvatarを生成する
    private void RoomPropertiesUpDate_CreateAvatar()
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Game" && avatarCreated == false)
        {
            roomAPI.CreateAvatar();
            text.Value = "";
            canStart.Value = false;
            avatarCreated = true;
        }
    }
    //DeadLineから敗者がこの関数を呼ぶ
    public void GameJudge(string loser)
    {
        if (loser == myTeam)
        {
            Debug.Log("負け");
            result.Value = "Lose";
            roomAPI.LoseEvent(loser);
            GameInit();
        }
    }
    //勝者の表示処理
    public void RoomPropertiesUpDate_Checkmywin()
    {
        string winner = (string)PhotonNetwork.CurrentRoom.CustomProperties["Winner"];
        if (winner == myTeam)
        {
            result.Value = "Win";
            GameInit();
        }

    }
    //ゲームの初期化処理
    public void GameInit()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //ボール消去
            photonView.RPC(nameof(roomAPI.BallDestroy), RpcTarget.All);
            //再戦ボタンの表示
            canRetry.Value = true;
        }
    }
    //ゲーム再開処理
    public void Retry()
    {
        //UIの消去
        canRetry.Value = false;
        photonView.RPC(nameof(RpcResultUIInit), RpcTarget.All);
        roomAPI.CreateBall();
    }
    [PunRPC]
    void RpcResultUIInit()
    {
        Debug.Log("勝敗表示を消去");
        result.Value = "";
    }
}
