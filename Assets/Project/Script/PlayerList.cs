using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;

public class PlayerList : MonoBehaviourPunCallbacks
{
    public ReactiveProperty<string> leftPlayerList;
    public ReactiveProperty<string> rightPlayerList;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Ready")
        {
            leftPlayerList.Value = "";
            rightPlayerList.Value = "";
            var players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                string myTeam = (string)players[i].CustomProperties["myTeam"];
                if (myTeam == "Right")
                {
                    rightPlayerList.Value = rightPlayerList.Value + players[i].NickName + players[i].ActorNumber + "\n";
                }
                else if (myTeam == "Left")
                {
                    leftPlayerList.Value = leftPlayerList.Value + players[i].NickName + players[i].ActorNumber + "\n";
                }
            }
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Game")
        {
            leftPlayerList.Value = "";
            rightPlayerList.Value = "";
        }
    }
}
