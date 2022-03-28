using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AvatarManager : MonoBehaviourPunCallbacks
{
    public bool gameStart = false;
    public bool IsMaster = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IsMaster = true;
        }
        else
        {
            IsMaster = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            var players = PhotonNetwork.PlayerList;
            if (canStartGame()==true && gameStart == false && IsMaster == true)
            {
                //ゲーム開始処理
                Debug.Log("gamestart");
                var position = new Vector3(-3f, 0f);
                PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
                gameStart = true;
            }
            else if (players.Length == 1)
            {
                gameStart = false;
            }
        }
    }

    //ゲーム開始可能かどうかの判定
    private bool canStartGame()
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
            /*string myTeam=players[i].CustomProperties["myTeam"] is string message) ? message: string.Empty;*/
        }
        return (right == true && left == true);
    }
}
