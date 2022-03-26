using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using System;

public class Manager : MonoBehaviourPunCallbacks
{
    public ReactiveProperty<String> result;
    public ReactiveProperty<bool> canRetry;
    // Start is called before the first frame update
    void Awake()
    {
        canRetry.Value = false;
    }
    private void Start()
    {
        IDisposable subscription = canRetry.Subscribe(x => {
            gameInit(x);
        });
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("自身がマスタークライアントです");
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());

        }
        else
        {
            Debug.Log("マスタークライアントではありません");
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
        }
    }
    void winEvent()
    {
        Debug.Log("勝ち");
        result.Value = "Win";
        gameEndEvent();
    }
    void loseEvent()
    {
        Debug.Log("負け");
        result.Value = "Lose";
        gameEndEvent();
    }
    void gameEndEvent()
    {
        //共通処理部分
        if (PhotonNetwork.IsMasterClient)
        {
            canRetry.Value = true;
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
                tmp[i].GetComponent<AvatarManager>().gameStart = false;
            }


        }
    }
    [PunRPC]
    void RpcGameInit()
    {
        result.Value = "";
        var ball = GameObject.FindWithTag("ball");
        Destroy(ball);
    }
}
