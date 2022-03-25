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
    // Start is called before the first frame update
    void Start()
    {


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
    }
}
