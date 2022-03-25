using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Manager : MonoBehaviourPunCallbacks
{
    bool IsMaster = false;
    bool gameStart = false;
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("自身がマスタークライアントです");
            IsMaster = true;
        }
        else { IsMaster = false; }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            var players = PhotonNetwork.PlayerList;
            if (players.Length > 1 && gameStart == false&&IsMaster==true)
            {
                //ゲーム開始処理
                var position = new Vector3(-3f, 0f);
                PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
                gameStart = true;
            }
        }
    }
}
