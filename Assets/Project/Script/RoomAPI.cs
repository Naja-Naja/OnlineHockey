using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RoomAPI : MonoBehaviourPunCallbacks
{
    public void ConnectServer()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 30;
    }
    public void roomPropatiesInit()
    {
        int playernum = PhotonNetwork.LocalPlayer.ActorNumber;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["myTeam"] = "Choice";
        hashtable["myNumber"] = playernum;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (PhotonNetwork.IsMasterClient)
        {
            //���g��������ł���Ε����̃v���p�e�B��"Matching"�ɐݒ肷��
            SetRoomProperties("Matching");
            //hashtable["RoomState"] = "Matching";
            //PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }
    public void JoinTeam(string joinTeam)
    {
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["myTeam"] = joinTeam;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    public bool CheckCanGameStart()
    {
        var players = PhotonNetwork.PlayerList;
        bool right = false;
        bool left = false;
        for (int i = 0; i < players.Length; i++)
        {
            string myTeam = (string)players[i].CustomProperties["myTeam"];
            if (myTeam == "Right")
            {
                right = true;
                Debug.Log("RightHere");
            }
            else if (myTeam == "Left")
            {
                left = true;
                Debug.Log("LeftHere");
            }
        }
        return (right == true && left == true);
    }
    public void SetRoomProperties(string roomProperties)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["RoomState"] = roomProperties;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }
    public void CreateAvatar()
    {
        Debug.Log("�����J�n�\�����`�F�b�N");
        string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        if (myTeam == "Left")
        {
            //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            var position = new Vector3(-6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        }
        else if (myTeam == "Right")
        {
            //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            var position = new Vector3(6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
    }
    public void CreateBall()
    {
        var position = new Vector3(-3f, 0f);
        PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
    }
    public void LoseEvent(string loser)
    {

        var hashtable = new ExitGames.Client.Photon.Hashtable();
        if (loser == "Right")
        {
            hashtable["Winner"] = "Left";
        }
        else if (loser == "Left")
        {
            hashtable["Winner"] = "Right";
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }
    [PunRPC]
    public void BallDestroy()
    {
        //�{�[�����폜
        var ball = GameObject.FindWithTag("ball");
        Destroy(ball);
    }
}
