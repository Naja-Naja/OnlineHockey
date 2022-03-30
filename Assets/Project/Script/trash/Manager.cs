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
        //retry�{�^����gameInit(false)���Ă�
        IDisposable subscription = canRetry.Subscribe(x => {
            gameInit(x);
        });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("���g���}�X�^�[�N���C�A���g�ł�");
            team = "Left";
            GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
            //GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());

        }
        else
        {
            Debug.Log("�}�X�^�[�N���C�A���g�ł͂���܂���");
            team = "Right";
            //GameObject.Find("ownerDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => winEvent());
            GameObject.Find("guestDeadline").GetComponent<DeadLine>().GameOver.Subscribe(x => loseEvent());
        }
    }
    
    void gameInit(bool retry)
    {
        Debug.Log("InitCall");
        if (retry == true) { return; }
        //����������
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

    //������Ɣ��΂��ĕ����̃v���p�e�B��ݒ�
    void loseEvent()
    {
        Debug.Log("����");
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

    //�����̃v���p�e�B���ύX�����Ǝ��g�����҂����肷��
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // �X�V���ꂽ���[���̃J�X�^���v���p�e�B�̃y�A���R���\�[���ɏo�͂���
        //foreach (var prop in propertiesThatChanged)
        //{
        //    Debug.Log($"{prop.Key}: {prop.Value}");
        //}
        string winner = (PhotonNetwork.CurrentRoom.CustomProperties["Winner"] is string value) ? value : "null";
        if (winner == team)
        {
            winEvent();
        }
        //�{�[�����폜
        var ball = GameObject.FindWithTag("ball");
        Destroy(ball);
        gameEndEvent();
    }

    void winEvent()
    {
        Debug.Log("����");
        result.Value = "Win";
    }

    //�}�X�^�[�N���C�A���g�͍Đ�{�^�����o��
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
