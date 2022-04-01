using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BGMChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] AudioClip gameBGM;
    [SerializeField] AudioClip roomBGM;
    AudioClip nowBGM;
    private void Start()
    {
        AudioManager.BGM_Play(roomBGM);
        nowBGM = roomBGM;
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Game" || room == "Result")
        {
            if (nowBGM != gameBGM)
            {
                AudioManager.BGM_Play(gameBGM);
                nowBGM = gameBGM;
            }

        }
        else if(nowBGM!=roomBGM)
        {
            AudioManager.BGM_Play(roomBGM);
            nowBGM = roomBGM;
        }
    }
}
