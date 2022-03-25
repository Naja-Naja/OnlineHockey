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
            Debug.Log("���g���}�X�^�[�N���C�A���g�ł�");
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());

        }
        else
        {
            Debug.Log("�}�X�^�[�N���C�A���g�ł͂���܂���");
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
        }
    }
    void winEvent()
    {
        Debug.Log("����");
        result.Value = "Win";
        gameEndEvent();
    }
    void loseEvent()
    {
        Debug.Log("����");
        result.Value = "Lose";
        gameEndEvent();
    }
    void gameEndEvent()
    {
        //���ʏ�������
    }
}
