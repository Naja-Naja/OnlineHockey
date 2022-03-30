using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using System;
using ExitGames.Client.Photon;
public class Manager : MonoBehaviourPunCallbacks
{
    public ReactiveProperty<String> result;
    public ReactiveProperty<bool> canRetry;
    private String team = "null";

    void Awake()
    {
        canRetry.Value = false;
    }

    private void Start()
    {
        //retryボタンでgameInit(false)を呼ぶ
        IDisposable subscription = canRetry.Subscribe(x => {
            gameInit(x);
        });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("自身がマスタークライアントです");
            team = "Left";
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
            //GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());

        }
        else
        {
            Debug.Log("マスタークライアントではありません");
            team = "Right";
            //GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
        }
    }
    
    void gameInit(bool retry)
    {
        Debug.Log("InitCall");
        if (retry == true) { return; }
        //初期化処理
        photonView.RPC(nameof(RpcGameInit), RpcTarget.All);
        var tmp= GameObject.FindGameObjectsWithTag("Player");
        //GameObject tmp = GameObject.Find("Avatar");//.GetComponent<AvatarManager>().gameStart = false;
        if (tmp != null)
        {
            Debug.Log("Setfalse");
            for (int i = 0; i < tmp.Length-1; i++)
            {
                //tmp[i].GetComponent<AvatarManager>().gameStart = false;
            }
        }
    }

    //負けると発火して部屋のプロパティを設定
    void loseEvent()
    {
        Debug.Log("負け");
        result.Value = "Lose";
        //photonView.RPC(nameof(RpcGameInit), RpcTarget.All);
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        if (team == "Right")
        {
            hashtable["Winner"] = "Left";
        }
        else if (team == "Left")
        {
            hashtable["Winner"] = "Right";
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    //部屋のプロパティが変更されると自身を勝者か判定する
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // 更新されたルームのカスタムプロパティのペアをコンソールに出力する
        //foreach (var prop in propertiesThatChanged)
        //{
        //    Debug.Log($"{prop.Key}: {prop.Value}");
        //}
        string winner = (PhotonNetwork.CurrentRoom.CustomProperties["Winner"] is string value) ? value : "null";
        if (winner == team)
        {
            winEvent();
        }
        //ボールを削除
        var ball = GameObject.FindWithTag("ball");
        Destroy(ball);
        gameEndEvent();
    }

    void winEvent()
    {
        Debug.Log("勝ち");
        result.Value = "Win";
    }

    //マスタークライアントは再戦ボタンを出現
    void gameEndEvent()
    {        
        //photonView.RPC(nameof(RpcGameInit), RpcTarget.All);
        if (PhotonNetwork.IsMasterClient)
        {
            canRetry.Value = true;
        }
    }

    [PunRPC]
    void RpcGameInit()
    {
        result.Value = "";
    }





}
