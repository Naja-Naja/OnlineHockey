using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RoomAPI : MonoBehaviourPunCallbacks
{
    public void ConnectServer(string nickName)
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = nickName;
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
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
            //自身が部屋主であれば部屋のプロパティを"Matching"に設定する
            SetRoomProperties("Ready");
            //hashtable["RoomState"] = "Matching";
            //PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        //AvatarDestroy();
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
        Debug.Log("試合開始可能かをチェック");
        string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        if (myTeam == "Left")
        {
            //自身のアバター（ネットワークオブジェクト）を生成する
            string mypos = CheckAmIFront();
            if (mypos == "front"||mypos=="None")
            {
                var position = new Vector3(-6f, 0f);
                PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            }
            else if (mypos == "back")
            {
                var position = new Vector3(-4f, 0f);
                PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            }
        }
        else if (myTeam == "Right")
        {
            string mypos = CheckAmIFront();
            if (mypos == "front" || mypos == "None")
            {
                var position = new Vector3(6f, 0f);
                PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            }
            else if (mypos == "back")
            {
                var position = new Vector3(4f, 0f);
                PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            }
        }
    }
    private string CheckAmIFront()
    {
        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //var players = PhotonNetwork.PlayerList;
        var players = PhotonNetwork.PlayerListOthers;

        int mynum = PhotonNetwork.LocalPlayer.ActorNumber;
        string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        for (int i = 0; i < players.Length; i++)
        {
            string playerTeam = (players[i].CustomProperties["myTeam"] is string _value) ? _value : "null";
            if (myTeam == playerTeam)
            {
                if (players[i].ActorNumber > mynum)
                {
                    return "front";
                }
                else
                {
                    return "back";
                }
            }
        }
        return "None";
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
        //ボールを削除
        var ball = GameObject.FindWithTag("ball");
        PhotonNetwork.Destroy(ball);
    }
    [PunRPC]
    public void AvatarDestroy()
    {
        //アバターを削除
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            PhotonNetwork.Destroy(players[i]);
            //Destroy(players[i]);
        }
    }
}
